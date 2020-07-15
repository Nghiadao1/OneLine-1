using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager class. Manages all changes between scenes, the levels that will be
/// played and the challenges. 
/// 
/// Has all the information about the player: how many coins they have, levels completed
/// per difficulty and medals gained in challenges. 
/// 
/// Serializes this information and stores it in a file. 
/// 
/// Has the instance of the different AssetBundles. All the scripts will access this instance
/// to retrieve this information. 
/// </summary>
public class GameManager : MonoBehaviour {

    #region Variables
    // Públicas    
    [Header("ImportantObjects")]
    public Canvas cnv;
    public Camera cam;

    [Header("Game Configuration")]
    public string[] difficulties;
    public int[] _levelsInDifficulty;
    public int hintPrice;
    public int _coinsMaxRewardAd = 30;
    public int _challengeTime = 30;
    public int _challengeReward = 50;
    public int _challengePrice = 25;

    // Privadas
    bool challenge = false;
    int difficulty = 0;
    int level = 0;
    Vector2 scalingReferenceResolution;

    RectTransform panelSuperior;
    RectTransform panelInferior;

    Scaling scalator;

    Vector2 lastTouchPosition;

    LevelManager lm;

    Random rnd;

    int _lastScene;

    int _maxDifficulty = 4;

    bool _challengeCompleted = false;

    LoadAssetBundle lab;
    PlayerData currentPlayerData;
    GameInfo _gi;

    bool adRewardCoins = false;
    bool adDoubleChallenge = false;
    bool adChallengeInit = false;
    #endregion

    #region Utilities

    int ConvertDateToSecond()
    {
        int totalHours = 0;

        totalHours += System.DateTime.Now.Second;
        totalHours += System.DateTime.Now.Minute * 60;
        totalHours += System.DateTime.Now.Hour * 60 * 60;
        totalHours += System.DateTime.Now.Day * 24 * 60 * 60;
        totalHours += System.DateTime.Now.Month * 30 * 24 * 60 * 60;

        return 0;
    }

    #endregion

    #region StartUpGameManager
    /// <summary>
    /// Variable que establece el singleton del GameManager.
    /// </summary>
    private static GameManager instance;

    private void Awake()
    {
        // Si no se ha inicializado el GameManager en ningún momento, lo crea e inicializa
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            // Nos aseguramos que el canvas tenga la resolución de referencia correcta
            scalingReferenceResolution = cnv.GetComponent<CanvasScaler>().referenceResolution;

            // Aquí iría la inicialización de los datos del jugador
            scalator = new Scaling(new Vector2(Screen.width, Screen.height), scalingReferenceResolution, (int)cam.orthographicSize);

            // Buscamos los paneles para luego realizar los cálculos
            ReloadPanels();

            lab = new LoadAssetBundle();

#if !UNITY_EDITOR && UNITY_ANDROID
            lab.LoadBundlesAndroid(Application.streamingAssetsPath + "/AssetBundles/");
#else
            lab.LoadBundlesWindows(Application.streamingAssetsPath + "/AssetBundles/");
#endif

            rnd = new Random();

            _gi = LoadingFiles.ReadGameInfo();

            SetGameInfo();

            // GetPlayer information
            currentPlayerData = LoadingFiles.ReadPlayerData(_maxDifficulty);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetGameInfo()
    {
        int difficulties = GetInstance()._gi._numDifficulties;

        GetInstance()._maxDifficulty = difficulties;

        _levelsInDifficulty = new int[GetInstance()._maxDifficulty];

        for (int i = 0; i < _levelsInDifficulty.Length; i++)
        {
            LevelReader temp = new LevelReader(i);
            _levelsInDifficulty[i] = temp.GetNumLevels();
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GetInstance().difficulty = Random.Range(0, _maxDifficulty);
            LevelReader temp = new LevelReader(GetInstance().difficulty);
            GetInstance().level = Random.Range(1, temp.GetNumLevels() + 1);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GetInstance().difficulty = Random.Range(0, _maxDifficulty);
        }
    }

    public void ReloadPanels()
    {
        foreach (Transform child in cnv.transform)
        {
            if (child.name == "Top")
            {
                GetInstance().panelSuperior = child.GetComponent<RectTransform>();
            }
            else if (child.name == "Bottom")
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
    public void ScreenTouched(Vector2 touchPosition)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            // Comprobar si el click ha sido dentro de la zona de juego
            if (IsInPlayZone(touchPosition))
            {
                // Si es así, informar al level manager
                touchPosition = GetInstance().scalator.ScreenToWorldPosition(touchPosition);

                GetInstance().lm.ScreenTouched(touchPosition);
            }
        }
    }

    public void ScreenReleased()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
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
        if (GetInstance().level == GetInstance().currentPlayerData._completedLevelsInDifficulty[GetInstance().difficulty])
        {
            GetInstance().currentPlayerData._completedLevelsInDifficulty[GetInstance().difficulty] += 1;
        }
    }

