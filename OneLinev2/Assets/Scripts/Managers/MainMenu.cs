using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages all of the canvas information in the main menu.
/// Sets and upgrade the texts with the player data.
/// Counts and checks the elapsed time to update the timer of the challenge waiting.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public Canvas _mainMenuCanvas;              // One of the canvas of the MainMenu scene, the one with top and bottom panels
    public Camera _mainCamera;                  // Main camera of the MainMenu scene

    public Text _playerCoins;                   // Shows the actual player coins

    public Text _beginnerCompletedLevels;       // Shows the actual completed levels in the beginner difficulty
    public Text _regularCompletedLevels;        // Shows the actual completed levels in the regular difficulty
    public Text _advancedCompletedLevels;       // Shows the actual completed levels in the advanced difficulty
    public Text _expertCompletedLevels;         // Shows the actual completed levels in the expert difficulty
    public Text _masterCompletedLevels;         // Shows the actual completed levels in the master difficulty
    public Text _challengesCompleted;           // Shows the actual completed levels in the challenges

    public GameObject _challengePanel;          // Panel with the challenge information
    public Text _challengeTime;                 // Shows the elapsed time in the timer to play challenges again
    public Text _rewardText;                    // Shows the number of coins given by reward of complete a challenge
    public Text _challengeCost;                 // Shows the coins cost of playing a challenge

    public Button _challenge;                   // Calls the activation of the challenges panel
    public GameObject _challengeBlocked;        // Image that shows the timer to next challenge upon the challenge button
    public Text _challengeTimeLeft;             // Shows the timer to next challenge
    
    bool _challengeWaiting = false;             // If is needed to wait for the next challenge or not

    // Start is called before the first frame update
    /// <summary>
    /// Sets the state of the panels and the challenge timer.
    /// Also calls the initiation of all text of the main menu buttons and values of the player data
    /// </summary>
    void Start()
    {
        GameManager.GetInstance().setMainMenu(this);
        GameManager.GetInstance().SetCanvas(_mainMenuCanvas);
        GameManager.GetInstance().SetCamera(_mainCamera);

        _challengeWaiting = GameManager.GetInstance().getChallengeWaiting();

        InitTexts();

        // Deactivates the challenge panel and the blocked image of the button
        _challengePanel.SetActive(false);
        _challengeBlocked.SetActive(false);

        // If the timer not finish yet
        if (_challengeWaiting)
        {
            _challengeBlocked.SetActive(true);
            _challenge.interactable = false;
        }
    
    }

    /// <summary>
    /// Initializes all the canvas text in MainMenu scene.
    /// Sets the player coins and the player completed levels to show them.
    /// Also sets the values of the time, cost and rewards of playing a challenge
    /// </summary>
    void InitTexts()
    {
        // Player coins
        _playerCoins.text = GameManager.GetInstance().getPlayerCoins().ToString();

        // Completed levels of each difficulty
        _beginnerCompletedLevels.text = (GameManager.GetInstance().getCompletedLevelsInDifficulty(0) - 1).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(0).ToString();

        _regularCompletedLevels.text = (GameManager.GetInstance().getCompletedLevelsInDifficulty(1) - 1).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(1).ToString();

        _advancedCompletedLevels.text = (GameManager.GetInstance().getCompletedLevelsInDifficulty(2) - 1).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(2).ToString();

        _expertCompletedLevels.text = (GameManager.GetInstance().getCompletedLevelsInDifficulty(3) - 1).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(3).ToString();

        _masterCompletedLevels.text = (GameManager.GetInstance().getCompletedLevelsInDifficulty(4) - 1).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(4).ToString();

        _challengesCompleted.text = GameManager.GetInstance().getChallengesCompleted().ToString();

        // Challenge panel texts
        _challengeTime.text = GameManager.GetInstance().getChallengeTime().ToString() + "s";
        _rewardText.text = "+" + GameManager.GetInstance().getChallengeReward().ToString();
        _challengeCost.text = GameManager.GetInstance().getChallengePrice().ToString();
    }

    /// <summary>
    /// Checks and calculate the next challenge time once per frame
    /// </summary>
    private void Update()
    {
        _challengeWaiting = GameManager.GetInstance().getChallengeWaiting();
    }

    public void ChallengeAvailable()
    {
        _challenge.interactable = true;
        _challengeBlocked.SetActive(false);
        _challengeWaiting = false;
    }

    public void UpdateTime(float time)
    {
        // Sets the seconds to minutes and second in the 00:00 format
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = Mathf.RoundToInt(time % 60).ToString("00");

        _challengeTimeLeft.text = minutes + ":" + seconds;
    }

    /// <summary>
    /// Activates the chanel panel
    /// </summary>
    public void InitChallenge()
    {
        _challengePanel.SetActive(true);
    }

    /// <summary>
    /// Sets the waiting time when a challenge is ended.
    /// Actives the blocked image and sets the challenge button not interactable
    /// </summary>
    public void ChallengeCompleted()
    {
        _challengeWaiting = true;
        _challengeBlocked.SetActive(true);
        _challenge.interactable = false;
    }

    /// <summary>
    /// Close the challenge panel when its cross button is pressed and deactivates it
    /// </summary>
    public void CloseChallengePanel()
    {
        _challengePanel.SetActive(false);
    }
}
