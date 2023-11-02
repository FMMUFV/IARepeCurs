using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Attack_Mage : StateMachineBehaviour
{
    private Agent script; // Referencia al componente "Agent" del personaje

    // Tiempo del ataque
    public float countdownTime = 3.0f; // Tiempo en segundos para la cuenta regresiva
   

    RaycastHit hit; // Informaci�n sobre el raycast (rayo de colisi�n)
    public float raycas; // Longitud del rayo de colisi�n

    public bool EstaDiedMetros; // Indicador si el jugador est� a menos de 10 metros


    public float currentTime;
    public bool AtacaDeNuevo;
    float distanciaDeAtaque = 10f;
    // OnStateEnter se llama cuando se inicia una transici�n y se eval�a este estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>(); // Acceder al componente "Agent" del objeto controlado por el Animator


        AtacaDeNuevo = true;
        // Configura el tiempo actual con el valor inicial
        currentTime = countdownTime;
    }

    // OnStateUpdate se llama en cada cuadro entre las llamadas de OnStateEnter y OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Rayo(animator);
     

    }

    public void Rayo(Animator animator)
    {
        script = animator.gameObject.GetComponent<Agent>();
        raycas = script.raycas;
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
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

                float distanciaAlJugador = Vector3.Distance(animator.transform.position, hit.transform.position);

                if (distanciaAlJugador < distanciaDeAtaque)
                {
                    // El jugador est� a una distancia de ataque, as� que ataca
                    aget.stoppingDistance = 10;

                    if (AtacaDeNuevo == true)
                    {
                        script.Jugador.SendMessage("Damage", 1); // Infligir da�o al jugador
                        AtacaDeNuevo = false;
                    }
                    Contados(animator);


                    // Puedes agregar l�gica para ejecutar el ataque aqu�
                }
                else
                {
                    aget.stoppingDistance = 0;
                    animator.SetBool("Attack", false);
                    animator.SetBool("Pursue", true);
                }
            }
             else
             {
                 aget.stoppingDistance = 0;
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
            // La cuenta atr�s ha llegado a cero, puedes agregar acciones aqu�
            AtacaDeNuevo = true;
            currentTime = countdownTime;
            //animator.SetBool("Patrol", true);
            animator.SetTrigger("Patrol_1");
        }
    }





    
}
