using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages the function of the buttons from the Level
/// Selection. 
/// 
/// Set the Level that will be loaded in the Level Scene. 
/// </summary>
public class LevelSelectionButton : MonoBehaviour
{
    // Level to be loaded in the GameScene
    int _level;

    /// <summary>
    /// Adds the TaskOnClick function to the Button component Listener.
    /// </summary>
    private void Start()
    {
        // This GameObject Button component
        Button bt = this.GetComponent<Button>();

        // Adds the listener function
        bt.onClick.AddListener(TaskOnClick);
    }

    /// <summary>
    /// Tells the GameManager to initialize the Level Scene with the Level
    /// configured in this button. 
    /// </summary>
    public void TaskOnClick()
    {
        GameManager.GetInstance().InitLevel(_level);
    }

    /// <summary>
    /// Sets the information of the Button. The level that will be loaded 
    /// and the Text that will be shown in the Button. 
    /// </summary>
    /// <param name="l">Level to be loaded number</param>
    public void SetLevel(int l)
    {
        // Save the Level
        _level = l;

        // Search for the Text component and assign the level text
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.name == "Text")
            {
                // Assign the text
                child.GetComponent<Text>().text = _level.ToString("000");
            }
        }
    }

    /// <summary>
    /// Manages if the text and star have to be shown or not, deppending 
    /// if the button is interactable or not. 
    /// </summary>
    public void TextManagement()
    {
        if (this.gameObject.GetComponent<Button>().interactable)
        {
            // Activate the children
            foreach (Transform child in this.gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            // Deactivate the children 
            foreach (Transform child in this.gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Change the interactable status of this button. This will be used to controll
    /// if the children are active or not and if the Button is interactable. The sprite
    /// changes because it's assigned in the editor.
    /// </summary>
    /// <param name="dis">Value to be set for the button</param>
    public void SetInteractable(bool interactable)
    {
        this.gameObject.GetComponent<Button>().interactable = interactable;

        // Activate the images or not and set the text
        TextManagement();
    }
}
