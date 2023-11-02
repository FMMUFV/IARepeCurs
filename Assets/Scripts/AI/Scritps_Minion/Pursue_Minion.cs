using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursue_Minion : StateMachineBehaviour
{
    RaycastHit hit;//rayo
    public float raycas;

    // Variable para almacenar la distancia de ataque deseada
    float distanciaDeAtaque = 1f;

    public float countdownTime = 5.0f; // Tiempo inicial de la cuenta atr�s en segundos
    public float currentTime;


    private Agent script;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Configura el tiempo actual con el valor inicial
        currentTime = countdownTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        script = animator.gameObject.GetComponent<Agent>();
        
        Rayo(animator);

    }



    public void Rayo(Animator animator)
    {
        raycas = script.raycas;
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        // Obtener la direcci�n del rayo en funci�n de la rotaci�n del animator
        Vector3 rayDirection = animator.transform.forward;

        // Dibuja un rayo de depuraci�n (para visualizaci�n en el Editor de Unity)
        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.green);

        // Realiza un raycast y almacena la informaci�n de colisi�n en 'hit'
       
        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {
            // Comprobar si el objeto golpeado tiene una etiqueta "Player"
           
            if (hit.transform.gameObject.tag == "Player")
            {
                
                float distanciaAlJugador = Vector3.Distance(animator.transform.position, hit.transform.position);

                if (distanciaAlJugador < distanciaDeAtaque)
                {
                    // El jugador est� a una distancia de ataque, as� que ataca

                    
                    animator.SetBool("Attack", true);
                    // Puedes agregar l�gica para ejecutar el ataque aqu�
                }
                else
                {
                    // El jugador est� fuera de la distancia de ataque, as� que persigue

                    // Configura la posici�n de destino del enemigo al jugador�
                    
                    aget.stoppingDistance = 0;
                    aget.destination = hit.transform.position;
                }
            }
            else 
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Pursue", false);
                animator.SetBool("Patrol", true); 
            }
        }
        else
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Pursue", false);
            animator.SetBool("Patrol", true);
        }



    }



  


    
}
