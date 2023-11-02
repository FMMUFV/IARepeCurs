using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Search_Mage : StateMachineBehaviour
{

    private Agent script;
    public float raycas;
    RaycastHit hit;//rayo
    private Vector3 Destino;//Direccion a la que tiene que ir

    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
        //Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();

        
        
       
        aget.stoppingDistance = 0;
        raycas = script.raycas;
    }


    bool Scan = false;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rayo(animator);

    /*
        //Si a llegado a scan
             NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
                    //Variable Dist para ver la distancia que hay de su destino
                    float dist = Vector3.Distance(Destino, aget.transform.position);

                    //Si a llegado o esta posicion pasa a scad
                    if(dist < 1)
                    {

                // animator.SetBool("Scan", true);
                       Scan = true;

                    }
           */         
    }

    public void Rayo(Animator animator)
    {

        float DistanciaVePorUltimavez = 1f;
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();

        Vector3 rayDirection = animator.transform.forward;

        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.red);

        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {

            if (hit.transform.gameObject.tag == "Player")
            {
                animator.SetBool("Search", false);
                animator.SetBool("Pursue", true);

                
            }
            else
            {
                //Aqui tiene que ir a la ultima posicion del jugador
                float distanciaAlJugador = Vector3.Distance(animator.transform.position, script.UltimaPosicion_Jugador);

                if (distanciaAlJugador < DistanciaVePorUltimavez)
                {
                    // El jugador está a una distancia de ataque, así que ataca

                    aget.stoppingDistance = 10;

                    //-----------Tengo que programar que a llegado donde estaba el jugador y pase a scan
                    animator.SetBool("Search", false);
                    animator.SetBool("Pursue", false);
                    animator.SetBool("Scan", true);

                }
                else
                {       
                    // El jugador está fuera de la distancia de ataque, así que persigue

                    // Configura la posición de destino del enemigo al jugadorÇ

                    aget.stoppingDistance = 0;
                    aget.destination = script.UltimaPosicion_Jugador;
                }
            }
        }

    }
}
