using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

//�Ϲ� ���� Idle ���� -> MonoBehaviour�� ��ü�� �������ؼ� �������̽��� �����.
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
        Debug.Log("Idle���·� ����");
        //���⿡�� �Ϲ� ���Ͱ� ������ ���鼭 �ִ� �Լ��� ����. �� 3�� �������ٰ� 2�� ���ߴ� �����̸� ������
        //X���� Unit�� �����ǿ��� +-unit.moveRange ������ �����δ�.
        unit.isMove = true;
        destination = unit.SetRandomPosition();
        //unit.InitMoveSpeed();
        unit.moveSpeed = 1f;

    //���� �ϴ� ����ǥ�� �����.
    unit.HideExclamationMark();
    }

    private float updateInterval = 0.5f; // ������Ʈ �ֱ�
    private float updateTimer = 0f;

    public void Update(Unit unit)
    {
        //updateTimer += Time.deltaTime;
        //if (updateTimer >= updateInterval)
        //{
        //    updateTimer = 0f;
        //    PerformUpdate(unit);
        //}
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
   
    //private void PerformUpdate(Unit unit)
    //{
        
    //    //2�� ��ٸ�
    //    if (isWaiting)
    //    {
    //        Debug.Log("�ٽ� ������");
    //        waitTimer -= Time.deltaTime;
    //        //2�� �Ŀ��� �ٽ� ������ �� �ְԲ� �ؾ��Ѵ�.
    //        if (waitTimer <= 0)
    //        {
    //            isWaiting = false;
    //            destination = unit.SetRandomPosition();
    //        }
    //    }
    //    //�ٽ� ������
    //    else
    //    {
    //        //ó�� ���� ���� ��ġ�� �����̸� ��� ���� �� �ٽ� ������
    //        unit.UnitMove(destination);
    //        unit.UnitRotation(destination);
    //        //Debug.Log($"�������� �־��� ��ǥ : {destination}");
    //        if (Vector3.Distance(unit.transform.position, destination) < 0.1f)
    //        {
    //            //���⼭ ��� �����ִ� �Լ� �־�����.
    //            Debug.Log("�̵���");
    //            isWaiting = true;
    //            waitTimer = waitTime;
    //        }
    //    }

    //    //Debug.Log($"���� NormalIdleState�� Update�� ����");
    //    //���⿡�� �÷��̾ �߰��ϸ� ����ǥ�� �߸鼭 NormalChaseState�� �ٲ�����
    //    if (DetectPlayer(unit))
    //    {
    //        unit.ChangeState(new NormalChaseState());
    //        Debug.Log("IdleState���� ChaseState�� ��");

    //        Debug.Log("������");
    //        return;
    //    }
    //}

    private bool DetectPlayer(Unit unit)
    {
        //�÷��̾��� �±װ� Player�� �Ǿ��־�� �Ѵ�
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //�׽�Ʈ
        if (player == null)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�.");
            return false;
        }

        Debug.Log("before UnitSight(unit, player)");
        //�÷��̾ ������
        if (UnitSight(unit, player))
        {
            Debug.Log("�÷��̾� ������");
            return true;
        }

        return false;
    }
    
    //Vector3 directionToPlayer = Vector3.zero;
    private bool UnitSight(Unit unit, GameObject player)
    {
        //�÷��̾����� ���� ����
        Vector3 directionToPlayer = (player.transform.position + new Vector3(0, 0.5f, 0) - unit.transform.position).normalized;
        //���� = (������ ���� ���� ����, �÷��̾ ���� ����)
        float angle = Vector3.Angle(unit.transform.forward, directionToPlayer);

        Debug.Log("1");
        //�þ߰� �ȿ� �ְ�
        if (angle < unit.sightAngle / 2)
        {
            Debug.Log($"2 : {directionToPlayer}, {unit.detectRange}");
            //���� �����ϰ�
            if (Physics.Raycast(unit.transform.position, directionToPlayer, out RaycastHit hit, unit.detectRange, LayerMask.GetMask("Player")))
            {
                Debug.Log($"hit : {hit}");
                Debug.Log($"hit2 : {hit.collider.name}");
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
