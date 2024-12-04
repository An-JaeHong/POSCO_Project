using System.Collections;
using System.Collections.Generic;
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
    public GameObject exclamationMark; //느낌표 오브젝트
    public Transform spawnPosition; //스폰되는 장소

    public float moveSpeed;   //움직임 속도
    public float moveRange;   //움직임 범위 (일단은 정사각형이다)
    public float sightAngle;  //시야각
    public float detectRange; //탐지 범위

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
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
        //움직이는 방향 노말벡터
        Vector3 direction = (destination - transform.position).normalized;

        //속도 = 방향 * 크기
        Vector3 velocity = direction * moveSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    //느낌표를 띄워줄 함수
    public void ShowExclamationMark()
    {
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
}