    public void ChallengeCompleted()
    {
        GetInstance().challenge = false;
    }

    public void TimeIsUp()
    {
        GetInstance().lm.EndGame();
    }

    public void ResetLevel()
    {
        GetInstance().lm.ReloadLevel();
    }

    void AddCoins()
    {
        int add = Random.Range(10, GetInstance()._coinsMaxRewardAd + 1);
        GetInstance().currentPlayerData._coinsPlayer += add;
    }

    void AddChallengeExtraCoins()
    {
        GetInstance().currentPlayerData._coinsPlayer += GetInstance()._challengeReward;
        ReturnToMenu();
    }

    public void AdRewardCoins()
    {
        GetInstance().adRewardCoins = true;
        AdManager.GetInstance().ShowRewardedVideo();
    }

    public void AdRewardDoubleBounty()
    {
        GetInstance().adDoubleChallenge = true;
        AdManager.GetInstance().ShowRewardedVideo();
    }

    public void AdRewardInitChallenge()
    {
        GetInstance().adChallengeInit = true;
        AdManager.GetInstance().ShowRewardedVideo();
    }

    public void AdEnded()
    {
        if (GetInstance().adRewardCoins)
        {
            AddCoins();
        }
        else if (GetInstance().adDoubleChallenge)
        {
            AddChallengeExtraCoins();
        }
        else if (GetInstance().adChallengeInit)
        {
            InitChallenge();
        }
    }
    #endregion

    #region LevelSelectionManagement

