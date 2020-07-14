using System.Collections;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
/// <summary>
/// Crea e instancia los botones, no mucho más 
/// </summary>
public class LevelSelectionMenu : MonoBehaviour
{
    public Canvas cnv;

    public GameObject levelTilePrefab;
    public RectTransform buttonZone;
    public RectTransform column;
    public RectTransform raws;

    AssetBundle ab;
    int currentButton;
    int completedLevels = 1;
    int numButtons = 5;
    int topLimit = 5;
    int bottomLimit = 10;
    int spacingRaws = 12;
    LevelReader lr;

    public void Start()
    {
        GameManager.GetInstance().SetCanvas(cnv);
        GameManager.GetInstance().CreateTextLevelSelectionMenu();

        int diff;
        // Obtener los datos necesarios para la instancia de botones
        diff = GameManager.GetInstance().getDifficulty();

        completedLevels = GameManager.GetInstance().getCompletedLevelsInDifficulty(diff);

        lr = new LevelReader(diff);
        
        int rawsNumber = lr.GetNumLevels() / numButtons;

        float origHeight = buttonZone.rect.height;

        float newHeight = (raws.rect.height * rawsNumber) + ((spacingRaws + topLimit + bottomLimit) * rawsNumber);

        buttonZone.sizeDelta = new Vector2 (buttonZone.rect.width, newHeight);

        float posY = buttonZone.position.y;

        float testing = (posY * newHeight) / origHeight;

        newHeight = (newHeight / GameManager.GetInstance().GetScaling().UnityUds()) / 2;
        origHeight = (origHeight / GameManager.GetInstance().GetScaling().UnityUds()) / 2;

        buttonZone.SetPositionAndRotation(new Vector3(buttonZone.position.x, (posY + (origHeight - newHeight))), buttonZone.rotation);
        
        // Instanciar líneas
        InstantiateRaws(rawsNumber);

        raws.transform.parent = null;

        raws.gameObject.SetActive(false);
    }

    public void InstantiateRaws(int num)
    {
        GameObject r;
        currentButton = 1;

        column.GetComponent<VerticalLayoutGroup>().padding.top = topLimit;
        column.GetComponent<VerticalLayoutGroup>().padding.bottom = bottomLimit;

        column.GetComponent<VerticalLayoutGroup>().spacing = spacingRaws;

        for (int i = 0; i < num; i++)
        {
            r = Instantiate(raws.gameObject, column.transform);

            InstantiateButtons(r);
        }

        //r = Instantiate(raws.gameObject, column.transform);
    }

    public void InstantiateButtons(GameObject raw)
    {
        GameObject temp;

        raw.gameObject.GetComponent<HorizontalLayoutGroup>().padding.left = 12;
        raw.gameObject.GetComponent<HorizontalLayoutGroup>().padding.right = 12;

        raw.gameObject.GetComponent<HorizontalLayoutGroup>().spacing = 17;
        raw.gameObject.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;

        for (int i = 0; i < numButtons; i++)
        {
            temp = Instantiate(levelTilePrefab, raw.transform);

            SetButton(temp);
        }
    }

    public void SetButton(GameObject bt)
    {
        bt.GetComponent<LevelSelectionButton>().SetLevel(currentButton);

        if (currentButton <= completedLevels)
        {
            bt.GetComponent<LevelSelectionButton>().SetInteractable(true);
        }
        else
        {
            bt.GetComponent<LevelSelectionButton>().SetInteractable(false);
        }

        if (currentButton <= lr.GetNumLevels())
        {
            currentButton++;
        }
    }
    
}
