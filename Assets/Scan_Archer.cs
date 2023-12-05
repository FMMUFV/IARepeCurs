using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan_Archer : StateMachineBehaviour
{

    public float raycas;
    RaycastHit hit;//rayo
    private Agent script;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       script = animator.gameObject.GetComponent<Agent>();
       raycas = script.raycas;
       Rayo(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // Rayo(animator);
    }




   

    public void Rayo(Animator animator)
    {
        Vector3 rayDirection = animator.transform.forward;


        Vector3 ultimapos = script.UltimaPosicion_Jugador;
        Vector3 direccion = ultimapos - animator.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direccion);

        rayDirection = rotation * Vector3.forward;






        Debug.DrawRay(animator.transform.position + Vector3.up * 1.5f, rayDirection * raycas, Color.red);

        if (Physics.Raycast(animator.transform.position + Vector3.up*1.5f, rayDirection, out hit, raycas))
        {

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                animator.SetBool("Patrol", false);
                animator.SetBool("Pursue", true);

                script.Jugador = hit.transform.gameObject;

            }

        }
    }

}
