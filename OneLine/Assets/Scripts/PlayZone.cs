using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Esta clase definirá la zona de juego. En ella se establecerán las diferentes configuraciones 
/// de tableros en función de los tiles de juego que haya. 
/// 
/// Es un rectángulo lógico (no se renderiza, pero traduce sus posiciones a unidades de Unity) 
/// que se reescala en función del espacio que tenga en pantalla y la resolución de la misma. 
/// </summary>
public class PlayZone : MonoBehaviour
{
    private struct Dimensions
    {
        public int width;
        public int height;
    }

    public GameManager gm;
    public int marginTop;
    public int marginLeft;

    Dimensions dims; 

    // Start is called before the first frame update
    void Start()
    {
        SetDimensions();
    }

    void SetDimensions()
    {
        int refHeight;
        int refWidth;

        gm.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
