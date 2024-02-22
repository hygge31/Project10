using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{
    public Animator animator;
    public GameObject manu;
    
    PlayerMovement playerMovement;

    public Button moveBtn;
    public Button attackBtn;
    public Button actionBtn;
    public Button cencelBtn;

    public bool possibleOpenUI = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cencelBtn.onClick.AddListener(CancelBtn);
        moveBtn.onClick.AddListener(MoveBtn);
        actionBtn.onClick.AddListener(ActionUiOpen);
        attackBtn.onClick.AddListener(AttackBtn);
    }


    private void Start()
    {
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
    }


    public void OpenUi()
    {
        animator.SetTrigger("open");
        transform.position = UIManager.Instance.playerInteractionUiTransform.position;
        UIManager.Instance.infoText.text = "행동 선택";
        UIManager.Instance.isInteractionUI = true;
        possibleOpenUI = false;
    }

    public void CloseUi()
    {
        UIManager.Instance.CallOnCloseUI();
        animator.SetTrigger("close");
        UIManager.Instance.isInteractionUI = false;
    }


    void MoveBtn()
    {
        CloseUi();
        possibleOpenUI = false;
        playerMovement.StartDrawNavMeshAgentPath();
    }

    public void CancelBtn()
    {
        UIManager.Instance.CallOnCloseUI();
        animator.SetTrigger("close");
        possibleOpenUI = true;
        UIManager.Instance.isInteractionUI = false;
    }


    void ActionUiOpen()
    {
        animator.SetTrigger("action_open");
        CloseUi();
    }

    void AttackBtn()
    {
        CloseUi();
        GameManager.instance.player.GetComponent<PlayerAttack>().ReadyForAttack();
    }
}
