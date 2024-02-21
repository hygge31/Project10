using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionObj : MonoBehaviour
{
    public Button button;


    private void Awake()
    {
        button.onClick.AddListener(Interaction);
    }

    public void Interaction()
    {
        gameObject.SetActive(false);

        Debug.Log("wall");
    }
}
