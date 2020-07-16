using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates and instantiates each level button of a difficulty
/// Marks with a padlock the blocked ones and sets active the unlocked ones
/// </summary>
public class LevelSelectionMenu : MonoBehaviour
{
    public Canvas _cnv;                     // Canvas of the LevelSelectionMenu scene

    public GameObject _levelTilePrefab;     // Prefab of the level tile to use like buttons
    public RectTransform _buttonZone;       // Empty game object to set the buttons zone
    public RectTransform _column;           // Rectangle to keep and set the colum of buttons
    public RectTransform _raws;             // Rectangle to keep and set the raws of buttons to clone them in the colum

    int _currentButton;                     // Number of current level buttons  
    int _completedLevels = 1;               // Number of completed levels, initializes with 1, later with the player data
    int _numButtons = 5;                    // Number of buttons per raw
    int _topLimit = 5;                      // Distance of the top limit
    int _bottomLimit = 10;                  // Distance of the bottom limit
    int _spacingRaws = 12;                  // Distance between raws
    LevelReader _lr;                        // Access to the LevelReader to keep the difficulty information

    /// <summary>
    /// Sets and instantiate all the variables 
    /// </summary>
    public void Start()
    {
        GameManager.GetInstance().SetCanvas(_cnv);
        GameManager.GetInstance().CreateTextLevelSelectionMenu();

        int diff;

        // Gets the necesary information for instantiate buttons
        diff = GameManager.GetInstance().getDifficulty();

        // Gets the player's completed levels in this difficulty
        _completedLevels = GameManager.GetInstance().getCompletedLevelsInDifficulty(diff);

        // Instantiate values
        _lr = new LevelReader(diff);

        int rawsNumber = _lr.GetNumLevels() / _numButtons;

        float origHeight = _buttonZone.rect.height;

        float newHeight = (_raws.rect.height * rawsNumber) + ((_spacingRaws + _topLimit + _bottomLimit) * rawsNumber);

        _buttonZone.sizeDelta = new Vector2(_buttonZone.rect.width, newHeight);

        float posY = _buttonZone.position.y;

        float testing = (posY * newHeight) / origHeight;

        newHeight = (newHeight / GameManager.GetInstance().GetScaling().UnityUds()) / 2;
        origHeight = (origHeight / GameManager.GetInstance().GetScaling().UnityUds()) / 2;

        // Sets the position and rotation to the button zone
        _buttonZone.SetPositionAndRotation(new Vector3(_buttonZone.position.x, (posY + (origHeight - newHeight))), _buttonZone.rotation);

        // Instantiate raws
        InstantiateRaws(rawsNumber);
    }

    /// <summary>
    /// Instantiate each raw with buttons considering the buttons zone and the top and bottom limits
    /// </summary>
    /// <param name="num">Number of raws per colum</param>
    public void InstantiateRaws(int num)
    {
        GameObject r;
        _currentButton = 1;

        // Sets the limits and space between raws
        _column.GetComponent<VerticalLayoutGroup>().padding.top = _topLimit;
        _column.GetComponent<VerticalLayoutGroup>().padding.bottom = _bottomLimit;

        _column.GetComponent<VerticalLayoutGroup>().spacing = _spacingRaws;

        // For each raw to the maximun number of raws instantiates n buttons
        for (int i = 0; i < num; i++)
        {
            r = Instantiate(_raws.gameObject, _column.transform);

            InstantiateButtons(r);
        }
    }

    /// <summary>
    /// Sets the raws size and the spacing between them to instantiate the buttons inside
    /// </summary>
    /// <param name="raw">Empty game object that keeps the buttons per raw</param>
    public void InstantiateButtons(GameObject raw)
    {
        GameObject temp;

        raw.gameObject.GetComponent<HorizontalLayoutGroup>().padding.left = 12;
        raw.gameObject.GetComponent<HorizontalLayoutGroup>().padding.right = 12;

        raw.gameObject.GetComponent<HorizontalLayoutGroup>().spacing = 17;
        raw.gameObject.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;

        // For each space for buttons in a raw instantiate one of them and sets all them need
        for (int i = 0; i < _numButtons; i++)
        {
            temp = Instantiate(_levelTilePrefab, raw.transform);

            SetButton(temp);
        }
    }

    /// <summary>
    /// Checks if the level buttons correspond to a completed or not completed level to activate it or not
    /// </summary>
    /// <param name="bt">Level button</param>
    public void SetButton(GameObject bt)
    {
        bt.GetComponent<LevelSelectionButton>().SetLevel(_currentButton);

        // If the button that is instantiating is lower or equal to the competed level
        if (_currentButton <= _completedLevels)
        {
            // Sets this level button active and interactable
            bt.GetComponent<LevelSelectionButton>().SetInteractable(true);
        }
        else
        {
            // If the button is higher to the completed level, sets it non active and not interactiable
            bt.GetComponent<LevelSelectionButton>().SetInteractable(false);
        }

        // If there are more levels increase the actual button value
        if (_currentButton <= _lr.GetNumLevels())
        {
            _currentButton++;
        }
    }
}
