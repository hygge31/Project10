using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class InteractionWall : MonoBehaviour
{
    public GameObject interactionUI;
    public Button btn;

    Animator animator;
    Transform playerTransform;
    bool onInteraction;
    bool find;


    public float distance;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        playerTransform = GameManager.instance.player.transform;
        btn.onClick.AddListener(OnClick);
    }


    private void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < 2f && !onInteraction && !find)
        {
            find = true;
            interactionUI.SetActive(true);
        }
        if (distance > 2f&& find)
        {
            find = false;
            interactionUI.SetActive(false);
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
        onInteraction = true;
        animator.SetTrigger("open");
        yield return new WaitForSeconds(1f);
        GameManager.instance.line.SetActive(true);
        GameManager.instance.map2.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.instance.Bake();
    }
}
