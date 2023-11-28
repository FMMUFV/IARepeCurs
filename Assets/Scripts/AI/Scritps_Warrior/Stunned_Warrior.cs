using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunned_Warrior : StateMachineBehaviour
{

    public float countdownTime = 3.0f; // Tiempo inicial de la cuenta atrás en segundos
    public float currentTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Configura el tiempo actual con el valor inicial
        currentTime = countdownTime;
      

    }


 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Contados(animator);
    }



    public void Contados(Animator animator)
    {


        // Verifica si el tiempo actual es mayor que 0
        if (currentTime > 0)
        {
            // Reduce el tiempo actual en cada fotograma
            currentTime -= Time.deltaTime;

            // Muestra el tiempo actual en la consola (puedes adaptarlo para mostrarlo en una UI)
        }
        else
        {
            currentTime = countdownTime;
            // La cuenta atrás ha llegado a cero, puedes agregar acciones aquí
            animator.SetBool("Stunned", false);

        }
    }

   
}
