using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages the showing of tha clearing panels after 
/// the player completes a level or a challenge. 
/// </summary>
public class ClearPanelController : MonoBehaviour {

    // The different panels to activate
    public GameObject challengeComplete;        // Challenge complete panel
    public GameObject levelComplete;            // Level complete panel
    public GameObject challengeFailed;          // Challenge failed panel

    // Text to show in the LevelComplete clear panel
    public Text difficultyText;                 // Difficulty text
    public Text levelText;                      // Level

    /// <summary>
    /// Makes all panels inactive to activate them when required.
    /// </summary>
    private void Start()
    {
        // Make challenge complete panel inactive
        challengeComplete.SetActive(false);
        SetChallengeComplete(challengeComplete);
        SetButtons(challengeComplete);

        // Make challenge failed panel inactive
        challengeFailed.SetActive(false);
        SetButtons(challengeFailed);

        // Make level complete panel inactive
        levelComplete.SetActive(false);
        SetButtons(levelComplete);
    }

    /// <summary>
    /// Sets the value of the challenge reward. Gets the object that 
    /// has the text to be changed and then changes it's value.
    /// </summary>
    /// <param name="cc">GameObject to set values</param>
    void SetChallengeComplete(GameObject cc)
    {
        for (int i = 0; i < cc.transform.childCount; i++)
        {
            if(cc.transform.GetChild(i).name == "Coin")
            {
                for (int j = 0; j < cc.transform.GetChild(i).childCount; j++)
                {
                    if(cc.transform.GetChild(i).GetChild(j).name == "Reward")
                    {
                        cc.transform.GetChild(i).GetChild(j).GetComponent<Text>().text = "+" + GameManager.GetInstance().getChallengeReward();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets the buttons to go back to Main Menu after completing a level or a challenge.
    /// </summary>
    /// <param name="set">Panel to set it's buttons</param>
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

    /// <summary>
    /// Function called when a challenge is completed successfully. Sets
    /// the panel active. 
    /// </summary>
    public void ChallengeComplete()
    {
        challengeComplete.SetActive(true);
    }

    /// <summary>
    /// Function called when the player fails to complete a challenge. 
    /// Activates the panel.
    /// </summary>
    public void ChallengeFailed()
    {
        challengeFailed.SetActive(true);
    }

    /// <summary>
    /// Function called when the player completes a Level. Sets the panel 
    /// active.
    /// </summary>
    public void LevelComplete()
    {
        levelComplete.SetActive(true);
    }

    /// <summary>
    /// Function called to set the difficulty text level in the level complete
    /// panel. 
    /// </summary>
    /// <param name="diff">Difficulty text</param>
    public void SetDifficultyText(string diff)
    {
        difficultyText.text = diff;
    }

    /// <summary>
    /// Function called to set the level number and show it in the Level 
    /// complete panel. 
    /// </summary>
    /// <param name="num">Level to set the text</param>
    public void SetLevelNumber(int num)
    {
        levelText.text = num.ToString();
    }
}
