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
        Rayo(animator);
    }




   

    public void Rayo(Animator animator)
    {
        script.Jugador = GameObject.FindGameObjectWithTag("Player");


        // ----------- Desede esta parte se le pasa la ultima posicion del jugador
        Vector3 rayDirection = animator.transform.forward;
        Vector3 direccion = script.UltimaPosicion_Jugador - animator.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direccion);

        rayDirection = rotation * Vector3.forward;

        //----------------------------------------------------------------------------


        //------Aqui se le pasa la posicion directamente del jugador
        Vector3 rayDirection2 = animator.transform.forward;
        Vector3 direccion2 = script.Jugador.transform.position - animator.transform.position;
        Quaternion rotation2 = Quaternion.LookRotation(direccion2);

        rayDirection2 = rotation2 * Vector3.forward;

        //----------------

        Debug.DrawRay(animator.transform.position + Vector3.up * 1.5f, rayDirection2 * raycas, Color.red);

        if (Physics.Raycast(animator.transform.position + Vector3.up*1.5f, rayDirection2, out hit, raycas))
        {

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("El arquero lo ve");

                animator.SetBool("Patrol", false);
                animator.SetBool("Pursue", true);

                script.Jugador = hit.transform.gameObject;

            }

        }
    }

}
