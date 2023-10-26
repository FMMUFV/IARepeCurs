using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scan_Mage : StateMachineBehaviour
{
    RaycastHit hit;//rayo
    private Agent script;
    public float raycas;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

       // Rayo(animator);
        raycas = script.raycas;

      //  Rotacion(animator);
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
                animator.SetBool("Pursue", true);
                script.Jugador = hit.transform.gameObject;
            }

        }
    }


    float x;

    public void Rotacion(Animator animator)
    {
        x += Time.deltaTime * 10;

        //Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();

        aget.transform.rotation = Quaternion.Euler(x, 0, 0);

        
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
