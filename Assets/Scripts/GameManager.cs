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
            Debug.Log("Ya hay otra de tipo gameManager");
        }

     

    }

    public void Start()
    {
        PasarWayponys();
    }
    public List<Transform> ListaWaipontsManager;
    public void PasarWayponys()
    {
        //Pasar del una lista de gameobject por un bucle for para añadirlo en una lista de tipo transfor
        
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
                ;
                agent.ListaWaypoints.Add(ListaWaipontsManager[Random.Range(0, ListaWaipontsManager.Count)]);
            }
            //Aqui se le pasa la lista creada
            //agent.ListaWaypoints = ListaWaipontsManager;
        }
    }



    public void Grito(Vector3 posicion)
    {
        List<GameObject>enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        if (enemigos.Count > 0)
        {
            // Se encontraron uno o más GameObjects con el tag "Enemigo"
            // Puedes acceder a cada uno de ellos en la lista de enemigos
            foreach (GameObject enemigo in enemigos)
            {
                // Realiza acciones con cada GameObject enemigo

                //Sirve para entrar es su en su componete animator y pasarlo al estado Warcry
                Animator animator = enemigo.GetComponent<Animator>();
                Agent agent = enemigo.GetComponent<Agent>();
               
                agent.UltimaPosicion_Jugador = posicion;
                // if(agent.Agritado == true)
               
                animator.SetBool("Warcry", true);
            }
        }
        else
        {
            // No se encontraron GameObjects con el tag "Enemigo"
            // Realiza alguna acción alternativa o muestra un mensaje de error

        }
    }
}
