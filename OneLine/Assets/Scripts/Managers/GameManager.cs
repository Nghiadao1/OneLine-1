using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    // Públicas

    [Header("Debugging")]
    public bool debugGame;
    
    [Header("ImportantObjects")]
    public Canvas cnv;
    public Camera cam;

    [Header("FondoGame")]
    public SpriteRenderer fondo;

    public bool challenge = false;

    // Privadas
    Vector2 scalingReferenceResolution;

    RectTransform panelSuperior;
    RectTransform panelInferior;

    Scaling scalator;

    Vector2 lastTouchPosition;

    AssetBundle skins;

    LevelManager lm;
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

            skins = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles/skins"));

            if(skins == null)
            {
                Debug.Log("Failed to load AssetBundle!");
            }

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
        if (SceneManager.GetActiveScene().buildIndex == 2 || debugGame != true)
        {
            Vector3 result = scalator.ScaleToFitScreen(fondo.sprite.bounds.size, fondo.transform.localScale);
            fondo.transform.localScale = result;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScreenTouched(Vector2 touchPosition)
    {

    }

    public void ScreenTouchedAndDrag(Vector2 touchPosition)
    {


        lastTouchPosition = touchPosition;
    }

    public void ScreenReleased(Vector2 touchPosition)
    {

    }

    #endregion

    #region Setters

    public void setLevelManager(LevelManager man)
    {
        instance.lm = man;
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
    
    public AssetBundle getSkins()
    {
        return skins;
    }

    #endregion
}
