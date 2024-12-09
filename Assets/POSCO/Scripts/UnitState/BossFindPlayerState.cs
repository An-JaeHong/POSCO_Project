using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFindPlayerState : IUnitState
{
    private GameObject player;
    public void Enter(Unit unit)
    {
        unit.ShowExclamationMark();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update(Unit unit)
    {
        //플레이어까지와의 거리를 계속해서 젠 다음
        float distance = Vector3.Distance(unit.transform.position, player.transform.position);

        //만약 거리가 사정거리보다 멀어지면 idle로 바꾼다
        if (distance > unit.detectRange)
        {
            unit.ChangeState(new BossIdleState());
        }
    }

    public void Exit(Unit unit)
    {
        
    }

}
