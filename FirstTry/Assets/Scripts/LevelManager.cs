using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BoardManager bm;
    public GameObject background;
    public CanvasManager canvas;

    Vector2 _refResolution;       //Reference resolution
    float _cameraWidth;               // Available size
    float _cameraHeight;

    Escalate escale;

    string title;
    int actualLevel;

    // Start is called before the first frame update
    void Start()
    {
        _refResolution = canvas.GetReferenceResolution();

        _cameraHeight = Camera.main.orthographicSize * 2;
        _cameraWidth = _cameraHeight * Screen.width / Screen.height;

        Sprite bgSprite = background.GetComponent<SpriteRenderer>().sprite;

        bm.SetBoard(new Vector2(canvas.GetWidth(), canvas.GetHeight()), new Vector2(_cameraWidth, _cameraHeight));

        escale = new Escalate(_cameraWidth, _cameraHeight, _refResolution);

        background.transform.localScale = escale.EscaleToCamWidth(bgSprite.rect.width, bgSprite.rect.height, background.transform.localScale);

        canvas.SetCoinsNum(GameManager.instance.GetCoins());

        actualLevel = GameManager.instance.GetActualLevel() + 1;

        title = GameManager.instance.GetCurrentDifficultyText() + " " + actualLevel;
        canvas.SetTitleText(title);
        title = GameManager.instance.GetCurrentDifficultyText() + "\n" + actualLevel;
        canvas.SetEndText(title);
    }

    // Update is called once per frame
    void Update()
    {
        canvas.SetCoinsNum(GameManager.instance.GetCoins());
    }

    public void RestartLevel()
    {
        bm.ResetLevel();
    }

    public bool CheckCompleted()
    {
        if (bm.Ended())
        {
            canvas.LevelCompleted();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Hint()
    {
        if(GameManager.instance.GetCoins() >= 25)
        {
            bm.HintGiven();
        }
    }
}
