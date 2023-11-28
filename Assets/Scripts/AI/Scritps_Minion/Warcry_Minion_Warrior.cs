using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warcry_Minion_Warrior : StateMachineBehaviour
{
    Agent ScritpAgent;
    int DistanUltimaPosJugador = 1;
    public float raycas;
    RaycastHit hit;//rayo
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        ScritpAgent = animator.GetComponent<Agent>();
        aget.destination = ScritpAgent.UltimaPosicion_Jugador;

        Rayo(animator);

        //aget.remainingDistance= La distancia entre la posición del agente y el destino en la ruta actual. 

        if (aget.hasPath && aget.remainingDistance < 1)
        {
           
            animator.SetBool("Warcry", false);
            animator.SetBool("Patrol", true);

        }

    }


    public void Rayo(Animator animator)
    {
        ScritpAgent = animator.gameObject.GetComponent<Agent>();
        raycas = ScritpAgent.raycas;
        // Obtener la dirección del rayo en función de la rotación del animator
        Vector3 rayDirection = animator.transform.forward;

        // Dibuja un rayo de depuración (para visualización en el Editor de Unity)
        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.green);

        // Realiza un raycast y almacena la información de colisión en 'hit'

        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {
            // Comprobar si el objeto golpeado tiene una etiqueta "Player"

            if (hit.transform.gameObject.tag == "Player")
            {
                animator.SetBool("Pursue", true);  
             
            }
       
        }
    
    }




    int PuenteMask;
    private bool EnlaArena = false;
    //Puente O pasarela
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




}
