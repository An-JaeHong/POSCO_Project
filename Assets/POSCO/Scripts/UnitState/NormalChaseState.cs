using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unit의 시야각에 들어오면 계속 쫓아와야함
public class NormalChaseState : IUnitState
{
    private GameObject player;
    public void Enter(Unit unit)
    {
        //쫓아오는 상태가 되면 느낌표를 계속 띄워야함
        unit.ShowExclamationMark();
        //player = GameObject.FindGameObjectWithTag("Player");
        //if (player == null)
        //{
        //    Debug.LogError("플레이어를 찾을 수 없습니다.");
        //}
    }

    public void Update(Unit unit)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
        }
        //player 찾기
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //플레이어가 있는 방향
            //Vector3 directionToPlayer = (player.transform.position - unit.transform.position).normalized;
            unit.UnitRotation(player.transform.position);

            //플레이어를 찾으면 속도가 1.5배 빨라짐
            //unit.SetMoveSpeedFast();
            unit.moveSpeed = 2f;

            unit.UnitMove(player.transform.position);

            //플레이어까지와의 거리
            float distance = Vector3.Distance(unit.transform.position, player.transform.position);

            //만약 거리가 사정거리보다 멀어지면 idle로 바꾼다
            if (distance > unit.detectRange)
            {
                unit.ChangeState(new NormalIdleState());
            }
        }
    }

    public void Exit(Unit unit)
    {
        
    }
}
