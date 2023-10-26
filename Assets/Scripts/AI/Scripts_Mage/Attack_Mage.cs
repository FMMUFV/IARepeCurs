using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Attack_Mage : StateMachineBehaviour
{
    private Agent script;

    public bool ataca;


    //Tiempo del ataque
    public float countdownTime = 10.0f; // Tiempo en segundos para la cuenta regresiva
    private float currentTime;


    RaycastHit hit;//rayo
    public float raycas;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script = animator.gameObject.GetComponent<Agent>();

        //Es para la cuenta atras del ataque
        currentTime = countdownTime;

        raycas = 10;
        ataca = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

       // ataque(animator);

        Rayo(animator);

    }

 
    public void ataque(Animator animator)
    {
        //Inicializar y crear variable aget
        NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();
        //Variable Dist para ver la distancia que hay de su destino
        float dist = Vector3.Distance(script.Jugador.transform.position, aget.transform.position);

        if (dist < 10)
        {
            //Ataca si esta a menos de 10
            //Lanza un hechizo y no se mueve

            aget.stoppingDistance = 10;

            LanzaHechizo(animator);
        }
        else
        {
            //Solo cuando no esta a una distancia menor de 10 metros
            aget.stoppingDistance = 0;
            animator.SetBool("Attack", false);


        }
    }

    public void Rayo(Animator animator)
    {

        Vector3 rayDirection = animator.transform.forward;

        Debug.DrawRay(animator.transform.position + Vector3.up, animator.transform.forward * raycas, Color.red);

        if (Physics.Raycast(animator.transform.position + Vector3.up, animator.transform.forward, out hit, raycas))
        {
            NavMeshAgent aget = animator.GetComponent<NavMeshAgent>();

            // Detectar al jugador
            if (hit.transform.gameObject.tag == "Player")
            {
                aget.stoppingDistance = 10;
                LanzaHechizo(animator);
                Debug.Log("Sigue atacando");
            }
            else
            {
                //Solo cuando no esta a una distancia menor de 10 metros
                aget.stoppingDistance = 0;
                animator.SetBool("Attack", false);
                Debug.Log("pasa a purse");
            }

        }
    }

    //-------------------------------
    //PROBLEMA ARREGLAR
    //
    //SE PARA EL CONTADOR CUADO DEJA DE TOCAR EL RAYO HAY QUE HACER QUE EL CONTADOR SIGA POR SU CUENTA


    //Aqui hace daño al jugador
    public void LanzaHechizo(Animator animator)
    {



        //hacion de atacar
        if(ataca == true)
        {
            script.Jugador.SendMessage("Damage", 1);
            Debug.Log("Lanza un hechizo");
            ataca = false;
        }

        else
        {
            //cuenta atras del ataque
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                Debug.Log("Tiempo restante: " + currentTime.ToString("0"));
            }
            else
            {
                currentTime = 0;
                ataca = true;
                currentTime = countdownTime;
            }
        }

        


    }


}
