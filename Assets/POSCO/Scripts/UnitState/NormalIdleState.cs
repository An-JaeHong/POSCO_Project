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
    private Vector3 destination;
    private float waitTime = 2f; //���ð�
    private float waitTimer = 0;
    private bool isWaiting = false;
    private bool hasRandomPosition = true;

    private Vector3 lastPosition; //�ֱ� ��ġ
    private float stuckCheckTime = 0.5f; //üũ �ֱ�
    private float stuckCheckTimer = 0;
    private float minMovementDistance = 0.1f; //�ּ� �̵��Ÿ� -> �̰ɷ� �� �� �Ǻ�
    

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
        lastPosition = unit.transform.position; //�ʱ� ��ġ ����

    //���� �ϴ� ����ǥ�� �����.
    unit.HideExclamationMark();
    }

    private float updateInterval = 0.5f; // ������Ʈ �ֱ�
    private float updateTimer = 0f;

    public void Update(Unit unit)
    {
        //����
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            unit.animator.SetBool("IsMoving", false);

            if (waitTimer <= 0)
            {
                isWaiting = false;
                destination = unit.SetRandomPosition();
            }
        }
        //�ٽ� ������
        else
        {
            unit.UnitMove(destination);
            unit.UnitRotation(destination);

            stuckCheckTimer += Time.deltaTime;
            if (stuckCheckTimer >= stuckCheckTime)
            {
                float moveDistance = Vector3.Distance(unit.transform.position, lastPosition);

                if (moveDistance < minMovementDistance)
                {
                    Debug.Log("���� ��� ��ġ�� �ٽ� ��´�");
                    destination = unit.SetRandomPosition();
                }

                stuckCheckTimer = 0;
                lastPosition = unit.transform.position;
            }

            Vector3 unitXZPosition = new Vector3(unit.transform.position.x, destination.y, unit.transform.position.z);

            if (Vector3.Distance(unitXZPosition, destination) < 0.1f)
            {
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

        if (angle < unit.sightAngle / 2)
        {
            if (Physics.Raycast(unit.transform.position, directionToPlayer, out RaycastHit hit, unit.detectRange, LayerMask.GetMask("Player")))
            {
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
