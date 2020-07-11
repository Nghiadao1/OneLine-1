using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanelController : MonoBehaviour
{
    public GameObject challengeComplete;
    public GameObject levelComplete;

    public Text difficultyText;
    public Text levelText;

    private void Start()
    {
        challengeComplete.SetActive(false);
        levelComplete.SetActive(false);
    }

    public void ChallengeComplete()
    {
        challengeComplete.SetActive(true);
    }

    public void LevelComplete()
    {
        levelComplete.SetActive(true);
    }

    public void SetDifficultyText(string diff)
    {
        difficultyText.text = diff;
    }

    public void SetLevelNumber(int num)
    {
        levelText.text = num.ToString();
    }

    public void Reset()
    {
        challengeComplete.SetActive(false);
        levelComplete.SetActive(false);
    }
}
