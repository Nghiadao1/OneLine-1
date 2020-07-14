using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas _mainMenuCanvas;
    public Camera _mainCamera;

    public Text _playerCoins;

    public Text _beginnerCompletedLevels;
    public Text _regularCompletedLevels;
    public Text _advancedCompletedLevels;
    public Text _expertCompletedLevels;
    public Text _masterCompletedLevels;
    public Text _challengesCompleted;

    public GameObject _challengePanel;
    public Text _challengeTime;
    public Text _rewardText;
    public Text _challengeCost;

    public Button _challenge;
    public GameObject _challengeBlocked;
    public Text _challengeTimeLeft;

    // 
    [Header("Time waiting in minutes")]
    public float _timeChallengeWait = 30.0f;
    float _timeReset;
    bool _challengeWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetInstance().SetCanvas(_mainMenuCanvas);
        GameManager.GetInstance().SetCamera(_mainCamera);

        _timeChallengeWait *= 60.0f;

        _timeReset = _timeChallengeWait;

        _challengeWaiting = GameManager.GetInstance().getChallengeCompleted();

        InitTexts();

        _challengePanel.SetActive(false);
        _challengeBlocked.SetActive(false);

        if (_challengeWaiting || GameManager.GetInstance().getTimeRemaining() > 0.0f)
        {
            if (!_challengeWaiting)
            {
                _timeChallengeWait = GameManager.GetInstance().getTimeRemaining();
                _challengeWaiting = true;
            }

            GameManager.GetInstance().SetChallengeTimeRemaining((int)_timeChallengeWait);
            _challengeBlocked.SetActive(true);
            _challenge.interactable = false;
        }
        else
        {
            _challengeBlocked.SetActive(false);
        }
    }

    void InitTexts()
    {
        _playerCoins.text = GameManager.GetInstance().getPlayerCoins().ToString();

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

        _challengeTime.text = GameManager.GetInstance().getChallengeTime().ToString() + "s";
        _rewardText.text = "+" + GameManager.GetInstance().getChallengeReward().ToString();
        _challengeCost.text = GameManager.GetInstance().getChallengePrice().ToString();
    }

    private void Update()
    {
        if (_challengeWaiting)
        {
            _timeChallengeWait -= Time.deltaTime;

            string minutes = Mathf.Floor(_timeChallengeWait / 60).ToString("00");
            string seconds = Mathf.RoundToInt(_timeChallengeWait % 60).ToString("00");

            _challengeTimeLeft.text = minutes + ":" + seconds;

            if(_timeChallengeWait <= 0)
            {
                _timeChallengeWait = _timeReset;
                _challenge.interactable = true;
                _challengeBlocked.SetActive(false);
                _challengeWaiting = false;

                GameManager.GetInstance().SetChallengeWaiting(false);
            }
        }
    }

    public void InitChallenge()
    {
        _challengePanel.SetActive(true);
    }

    public void ChallengeCompleted()
    {
        _challengeWaiting = true;
        _challengeBlocked.SetActive(true);
        _challenge.interactable = false;
    }

    public void CloseChallengePanel()
    {
        _challengePanel.SetActive(false);
    }

}
