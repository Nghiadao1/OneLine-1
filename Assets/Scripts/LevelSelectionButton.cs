using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    int level;

    public void OnClick()
    {
        Debug.Log("AY QUE YA NOS VAMOS");
        GameManager.instance.ChangeLevelScene(level);
    }

    public void SetLevel(int i)
    {
        level = i;
    }
}
