using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scan_Mage : StateMachineBehaviour
{
    RaycastHit hit;//rayo
    private Agent script;
    public float raycas;
    public float VelocidadRotar = 20f;
    public Transform RotarEnemigo;
    public float rotacion;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        aget.isStopped = true;
        RotarEnemigo = animator.gameObject.transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
       Rayo(animator);
       raycas = script.raycas;

        Rotacion(animator);
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
                animator.SetBool("Scan", false);
                animator.SetBool("Patrol", false);
                animator.SetBool("Pursue", true);
                script.Jugador = hit.transform.gameObject;
                
            }

        }
    }


  
   

    public void Rotacion(Animator animator)
    {


        rotacion += Time.deltaTime * VelocidadRotar;
        RotarEnemigo.Rotate(0f, Time.deltaTime * VelocidadRotar, 0f);
        if(rotacion >= 360)
        {
            //La rotacion esta completa por lo tanto tiene que ir a patrol

            //Y cortar los otros caminos activados
           
            rotacion = 0f;
            animator.SetBool("Pursue", false);
            animator.SetBool("Scan", false);
            animator.SetBool("Patrol", true);
            
        }
        
    }

    
}
