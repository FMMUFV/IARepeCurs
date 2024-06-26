using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mage : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform Destino;//Direccion a la que tiene que ir
    private int siguientePos;
    private int NumeDelaLista;

    private int PuenteMask;//Paraque afecte cuando esta en el puente
    public float VelocidadIni;


    public List<Transform> ListaWaypoints;

    void Start()
    {
        //iniciar();

    }

    // Update is called once per frame
    void Update()
    {


        //patrulla();

        //EnPuente();
    }


    private bool EnlaArena = false;

    //Puente O pasarela
    public void EnPuente()
    {


        PuenteMask = 1 << NavMesh.GetAreaFromName("Scaffold");
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, PuenteMask))
        {
           

            if (EnlaArena == true) //Para que le cambie la velocidad solo una vez cuando este la arena
            {
                //NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
                NavMeshAgent aget = GetComponent<NavMeshAgent>();
                aget.speed = VelocidadIni / 2;
                EnlaArena = false;
                Debug.Log("pisando");
            }



        }
        else
        {
            NavMeshAgent aget = GetComponent<NavMeshAgent>();
            aget.speed = VelocidadIni;
            EnlaArena = true;
        }
    }


    public void patrulla()
    {
        //Inicializar y crear variable aget
        NavMeshAgent aget = GetComponent<NavMeshAgent>();
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


    public void iniciar()
    {
        //-----Puente recojer velocidad inicial------
        NavMeshAgent aget = GetComponent<NavMeshAgent>();
        VelocidadIni = aget.speed;

        //Asignar el primer destino
        Destino = ListaWaypoints[0];
    }


}
