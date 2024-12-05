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
    private bool hasRandomPosition = true;

    //public NormalChaseState(Unit unit) { }

    public void Enter(Unit unit)
    {
        //���⿡�� �Ϲ� ���Ͱ� ������ ���鼭 �ִ� �Լ��� ����. �� 3�� �������ٰ� 2�� ���ߴ� �����̸� ������
        //X���� Unit�� �����ǿ��� +-unit.moveRange ������ �����δ�.
        unit.isMove = true;
        destination = unit.SetRandomPosition();
        //unit.InitMoveSpeed();
        unit.moveSpeed = 1f;

    //���� �ϴ� ����ǥ�� �����.
    unit.HideExclamationMark();
    }


    public void Update(Unit unit)
    {
        //2�� ��ٸ�
        if (isWaiting)
        {
            Debug.Log("�ٽ� ������");
            waitTimer -= Time.deltaTime;
            //2�� �Ŀ��� �ٽ� ������ �� �ְԲ� �ؾ��Ѵ�.
            if (waitTimer <= 0)
            {
                isWaiting = false;
                destination = unit.SetRandomPosition();
            }
        }
        //�ٽ� ������
        else
        {
            //ó�� ���� ���� ��ġ�� �����̸� ��� ���� �� �ٽ� ������
            unit.UnitMove(destination);
            unit.UnitRotation(destination);
            //Debug.Log($"�������� �־��� ��ǥ : {destination}");
            if (Vector3.Distance(unit.transform.position, destination) < 0.1f)
            {
                //���⼭ ��� �����ִ� �Լ� �־�����.
                Debug.Log("�̵���");
                isWaiting = true;
                waitTimer = waitTime;
            }
        }

        //Debug.Log($"���� NormalIdleState�� Update�� ����");
        //���⿡�� �÷��̾ �߰��ϸ� ����ǥ�� �߸鼭 NormalChaseState�� �ٲ�����
        if (DetectPlayer(unit))
        {
            unit.ChangeState(new NormalChaseState());
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

        //�þ߰� �ȿ� �ְ�
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
        //������ ������? �Ϲ� ���°� �����°� �״� �� ����� ���� �� ������...
    }

}
