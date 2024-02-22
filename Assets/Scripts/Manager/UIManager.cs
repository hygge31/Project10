using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public event Action OnCloseUi;

    public TextMeshProUGUI infoText;
    


    [Header("Components")]
    PlayerMovement playerMovement;

    [Header("Interaction UI")]
    public Transform playerInteractionUiTransform;
    public PlayerInteractionUI interactionUI;
    public bool isInteractionUI;

    //public GameObject interactionWallObj;


    [Header("Action UI")]
    public GameObject actionManu;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        OnCloseUi += CloseInteractionUi;

        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !playerMovement.isMoveing && interactionUI.possibleOpenUI)
        {
            interactionUI.transform.position = playerInteractionUiTransform.position;
            
            interactionUI.OpenUi();
        }

    }

    public void CallOnCloseUI()
    {
        OnCloseUi?.Invoke();
    }
    public void CloseInteractionUi()
    {
        infoText.text = "I : 캐릭터 행동 메뉴";
    }


    public void Reset()
    {
        playerMovement.ClearDrawNavMeshPath();
        interactionUI.OpenUi();

    }

}
