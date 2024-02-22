using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    PlayerController playerController;
    PlayerAnimationController playerAnimationController;
    PlayerMovement playerMovement;


    public delegate void ActiveDashEvent();


    [Header("Skill State")]
    public float cooltime;
    public float curCooltime;

    public bool onDash;

    float initSpeed = 3f;
    float maxSpeed = 4f;

    public float accelerationTime = 2f; // 가속 시간
    public float decelerationTime = 2f; // 감속 시간

    public float currentAccelerationTime; // 현재 가속 시간
    public float currentDecelerationTime; // 현재 감속 시간

    



    private void Start()
    {
        

        playerController = GameManager.instance.player.GetComponent<PlayerController>();
        playerAnimationController = GameManager.instance.player.GetComponent<PlayerAnimationController>();
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();

    }

    public void ActiveDash()
    {
        //if (!onDash && curCooltime <= 0)
        //{
        //    StartCoroutine(ActiveDashCo());
        //}

    }

    //IEnumerator ActiveDashCo()
    //{
    //    onDash = true;
    //    curCooltime = cooltime;

    //    float initspeed = 1;
    //    float maxspeed = 2;

    //    StartCoroutine(DashCooltimeCo());
    //    StartCoroutine(ActiveTrailCo(3.5f));
    //    playerAnimationController.animator.SetBool("isRunning", true);
    //    playerAnimationController.isRunning = true;

    //    while (currentAccelerationTime < accelerationTime) //1
    //    {
    //        currentAccelerationTime += Time.deltaTime;
    //        playerMovement.navMeshAgent.speed = Mathf.Lerp(initSpeed, maxSpeed, currentAccelerationTime / accelerationTime);


    //        float curSpeed = Mathf.Lerp(initspeed, maxspeed, currentAccelerationTime / accelerationTime);
    //        playerAnimationController.animator.SetFloat("addSpeed", curSpeed);

    //        yield return null;
    //    }

    //    while (currentDecelerationTime < decelerationTime) //1
    //    {
    //        currentDecelerationTime += Time.deltaTime;
    //        playerMovement.navMeshAgent.speed = Mathf.Lerp(maxSpeed, initSpeed, currentDecelerationTime / decelerationTime);

    //        float curSpeed = Mathf.Lerp(maxspeed, initspeed, currentDecelerationTime / decelerationTime);
    //        playerAnimationController.animator.SetFloat("addSpeed", curSpeed);


    //        yield return null;
    //    }
    //    playerAnimationController.animator.SetBool("isRunning", false);
    //    playerAnimationController.isRunning = false;

    //    currentAccelerationTime = 0;
    //    currentDecelerationTime = 0;

    //    onDash = false;
    //}


    IEnumerator DashCooltimeCo()
    {
        while (curCooltime > 0)
        {
            curCooltime -= Time.deltaTime;
            yield return null;
        }

        curCooltime = 0;
    }
}



    

