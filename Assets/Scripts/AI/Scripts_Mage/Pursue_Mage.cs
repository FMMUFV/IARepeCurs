using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Pursue_Mage : StateMachineBehaviour
{
    private Agent script;
    RaycastHit hit; // Informaci�n sobre el raycast (rayo de colisi�n)
    public float raycas;

    // OnStateEnter se llama cuando se inicia una transici�n y se eval�a este estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Pursue", false);

 
       
    }

    // OnStateUpdate se llama en cada cuadro entre las llamadas de OnStateEnter y OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();

        Rayo(animator);
        EnPuente(animator);
    }

    private  float distanciaDeAtaque = 10f;
    public void Rayo(Animator animator)
    {
        raycas = script.raycas;
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
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
                script.UltimaPosicion_Jugador = hit.transform.gameObject.transform.position;
              

                if (hit.distance < distanciaDeAtaque)
                {
                    // El jugador está a una distancia de ataque, así que ataca

                    aget.isStopped = true;
                    animator.SetBool("Attack", true);
                    // Puedes agregar lógica para ejecutar el ataque aquí
                }
                else
                {
                    // El jugador está fuera de la distancia de ataque, así que persigue

                    // Configura la posición de destino del enemigo al jugador

                    aget.isStopped = false;
                    aget.destination = hit.transform.position;
                }
            }
            else
            {
                
                animator.SetBool("Pursue", false);
                animator.SetBool("Patrol", true);
            }
        }


        
    }



    int PuenteMask;
    private bool EnlaArena = false;
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

            aget.speed = scriptAgent.velocidadSueloNormal;

            EnlaArena = true;
        }
    }



}
