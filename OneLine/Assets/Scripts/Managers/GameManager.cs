using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Ordenar este coso
public class GameManager : MonoBehaviour
{
    #region Variables
    // Públicas    
    [Header("ImportantObjects")]
    public Canvas cnv;
    public Camera cam;

    public string[] difficulties;


    [Header("Public only for debugging")]
    public bool challenge = false;
    public int difficulty = 0;
    public int level;

    // Privadas
    Vector2 scalingReferenceResolution;

    RectTransform panelSuperior;
    RectTransform panelInferior;

    Scaling scalator;

    Vector2 lastTouchPosition;

    AssetBundle skins;
    AssetBundle config;

    LevelManager lm;

    int _lastScene;

    public int hintPrice;

    int maxDifficulty = 4;

    // Referentes al jogador
    int coins;
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
            config = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles/config"));

            if (skins == null)
            {
                Debug.Log("Failed to load AssetBundle!");
            }

            // Buscamos los paneles para luego realizar los cálculos
            ReloadPanels();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ReloadPanels()
    {
        foreach (Transform child in cnv.transform)
        {
            if (child.name == "Superior")
            {
                GetInstance().panelSuperior = child.GetComponent<RectTransform>();
            }
            else if (child.name == "Inferior")
            {
                GetInstance().panelInferior = child.GetComponent<RectTransform>();
            }
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
            touchPosition = GetInstance().scalator.ScreenToWorldPosition(touchPosition);

            GetInstance().lm.ScreenTouched(touchPosition);
        }
    }

    public void ScreenReleased()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
            GetInstance().lm.ScreenReleased();
    }

    public bool IsInPlayZone(Vector2 position)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
            return position.y < (GetInstance().scalator.CurrentResolution().y - panelSuperiorHeight() * GetInstance().cnv.scaleFactor) && position.y > (panelInferiorHeight() * GetInstance().cnv.scaleFactor);
        else
            return false;
    }

    public void LevelCompleted()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetLevel()
    {
        GetInstance().lm.ReloadLevel();
    }

    public int AdRewarded()
    {
        // Llamar al AdManager para que haga cosas

        return 100;
    }
    #endregion

    #region LevelSelectionManagement

    // Primero hay que gestionar la creación del menú
    public void CreateTextLevelSelectionMenu()
    {
        foreach  (Transform t in panelSuperior.transform)
        {
            if(t.name == "Difficulty")
            {
                t.GetComponent<Text>().text = getDifficultyText();
            }
        }
    }

    #endregion

    #region SceneManagement

    public void ReturnToLastScene()
    {
        // Comprobar si estamos en un nivel para 

        SceneManager.LoadScene(GetInstance()._lastScene);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        GetInstance().level += 1;

        if(GetInstance().level > 100)
        {
            GetInstance().level = 1;

            GetInstance().difficulty += 1;
            if(GetInstance().difficulty > GetInstance().maxDifficulty)
            {
                GetInstance().difficulty = 0;
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void InitLevel(int l)
    {
        GetInstance()._lastScene = SceneManager.GetActiveScene().buildIndex;
        GetInstance().level = l;

        SceneManager.LoadScene(2);
    }

    public void ChangeToLevelSelection(int difficulty)
    {

    }

    public void InitChallenge()
    {

    }

    #endregion

    #region Setters
    public void setLevelManager(LevelManager man)
    {
        GetInstance().lm = man;
    }

    public void CoinsUsed()
    {
        GetInstance().coins -= GetInstance().hintPrice;
    }

    public void SetCanvas(Canvas can)
    {
        GetInstance().cnv = can;

        ReloadPanels();
    }

    public void SetCamera(Camera c)
    {
        GetInstance().cam = c;
    }

    #endregion

    #region Getters
    public Scaling GetScaling()
    {
        return GetInstance().scalator;
    }

    public Canvas GetCanvas()
    {
        return GetInstance().cnv;
    }

    public Vector2 getResolution()
    {
        return new Vector2(Screen.width, Screen.height);
    }

    public string getDifficultyText()
    {
        return difficulties[difficulty];
    }

    // COMENTAAAAAAR
    public Vector2 getReferenceResolution()
    {
        return GetInstance().scalingReferenceResolution;
    }

    public float panelSuperiorHeight(){
        return GetInstance().panelSuperior.rect.height;
    }

    public float panelInferiorHeight()
    {
        return GetInstance().panelInferior.rect.height;
    }
    
    public AssetBundle getSkins()
    {
        return GetInstance().skins;
    }

    public AssetBundle getConfig()
    {
        return GetInstance().config;
    }

    public int getDifficulty()
    {
        return GetInstance().difficulty;
    }

    public int getLevel()
    {
        return GetInstance().level;
    }

    public int getPrice()
    {
        return GetInstance().hintPrice;
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
