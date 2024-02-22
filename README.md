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

1. UI 를 통해 캐릭터를 제어하고 원하는 액션을 실행시킵니다.


    Move
- 마우스를 클릭했을때 바로 움직이는게 아니라 이동 위치를 찍고, 시각화 한 다음, 모든 행동을 끝냈을때 캐릭터가 움직입니다.
- 위를 구현하기위해 기본적으로 Navigation을 사용했으며, NavMeshAgent를 통해 목표위치를 설정하고 목표위치까지의 이동경로를 배열로 받아와 먼저 LineRenderer를 통해 이동경로를 그려주었습니다.
- 이동경로를 그려주고 사용자의 왼쪽 마우스 클릭을 Input으로 받아 이동경로를 확정해주고, 엔터키를 누르면 해당위치로 이동하게 로직을 만들어 주었습니다

   Attack
- 캐릭터가 공격 태세로 전환합니다. 공격 모션 이후의 로직은 아직 구현하지 않았습니다.

   Action
- 해당 버프를 클릭 시 캐릭터에게 유용한 버프가 적용되며, 쿨타임이 생성됩니다. (사진)

   상호작용
- 특정 물체와 가까워지면 상호작용 UI 가 나오며 클릭을 누르면 특정 이벤트가 일어납니다.
  (사진)
  <p>
      <h3>횃불 상호작용</h3>
  <img src="https://github.com/hygge31/CodingTest_Csharp/assets/121877159/f2ab462e-cf9b-4e0f-ba5c-8ef23d5ef81f" width="350px" />
  </p>
   <p>
      <h3>숨겨진 문 상호작용</h3>
  <img src="https://github.com/hygge31/CodingTest_Csharp/assets/121877159/b9d5d8d4-b51c-4e40-998f-be6dcc6a0fe8" width="350px" />
  </p>
 

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
