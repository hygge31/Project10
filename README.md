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
구현 목록
1. 마우스 클릭으로 캐릭터 움직이기
- 마우스를 클릭했을때 바로 움직이는게 아니라 이동 위치를 찍고, 시각화 한 다음, 모든 행동을 끝냈을때 움직이는 것을 목표로 구현하였습니다.
- 위를 구현하기위해 기본적으로 Navigation을 사용했으며, NavMeshAgent를 통해 목표위치를 설정하고 목표위치까지의 이동경로를 배열로 받아와 먼저 LineRenderer를 통해 이동경로를 그려주었습니다.
- 


1.StartDrawNavMeshAgentPath -> 마우스 왼쪽 클릭 ->  SetNavMeshAgentPath -> 엔터 클릭 -> PlayerMovePath


- 선택요구사항
    
    적용해볼 만한 기능들을 몇가지 정리했습니다.
    필요에 따라 적용해보세요.
    
    1. 오브젝트 폴링
    2. Instantiate 로 오브젝트 생성
    3. InputAction 사용
    4. 스크립트로 버튼에 이벤트 추가
    5. delegate 사용
    6. raycast 
    7. generic 을 이용한 싱글톤
    8. FSM
    9. Dictionary 활용
    10. Queue, Stack 활용
