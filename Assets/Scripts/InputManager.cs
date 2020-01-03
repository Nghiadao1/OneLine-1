using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    GameManager gm;

    void Update()
    {
        CheckInput();
    } // Update

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
            //Sacar la pos en coordenadas del mundo
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos2D = new Vector2(pos.x, pos.y);

            RaycastHit2D ray = Physics2D.Raycast(pos2D, Vector2.zero);
            if (ray)
            {
                if (ray.collider.gameObject.GetComponent<Tile>())
                {
                    ray.collider.gameObject.GetComponent<Tile>().OnClick();
                }
            }
        } // if
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 posDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        } // else if
        else if (Input.GetMouseButtonUp(0))
        {

        } // else if
#endif
    } // CheckInput



    // touchcount nos dice el nº de toques en la pantalla
    // No podemos fiarnos de que el primer dedo esté siempre en la posición cero
    // fingerId ID del dedo
    // touchSupported si soporta los touches
    // mousePresent si hay un mouse o no

}
