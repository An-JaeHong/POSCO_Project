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
    public float moveSpeed;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (isBoss)
        {
            SetState(new BossIdleState());
        }
        else
        {
            SetState(new NormalIdleState());
        }
    }

    //Interface라서 Update를 직접 불러줘야한다.
    private void Update()
    {
        currentState?.Update(this);
    }

    //상태를 바꾸는 함수
    public void SetState(IUnitState newState)
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

    public void SwitchExclamationMark(bool isActive)
    {
        exclamationMark.SetActive(isActive);
    }
}
