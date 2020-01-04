using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Auí hay ue meter lo de los anunsios para los dineros
// Tambien la lectura de niveles y el paso de escenas
public class GameManager : MonoBehaviour
{
    // Welcome to the GameManager script, enjoy the visit and left some comments below. 

    int coins;

    #region SingletonInstance
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        coins = 0;
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins(int sum)
    {
        Debug.Log("LO ACTUALISAMOS");
        coins += sum;
    }

    public int GetCoins()
    {
        Debug.Log(coins);
        return coins;
    }

    public void ChangeScene()
    {
        Scene temp = SceneManager.GetActiveScene();

        Debug.Log("NOS VAMOS");

        SceneManager.LoadScene(1);

        Debug.Log(coins);
    }

    public void Hint()
    {
        Debug.Log("TE DAMOS UNA PISTAAAAAAAAA");

        coins -= 25;
    }
}
