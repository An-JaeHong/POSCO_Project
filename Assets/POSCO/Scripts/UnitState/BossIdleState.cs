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
        unit.HideBossSpeech();
        //IdleState에서는 앉아있는다
        unit.animator.SetBool("IsStanding", false);
    }

    public void Update(Unit unit)
    {
        //Debug.Log($"현재 NormalIdleState에 Update로 들어옴");
        //여기에서 플레이어를 발견하면 느낌표가 뜨면서 BossFindPlayerState로 바뀌어야함
        if (DetectPlayer(unit))
        {
            unit.ChangeState(new BossFindPlayerState());
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
        //플레이어쪽을 보는 방향 -> 0.5를 안더해줬더니 기즈모 방향이 아래를 바라봐서 플레이어를 감지하지 못했습니다.
        Vector3 directionToPlayer = (player.transform.position + new Vector3(0, 0.5f, 0) - unit.transform.position).normalized;
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
