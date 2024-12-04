using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스 Idle상태
public class BossIdleState : IUnitState
{
    public void Enter(Unit unit)
    {
        //보스는 들어가면 그냥 Idle 상태만 재생 가끔 딴짓? 넣어주면 좋을듯
    }

    public void Update(Unit unit)
    {
        //플레이어가 근처로 가면 느낌표를 띄울까?
    }

    public void Exit(Unit unit)
    {
        
    }

}
