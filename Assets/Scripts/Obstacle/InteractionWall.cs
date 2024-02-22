using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class InteractionWall : InteractionBase
{

    protected override void Start()
    {
        base.Start();
        btn.onClick.AddListener(OnClick);
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
