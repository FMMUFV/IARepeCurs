using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float raycas;
    public List<Transform> ListaWaypoints;
    public GameObject Jugador;
    public Vector3 UltimaPosicion_Jugador;

    public bool Ataca = true, Warrior = false, Agritado = false;

    public float currentTime;

    public int Vidas2 = 1;

   
}
