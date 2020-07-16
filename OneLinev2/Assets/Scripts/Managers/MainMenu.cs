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
 
    [Header("Time waiting in minutes")]
    public float _timeChallengeWait = 30.0f;    // Number of minutes that needs the challenge timer to allow a new challenge
    float _timeReset;                           // Keep the initial minutes of the challenge timer to reset them when is needed
    bool _challengeWaiting = false;             // If is needed to wait for the next challenge or not

    // Start is called before the first frame update
    /// <summary>
    /// Sets the state of the panels and the challenge timer.
    /// Also calls the initiation of all text of the main menu buttons and values of the player data
    /// </summary>
    void Start()
    {
        GameManager.GetInstance().SetCanvas(_mainMenuCanvas);
        GameManager.GetInstance().SetCamera(_mainCamera);

        // Calculates the time to next challenge in seconds
        _timeChallengeWait *= 60.0f;
        // Sets the reset time value with the initial time to wait for next player
        _timeReset = _timeChallengeWait;

        _challengeWaiting = GameManager.GetInstance().getChallengeCompleted();

        InitTexts();

        // Deactivates the challenge panel and the blocked image of the button
        _challengePanel.SetActive(false);
        _challengeBlocked.SetActive(false);

        // If the timer not finish yet
        if (_challengeWaiting || GameManager.GetInstance().getTimeRemaining() > 0.0f)
        {
            // And not stating waiting
            if (!_challengeWaiting)
            {
                // Gets the seconds left to next challenge and start the waiting boolean
                _timeChallengeWait = GameManager.GetInstance().getTimeRemaining();
                _challengeWaiting = true;
            }

            // Sets the actual seconds to next challenge, activates the blocked image and set the challenge button not interactable
            GameManager.GetInstance().SetChallengeTimeRemaining((int)_timeChallengeWait);
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
        _beginnerCompletedLevels.text = GameManager.GetInstance().getCompletedLevelsInDifficulty(0).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(0).ToString();

        _regularCompletedLevels.text = GameManager.GetInstance().getCompletedLevelsInDifficulty(1).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(1).ToString();

        _advancedCompletedLevels.text = GameManager.GetInstance().getCompletedLevelsInDifficulty(2).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(2).ToString();

        _expertCompletedLevels.text = GameManager.GetInstance().getCompletedLevelsInDifficulty(3).ToString()
            + "/" + GameManager.GetInstance().getLevelsInDifficulty(3).ToString();

        _masterCompletedLevels.text = GameManager.GetInstance().getCompletedLevelsInDifficulty(4).ToString()
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
        // If is waiting time
        if (_challengeWaiting)
        {
            // Calculates the elapsed time since the last frame
            _timeChallengeWait -= Time.deltaTime;

            // Sets the seconds to minutes and second in the 00:00 format
            string minutes = Mathf.Floor(_timeChallengeWait / 60).ToString("00");
            string seconds = Mathf.RoundToInt(_timeChallengeWait % 60).ToString("00");

            _challengeTimeLeft.text = minutes + ":" + seconds;

            // If the timer ends
            if (_timeChallengeWait <= 0)
            {
                // Restart the time information and the original state of the main menu
                // The challenge button is active and not blocked again 
                _timeChallengeWait = _timeReset;
                _challenge.interactable = true;
                _challengeBlocked.SetActive(false);
                _challengeWaiting = false;

                // Stops the waiting time
                GameManager.GetInstance().SetChallengeWaiting(false);
            }
        }
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
