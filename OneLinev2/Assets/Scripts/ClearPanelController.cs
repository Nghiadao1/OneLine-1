using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanelController : MonoBehaviour {
    public GameObject challengeComplete;
    public GameObject levelComplete;
    public GameObject challengeFailed;

    public Text difficultyText;
    public Text levelText;

    private void Start()
    {
        challengeComplete.SetActive(false);

        SetButtons(challengeComplete);

        challengeFailed.SetActive(false);

        SetButtons(challengeFailed);

        levelComplete.SetActive(false);

        SetButtons(levelComplete);


    }

    void SetButtons(GameObject set)
    {
        for (int i = 0; i < set.transform.childCount; i++)
        {
            if (set.transform.GetChild(i).name == "OK" || set.transform.GetChild(i).name == "Home")
            {
                set.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(GameManager.GetInstance().ReturnToMenu);
            }
        }
    }

    public void ChallengeComplete()
    {
        challengeComplete.SetActive(true);
    }

    public void ChallengeFailed()
    {
        challengeFailed.SetActive(true);
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
