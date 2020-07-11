using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    int _level;

    private void Start()
    {
        Button bt = this.GetComponent<Button>();

        bt.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        GameManager.GetInstance().InitLevel(_level);
    }

    public void SetLevel(int l)
    {
        _level = l;

        foreach (Transform child in this.gameObject.transform)
        {
            if(child.name == "Text")
            {
                child.GetComponent<Text>().text = _level.ToString("000");
            }
        }
    }

    public void TextManagement()
    {
        if (this.gameObject.GetComponent<Button>().interactable)
        {
            foreach(Transform child in this.gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in this.gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void SetInteractable(bool dis)
    {
        this.gameObject.GetComponent<Button>().interactable = dis;

        TextManagement();
    }
}
