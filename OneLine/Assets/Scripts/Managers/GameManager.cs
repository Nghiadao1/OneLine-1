using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables

    // Públicas
    public Canvas cnv;

    public Camera cam;

    public SpriteRenderer fondo;

    public bool challenge = false;

    // Privadas
    Vector2 scalingReferenceResolution;

    RectTransform panelSuperior;
    RectTransform panelInferior;

    Scaling scalator;

    #endregion

    #region StartUpGameManager
    /// <summary>
    /// Variable que establece el singleton del GameManager.
    /// </summary>
    private static GameManager instance; 

    private void Awake()
    {
        // Si no se ha inicializado el GameManager en ningún momento, lo crea e inicializa
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            // Nos aseguramos que el canvas tenga la resolución de referencia correcta
            scalingReferenceResolution = cnv.GetComponent<CanvasScaler>().referenceResolution;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #endregion

    #region Getters
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
