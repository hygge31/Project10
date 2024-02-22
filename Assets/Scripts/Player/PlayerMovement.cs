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

    public bool isMoveing;
    //public float orgPlayerSpeed;
    public float runSpeed;
    public bool isRuning;
    public float rollingCooltime;
    public float curRollingCooltime;
    bool isRolling;



    //-----test
    public bool onMouseClick;
    Vector3[] paths;

    bool isSetNavMeshAgentPath;
    bool isDrawPath;
    bool moveReady;


    //-----test
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
        navMeshAgent.isStopped = true;
        lineRenderer.enabled = false;

        pooling = GetComponent<Pooling>();
    }


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimationController = GetComponent<PlayerAnimationController>();

        //orgPlayerSpeed = playerController.playerSpeed;
        arrivalPoint = transform.position;

        pooling.CreatePoolItem(GameManager.instance.poolingBox);
    }

    private void FixedUpdate()
    {
        PlayerMoveing(moveDir, arrivalPoint);
    }


    private void Update()
    {
        if (onMouseClick)
        {
            DrawNavMeshAgentPath();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSetNavMeshAgentPath && isDrawPath)
        {
            SetNavMeshAgentPath();
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && isSetNavMeshAgentPath &&!isMoveing)
        {
            UIManager.Instance.Reset();
        }

        if(Input.GetKeyDown(KeyCode.Return) && !isMoveing && moveReady)
        {
            OnMove();
        }
    }


    //-----------test

    public void StartDrawNavMeshAgentPath()//1
    {
        UIManager.Instance.infoText.text = "마우스 왼쪽 클릭 : 이동 위치 지정";
        onMouseClick = true;
        isDrawPath = true;
        lineRenderer.enabled = true;
    }

    public void SetNavMeshAgentPath()//2
    {
        Debug.Log("click");
        UIManager.Instance.infoText.text = "오른쪽 마우스 클릭 : 취소\nEnter: 이동";
        onMouseClick = false;
        isDrawPath = false;
        isSetNavMeshAgentPath = true;
        NavMeshAgentPath();
    }

    public void CancelDrawNavMeshAgentPath() //3
    {
        ClearDrawNavMeshPath();
    }

    public void OnMove() //3
    {
        PlayerMovePath();
    }


    void DrawNavMeshAgentPath()
    {
        RaycastHit hit;
        if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayerMask))
        {
            if ((clickableLayerMask.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                navMeshAgent.destination = hit.point;
                UpdateLineRenderer(navMeshAgent.path.corners);
                GameObject obj = pooling.GetPoolItem("Point");
                obj.SetActive(true);
                obj.transform.position = new Vector3(hit.point.x, 0.2f, hit.point.z);
            }
        }
    }

    void NavMeshAgentPath()
    {
        paths = navMeshAgent.path.corners;
        moveReady = true;
    }

    void PlayerMovePath()
    {
        StartCoroutine(PlayerMovePathCo());
    }

    IEnumerator PlayerMovePathCo()
    {
        UIManager.Instance.infoText.text = "이동중";
        isMoveing = true;
        moveReady = false;
        for (int i = 1; i < paths.Length; i++)
        {
            Vector3 dir = (paths[i] - transform.position).normalized;
            moveDir = dir;
            float distance = Vector3.Distance(transform.position, paths[i]);
            arrivalPoint = paths[i];
            while (distance > 0.1f)
            {
                RotateForward(paths[i]);
                if ((paths[i] - transform.position).normalized != dir)
                {
                    dir = (paths[i] - transform.position).normalized;
                    moveDir = dir;
                }
                distance = Vector3.Distance(transform.position, paths[i]);
                UpdateLineRenderer(navMeshAgent.path.corners);

                yield return null;
            }

        }
        ClearDrawNavMeshPath();
        UIManager.Instance.infoText.text = "I : 캐릭터 행동 메뉴";
        UIManager.Instance.interactionUI.possibleOpenUI = true;
    }

   
    public void ClearDrawNavMeshPath()
    {
        onMouseClick = false;
        isDrawPath = false;
        isSetNavMeshAgentPath = false;
        lineRenderer.enabled = false;
        pooling.AllDestroy("Point");
        isMoveing = false;
        moveReady = false;
        //paths = null;
    }

    void RotateForward(Vector3 targetPoint)
    {
        Vector3 dir = (targetPoint - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
        }
        else
        {
            transform.position += dir * runSpeed * Time.deltaTime;
        }

    }
    //-----------test




    #region term




    //public void MouseClickToMove()
    //{
    //    if (!isRolling)
    //    {
    //        RaycastHit hit;
    //        if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayerMask | obstacle))
    //        {
    //            Vector3 dir = (hit.point - transform.position).normalized;
    //            if ((clickableLayerMask.value & (1<< hit.collider.gameObject.layer))>0){
    //                if (Vector3.Dot(moveDir, dir) < -0.9f)
    //                {
    //                    Debug.Log("반대방향 && 뛰는중");
    //                }
    //                navMeshAgent.destination = hit.point;

    //                GameObject obj = pooling.GetPoolItem("Point");
    //                obj.SetActive(true);
    //                obj.transform.position = new Vector3(hit.point.x, 0.2f, hit.point.z);



    //                moveDir = dir;
    //                arrivalPoint = hit.point;
    //            }
    //            if((obstacle.value & (1<<hit.collider.gameObject.layer)) > 0)
    //            {
    //                Vector3[] paths = GetNavPath();
    //                int idx = GetMoveablePosition(paths);
    //                navMeshAgent.destination = paths[idx];
    //                moveDir = dir;
    //                arrivalPoint = paths[idx];
    //            }

    //        }

    //    }

    //}

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
    #endregion
}
