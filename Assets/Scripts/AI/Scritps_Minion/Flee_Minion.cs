using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee_Minion : StateMachineBehaviour
{

    private Agent script;
    public List<Transform> ListaWaypoints;
    private Transform Destino;//Direccion a la que tiene que ir

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();
        ListaWaypoints = script.ListaWaypoints;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        //Variable Dist para ver la distancia que hay de su destino
        float dist = Vector3.Distance(aget.transform.position, script.Jugador.transform.position);
        if(dist > 15)
        {
            animator.SetBool("Flee", false);
        }
        else
        {
            HuirDelJugador(animator);
        }

        

    }


    public void HuirDelJugador(Animator animator)
    {
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        // aget.destination = script.Jugador.transform.position + new Vector3(0, 10, 0);

        // Obtiene una referencia al componente NavMeshAgent en el mismo objeto que el 'animator'
        // aget es una abreviatura de NavMeshAgent para facilitar su uso en el c�digo

        //----------------------Buscamos el punto de patrulla m�s alejado del jugador y se lo asignamos al agente enemigo

        int ContadorArray = 0; // Variable para almacenar el �ndice del punto de patrulla m�s alejado
        float distancia = 0; // Variable para almacenar la distancia m�s larga encontrada

        // Itera a trav�s de una lista de puntos de patrulla (ListaWaypoints)
        for (int i = 0; i < ListaWaypoints.Count; i++)
        {
            // Calcula la distancia entre el punto de patrulla actual y la posici�n del jugador
            float dist = Vector3.Distance(ListaWaypoints[i].position, script.Jugador.transform.position);

            // Comprueba si es la primera iteraci�n o si la distancia actual es mayor que la distancia anterior
            if (i == 0 || (distancia < dist))
            {
                // Si es la primera iteraci�n o la distancia actual es mayor, actualiza el �ndice y la distancia m�xima
                ContadorArray = i;
                distancia = dist;
            }
        }

        // Asigna el destino del agente enemigo (NavMeshAgent) al punto de patrulla m�s alejado
        aget.destination = ListaWaypoints[ContadorArray].transform.position;


    }
}