using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Attack_Mage : StateMachineBehaviour
{
    private Agent script; // Referencia al componente "Agent" del personaje

    // Tiempo del ataque
    public float countdownTime = 3.0f; // Tiempo en segundos para la cuenta regresiva
   

    RaycastHit hit; // Información sobre el raycast (rayo de colisión)
    public float raycas; // Longitud del rayo de colisión

    public bool EstaDiedMetros; // Indicador si el jugador está a menos de 10 metros


    public float currentTime;
    public bool AtacaDeNuevo;
    float distanciaDeAtaque = 10f;

     NavMeshAgent aget;
    // OnStateEnter se llama cuando se inicia una transición y se evalúa este estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>(); // Acceder al componente "Agent" del objeto controlado por el Animator


        AtacaDeNuevo = true;
        // Configura el tiempo actual con el valor inicial
        currentTime = countdownTime;
        aget = animator.GetComponent<NavMeshAgent>();
       
    }

    // OnStateUpdate se llama en cada cuadro entre las llamadas de OnStateEnter y OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Rayo(animator);
     

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        aget.isStopped = false;
    }

    public void Rayo(Animator animator)
    {
        script = animator.gameObject.GetComponent<Agent>();
        raycas = script.raycas;
        
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

              

                if (hit.distance < distanciaDeAtaque)
                {
                    // El jugador está a una distancia de ataque, así que ataca
                    aget.isStopped = true;

                    if (AtacaDeNuevo == true)
                    {
                        script.Jugador.SendMessage("Damage", 1); // Infligir daño al jugador
                        AtacaDeNuevo = false;
                    }
                    Contados(animator);


                    // Puedes agregar lógica para ejecutar el ataque aquí
                }
                else
                {
                    aget.isStopped = false;
                    animator.SetBool("Attack", false);
                    animator.SetBool("Pursue", true);
                }
            }
             else
             {
                 aget.isStopped = false;
                 animator.SetBool("Attack", false);
                 animator.SetBool("Pursue", true);

             }
        }



    }



    public void Contados(Animator animator)
    {


        // Verifica si el tiempo actual es mayor que 0
        if (currentTime > 0)
        {
            // Reduce el tiempo actual en cada fotograma
            currentTime -= Time.deltaTime;

            // Muestra el tiempo actual en la consola (puedes adaptarlo para mostrarlo en una UI)
        }
        else
        {
            AtacaDeNuevo = true;
            currentTime = countdownTime;
          
        }
    }





    
}
