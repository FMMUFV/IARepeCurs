using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> enemigos;
  

    void Start()
    {

          enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
          if (enemigos.Count > 0)
          {
              // Se encontraron uno o más GameObjects con el tag "Enemigo"
              // Puedes acceder a cada uno de ellos en la lista de enemigos
              foreach (GameObject enemigo in enemigos)
              {
                // Realiza acciones con cada GameObject enemigo
                Agent ScritEnemigo;
                ScritEnemigo = enemigo.GetComponent<Agent>();
                ScritEnemigo.Vidas2 = 100;

              }
          }
          else
          {
              // No se encontraron GameObjects con el tag "Enemigo"
              // Realiza alguna acción alternativa o muestra un mensaje de error

          }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
