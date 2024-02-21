using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionUI : MonoBehaviour
{
    PlayerMovement playerMovement;
    public Button closeBtn;
    public Button action_1Btn;
    public Button action_2Btn;


    Animator animator;


    private void Awake()
    {
        closeBtn.onClick.AddListener(CloseBtn);
    }

    private void Start()
    {
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        animator = UIManager.Instance.interactionUI.animator;
    }



    void Action_1Btn()
    {
        
    }

    void CloseBtn()
    {
        
        StartCoroutine(Delay());
        
    }

    IEnumerator Delay()
    {
        animator.SetTrigger("action_close");
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.interactionUI.OpenUi();
    }

}
