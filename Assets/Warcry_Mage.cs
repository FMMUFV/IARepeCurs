using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warcry_Mage : StateMachineBehaviour
{
    Agent ScritpAgent;
    int DistanUltimaPosJugador = 1;
    public float raycas;
    RaycastHit hit;//rayo
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetBool("Scan", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        ScritpAgent = animator.GetComponent<Agent>();
    

        Rayo(animator);

        //aget.remainingDistance= La distancia entre la posición del agente y el destino en la ruta actual. 

        //Mirar esta parte parece que no coje la posicion

        PasarScan(animator);

    }

    public void PasarScan(Animator animator)
    {
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        aget.destination = ScritpAgent.UltimaPosicion_Jugador;
        ScritpAgent = animator.GetComponent<Agent>();

        
        float dist = Vector3.Distance(aget.transform.position, ScritpAgent.UltimaPosicion_Jugador);
        //if (dist < 1)
        //{
        //    //Este if pasa toalment de el //Preguntar pro que lo tengo que hacer con el de arriva
        if (!aget.hasPath || aget.remainingDistance < 1)
        {
            animator.SetBool("Warcry", false);
            animator.SetBool("Scan", true);
            Debug.Log("Esta en la posicion");
        }
        //}
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





    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
