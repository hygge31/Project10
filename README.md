<h1>게임 개발 심화 과제</h1> 

- 필수요구사항
    1. 인트로 씬 구성
    2. 자유 게임 만들기
     

<h3>자유 게임 만들기</h3>

1. UI 를 통해 캐릭터를 제어하고 원하는 액션을 실행시킵니다.
   
  <p>
  <img src="https://github.com/hygge31/CodingTest_Csharp/assets/121877159/cd4868b1-a19a-4b0a-b83d-9ab2ab10e2d0" width="350px" alt="이미지 로딩 중" />
  </p>
  
  Move
- 마우스를 클릭했을때 바로 움직이는게 아니라 이동 위치를 찍고, 시각화 한 다음, 모든 행동을 끝냈을때 캐릭터가 움직입니다.
- 위를 구현하기위해 기본적으로 Navigation을 사용했으며, NavMeshAgent를 통해 목표위치를 설정하고 목표위치까지의 이동경로를 배열로 받아와 먼저 LineRenderer를 통해 이동경로를 그려주었습니다.
- 이동경로를 그려주고 사용자의 왼쪽 마우스 클릭을 Input으로 받아 이동경로를 확정해주고, 엔터키를 누르면 해당위치로 이동하게 로직을 만들어 주었습니다

   Attack
- 캐릭터가 공격 태세로 전환합니다. 공격 모션 이후의 로직은 아직 구현하지 않았습니다.

   Action
- 해당 버프를 클릭 시 캐릭터에게 유용한 버프가 적용되며, 쿨타임이 생성됩니다. (탐색버프 미완) (사진)

   상호작용
- 특정 물체와 가까워지면 상호작용 UI 가 나오며 클릭을 누르면 특정 이벤트가 일어납니다.
  (사진)
  횃불의 경우 애니메이션 커브를 사용해 빛이 일렁이는 효과를 주었습니다.
  <p>
      <h3>횃불 상호작용</h3>
  <img src="https://github.com/hygge31/CodingTest_Csharp/assets/121877159/f2ab462e-cf9b-4e0f-ba5c-8ef23d5ef81f" width="350px" alt="이미지 로딩 중" />
  </p>
  숨겨진 문과 캐릭터가 가까워지면 상호작용 키가 등장하고 클릭하면 벽이 사라지고 새로운 맵이 등장합니다.
   <p>
      <h3>숨겨진 문 상호작용</h3>
  <img src="https://github.com/hygge31/CodingTest_Csharp/assets/121877159/b9d5d8d4-b51c-4e40-998f-be6dcc6a0fe8" width="350px" alt="이미지 로딩 중" />
  </p>
 

2. 

트러블 슈팅
 
캐릭터가 목표 지점으로 이동할때,  이동하는 방향이 아닌 도착지점을 바라보고 이동하는 문제점과 캐릭터가 움직여도 이동경로가 처음 출발위치부터 도착위치까지 계속 표시되는 문제가 있었습니다.
Navigation 으로 목표위치를 지정 해주고, 캐릭터의 방향을 단순히 목표위치에서 현재위치를 뺀 값을 노멀라이즈 해준 것으로 캐릭터의 방향을 결정한 것과,
Navigation의 목표를 지정해줄때 LineRenderer의 이동경로를 딱 한번만 그려주고 있었기 떄문이었습니다.
또한 캐릭터의 행동을 UI를 통해 통제 할 것이기 때문에 마우스 클릭으로 목표위치를 정한뒤 바로 움직이는게 아닌 모든 행동이 끝났을때 사용자가 엔터키를 누르면 움직여야 했기 떄문에
Navigation 에 목표위치를 정해주고, Navigation 이 목표위치까지 이동경로를 계산해주면, 이동경로만 배열로 받아와 사용하는 것으로 로직을 수정했습니다.

순서

상호작용 UI 에서 Move 선택 -> StartDrawNavMeshAgentPath() :이동경로 그려주기 -> 마우스 오른쪽 클릭(왼쪽은 취소) -> SetNavMeshAgentPath(): 이동경로 확정 -> 엔터키 입력 -> PlayerMovePath() : 플레이어 이동
실질적으로 PlayerMovePath() 는 캐릭터가 이동할 위치만 업데이트 해주고, FixedUpdate 에서 위치가 업데이트 되면 해당위치로 캐릭터가 이동합니다.

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

     void UpdateLineRenderer(Vector3[] paths) //경로 그려주기
    {
        lineRenderer.enabled = true;
        Debug.Log(paths.Length);
        lineRenderer.positionCount = paths.Length;
        for (int i = 0; i < paths.Length; i++)
        {
            lineRenderer.SetPosition(i, paths[i]);
        }

    }

</details>
