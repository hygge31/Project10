using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ActionMaunUI_item : Util
{
    public TextMeshProUGUI text;
    public BuffType buffType;
    float cooltime;
    public Button button;



    private void Awake()
    {
        button.onClick.AddListener(RunCooltime);
    }

    void SetCooltime()
    {
        switch (buffType)
        {
            case BuffType.Search:
                cooltime = StateManager.Instance.cooltimeData.searchBuffCooltime;
                break;
            case BuffType.Move:
                cooltime = StateManager.Instance.cooltimeData.movementBuffCooltime;
                break;
            case BuffType.Attack:
                cooltime = StateManager.Instance.cooltimeData.attackBuffCooltime;
                break;
        }
    }


    void RunCooltime()
    {
        StartCoroutine(RunCooltimeCo());
    }

    void SetEffect()
    {
        switch (buffType)
        {
            case BuffType.Search:
                
                break;
            case BuffType.Move:
                StateManager.Instance.MoveBuffEffect(cooltime);
                break;
            case BuffType.Attack:
                
                break;
        }
    }

    IEnumerator RunCooltimeCo()
    {
        SetCooltime();
        SetEffect();
        button.interactable = false;
        while(cooltime > 0)
        {
            cooltime -= Time.deltaTime;
            text.text = GetChangeTimeFormat(cooltime);
            yield return null;
        }
        text.text = "";
        button.interactable = true;
        cooltime = 0;
    }



}
