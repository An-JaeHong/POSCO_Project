using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//�Ϲ� ���� Idle ����
public class NormalIdleState : IUnitState
{
    Vector3 destination;
    private float waitTime = 2f; //���ð�
    private float waitTimer = 0;
    private bool isWaiting = false;

    //public NormalChaseState(Unit unit) { }

    public void Enter(Unit unit)
    {
        //���⿡�� �Ϲ� ���Ͱ� ������ ���鼭 �ִ� �Լ��� ����. �� 3�� �������ٰ� 2�� ���ߴ� �����̸� ������
        //X���� Unit�� �����ǿ��� +-unit.moveRange ������ �����δ�.
        destination = unit.SetRandomPosition();

        //���� �ϴ� ����ǥ�� �����.
        unit.HideExclamationMark();
    }


    public void Update(Unit unit)
    {
        //Debug.Log($"���� NormalIdleState�� Update�� ����");
        //���⿡�� �÷��̾ �߰��ϸ� ����ǥ�� �߸鼭 NormalChaseState�� �ٲ�����
        if (DetectPlayer(unit))
        {
            unit.ChangeState(new NormalChaseState());

            Debug.Log("������");
            return;
        }

        if(isWaiting)
        {
            Debug.Log("�ٽ� ������");
            waitTimer -= Time.deltaTime;
            if(waitTimer <= 0)
            {
                isWaiting = false;
                destination = unit.SetRandomPosition();
            }
        }
        else
        {
            //ó�� ���� ���� ��ġ�� �����̸� ��� ���� �� �ٽ� ������
            unit.UnitMove(destination);
            if (Vector3.Distance(unit.transform.position, destination) < 0.1f)
            {
                //���⼭ ��� �����ִ� �Լ� �־�����.
                Debug.Log("�̵���");
                isWaiting = true;
                waitTimer = waitTime;
                StopUnitMove(unit);
            }
        }
    }
    private void StopUnitMove(Unit unit)
    {
        // ������ �̵��� ���߰� �ϴ� ����
        Debug.Log("����");
        unit.UnitMove(unit.transform.position);
    }

    private bool DetectPlayer(Unit unit)
    {
        //�÷��̾��� �±װ� Player�� �Ǿ��־�� �Ѵ�
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //�÷��̾ ������
        if (player != null && UnitSight(unit, player))
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
            RaycastHit hit;
            //���� �����ϰ�
            if (Physics.Raycast(unit.transform.position, directionToPlayer, out hit, unit.detectRange))
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
        //������ ������? �Ϲ� ���°� �����°� �״� �� ����� ���� �� ������...
    }

}
