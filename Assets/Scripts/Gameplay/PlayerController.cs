using UnityEngine;
using UnityEngine.InputSystem;

// Use a separate PlayerInput component for setting up input.
public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    [Tooltip("Melee attack distance")]
    public float AttackDistance = 4f;
    [Tooltip("Attack damage applied")]
    public float AttackDamage = 1f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    public delegate void OnValueUpdate(Vector2 value);
    public event OnValueUpdate OnAimUpdate;

    private CharacterController m_CharacterController;
    private Animator m_AnimatorComponent;
    private Vector2 m_Aim;
    private Vector2 m_Move;
    private float m_Speed;
    private float m_AnimationBlend;
    private float m_TargetRotation = 0.0f;
    private float m_RotationVelocity;
    private float m_VerticalVelocity;
    private float m_TerminalVelocity = 53.0f;
    private bool m_Attacking = false;

    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_AnimatorComponent = GetComponentInChildren<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        m_Aim = context.ReadValue<Vector2>();
        OnAimUpdate?.Invoke(m_Aim);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !m_Attacking)
        {
            Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(m_Aim.x, m_Aim.y, Camera.main.nearClipPlane));
            // Rotate towards
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(m_Aim.x, m_Aim.y, Camera.main.nearClipPlane));
            Plane plane = new Plane(transform.up, transform.position);
            //Initialise the enter variable
            float enter = 0.0f;
            if (plane.Raycast(ray, out enter))
            {
                Vector3 playerDirection = (ray.GetPoint(enter) - transform.position).normalized;
                m_TargetRotation = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0.0f, m_TargetRotation, 0.0f);
            }
            // Try interaction
            Vector3 direction = mouseWorldPoint - Camera.main.transform.position;
            RaycastHit hit;
            Interactable interactableComponent;
            if (Physics.Raycast(mouseWorldPoint, direction, out hit, 100f) && hit.collider.TryGetComponent<Interactable>(out interactableComponent))
            {
                interactableComponent.Interact(gameObject);
            }
            else if (Physics.Raycast(transform.position,transform.forward, out hit, AttackDistance) && hit.collider.TryGetComponent<Interactable>(out interactableComponent))
            {
                interactableComponent.Interact(gameObject);
            }
            else
            {
                m_Attacking = true;
                if (m_AnimatorComponent != null)
                {
                    m_AnimatorComponent.SetTrigger("Attack1h");
                }
            }
        }
    }

    public void Update()
    {
        ApplyGravity();
        GroundedCheck();
        Move();
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (m_AnimatorComponent)
        {
            m_AnimatorComponent.SetBool("Grounded", Grounded);
        }
    }

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (m_Move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(m_CharacterController.velocity.x, 0.0f, m_CharacterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = m_Move.magnitude;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            m_Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            m_Speed = Mathf.Round(m_Speed * 1000f) / 1000f;
        }
        else
        {
            m_Speed = targetSpeed;
        }

        m_AnimationBlend = Mathf.Lerp(m_AnimationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (m_AnimationBlend < 0.01f) m_AnimationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(m_Move.x, 0.0f, m_Move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (m_Move != Vector2.zero)
        {
            m_TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_TargetRotation, ref m_RotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, m_TargetRotation, 0.0f) * Vector3.forward;

        // move the player
        if (!m_Attacking)
        {
            m_CharacterController.Move(targetDirection.normalized * (m_Speed * Time.deltaTime) + new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime);
        }

        // update animator if using character
        if (m_AnimatorComponent != null)
        {
            m_AnimatorComponent.SetFloat("Speed", m_AnimationBlend);
        }
    }

    private void ApplyGravity()
    {
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (m_VerticalVelocity < m_TerminalVelocity)
        {
            m_VerticalVelocity += Gravity * Time.deltaTime;
        }
    }

    public void AttackEffect()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, AttackDistance))
        {
            hit.collider.SendMessage("Damage", 1f, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void EndAttack()
    {
        m_Attacking = false;
    }
}
