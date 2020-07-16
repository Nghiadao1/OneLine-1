using UnityEngine;

/// <summary>
/// Class that manages the gameplat and the Game Scene. 
/// 
/// It's in charge to load the different configurations of the scene 
/// for the different type of gamemodes. 
/// 
/// Manages the flow of the game, when to end it, when to change to 
/// the next one and to load the actual level. 
/// 
/// This also communicates with the BoardManager, which manages the creation
/// of the board and checks if the level is ended or not. 
/// 
/// Checks if the player can buy a hint or not and notifies the BoardManager. 
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Variables
    //Public
    // Important objects of the scene to set the GameManager
    [Header("Scene Information")]
    public Canvas _canvas;                                           // Canvas of the GameScene
    public Camera _camera;                                           // Camera of the GameScene
    public SpriteRenderer _backGround;                               // Background of the game, for scaling it
    public BoardManager _boardManager;                               // BoardManager for Communication

    // Canvas info 
    [Header("Canvas Information")]
    public GameObject[] _levelInterface = new GameObject[2];         // NormalLevel Interfaces
    public GameObject[] _challengeInterface = new GameObject[2];     // ChallengeLevel Interfaces
    public ClearPanelController _clearPanelController;               // Clear panel for managing ending window

    // Private
    // Touch feedback
    GameObject _touchFB;

    // Consulted in the GameManager 
    int _level = 0;                                                  // Level to display
    int _difficulty = 0;                                             // Difficulty to search the level in
    int _coins;                                                      // coins to show in Screen
    
    // Flag to control if the actual level is a challenge or not
    bool _challenge = false;                                         // Flag for GameMode

    // Game status
    bool _ended = false;                                             // For stopping processing input

    // To load different levels
    LevelReader _lr;
    #endregion

    #region StartUpLevelManager
    /// <summary>
    /// Start of the LevelManager scripts. 
    /// 
    /// Initializes all variables retrieving their value from the GameManager. 
    /// 
    /// Sets up the different interfaces depending on the GameMode, communicating 
    /// with the InterfacesControllers. Also sets the different win and losing 
    /// screens. 
    /// 
    /// Sets up the BoardManager and prepares the whole level to be played. 
    /// </summary>
    private void Start()
    {
        // Initialize the variables of the GameManager for playing and calculating space
        GameManager.GetInstance().setLevelManager(this);
        GameManager.GetInstance().SetCanvas(_canvas);
        GameManager.GetInstance().SetCamera(_camera);
        GameManager.GetInstance().ReloadPanels();

        // Get the coins the player already has
        _coins = GameManager.GetInstance().getPlayerCoins();

        // Configurate the canvas information and different panels
        ConfigCanvas();

        // Set the level to be played and it's difficulty
        _level = GameManager.GetInstance().getLevel();
        _difficulty = GameManager.GetInstance().getDifficulty();

        // Check if the level is a challenge or not
        _challenge = GameManager.GetInstance().getChallenge();

        // Scale the background image to fit the screen
        Vector3 result = GameManager.GetInstance().GetScaling().ScaleToFitScreen(_backGround.sprite.bounds.size, _backGround.transform.localScale);
        _backGround.transform.localScale = result;

        // Load the difficulty and the level needed
        LoadLevels(_difficulty);

        // Set the panels and parts of the canvas that will be used in this game
        SetCanvas();

        // Generate a color for the whole level randomly
        int color = Random.Range(1, 8);

        // Create the touching feedback object 
        _touchFB = Instantiate(GameManager.GetInstance().getPrefabFromTouchAssetBundle("block_0" + color + "_touch"));
        _touchFB.SetActive(false);

        // Initialize the BoardManager with the new information
        _boardManager.Init(_lr.GetLevel(_level), color);
    }

    /// <summary>
    /// Loads the whole file with the levels of the Difficulty defined in the Start
    /// and received from the GameManager. 
    /// </summary>
    /// <param name="difficulty">Difficulty number to load the file</param>
    public void LoadLevels(int difficulty)
    {
        // Load the levels for lating accessing them 
        _lr = new LevelReader(difficulty);
    }

    /// <summary>
    /// Configurates the whole canvas, deactivating the different panels and 
    /// interfaces for activating them after, when the game is playing. 
    /// </summary>
    public void ConfigCanvas()
    {
        // Check if all the interfaces are inactive for activating them later
        if (_levelInterface != null && _challengeInterface != null)
        {
            for (int i = 0; i < _levelInterface.Length; i++)
            {
                _levelInterface[i].SetActive(false);
            }

            for (int i = 0; i < _challengeInterface.Length; i++)
            {
                _challengeInterface[i].SetActive(false);
            }
        }
        // Log an error if something goes wrong
        else
        {
            Debug.LogError("Game Interfaces not assigned correctly! Here: " + this.gameObject);
        }
    }

    /// <summary>
    /// Searches in all the interfaces the LevelManager has for a specific one and
    /// returns it for setting it's information or consulting something from them.
    /// </summary>
    /// <param name="t">Interface type needed</param>
    /// <returns>InterfaceType in the gamescene</returns>
    public LevelInterfaceController GetInterfacePart(InterfaceType t)
    {
        // Check if the actual level is a challenge or not to retrieve another interface
        if (_challenge)
        {
            for (int i = 0; i < _challengeInterface.Length; i++)
            {
                if (_challengeInterface[i].GetComponent<LevelInterfaceController>().getType() == t)
                {
                    return _challengeInterface[i].GetComponent<LevelInterfaceController>();
                }
            }
        }
        // Retrieve the normal level Interfaces
        else
        {
            for (int i = 0; i < _levelInterface.Length; i++)
            {
                if (_levelInterface[i].GetComponent<LevelInterfaceController>().getType() == t)
                {
                    return _levelInterface[i].GetComponent<LevelInterfaceController>();
                }
            }
        }

        // If no Interface is found, return null
        return null;
    }

    /// <summary>
    /// Set the canvas depending on GameMode and player information.
    /// </summary>
    public void SetCanvas()
    {
        // For updating texts in the Interface
        LevelInterfaceController lic;

        // Normal level canvas settings
        if (!_challenge)
        {
            // Set the top panel with the corresponding texts
            if ((lic = GetInterfacePart(InterfaceType.NormalSuperior)) != null)
            {
                lic.SetLevelSuperior(_difficulty, _level, _coins);
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.NormalSuperior);
            }
            // Set the hint price in the buy hint button
            if ((lic = GetInterfacePart(InterfaceType.NormalInferior)) != null)
            {
                lic.SetLevelInferior(GameManager.GetInstance().getPrice());
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.NormalInferior);
            }
        }
        // Set the challenge configurations
        else
        {
            // Set the top interface for the challenge
            if ((lic = GetInterfacePart(InterfaceType.ChallengeSuperior)) != null)
            {
                lic.SetChallengeSuperior();
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.ChallengeSuperior);
            }

            // Set bottom timer and interface
            if ((lic = GetInterfacePart(InterfaceType.ChallengeInferior)) != null)
            {
                lic.SetChallengeInferior();
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.ChallengeInferior);
            }
        }
    }
    #endregion

    #region Gameplay Management
    /// <summary>
    /// Function called when the player wants to receive a hint to complete the 
    /// level. 
    /// 
    /// First checks if the hint can be bought (if there are hints left for showing
    /// to the player). Then, if the player has enough money to buy a hint, notifies
    /// the responsible managers that a hint has been given. 
    /// </summary>
    public void HintRequested()
    {
        // This variable is for updating the coins
        LevelInterfaceController lic;

        // Check if the player can buy a hint
        if (_boardManager.CanBuyHint())
        {
            // Check if player has enough money to buy a hint
            if (_coins >= GameManager.GetInstance().getPrice())
            {
                // Retrieve the price of the hint
                _coins -= GameManager.GetInstance().getPrice();

                // Update GameManagers coins
                GameManager.GetInstance().CoinsUsed();

                // Update the coins text shown in the interface
                if ((lic = GetInterfacePart(InterfaceType.NormalSuperior)) != null)
                {
                    lic.ChangeCoins(_coins);
                }
                else
                {
                    Debug.LogError("Interface parts missing: " + InterfaceType.NormalSuperior);
                }

                // Notify BoardManager to show the hint
                _boardManager.HintGiven();
            }
        }
    }

    /// <summary>
    /// Function called by the ad rewarded button. Informs the GameManager to 
    /// play an ad and reward it with coins if the add succeded to show to the
    /// player. 
    /// </summary>
    public void AdRequested()
    {
        GameManager.GetInstance().AdRewardCoins();
    }

    /// <summary>
    /// Updates the coins handled by the player on screen. 
    /// </summary>
    public void UpdateCoins()
    {
        // LevelInterface for updating the coins 
        LevelInterfaceController lic;

        // New coins value
        _coins = GameManager.GetInstance().getPlayerCoins();

        // Update Interface
        if ((lic = GetInterfacePart(InterfaceType.NormalSuperior)) != null)
        {
            lic.ChangeCoins(_coins);
        }
        else
        {
            Debug.LogError("Interface parts missing: " + InterfaceType.NormalSuperior);
        }
    }

    /// <summary>
    /// Function called by the button of reseting level. Notifies
    /// the BoardManager to delete the path that the player has created and
    /// set the level like at the beginning. 
    /// </summary>
    public void ReloadLevel()
    {
        _boardManager.ResetLevel();
    }

    /// <summary>
    /// Receives when the screen has been released, to check if the player has completed
    /// the level or not. 
    /// Makes the touching feedback dissapear.
    /// </summary>
    public void ScreenReleased()
    {
        // Check if the level has ended to stop the input processing
        if (!_ended)
        {
            // Touching feedback GO
            _touchFB.SetActive(false);

            // Check if the player has completed the level
            if (_boardManager.Ended())
            {
                // Manage the level ending
                _ended = true;
                EndGame();
            }
        }
    }

    /// <summary>
    /// Receives the input position and notificates the BoardManager. 
    /// Places the touching feedback GO.
    /// </summary>
    /// <param name="position">Position of the input in unity units</param>
    public void ScreenTouched(Vector2 position)
    {
        // Check if the game Ended to stop processing input
        if (!_ended)
        {
            // Place the touching feedback in screen
            _touchFB.transform.SetPositionAndRotation(position, Quaternion.identity);

            // Check if the touch feedback GO is active and activate it
            if (!_touchFB.active)
            {
                _touchFB.SetActive(true);
            }

            // Tell the Boardmanager the input position
            _boardManager.Touched(position);
        }
    }

    /// <summary>
    /// Ending game function that manages the ending of a level. 
    /// 
    /// If the level was a normal level, show the complete panel and give the 
    /// option to continue to the next level or going back to the main menu.
    /// 
    /// If the level was a challenge, check if the level is complete and show 
    /// the different panels if the player succeded or not. 
    /// 
    /// Tell the GameManager that the level is ended. 
    /// </summary>
    public void EndGame()
    {
        _ended = true;
        // Activate the panel corresponding for the different gamemodes
        if (!_challenge)
        {
            // Activate LevelClear
            _clearPanelController.LevelComplete();

            // Set the information
            _clearPanelController.SetDifficultyText(GameManager.GetInstance().getDifficultyText());
            _clearPanelController.SetLevelNumber(_level);

            // Tell GameManager that level ended
            GameManager.GetInstance().LevelCompleted();
        }
        else
        {
            //Check if the player completed the challenge
            if (_boardManager.Ended())
            {
                // If so, show winning panel
                _clearPanelController.ChallengeComplete();
                GetInterfacePart(InterfaceType.ChallengeInferior).ChallengeCompleted();
                GameManager.GetInstance().ChallengeWin();
            }
            else
            {
                // If not, the time's up, show losing panel
                _clearPanelController.ChallengeFailed();
                GameManager.GetInstance().ChallengeFailed();
            }
        }
    }
    #endregion
}
