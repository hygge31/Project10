using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class InteractionWall : MonoBehaviour
{
    public GameObject interactionUI;
    public Button btn;
    Transform playerTransform;
    bool onInteraction;
    bool find;


    public float distance;

    private void Start()
    {
        playerTransform = GameManager.instance.player.transform;
        btn.onClick.AddListener(OnClick);
    }


    private void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        if (Vector3.Distance(transform.position, playerTransform.position) < 2f && !onInteraction && !find)
        {
            find = true;
            interactionUI.SetActive(true);
        }
    }



    void OnClick()
    {
        interactionUI.SetActive(false);
        find = true;
        StartCoroutine(ShowMap2());
    }


    IEnumerator ShowMap2()
    {
        gameObject.transform.position = new Vector3(10, 20, 10);
        yield return new WaitForSeconds(1f);
        GameManager.instance.line.SetActive(true);
        GameManager.instance.map2.SetActive(true);
        GameManager.instance.Bake();
    }
}
