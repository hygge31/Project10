using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Button startBtn;


    private void Awake()
    {
        startBtn.onClick.AddListener(() => ActiveStart());
    }


    void ActiveStart()
    {
        SceneManager.LoadScene("Main");
    }
}
