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
    // OnStateEnter se llama cuando se inicia una transici�n y se eval�a este estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>(); // Acceder al componente "Agent" del objeto controlado por el Animator

      
      

        raycas = 10; // Configurar la longitud del rayo de colisi�n
    }

    // OnStateUpdate se llama en cada cuadro entre las llamadas de OnStateEnter y OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Realizar el raycast
        Rayo(animator);

        // Si el jugador est� a menos de 10 metros, lanzar un hechizo y no moverse
        if (EstaDiedMetros == true)
        {
            LanzaHechizo(animator);
        }
        else
        {
            // Si el jugador se aleja a m�s de 10 metros, cambiar al estado "Pursue"
            animator.SetBool("Attack", false);
        }
    }

  

    public void Rayo(Animator animator)
    {
        Vector3 rayDirection = animator.transform.forward;

        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.red);
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                aget.stoppingDistance = 10; // Establecer la distancia de detenci�n al jugador
                EstaDiedMetros = true; // El jugador est� a menos de 10 metros, permitir el ataque
                Debug.Log("Sigue atacando");
            }
            else
            {
                aget.stoppingDistance = 0; // Restablecer la distancia de detenci�n a 0 (no se mueve hacia el jugador)
                EstaDiedMetros = false; // El jugador est� a m�s de 10 metros, no permitir el ataque
                Debug.Log("Pasa a Pursue");
                animator.SetBool("Attack", false); // Cambiar al estado "Pursue"
            }
        }
        else
        {
            aget.stoppingDistance = 0; // Restablecer la distancia de detenci�n a 0 (no se mueve hacia el jugador)
            EstaDiedMetros = false; // El jugador est� a m�s de 10 metros, no permitir el ataque
            Debug.Log("Pasa a Pursue");
            animator.SetBool("Attack", false); // Cambiar al estado "Pursue"
        }
    }

    public void LanzaHechizo(Animator animator)
    {
        if (script.Ataca == true)
        {
            script.Jugador.SendMessage("Damage", 1); // Infligir da�o al jugador
            Debug.Log("Lanza un hechizo");
            script.Ataca = false; // Restablecer la capacidad de ataque
        }
        else
        {
            if (script.currentTime > 0)
            {
                script.currentTime -= Time.deltaTime; // Actualizar el tiempo restante
                Debug.Log("Tiempo restante: " + script.currentTime.ToString("0"));
            }
            else
            {
                script.Ataca = true; // Activar la capacidad de ataque
                script.currentTime = countdownTime; // Restablecer el tiempo de ataque
            }
        }
    }
}
