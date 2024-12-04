using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unit�� �þ߰��� ������ ��� �Ѿƿ;���
public class NormalChaseState : IUnitState
{
    public void Enter(Unit unit)
    {
        //�Ѿƿ��� ���°� �Ǹ� ����ǥ�� ��� �������
        unit.ShowExclamationMark();
    }

    public void Update(Unit unit)
    {
        //player ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //�÷��̾ �ִ� ����
            Vector3 directionToPlayer = (player.transform.position - unit.transform.position).normalized;

            //�÷��̾ ã���� �ӵ��� 1.5�� ������
            float followPlayerSpeed = unit.moveSpeed * 1.5f;
            Vector3 velocity = directionToPlayer * followPlayerSpeed;


            unit.characterController.Move(velocity * Time.deltaTime);

            //�÷��̾�������� �Ÿ�
            float distance = Vector3.Distance(unit.transform.position, player.transform.position);

            //���� �Ÿ��� �����Ÿ����� �־����� idle�� �ٲ۴�
            if (distance > unit.detectRange)
            {
                unit.ChangeState(new NormalIdleState());
                unit.HideExclamationMark();
            }
        }
    }

    public void Exit(Unit unit)
    {
        
    }

}
