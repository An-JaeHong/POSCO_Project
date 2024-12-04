using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� Idle����
public class BossIdleState : IUnitState
{
    public void Enter(Unit unit)
    {
        //������ ���� �׳� Idle ���¸� ��� ���� ����? �־��ָ� ������
        unit.HideExclamationMark();
    }

    public void Update(Unit unit)
    {
        //Debug.Log($"���� NormalIdleState�� Update�� ����");
        //���⿡�� �÷��̾ �߰��ϸ� ����ǥ�� �߸鼭 NormalChaseState�� �ٲ�����
        if (DetectPlayer(unit))
        {
            unit.ChangeState(new BossFindPlayerState());
            Debug.Log("IdleState���� ChaseState�� ��");

            Debug.Log("������");
            return;
        }
    }

    private bool DetectPlayer(Unit unit)
    {
        //�÷��̾��� �±װ� Player�� �Ǿ��־�� �Ѵ�
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //�÷��̾ ������
        if (UnitSight(unit, player))
        {
            return true;
        }

        return false;
    }

    private bool UnitSight(Unit unit, GameObject player)
    {
        //�÷��̾����� ���� ����
        Vector3 directionToPlayer = (player.transform.position - unit.transform.position).normalized;
        //���� = (������ ���� ���� ����, �÷��̾ ���� ����)
        float angle = Vector3.Angle(unit.transform.forward, directionToPlayer);

        //���� �����ϰ�
        if (angle < unit.sightAngle / 2)
        {
            //���� �����ϰ�
            if (Physics.Raycast(unit.transform.position, directionToPlayer, out RaycastHit hit, unit.detectRange))
            {
                //�÷��̾� �����ϸ� true
                if (hit.collider.gameObject == player)
                {
                    Debug.Log($"���� ���� ������Ʈ : {hit.collider.gameObject}");
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
