using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Attack_Mage : StateMachineBehaviour
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
        float dist = Vector3.Distance(script.Jugador.transform.position, aget.transform.position);

        if (dist < 10)
        {
            //Ataca si esta a menos de 10
            //Lanza un hechizo y no se mueve

            aget.stoppingDistance = 10;
          
            LanzaHechizo(animator);
        }
        else
        {
            //Solo cuando no esta a una distancia menor de 10 metros
            aget.stoppingDistance = 0;
            animator.SetBool("Attack", false);
           

        }
    }

    public bool ataca = true;

    //Aqui hace daño al jugador
    public void LanzaHechizo(Animator animator)
    {
        if(ataca == true)
        {
            script.Jugador.SendMessage("Damage", 1);
            Debug.Log("Lanza un hechizo");
            ataca = false;
        }
        
    }

}
