using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursue_Mage : StateMachineBehaviour
{
    private Agent script;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        //Variable Dist para ver la distancia que hay de su destino
        float dist = Vector3.Distance(script.Jugador.transform.position , aget.transform.position);

        if (dist < 10)
        {
            //PASA a atacar cuado el jugador esta a una distancia menor que 10 metros
            animator.SetBool("Attack", true);
            Debug.Log("Esta atacando");
           
        }
        else
        {
            //Solo cuando no esta a una distancia menor de 10 metros
            script.UltimaPosicion_Jugador = script.Jugador.transform;
            animator.SetBool("Search", true);
            
        }
    }




}
