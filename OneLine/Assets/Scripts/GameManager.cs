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

    public Vector2 scalingReferenceResolution = new Vector2(720, 1280);

    RectTransform panelSuperior;
    RectTransform panelInferior;

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

            // Nos aseguramos que el canvas tenga la resolución de referencia correcta
            cnv.GetComponent<CanvasScaler>().referenceResolution = scalingReferenceResolution;

            // Aquí iría la inicialización de los datos del jugador
            scalator = new Scaling(new Vector2 (Screen.width, Screen.height), scalingReferenceResolution, (int)cam.orthographicSize);


            // Buscamos los paneles para luego realizar los cálculos
            foreach (Transform child in cnv.transform)
            {
                if (child.name == "Superior")
                {
                    panelSuperior = child.GetComponent<RectTransform>();
                }
                else if (child.name == "Inferior")
                {
                    panelInferior = child.GetComponent<RectTransform>();
                }
            }
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
    public static GameManager GetInstance()
    {
        return instance;
    }
    #endregion

    #region GameManagement



    // Start is called before the first frame update
    void Start()
    {
        Vector3 result = scalator.ScaleToFitScreen(fondo.sprite.bounds.size, fondo.transform.localScale);

        fondo.transform.localScale = result;


        Debug.Log(panelInferior);
        Debug.Log(panelSuperior);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Scaling GetScaling()
    {
        return scalator;
    }

    public Canvas GetCanvas()
    {
        return cnv;
    }

    public Vector2 getResolution()
    {
        return new Vector2(Screen.width, Screen.height);
    }

    // COMENTAAAAAAR
    public Vector2 getReferenceResolution()
    {
        return scalingReferenceResolution;
    }

    public float panelSuperiorHeight(){
        return panelSuperior.rect.height;
    }

    public float panelInferiorHeight()
    {
        return panelInferior.rect.height;
    }

    #endregion
}
