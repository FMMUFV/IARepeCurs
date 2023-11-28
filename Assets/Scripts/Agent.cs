using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float raycas;
    public List<Transform> ListaWaypoints;

    [Header("Información del jugador")]
    public GameObject Jugador;
    public Vector3 UltimaPosicion_Jugador;

    [Space(10)]
    public bool Ataca = true ;
    public bool Warrior = false;
    public bool PuedeGritar = true;
    [Space(10)]
    public float currentTime;

    [Header("Velocidad en distitntos suelos")]

    public float velocidadSueloNormal ;

    public float velocidadSueloPuente;
}
