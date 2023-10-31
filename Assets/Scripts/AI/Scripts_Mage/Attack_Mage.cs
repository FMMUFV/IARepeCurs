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
    // OnStateEnter se llama cuando se inicia una transición y se evalúa este estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>(); // Acceder al componente "Agent" del objeto controlado por el Animator

      
      

        raycas = 10; // Configurar la longitud del rayo de colisión
    }

    // OnStateUpdate se llama en cada cuadro entre las llamadas de OnStateEnter y OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Realizar el raycast
        Rayo(animator);

        // Si el jugador está a menos de 10 metros, lanzar un hechizo y no moverse
        if (EstaDiedMetros == true)
        {
            LanzaHechizo(animator);
        }
        else
        {
            // Si el jugador se aleja a más de 10 metros, cambiar al estado "Pursue"
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
                aget.stoppingDistance = 10; // Establecer la distancia de detención al jugador
                EstaDiedMetros = true; // El jugador está a menos de 10 metros, permitir el ataque
                Debug.Log("Sigue atacando");
            }
            else
            {
                aget.stoppingDistance = 0; // Restablecer la distancia de detención a 0 (no se mueve hacia el jugador)
                EstaDiedMetros = false; // El jugador está a más de 10 metros, no permitir el ataque
                Debug.Log("Pasa a Pursue");
                animator.SetBool("Attack", false); // Cambiar al estado "Pursue"
            }
        }
        else
        {
            aget.stoppingDistance = 0; // Restablecer la distancia de detención a 0 (no se mueve hacia el jugador)
            EstaDiedMetros = false; // El jugador está a más de 10 metros, no permitir el ataque
            Debug.Log("Pasa a Pursue");
            animator.SetBool("Attack", false); // Cambiar al estado "Pursue"
        }
    }

    public void LanzaHechizo(Animator animator)
    {
        if (script.Ataca == true)
        {
            script.Jugador.SendMessage("Damage", 1); // Infligir daño al jugador
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