    // Primero hay que gestionar la creación del menú
    public void CreateTextLevelSelectionMenu()
    {
        foreach (Transform t in panelSuperior.transform)
        {
            if (t.name == "Difficulty")
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
        if (GetInstance().challenge)
        {
            GetInstance().challenge = false;
        }
        SceneManager.LoadScene(GetInstance()._lastScene);
    }

    public void ReturnToMenu()
    {
        if (ConvertDateToSecond() > GetInstance().currentPlayerData._dateForNextChallenge)
        {
            GetInstance().currentPlayerData._timeForNextChallenge = 0;
        }

        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        GetInstance().level += 1;

        if (GetInstance().level > 100)
        {
            GetInstance().level = 1;

            GetInstance().difficulty += 1;
            if (GetInstance().difficulty > GetInstance()._maxDifficulty)
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

    public void ChangeToLevelSelection(int diff)
    {
        GetInstance().difficulty = diff;

        GetInstance()._lastScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(1);
    }

    public void PaidChallenge()
    {
        GetInstance().currentPlayerData._coinsPlayer -= GetInstance()._challengePrice;

        InitChallenge();
    }

    public void InitChallenge()
    {
        GetInstance()._lastScene = SceneManager.GetActiveScene().buildIndex;
        GetInstance().challenge = true;
        GetInstance().difficulty = Random.Range(0, GetInstance()._maxDifficulty);
        GetInstance().level = Random.Range(1, GetInstance()._levelsInDifficulty[GetInstance().difficulty]);

        SceneManager.LoadScene(2);
    }

    public void ChallengeFailed()
    {
        GetInstance().challenge = false;
        //GetInstance()._challengeCompleted = true; DESCOMENTAR ESTO POR DIOS
    }

    public void ChallengeWin()
    {
        GetInstance().challenge = false;
        GetInstance().currentPlayerData._coinsPlayer += GetInstance()._challengeReward;
        GetInstance().currentPlayerData._challengesCompleted += 1;

        //GetInstance()._challengeCompleted = true;
    }
    #endregion

    #region Setters
    public void setLevelManager(LevelManager man)
    {
        GetInstance().lm = man;
    }

    public void CoinsUsed()
    {
        GetInstance().currentPlayerData._coinsPlayer -= GetInstance().hintPrice;
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

    public void SetChallengeTimeRemaining(int timing)
    {
        GetInstance().currentPlayerData._timeForNextChallenge = timing;
        GetInstance().currentPlayerData._dateForNextChallenge = GetInstance().ConvertDateToSecond() + timing;
    }

    public void SetChallengeWaiting(bool challengeWaiting)
    {
        GetInstance()._challengeCompleted = challengeWaiting;
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
        return GetInstance().difficulties[difficulty];
    }

    // COMENTAAAAAAR
    public Vector2 getReferenceResolution()
    {
        return GetInstance().scalingReferenceResolution;
    }

    public float panelSuperiorHeight()
    {
        return GetInstance().panelSuperior.rect.height;
    }

    public float panelInferiorHeight()
    {
        return GetInstance().panelInferior.rect.height;
    }

    public int getDifficulty()
    {
        return GetInstance().difficulty;
    }

    public int getLevelsInDifficulty(int diff)
    {
        if (difficulty < _levelsInDifficulty.Length)
        {
            return GetInstance()._levelsInDifficulty[diff];
        }
        else
        {
            Debug.LogError("Difficulty level not defined");
            return 0;
        }
    }

    public int getLevel()
    {
        return GetInstance().level;
    }

    public int getPrice()
    {
        return GetInstance().hintPrice;
    }

    // Info and prefab getters

    public int getNumPathSkins()
    {
        return GetInstance()._gi._numPathSkins;
    }

    public int getNumTouchSkins()
    {
        return GetInstance()._gi._numTouchSkins;
    }

    public int getNumTileSkins()
    {
        return GetInstance()._gi._numTileSkins;
    }

    public GameObject getPrefabFromTileAssetBundle(string name)
    {
        return GetInstance().lab.getTileSkins().LoadAsset<GameObject>(name);
    }

    public GameObject getPrefabFromPathAssetBundle(string name)
    {
        return GetInstance().lab.getPathSkins().LoadAsset<GameObject>(name);
    }

    public GameObject getPrefabFromTouchAssetBundle(string name)
    {
        return GetInstance().lab.getTouchSkins().LoadAsset<GameObject>(name);
    }

    // Challenge
    public bool getChallenge()
    {
        return GetInstance().challenge;
    }

    public int getChallengeTime()
    {
        return GetInstance()._challengeTime;
    }

    public int getChallengeReward()
    {
        return GetInstance()._challengeReward;
    }

    public int getChallengePrice()
    {
        return GetInstance()._challengePrice;
    }

    public float getTimeRemaining()
    {
        return GetInstance().currentPlayerData._timeForNextChallenge;
    }

    public bool getChallengeCompleted()
    {
        return GetInstance()._challengeCompleted;
    }
    // Player stats getters
    public int getPlayerCoins()
    {
        return GetInstance().currentPlayerData._coinsPlayer;
    }

    public int getCompletedLevelsInDifficulty(int difficulty)
    {
        if (difficulty < GetInstance().currentPlayerData._completedLevelsInDifficulty.Length)
        {
            return GetInstance().currentPlayerData._completedLevelsInDifficulty[difficulty];
        }
        else
        {
            Debug.LogError("Difficulty level not defined");
            return 0;
        }
    }

    public int getChallengesCompleted()
    {
        return GetInstance().currentPlayerData._challengesCompleted;
    }
    #endregion

    #region ApplicationLifeManagement
    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        LoadingFiles.SavePlayerData(currentPlayerData);
    }
    #endregion
}
