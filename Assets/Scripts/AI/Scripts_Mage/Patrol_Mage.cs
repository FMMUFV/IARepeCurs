using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol_Mage : StateMachineBehaviour
{

    private Transform Destino;//Direccion a la que tiene que ir
    private int siguientePos;
    private int NumeDelaLista;

    private int PuenteMask;//Paraque afecte cuando esta en el puente
    public float VelocidadIni;
    private bool EnlaArena = false;


    //Datos del scritp agent
    private Agent script;
    public float raycas;
    public List<Transform> ListaWaypoints;

    RaycastHit hit;//rayo

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //Asignar componencte scrit para obtener sus componentes del scrtip
        script = animator.gameObject.GetComponent<Agent>();
        ListaWaypoints = script.ListaWaypoints;
        raycas = script.raycas;

        //-----Puente recojer velocidad inicial------
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        VelocidadIni = aget.speed;
        aget.isStopped = false;
        //Asignar el primer destino
        Destino = ListaWaypoints[0];

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
        Agent scriptAgent = animator.gameObject.GetComponent<Agent>();
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();





        PuenteMask = 1 << NavMesh.GetAreaFromName("Scaffold");
        NavMeshHit hit;
        if (NavMesh.SamplePosition(animator.transform.position, out hit, 2.0f, PuenteMask))
        {


            if (EnlaArena == true) //Para que le cambie la velocidad solo una vez cuando este la arena
            {
                //NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();

                aget.speed = scriptAgent.velocidadSueloPuente;
                EnlaArena = false;

            }



        }
        else
        {

            aget.speed = scriptAgent.velocidadSueloNormal;

            EnlaArena = true;
        }
    }

    public void patrulla(Animator animator)
    {



        //Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        //Variable Dist para ver la distancia que hay de su destino
        //La dimension que tiene la lista de Waypoints
        NumeDelaLista = ListaWaypoints.Count;





        //Sirve !aget.hasPash  --> Si tiene asignado un destino al que ir 
        // Y aget.remainingDistance < 1 = sirve para comprobar que su distancia desde la maya de navegacion si es menor que 1.
        if (!aget.hasPath || (aget.remainingDistance < 1))
        {
            //--------------------------------------------------

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

            ///----------------Parte nueva comprovar a mas detalle------------------------------
            var path = new NavMeshPath();

            aget.CalculatePath(Destino.position, path);


            if (path.status == NavMeshPathStatus.PathComplete)
            {
                //Cuando esto suceda es que puede llegar a la posicion
                aget.destination = Destino.position;
            }

        }


    }




    public void Rayo(Animator animator)
    {

        Vector3 rayDirection = animator.transform.forward;

        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.red);

        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                animator.SetBool("Patrol", false);
                animator.SetBool("Pursue", true);

                script.Jugador = hit.transform.gameObject;

            }

        }
    }




}
