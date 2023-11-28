

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

    public GameObject Jugador;

    private int vidas;

    Health ScriptVida;
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
        
               ScriptVida = animator.gameObject.GetComponent<Health>();
        vidas = ScriptVida.m_Health;
        patrulla(animator);
        EnPuente(animator);

        Rayo(animator);


        if (ScriptVida.PasarEstun == true)
        {
            ScriptVida.PasarEstun = false;
            animator.SetBool("Patrol", true);
        }
        
    }




    int PuenteMask2;
    private bool EnlaArena2 = false;
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
            
            aget.speed = scriptAgent.velocidadSueloNormal ;

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





        //!aget.hasPath --> si no tiene ruta
        //aget.remainingDistance < 1 --> si su distancia es menor que 1 teniendo encuenta el navMeshAgent
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

                // Si el raycast golpea a un objeto con etiqueta "Player", establece el estado 'Pursue' en el animator
                Jugador = hit.transform.gameObject;

                scritc.Jugador = hit.transform.gameObject; // Asigna el objeto golpeado al atributo 'Jugador' en 'scritc'
                
                if (vidas <= 1)
                {
                    // Debug.Log("una o menos" + scritc.Vidas + "Pasa a Flee" );
                    animator.SetBool("Flee", true);
                }
                else if (vidas > 1)
                {

                    animator.SetBool("Patrol", false);
                    animator.SetBool("Pursue", true); // Establece el parámetro booleano 'Pursue' en 'true' en el animator

                }
            }

        }

    }

}

