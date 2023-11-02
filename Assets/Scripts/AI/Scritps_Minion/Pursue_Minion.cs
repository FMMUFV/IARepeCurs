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

    public float countdownTime = 5.0f; // Tiempo inicial de la cuenta atrás en segundos
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
                
                float distanciaAlJugador = Vector3.Distance(animator.transform.position, hit.transform.position);

                if (distanciaAlJugador < distanciaDeAtaque)
                {
                    // El jugador está a una distancia de ataque, así que ataca

                    
                    animator.SetBool("Attack", true);
                    // Puedes agregar lógica para ejecutar el ataque aquí
                }
                else
                {
                    // El jugador está fuera de la distancia de ataque, así que persigue

                    // Configura la posición de destino del enemigo al jugadorÇ
                    
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
