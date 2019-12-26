using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    GameManager gm;

    void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {

#if !UNITY_EDITOR && (!UNITY_ANDROID || UNITY_IOS)

    if (el usuario tiene el dedo en la pantalla){
		//Sacar la posicion en pixeles
		ProcessClick(Input.mousePosition);
	}


#else

        if (Input.GetMouseButton(0))
        {
            //Sacar la pos en pixeles
            ProcessClick(Input.mousePosition);

            gm.ScreenClicked(Input.mousePosition.x, Input.mousePosition.y);
           
        }
        else if (Input.GetMouseButtonDown(0))
        {
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ProcessLiberation();
        }

#endif

    }

    void ProcessClick (Vector3 pos)
    {

    }

    void ProcessLiberation ()
    {

    }


    // touchcount nos dice el nº de toques en la pantalla
    // No podemos fiarnos de que el primer dedo esté siempre en la posición cero
    // fingerId ID del dedo
    // touchSupported si soporta los touches
    // mousePresent si hay un mouse o no

}
