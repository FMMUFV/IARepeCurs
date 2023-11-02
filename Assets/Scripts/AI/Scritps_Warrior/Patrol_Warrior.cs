using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol_Warrior : StateMachineBehaviour
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
    public GameObject Jugador;

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
       

    }
   
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        patrulla(animator);
        EnPuente(animator);
        Rayo(animator);

        Health ScriptVida = animator.gameObject.GetComponent<Health>();

        if (ScriptVida.PasarEstun == true)
        {
            ScriptVida.PasarEstun = false;
            animator.SetBool("Stunned", true);
        }

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
                Debug.Log("pisando");
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
        raycas = scritc.raycas;
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

                // Si el raycast golpea a un objeto con etiqueta "Player", establece el estado 'Pursue' en el animator
                Jugador = hit.transform.gameObject;

                scritc.Jugador = hit.transform.gameObject; // Asigna el objeto golpeado al atributo 'Jugador' en 'scritc'

              

                    animator.SetBool("Patrol", false);
                    animator.SetBool("Pursue", true); // Establece el par�metro booleano 'Pursue' en 'true' en el animator

                
            }

        }

    }


}
