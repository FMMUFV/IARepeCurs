using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Search_Mage : StateMachineBehaviour
{

    private Agent script;
    public float raycas;
    RaycastHit hit;//rayo

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
        //Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
       //Va a la ultima posicion
        aget.destination = script.UltimaPosicion_Jugador.transform.position;
        raycas = script.raycas;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rayo(animator);
    }

    public void Rayo(Animator animator)
    {

        Vector3 rayDirection = animator.transform.forward;

        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.red);

        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                //vuelve a ver pasa a Pursue
                animator.SetBool("Search", false);
                script.Jugador = hit.transform.gameObject;

                
            }

        }
    }


}
