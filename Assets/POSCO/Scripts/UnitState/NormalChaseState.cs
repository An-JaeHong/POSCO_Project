using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unit�� �þ߰��� ������ ��� �Ѿƿ;���
public class NormalChaseState : IUnitState
{
    private GameObject player;
    public void Enter(Unit unit)
    {
        //�Ѿƿ��� ���°� �Ǹ� ����ǥ�� ��� �������
        unit.ShowExclamationMark();
        //player = GameObject.FindGameObjectWithTag("Player");
        //if (player == null)
        //{
        //    Debug.LogError("�÷��̾ ã�� �� �����ϴ�.");
        //}
    }

    public void Update(Unit unit)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�.");
        }
        //player ã��
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //�÷��̾ �ִ� ����
            //Vector3 directionToPlayer = (player.transform.position - unit.transform.position).normalized;
            unit.UnitRotation(player.transform.position);

            //�÷��̾ ã���� �ӵ��� 1.5�� ������
            //unit.SetMoveSpeedFast();
            unit.moveSpeed = 2f;

            unit.UnitMove(player.transform.position);

            //�÷��̾�������� �Ÿ�
            float distance = Vector3.Distance(unit.transform.position, player.transform.position);

            //���� �Ÿ��� �����Ÿ����� �־����� idle�� �ٲ۴�
            if (distance > unit.detectRange)
            {
                unit.ChangeState(new NormalIdleState());
            }
        }
    }

    public void Exit(Unit unit)
    {
        
    }
}
