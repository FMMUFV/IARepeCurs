using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Punto_Alto_Archer : StateMachineBehaviour
{

    public float raycas;
    RaycastHit hit;//rayo
    private Agent script;
    NavMeshAgent aget;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state


    public float countdownTime = 5.0f; // Tiempo en segundos para la cuenta regresiva
    public float currentTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
        raycas = script.raycas;
        aget = animator.GetComponent<NavMeshAgent>();


        currentTime = countdownTime;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aget.destination = script.PosicionAlta;
        EnPuente(animator);
        Rayo(animator);
    }




    public Transform RotarEnemigo;
    private GameObject Jugador;
    public void Rayo(Animator animator)
    {
        RotarEnemigo = animator.gameObject.transform;
        // Se pasa la referencia del jugador
        Jugador = script.Jugador;

        //------Aqui se le pasa la posicion directamente del jugador   
        Vector3 PosInicioRayo_Suma = new Vector3(0, 2.5f, 0) + animator.transform.position; // Inicializa la posición de inicio del rayo
        Vector3 direccion2 = (Jugador.transform.position - new Vector3(0, 1.5f, 0)) - (animator.transform.position);// Calcula la dirección del rayo
        Quaternion rotation2 = Quaternion.LookRotation(direccion2);// Calcula la rotación necesaria para apuntar en la dirección del rayo
        Vector3 rayDirection2 = rotation2 * Vector3.forward;// Calcula la dirección final del rayo 

        //----------------
        //Aqui rotara el arquero mirando al jugador
        //RotarEnemigo.Rotate(Jugador.transform.position);

        if ((aget.remainingDistance < 1))
        {

            //Quaternion
            RotarEnemigo.rotation = Jugador.transform.rotation;
           
            //-----------
        }


        Debug.DrawRay( PosInicioRayo_Suma, rayDirection2 * raycas, Color.yellow);

        if (Physics.Raycast( PosInicioRayo_Suma, rayDirection2, out hit, raycas))
        {

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("El arquero lo ve al jugador");
                script.PuntoAlto = true;
                animator.SetBool("Attack", true);

                script.Jugador = hit.transform.gameObject;

            }
            else
            {
                if ((aget.remainingDistance < 1))
                {
                    // animator.SetBool("Patrol", true);
                    Contados(animator);
                }

            }
           

        }
        else
        {
            if ((aget.remainingDistance < 1))
            {
                // animator.SetBool("Patrol", true);
                Contados(animator);
            }
        }

    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aget = animator.GetComponent<NavMeshAgent>();
        animator.SetBool("Warcry", false);
        aget.isStopped = false;
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
            animator.SetBool("Patrol", true);

        }
    }
}
