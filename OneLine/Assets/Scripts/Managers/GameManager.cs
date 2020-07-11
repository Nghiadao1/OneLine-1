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

            if(skins == null)
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
                panelSuperior = child.GetComponent<RectTransform>();
            }
            else if (child.name == "Inferior")
            {
                panelInferior = child.GetComponent<RectTransform>();
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

    public void ResetLevel()
    {
        lm.ReloadLevel();
    }

    public int AdRewarded()
    {
        // Llamar al AdManager para que haga cosas

        return 100;
    }
    #endregion

    #region SceneManagement

    public void ReturnToLastScene()
    {
        // Comprobar si estamos en un nivel para 

        SceneManager.LoadScene(_lastScene);
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
        instance.lm = man;
    }

    public void CoinsUsed()
    {
        coins -= hintPrice;
    }

    public void SetCanvas(Canvas can)
    {
        cnv = can;
    }

    public void SetCamera(Camera c)
    {
        cam = c;
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

    public int getPrice()
    {
        return hintPrice;
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
