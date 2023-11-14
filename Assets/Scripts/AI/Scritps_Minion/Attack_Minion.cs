using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack_Minion : StateMachineBehaviour
{

    RaycastHit hit;//rayo
    public float raycas;
    private Agent script;
    // Variable para almacenar la distancia de ataque deseada
    float distanciaDeAtaque = 1.3f;


    public float countdownTime = 3.0f; // Tiempo inicial de la cuenta atr�s en segundos
    public float currentTime;

    public bool AtacaDeNuevo;
    NavMeshAgent aget;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aget = animator.GetComponent<NavMeshAgent>();
        AtacaDeNuevo = true;
        // Configura el tiempo actual con el valor inicial
        currentTime = countdownTime;
        aget.isStopped = true;
        Rayo(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Contados(animator);

        Health ScriptVida = animator.gameObject.GetComponent<Health>();

        if (ScriptVida.PasarEstun == true)
        {
            ScriptVida.PasarEstun = false;
            animator.SetBool("Stunned", true);
        }
    }

    public void Rayo(Animator animator)
    {
        script = animator.gameObject.GetComponent<Agent>();
        raycas = script.raycas;
        // Obtener la direcci�n del rayo en funci�n de la rotaci�n del animator
        Vector3 rayDirection = animator.transform.forward;

        // Dibuja un rayo de depuraci�n (para visualizaci�n en el Editor de Unity)
        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.green);

        // Realiza un raycast y almacena la informaci�n de colisi�n en 'hit'

        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {
            // Comprobar si el objeto golpeado tiene una etiqueta "Player"

            if (hit.transform.gameObject.tag == "Player")
            {
                if (hit.distance < distanciaDeAtaque)
                {
                    // El jugador est� a una distancia de ataque, as� que ataca
                    script.Jugador.SendMessage("Damage", 1); // Infligir da�o al jugador
                   
                   if (script.Warrior == true )
                    {
                        //Aqui voy ha hacer haga el llamamineto
                        
                        GameManager.Instance.Grito(hit.transform.position);
                        Debug.Log("gritando");
                        animator.SetBool("Warcry", false);
                    }

                    // Puedes agregar l�gica para ejecutar el ataque aqu�
                }
                else
                {
                    animator.SetBool("Attack", false);
                }
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        aget.isStopped = false;
    }


    public void Contados(Animator animator)
    {


        // Verifica si el tiempo actual es mayor que 0
        if (currentTime > 0)
        {
            // Reduce el tiempo actual en cada fotograma
            currentTime -= Time.deltaTime;

            // Muestra el tiempo actual en la consola (puedes adaptarlo para mostrarlo en una UI)
        }
        else
        {
            currentTime = countdownTime;
            // La cuenta atr�s ha llegado a cero, puedes agregar acciones aqu�
            animator.SetBool("Attack", false);

        }
    }


}
