<h1>게임 개발 심화 과제</h1> 

- 필수요구사항
    1. 인트로 씬 구성
    2. 자유 게임 만들기
     

<h3>인트로 씬 구성</h3>
간단하게 AddListener을 사용해 시작버튼을 눌러 다음 씬으로 이동하게 만들었습니다.
<details>
  <summary>코드</summary>
  <pre>
    <code>
      startBtn.onClick.AddListener(() => SceneManager.LoadScene("Main"));
    </code>
  </pre>
</details>


<h3>자유 게임 만들기</h3>

1. 마우스 클릭으로 캐릭터 움직이기  
- 마우스를 클릭했을때 바로 움직이는게 아니라 이동 위치를 찍고, 시각화 한 다음, 모든 행동을 끝냈을때 움직이는 것을 목표로 구현하였습니다.
- 위를 구현하기위해 기본적으로 Navigation을 사용했으며, NavMeshAgent를 통해 목표위치를 설정하고 목표위치까지의 이동경로를 배열로 받아와 먼저 LineRenderer를 통해 이동경로를 그려주었습니다.
- 이동경로를 그려주고 사용자의 왼쪽 마우스 클릭을 Input으로 받아 이동경로를 확정해주고, 엔터키를 누르면 해당위치로 이동하게 로직을 만들어 주었습니다
2. UI
- 행동 에는 Move, Attack, Action Cancel 이 있으며, Move 로직 구현에 너무 많은 시간을 쏟아 Attack과 Action 부분을 구현하지  못했습니다.
 

2. 

트러블 슈팅
 
처음에는 LineRenderer 로 이동경로만 그려주기 위해 NavMeshAgent에서 이동경로만 받아와 이동경로 수만큼 라인을 그려줬는데,</br>
플레이어가 이동할때 이동경로 또한 도착지점까지 점점 줄어들어야 하는데 줄어들지 않는 문제와 이동할때 출발할때부터 도착위치를 바라보며 이동하는 문제가 있었습니다.
그래서 NavMeshAgent에 목표 위치를 지정해서 자동으로 움직이는게 아니라, 목표위치를 설정해놓고 이동경로만 배열로 가져와 이를 이용해 두가지 문제를 해결했습니다.


<details>
  <summary>코드</summary>

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
        playerAnimationController.animator.SetBool("isWalking", true);
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

        playerAnimationController.animator.SetBool("isWalking", false);
        ClearDrawNavMeshPath();
        UIManager.Instance.Reset();
    }

</details>
