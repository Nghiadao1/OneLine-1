using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameManager gm; 

    public Text[] texts;

    int[] completed;
    
    // Start is called before the first frame update
    void Start()
    {
        completed = gm.GetCompletedLevels();

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = completed[i] + "/100";
        }
    }
}
