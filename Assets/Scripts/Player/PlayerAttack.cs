using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isAttack;
    PlayerAnimationController playerAnimationController;

    private void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && isAttack)
        {
            UIManager.Instance.interactionUI.OpenUi();
            AttackCancel();
        }

    }






    public void ReadyForAttack()
    {
        UIManager.Instance.infoText.text = "공격 \n 마우스 오른쪽 : 공격 취소";
        playerAnimationController.animator.SetBool("isAttack", true);
        isAttack = true;
    }
    public void AttackCancel()
    {
        UIManager.Instance.interactionUI.OpenUi();
        playerAnimationController.animator.SetBool("isAttack", false);
        isAttack = false;
        
    }
}
