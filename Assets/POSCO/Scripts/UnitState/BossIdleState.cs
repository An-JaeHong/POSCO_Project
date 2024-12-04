using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스 Idle상태
public class BossIdleState : IUnitState
{
    public void Enter(Unit unit)
    {
        //보스는 들어가면 그냥 Idle 상태만 재생 가끔 딴짓? 넣어주면 좋을듯
        unit.HideExclamationMark();
    }

    public void Update(Unit unit)
    {
        //Debug.Log($"현재 NormalIdleState에 Update로 들어옴");
        //여기에서 플레이어를 발견하면 느낌표가 뜨면서 NormalChaseState로 바뀌어야함
        if (DetectPlayer(unit))
        {
            unit.ChangeState(new BossFindPlayerState());
            Debug.Log("IdleState에서 ChaseState로 감");

            Debug.Log("감지중");
            return;
        }
    }

    private bool DetectPlayer(Unit unit)
    {
        //플레이어의 태그가 Player로 되어있어야 한다
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //플레이어가 있으면
        if (UnitSight(unit, player))
        {
            return true;
        }

        return false;
    }

    private bool UnitSight(Unit unit, GameObject player)
    {
        //플레이어쪽을 보는 방향
        Vector3 directionToPlayer = (player.transform.position - unit.transform.position).normalized;
        //각도 = (유닛이 앞을 보는 방향, 플레이어를 보는 방향)
        float angle = Vector3.Angle(unit.transform.forward, directionToPlayer);

        //각도 만족하고
        if (angle < unit.sightAngle / 2)
        {
            //방향 만족하고
            if (Physics.Raycast(unit.transform.position, directionToPlayer, out RaycastHit hit, unit.detectRange))
            {
                //플레이어 만족하면 true
                if (hit.collider.gameObject == player)
                {
                    Debug.Log($"현재 닿은 오브젝트 : {hit.collider.gameObject}");
                    return true;
                }
            }
        }
        return false;
    }

    public void Exit(Unit unit)
    {
        
    }
}
