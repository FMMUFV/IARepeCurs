using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursue_Mage : StateMachineBehaviour
{
    private Agent script;
    RaycastHit hit; // Información sobre el raycast (rayo de colisión)
    public float raycas;

    // OnStateEnter se llama cuando se inicia una transición y se evalúa este estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // animator.SetBool("Pursue", false); 
        raycas = 10;
    }

    // OnStateUpdate se llama en cada cuadro entre las llamadas de OnStateEnter y OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
        Vector3 rayDirection = animator.transform.forward;

        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.red);
        NavMeshAgent aget = animator.GetComponent <NavMeshAgent>();
        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {
            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                animator.SetBool("Attack", true); // Cambiar al estado "Attack"
                Debug.Log("Pasa a atacar");
            }
            else
            {
                // Solo cuando no está a una distancia menor de 10 metros
                script.UltimaPosicion_Jugador = script.Jugador.transform.position;
                animator.SetBool("Search", true); // Cambiar al estado "Search"
            }
        }
        else
        {
            // Solo cuando no está a una distancia menor de 10 metros
            script.UltimaPosicion_Jugador = script.Jugador.transform.position;
            animator.SetBool("Search", true); // Cambiar al estado "Search"
        }
    }





public void medirDistancia(Animator animator)
    {
        script = animator.gameObject.GetComponent<Agent>();
        // Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        // Variable "dist" para ver la distancia que hay entre el personaje y el jugador
        float dist = Vector3.Distance(script.Jugador.transform.position, aget.transform.position);

        if (dist < 10)
        {
            // Pasar a "Attack" cuando el jugador está a una distancia menor que 10 metros
            animator.SetBool("Attack", true);
            Debug.Log("Pasa a atacar");
        }
        else
        {
            // Solo cuando no está a una distancia menor de 10 metros
            script.UltimaPosicion_Jugador = script.Jugador.transform.position;
            animator.SetBool("Search", true); // Cambiar al estado "Search"
        }
    }
}
