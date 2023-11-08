using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } 

    // Start is called before the first frame update
    public List<GameObject> enemigos;
    public Vector3 UltimaPosicion;
    public int llamada = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Debug.Log("Somos nosotros");
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Ya hay otra de tipo gameManager");
        }
    }

   

    public void Grito()
    {
        enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        if (enemigos.Count > 0)
        {
            // Se encontraron uno o más GameObjects con el tag "Enemigo"
            // Puedes acceder a cada uno de ellos en la lista de enemigos
            foreach (GameObject enemigo in enemigos)
            {
                // Realiza acciones con cada GameObject enemigo

                //Sirve para entrar es su en su componete animator y pasarlo al estado Warcry
                Animator animator = enemigo.GetComponent<Animator>();
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
