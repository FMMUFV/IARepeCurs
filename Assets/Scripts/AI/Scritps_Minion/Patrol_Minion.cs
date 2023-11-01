

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Patrol_Minion : StateMachineBehaviour
{
    private Transform Destino;//Direccion a la que tiene que ir
    private int siguientePos;
    private int NumeDelaLista;

    private int PuenteMask;//Paraque afecte cuando esta en el puente
    public float VelocidadIni;


    public List<Transform> ListaWaypoints;

    private Agent scritc;

    private bool EnlaArena = false;

     RaycastHit hit;//rayo
    public float raycas;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //Asignar componencte scrit para obtener sus componentes del scrtip
        scritc = animator.gameObject.GetComponent<Agent>();
        ListaWaypoints = scritc.ListaWaypoints;

        //-----Puente recojer velocidad inicial------
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        VelocidadIni = aget.speed;

        //Asignar el primer destino
        Destino = ListaWaypoints[0];
        raycas = scritc.raycas;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        patrulla(animator);
        EnPuente(animator);

        Rayo(animator);

    }





    //Puente O pasarela
    public void EnPuente(Animator animator)
    {


        PuenteMask = 1 << NavMesh.GetAreaFromName("Scaffold");
        NavMeshHit hit;
        if (NavMesh.SamplePosition(animator.transform.position, out hit, 2.0f, PuenteMask))
        {


            if (EnlaArena == true) //Para que le cambie la velocidad solo una vez cuando este la arena
            {
                //NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
                NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
                aget.speed = VelocidadIni / 2;
                EnlaArena = false;
             
            }



        }
        else
        {
            NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
            aget.speed = VelocidadIni;
            EnlaArena = true;
        }
    }


    public void patrulla(Animator animator)
    {
        //Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        //Variable Dist para ver la distancia que hay de su destino
        float dist = Vector3.Distance(Destino.position, aget.transform.position);
        //La dimension que tiene la lista de Waypoints
        NumeDelaLista = ListaWaypoints.Count;

        //if (dist < 1 && siguientePos < NumeDelaLista)
        if (dist < 1)
        {


            if (siguientePos >= (NumeDelaLista - 1))
            {
                siguientePos = 0;
            }
            else
            {
                siguientePos++;
            }
            Destino = ListaWaypoints[siguientePos];
            // aget.destination = Destino.position; 

        }
        aget.destination = Destino.position;

    }

        public void Rayo(Animator animator)
        {
       
        Vector3 rayDirection =  animator.transform.forward;

        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.green);

        if(Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {
  
        // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
               Debug.Log("pursue");
               animator.SetBool("Pursue", true);
               scritc.Jugador = hit.transform.gameObject;
            }

        }


        }

}

