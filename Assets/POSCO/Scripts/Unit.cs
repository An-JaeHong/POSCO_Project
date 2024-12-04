using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Xml.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//보스와 일반 몬스터들을 묶을 클래스
[RequireComponent(typeof(CharacterController))]
public class Unit : MonoBehaviour
{
    private IUnitState currentState; //현재상태
    public List<Monster> ownedMonsterList = new List<Monster>(); //소지하고 있는 몬스터 리스트 -> 보스 : 종류가 다른 3개의 몬스터.  일반몬스터 : 종류가 같은 3개의 몬스터
    public bool isBoss; //State패턴을 위한 변수. true : 보스, false : 일반 몬스터
    public GameObject exclamationMarkPrefab; //느낌표 프리팹
    private GameObject exclamationMark; //실제 켰다 껐다할 느낌표
    public Transform spawnPosition; //스폰되는 장소

    public string name; //유닛 이름
    public float moveSpeed;   //움직임 속도
    public float moveRange;   //움직임 범위 (일단은 정사각형이다)
    public float sightAngle;  //시야각
    public float detectRange; //탐지 범위
    public bool iscontactedPlayer; //플레이어를 만났는지

    public CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        InstantiateExclamationMark();
        if (isBoss)
        {
            ChangeState(new BossIdleState());
        }
        else
        {
            ChangeState(new NormalIdleState());
        }
    }

    //Interface라서 Update를 직접 불러줘야한다.
    private void Update()
    {
        currentState?.Update(this);
    }

    //상태를 바꾸는 함수
    public void ChangeState(IUnitState newState)
    {
        //현재 상태는 종료
        currentState?.Exit(this);
        print($"Before State : {currentState}");
        currentState = newState;
        //다시 시작
        currentState.Enter(this);
        print($"After State : {currentState}");
    }

    //목적지를 파라미터로 넣어주자
    public void UnitMove(Vector3 destination)
    {
        if (iscontactedPlayer)
        {
            return;
        }
        //움직이는 방향 노말벡터
        Vector3 direction = (destination - transform.position).normalized;

        //속도 = 방향 * 크기
        Vector3 velocity = direction * moveSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }


    //느낌표를 소환해줄 함수
    public void InstantiateExclamationMark()
    {
        exclamationMark = Instantiate(exclamationMarkPrefab, transform);
    }

    //느낌표를 띄워줄 함수
    public void ShowExclamationMark()
    {
        //자식으로 소환
        exclamationMark.SetActive(true);
    }

    //느낌표를 숨겨줄 함수
    public void HideExclamationMark()
    {
        exclamationMark.SetActive(false);
    }

    //정해진 포지션에 도달하면 다른 포지션을 생성하게끔 하자
    public Vector3 SetRandomPosition()
    {
        //함수 실행될때 마다 랜덤한 x,z값 생성하고 새로운 벡터를 생성
        float rangeX = Random.Range(spawnPosition.position.x - moveRange, spawnPosition.position.x + moveRange);
        float rangeZ = Random.Range(spawnPosition.position.z - moveRange, spawnPosition.position.z + moveRange);

        Vector3 randomPos = new Vector3(rangeX, 0 , rangeZ);

        print($"랜덤으로 제공된 포지션 : {randomPos}");
        //새로 만든 포지션을 리턴
        return randomPos;
    }

    //공격범위 그리는 함수
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward;
        //오른쪽 각도
        Vector3 rightBoundary = Quaternion.Euler(0, sightAngle / 2, 0) * forward;
        //왼쪽 각도
        Vector3 leftBoundary = Quaternion.Euler(0, -sightAngle / 2, 0) * forward;

        //Unit의 포지션
        Vector3 position = transform.position;
        //기즈모의 Y축 포지션을 0.1만큼 띄운다 0이면 바닥에 가려질 수도 있어서 0.1만큼 띄워서 생성할것이다
        position.y = 0.1f;

        //반지름 = 탐지범위
        float radius = detectRange;

        //좌우로 반지름 만큼의 선분을 그린다.
        Gizmos.DrawLine(position, position + rightBoundary * radius);
        Gizmos.DrawLine(position, position + leftBoundary * radius);

        //segments가 클수록 부드러운 호를 그린다.
        int segments = 20;
        //angleStep = 시야각 / n; 각도를 쪼개는 변수
        float angleStep = sightAngle / segments;
        //시작포인트는 왼쪽에서부터이다. 그리고 쭉 그려줄것이다.
        Vector3 previousPoint = position + leftBoundary * radius;

        for (int i = 1; i <= segments; i++)
        {
            //왼쪽 각 부터 계속 더하면서 angle을 업데이트
            float angle = -sightAngle / 2 + angleStep * i;
            //호에 그려지는 포인트
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            //호에 그려질 다음 포인트
            Vector3 nextPoint = position + direction * radius;
            //그 포인트들을 계속 이어간다.
            Gizmos.DrawLine(previousPoint, nextPoint);
            //그리고 이전 포인트는 다음 포인트로 초기화 -> 계속 이어나가면 호처럼 된다.
            previousPoint = nextPoint;
        }
    }
}
