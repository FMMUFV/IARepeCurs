using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } 

    // Start is called before the first frame update
    
    public Vector3 UltimaPosicion;
    public int llamada = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            //No hay un mas de un mismo gameManager y por lo tanto se le da permiso
            Instance = this;
           
        }

        else if (Instance != this)
        {
            //ya hay mas de un gameobject con el scritp GameManager por lo tanto destruye si Insace es diferente de this
            Destroy(gameObject);
          
        }

     

    }

    public void Start()
    {
        PasarWayponys();
    }
    public List<Transform> ListaWaipontsManager;
    public List<GameObject> ListaDePuntosAltosManager;
    public void PasarWayponys()
    {
        //Pasar del una lista de gameobject por un bucle for para a�adirlo en una lista de tipo transfor
        
        List<GameObject> Waypoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Waypoints"));
        for(int i = 0; i < Waypoints.Count; i++)
        {
            ListaWaipontsManager.Add(Waypoints[i].transform);
        }



        List<GameObject> enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
      foreach(GameObject enemigo in enemigos)
        {
            Agent agent = enemigo.GetComponent<Agent>();


            for(int i = 0; i < 6; i++)
            {
                
                agent.ListaWaypoints.Add(ListaWaipontsManager[Random.Range(0, ListaWaipontsManager.Count)]);
            }
            //Aqui se le pasa la lista creada
            //agent.ListaWaypoints = ListaWaipontsManager;
        }
    }




    public void Grito(Vector3 posicion, GameObject Jugador)
    {
        List<GameObject>enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        if (enemigos.Count > 0)
        {
            // Se encontraron uno o m�s GameObjects con el tag "Enemigo"
            // Puedes acceder a cada uno de ellos en la lista de enemigos
            foreach (GameObject enemigo in enemigos)
            {
                // Realiza acciones con cada GameObject enemigo

                //Sirve para entrar es su en su componete animator y pasarlo al estado Warcry
                Animator animator = enemigo.GetComponent<Animator>();
                Agent agent = enemigo.GetComponent<Agent>();
               
                agent.UltimaPosicion_Jugador = posicion;
                // if(agent.Agritado == true)


                //Aqui se progragama el claculo de la posicion de puntos altos
                if (agent.Archer == true)
                {

                    ListaDePuntosAltosManager = new List<GameObject>(GameObject.FindGameObjectsWithTag("PuntoAlto"));

                  
                    float distancia = Mathf.Infinity;
                    for(int i = 0;i< ListaDePuntosAltosManager.Count; i++)
                    {
                        float dist = Vector3.Distance(agent.UltimaPosicion_Jugador, ListaDePuntosAltosManager[i].transform.position);
                        
                        if(dist < distancia)
                        {
                            agent.Jugador = Jugador;
                            agent.PosicionAlta = ListaDePuntosAltosManager[i].transform.position;
                            distancia = dist;
                        }
                    }

                }


                animator.SetBool("Warcry", true);
                //animator.SetTrigger("Warcry_2");
            }
        }
       
    }
}
