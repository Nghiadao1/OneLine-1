using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanelController : MonoBehaviour
{
    public GameObject challengeComplete;
    public GameObject levelComplete;

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

    public void Reset()
    {
        challengeComplete.SetActive(false);
        levelComplete.SetActive(false);
    }
}
