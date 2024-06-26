using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack_Archer : StateMachineBehaviour
{
    RaycastHit hit;//rayo
    private GameObject Jugador;
    private Agent script;
    public float raycas;
    NavMeshAgent aget;


    // Tiempo del ataque
    public float countdownTime = 3.0f; // Tiempo en segundos para la cuenta regresiva
    public float currentTime;
    public bool AtacaDeNuevo;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
        raycas = script.raycas;
        aget = animator.GetComponent<NavMeshAgent>();
        aget.isStopped = true;


        AtacaDeNuevo = true;
        currentTime = countdownTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
 override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        EnPuente(animator);
        Rayo(animator);
        
    }


    private bool EnlaArena = false;
    private int PuenteMask;//Paraque afecte cuando esta en el puente

    public void EnPuente(Animator animator)
    {
        Agent scriptAgent = animator.gameObject.GetComponent<Agent>();
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();





        PuenteMask = 1 << NavMesh.GetAreaFromName("Scaffold");
        NavMeshHit hit;
        if (NavMesh.SamplePosition(animator.transform.position, out hit, 2.0f, PuenteMask))
        {


            if (EnlaArena == true) //Para que le cambie la velocidad solo una vez cuando este la arena
            {
                //NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();

                aget.speed = scriptAgent.velocidadSueloPuente;
                EnlaArena = false;

            }



        }
        else
        {

            aget.speed = scriptAgent.velocidadSueloNormal;

            EnlaArena = true;
        }
    }



    public Transform RotarEnemigo;
    public void Rayo(Animator animator)
    {
        // Se pasa la referencia del jugador
        Jugador = script.Jugador;

        //------Aqui se le pasa la posicion directamente del jugador   
        Vector3 PosInicioRayo_Suma = new Vector3(0, 2.5f, 0) + animator.transform.position; // Inicializa la posici�n de inicio del rayo
        Vector3 direccion2 = (Jugador.transform.position - new Vector3(0, 1.5f, 0)) - (animator.transform.position);// Calcula la direcci�n del rayo
        Quaternion rotation2 = Quaternion.LookRotation(direccion2);// Calcula la rotaci�n necesaria para apuntar en la direcci�n del rayo
        Vector3 rayDirection2 = rotation2 * Vector3.forward;// Calcula la direcci�n final del rayo 

        //----------------
        //--------Se programa la rotacion del enemigo mirando al jugador
        RotarEnemigo = animator.gameObject.transform;
        RotarEnemigo.rotation = Jugador.transform.rotation;

        //-----------------

        Debug.DrawRay( PosInicioRayo_Suma, rayDirection2 * raycas, Color.red);

        if (Physics.Raycast(PosInicioRayo_Suma, rayDirection2, out hit, raycas))
        {

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                //Aqui dispara solo cuando esta en una zona elevadas y al salir del estado siempre se pone a false
                if (script.PuntoAlto == true)
                {
                    
                    if (hit.distance < 15)
                    {
                        if (AtacaDeNuevo == true)
                        {
                            script.Jugador.SendMessage("Damage", 1); // Infligir da�o al jugador
                           
                            AtacaDeNuevo = false;

                          
                        }
                        Contados(animator);
                    }
                }
                else
                {
                    if (hit.distance < 10)
                    {
                        if (AtacaDeNuevo == true)
                        {
                            script.Jugador.SendMessage("Damage", 1); // Infligir da�o al jugador
                           
                            AtacaDeNuevo = false;
                           
                        }
                        Contados(animator);
                    }
                }
                


                //animator.SetBool("Patrol", false);


                script.Jugador = hit.transform.gameObject;
                script.UltimaPosicion_Jugador = hit.transform.position;

            }
            else
            {
                //Aqui pasa al estado Search por que lo ha perdido
                animator.SetBool("Attack", false);
            }

        }

        else
        {
            //Aqui pasa al estado Search por que lo ha perdido
            animator.SetBool("Attack", false);
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

       
        aget.isStopped = false;
        script.PuntoAlto = false;

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
            AtacaDeNuevo = true;
            currentTime = countdownTime;

        }
    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
