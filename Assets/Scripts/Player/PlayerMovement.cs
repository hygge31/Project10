using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
    public NavMeshAgent navMeshAgent;

    [Header("Layer")]
    public LayerMask clickableLayerMask;
    public LayerMask obstacle;
    public int clickableLayerMaskCode;

    [Header("LineRenderer, Pool")]
    public LineRenderer lineRenderer; // 라인 렌더러 컴포넌트
    Pooling pooling;

    private void Awake()
    {
        clickableLayerMaskCode = LayerMask.GetMask("Ground");
        navMeshAgent = GetComponent<NavMeshAgent>();

        lineRenderer.enabled = false;

        pooling = GetComponent<Pooling>();
    }


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimationController = GetComponent<PlayerAnimationController>();

        orgPlayerSpeed = playerController.playerSpeed;
        arrivalPoint = transform.position;

        pooling.CreatePoolItem(GameManager.instance.poolingBox);
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
            if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayerMask | obstacle))
            {
                Vector3 dir = (hit.point - transform.position).normalized;
                if ((clickableLayerMask.value & (1<< hit.collider.gameObject.layer))>0){
                    if (Vector3.Dot(moveDir, dir) < -0.9f)
                    {
                        Debug.Log("반대방향 && 뛰는중");
                    }
                    navMeshAgent.destination = hit.point;

                    GameObject obj = pooling.GetPoolItem("Point");
                    obj.SetActive(true);
                    obj.transform.position = new Vector3(hit.point.x, 0.2f, hit.point.z);
                    


                    moveDir = dir;
                    arrivalPoint = hit.point;
                }
                if((obstacle.value & (1<<hit.collider.gameObject.layer)) > 0)
                {
                    Vector3[] paths = GetNavPath();
                    int idx = GetMoveablePosition(paths);
                    navMeshAgent.destination = paths[idx];
                    moveDir = dir;
                    arrivalPoint = paths[idx];
                }
               
            }
            
        }
       
    }

    int GetMoveablePosition(Vector3[] paths)
    {
        
        Debug.Log("ob");
        for (int i = paths.Length-1; i >0; i--)
        {
            if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(paths[i]), 100, clickableLayerMask)){
                Debug.Log(i);
                return i;
            }
        }
        Debug.Log("0");
        return 0;
    }

    Vector3[] GetNavPath()
    {
        Vector3[] paths = navMeshAgent.path.corners;
        
        return paths;
    }


    void UpdateLineRenderer(Vector3[] paths)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = paths.Length;
        for (int i = 0; i < paths.Length; i++)
        {
            lineRenderer.SetPosition(i, paths[i]);
        }

    }



    void PlayerMoveing(Vector3 dir, Vector3 endPot)
    {
        float distance = Vector3.Distance(transform.position, endPot);
        playerAnimationController.CallOnMoveEvent(distance);

        if (distance < 0.1f)
        {
            playerController._rigidbody.velocity = Vector3.zero;
            isMoveing = false;
            RunOff();
            lineRenderer.enabled = false;
            pooling.AllDestroy("Point");
        }
        else
        {
            isMoveing = true;
            RotateForward(endPot);
            UpdateLineRenderer(GetNavPath());
        }

    }

    void RotateForward(Vector3 targetPoint)
    {
        Vector3 dir = (targetPoint - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
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
        navMeshAgent.speed += runSpeed;
        playerAnimationController.animator.SetBool("isRunning", true);
        playerAnimationController.isRunning = true;
    }
    public void RunOff()
    {
        navMeshAgent.speed = 1.5f; ;
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
        navMeshAgent.isStopped = true;

        isRolling = true;
        playerAnimationController.animator.SetTrigger("onRolling");

        playerController._rigidbody.AddForce(Vector3.forward * 50f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        isRolling = false;

        curRollingCooltime = rollingCooltime;

        navMeshAgent.isStopped = false;

        while (curRollingCooltime > 0)
        {
            curRollingCooltime -= Time.deltaTime;
            yield return null;
        }

        
        curRollingCooltime = 0;

    }
}
