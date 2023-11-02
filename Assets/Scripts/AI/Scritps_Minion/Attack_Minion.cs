using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack_Minion : StateMachineBehaviour
{

    RaycastHit hit;//rayo
    public float raycas;
    private Agent script;
    // Variable para almacenar la distancia de ataque deseada
    float distanciaDeAtaque = 1f;


    public float countdownTime = 3.0f; // Tiempo inicial de la cuenta atrás en segundos
    public float currentTime;

    public bool AtacaDeNuevo;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AtacaDeNuevo = true;
        // Configura el tiempo actual con el valor inicial
        currentTime = countdownTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rayo(animator);
    }

    public void Rayo(Animator animator)
    {
        script = animator.gameObject.GetComponent<Agent>();
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

                float distanciaAlJugador = Vector3.Distance(animator.transform.position, hit.transform.position);

                if (distanciaAlJugador < distanciaDeAtaque)
                {
                    // El jugador está a una distancia de ataque, así que ataca
                    aget.stoppingDistance = 10;
                    
                    if(AtacaDeNuevo == true)
                    {
                       script.Jugador.SendMessage("Damage", 1); // Infligir daño al jugador
                       AtacaDeNuevo = false;
                    }
                    Contados(animator);
                    
                    
                    // Puedes agregar lógica para ejecutar el ataque aquí
                }
                else
                {
                    aget.stoppingDistance = 0;
                    animator.SetBool("Attack", false);
                    animator.SetBool("Pursue", true);
                }
            }
           /* else
            {
                aget.stoppingDistance = 0;
                animator.SetBool("Attack", false);
                animator.SetBool("Pursue", true);
                
            }*/
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
            // La cuenta atrás ha llegado a cero, puedes agregar acciones aquí
            AtacaDeNuevo = true;
            currentTime = countdownTime;
            //animator.SetBool("Patrol", true);
            animator.SetTrigger("Patrol_1");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
