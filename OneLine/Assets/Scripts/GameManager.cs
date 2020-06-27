using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables

    public Canvas cnv;

    public Camera cam;

    public SpriteRenderer fondo;

    Image panelSuperior;
    Image panelInferior;

    Scaling scalator;

    #endregion

    #region StartUpGameManager
    /// <summary>
    /// Variable que establece el singleton del GameManager.
    /// </summary>
    static GameManager instance; 

    private void Awake()
    {
        // Si no se ha inicializado el GameManager en ningún momento, lo crea e inicializa
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            // Aquí iría la inicialización de los datos del jugador
            scalator = new Scaling(new int[] { Screen.width, Screen.height }, (int)cam.orthographicSize);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Da acceso al resto de objetos y clases a la información del GameManager.
    /// </summary>
    /// <returns></returns>
    public GameManager GetInstance()
    {
        return instance;
    }
    #endregion

    #region GameManagement



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(scalator.UnityUds());

        Debug.Log(fondo.sprite.bounds.size);

        Vector3 result = scalator.ScaleToScreen(fondo.sprite.bounds.size, fondo.transform.localScale);

        Debug.Log(result);

        fondo.transform.localScale = result;

        // Buscamos los paneles para luego realizar los cálculos
        foreach (Transform child in cnv.transform)
        {
            if(child.name == "Superior")
            {
                panelSuperior = child.GetComponent<Image>();
            }
            else if (child.name == "Inferior")
            {
                panelInferior = child.GetComponent<Image>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int panelSuperiorHeight(){
        return (int)panelSuperior.rectTransform.rect.height;
    }

    public int panelInferiorHeight()
    {
        return (int)panelInferior.rectTransform.rect.height;
    }

    #endregion
}
