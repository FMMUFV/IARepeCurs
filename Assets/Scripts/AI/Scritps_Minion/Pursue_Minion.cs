using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursue_Minion : StateMachineBehaviour
{

    private Agent script;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
        Rayo(animator);

    }

    RaycastHit hit;//rayo
    public float raycas;

    // Variable para almacenar la distancia de ataque deseada
    float distanciaDeAtaque = 1.0f;

    // Variable para almacenar si el enemigo est� persiguiendo al jugador
    bool persiguiendo = false;
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
                    Debug.Log("Atacando al jugador");

                    // Puedes agregar l�gica para ejecutar el ataque aqu�
                }
                else
                {
                    // El jugador est� fuera de la distancia de ataque, as� que persigue
                    Debug.Log("Persiguiendo al jugador");
                    persiguiendo = true;

                    // Configura la posici�n de destino del enemigo al jugador
                    aget.destination = hit.transform.position;
                }
            }
            else { animator.SetBool("Patrol", true); }
        }
        else { animator.SetBool("Patrol", true); }


    }
}
