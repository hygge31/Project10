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

트러블 슈팅
 
처음에는 LineRenderer 로 이동경로만 그려주기 위해 NavMeshAgent에서 이동경로만 받아와 이동경로 수만큼 라인을 그려줬는데,</br>
플레이어가 이동할때 이동경로 또한 도착지점까지 점점 줄어들어야 하는데 줄어들지 않는 문제가 있었습니다.</br>
이를 해결하기위해 상호작용 UI 에서 Move를 클릭했을 때 onMouseClick 값을 true로 만들어주면 Update 에서 마우스 클릭하기 전까지 계속해서 NavMeshAgent에 도착지점을 설정하고</br>
이동경로를 UpdateLineRenderer() 에 전달해 주었습니다.</br>

<details>
  <summary>코드</summary>

    private void Update()
    {
        if (onMouseClick)
        {
            DrawNavMeshAgentPath();
        }
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

    void UpdateLineRenderer(Vector3[] paths)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = paths.Length;
        for (int i = 0; i < paths.Length; i++)
        {
            lineRenderer.SetPosition(i, paths[i]);
        }
    }

</details>
