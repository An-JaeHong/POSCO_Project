using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//일반 몬스터 Idle 상태
public class NormalIdleState : IUnitState
{
    public void Enter(Unit unit)
    {
        //여기에서 일반 몬스터가 주위를 돌면서 있는 함수를 실행.
    }

    public void Update(Unit unit)
    {
        //여기에서 플레이어를 발견하면 느낌표가 뜨면서 NormalChaseState로 바뀌어야함
    }

    public void Exit(Unit unit)
    {
        //죽으면 나오나? 일반 상태가 끝나는건 죽는 것 말고는 없는 것 같은데...
    }

}
