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
     
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rayo(animator);
    }





    private GameObject Jugador;
    public void Rayo(Animator animator)
    {
        // Se pasa la referencia del jugador
        Jugador = GameObject.FindGameObjectWithTag("Player");

        //------Aqui se le pasa la posicion directamente del jugador
        Vector3 PosInicioRayo_Suma = new Vector3(0, 2.5f, 0);
        Vector3 rayDirection2 = animator.transform.forward;
        Vector3 direccion2 = (Jugador.transform.position) - (animator.transform.position + PosInicioRayo_Suma);
        Quaternion rotation2 = Quaternion.LookRotation(direccion2);

        rayDirection2 = rotation2 * Vector3.forward;

        //----------------

        Debug.DrawRay(animator.transform.position + PosInicioRayo_Suma, rayDirection2 * raycas, Color.red);

        if (Physics.Raycast(animator.transform.position + PosInicioRayo_Suma, rayDirection2, out hit, raycas))
        {

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("El arquero lo ve al jugador");

                //animator.SetBool("Patrol", false);
               // animator.SetBool("Pursue", true);

                script.Jugador = hit.transform.gameObject;

            }

        }
    }

}
