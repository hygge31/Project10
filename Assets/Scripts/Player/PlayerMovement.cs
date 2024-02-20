using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{
    [Header("Movement State")]
    [SerializeField] private float rotationSpeed = 5f;
    Vector3 moveDir;
    Vector3 arrivalPoint;

    bool isMoveing;
    public float orgPlayerSpeed;
    public float runSpeed;
    bool isRuning;
    public float rollingCooltime;
    public float curRollingCooltime;
    bool isRolling;

    [Header("Components")]
    PlayerController playerController;
    PlayerAnimationController playerAnimationController;

    [Header("Layer")]
    public LayerMask clickableLayerMask;
    public int clickableLayerMaskCode;

    private void Awake()
    {
        clickableLayerMaskCode = LayerMask.GetMask("Ground");
    }


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimationController = GetComponent<PlayerAnimationController>();

        orgPlayerSpeed = playerController.playerSpeed;
    }

    private void FixedUpdate()
    {
        PlayerMoveing(moveDir, arrivalPoint);        
    }

    public void MouseClickToMove()
    {
        if (!isRolling)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayerMask))
            {
                if (clickableLayerMask == clickableLayerMaskCode)
                {
                    Vector3 dir = (hit.point - transform.position).normalized;

                    if (Vector3.Dot(moveDir, dir) < -0.9f)
                    {
                        Debug.Log("반대방향 && 뛰는중");
                    }

                    moveDir = dir;
                    arrivalPoint = hit.point;
                }

            }
        }
       
    }


    void PlayerMoveing(Vector3 dir,Vector3 endPot)
    {
        float distance = Vector3.Distance(transform.position, endPot);
        playerAnimationController.CallOnMoveEvent(distance);

        if (distance < 0.1f)
        {
            playerController._rigidbody.velocity = Vector3.zero;
            isMoveing = false;
            RunOff();
        }
        else
        {
            isMoveing = true;
            playerController._rigidbody.velocity = dir * playerController.playerSpeed * Time.deltaTime;
            RotateForward();
        }

    }
    void RotateForward()
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    public void Run(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && isMoveing)
        {
            RunOn();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            RunOff();

        }
    }

    public void RunOn()
    {
        playerController.playerSpeed += 60;
        playerAnimationController.animator.SetBool("isRunning", true);
        playerAnimationController.isRunning = true;
    }
    public void RunOff()
    {
        playerController.playerSpeed = 60;
        playerAnimationController.animator.SetBool("isRunning", false);
        playerAnimationController.isRunning = false;
    }


    public void Rolling()
    {
        if(curRollingCooltime <= 0 && !isRolling)
        {
            
            StartCoroutine(RollingCoolTimeCo());
        }
    }



    IEnumerator RollingCoolTimeCo()
    {
        isRolling = true;
        playerAnimationController.animator.SetTrigger("onRolling");
        playerController.playerSpeed += 100;
        arrivalPoint = transform.position + moveDir *2f;
        yield return new WaitForSeconds(0.5f);
        isRolling = false;
        playerController.playerSpeed -= 100;

        curRollingCooltime = rollingCooltime;

        while (curRollingCooltime > 0)
        {
            curRollingCooltime -= Time.deltaTime;
            yield return null;
        }

        
        curRollingCooltime = 0;

    }
}
