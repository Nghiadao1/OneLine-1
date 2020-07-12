using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AssignOnClick : MonoBehaviour
{
    public UnityAction actionToBeAdded;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(actionToBeAdded);
    }
}
