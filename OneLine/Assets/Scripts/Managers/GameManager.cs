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
    [Header("ImportantObjects")]
    public Canvas cnv;
    public Camera cam;
    
    [Header("Public only for debugging")]
    public bool challenge = false;
    public int difficulty;
    public int level;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScreenTouched(Vector2 touchPosition)
    {
        // Comprobar si el click ha sido dentro de la zona de juego
        if(IsInPlayZone(touchPosition))
        {
            // Si es así, informar al level manager
            touchPosition = scalator.ScreenToWorldPosition(touchPosition);

            lm.ScreenTouched(touchPosition);
        }
    }

    public void ScreenReleased()
    {
        lm.ScreenReleased();
    }

    public bool IsInPlayZone(Vector2 position)
    {
        return position.y < (scalator.CurrentResolution().y - panelSuperiorHeight() * cnv.scaleFactor) && position.y > (panelInferiorHeight() * cnv.scaleFactor);
    }

    public void LevelCompleted()
    {
        SceneManager.LoadScene(0);
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

    public int getDifficulty()
    {
        return difficulty;
    }

    public int getLevel()
    {
        return level;
    }

    #endregion

    #region ApplicationLifeManagement
    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        // Salvar el estado del juego
    }
    #endregion 
}
