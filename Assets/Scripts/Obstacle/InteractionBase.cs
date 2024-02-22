using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBase : MonoBehaviour
{
    public GameObject interactionUI;
    public Button btn;

    protected Animator animator;
    protected Transform playerTransform;
    protected bool onInteraction;
    protected bool find;


    public float distance;


    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        playerTransform = GameManager.instance.player.transform;
       
    }


    protected void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < 2f && !onInteraction && !find)
        {
            find = true;
            interactionUI.SetActive(true);
        }
        if (distance > 2f && find)
        {
            find = false;
            interactionUI.SetActive(false);
        }
    }
}
