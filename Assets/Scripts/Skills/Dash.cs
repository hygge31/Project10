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

    public float initSpeed;
    public float maxSpeed;

    public float accelerationTime = 2f; // 가속 시간
    public float decelerationTime = 2f; // 감속 시간

    public float currentAccelerationTime; // 현재 가속 시간
    public float currentDecelerationTime; // 현재 감속 시간

    [Header("Trail Mesh")]
    float refreshRate = 0.05f;
    bool isTrailActive;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public Material mat;

    private void Start()
    {
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
        playerAnimationController = GameManager.instance.player.GetComponent<PlayerAnimationController>();
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();

        initSpeed = playerMovement.runSpeed;
    }

    public void ActiveDash()
    {
        if(!onDash && curCooltime <= 0)
        {
            StartCoroutine(ActiveDashCo());
        }
        
    }

    IEnumerator ActiveDashCo()
    {
        onDash = true;
        curCooltime = cooltime;

        float initspeed = 1;
        float maxspeed = 2;

        StartCoroutine(DashCooltimeCo());
        StartCoroutine(ActiveTrailCo(3.5f));
        playerAnimationController.animator.SetBool("isRunning", true);
        playerAnimationController.isRunning = true;

        while (currentAccelerationTime < accelerationTime) //1
        {
            currentAccelerationTime += Time.deltaTime;
            playerController.playerSpeed = Mathf.Lerp(initSpeed, maxSpeed, currentAccelerationTime / accelerationTime);


            float curSpeed = Mathf.Lerp(initspeed, maxspeed, currentAccelerationTime / accelerationTime);
            playerAnimationController.animator.SetFloat("addSpeed",curSpeed );

            yield return null;
        }

        while (currentDecelerationTime < decelerationTime) //1
        {
            currentDecelerationTime += Time.deltaTime;
            playerController.playerSpeed = Mathf.Lerp(maxSpeed, initSpeed, currentDecelerationTime / decelerationTime);

            float curSpeed = Mathf.Lerp(maxspeed, initspeed, currentDecelerationTime / decelerationTime);
            playerAnimationController.animator.SetFloat("addSpeed", curSpeed);


            yield return null;
        }
        playerController.playerSpeed = 60;
        playerAnimationController.animator.SetBool("isRunning", false);
        playerAnimationController.isRunning = false;

        currentAccelerationTime = 0;
        currentDecelerationTime = 0;

        onDash = false;
    }


    IEnumerator DashCooltimeCo()
    {
        while(curCooltime > 0)
        {
            curCooltime -= Time.deltaTime;
            yield return null;
        }

        curCooltime = 0;
    }




    IEnumerator ActiveTrailCo(float activeTime)
    {
        while(activeTime > 0)
        {
            activeTime -= refreshRate;
           
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(GameManager.instance.player.transform.position, GameManager.instance.player.transform.rotation);
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = mat;

                Destroy(gObj, 0.5f);

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

