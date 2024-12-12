using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFindPlayerState : IUnitState
{
    private GameObject player;
    public void Enter(Unit unit)
    {
        //unit.ShowExclamationMark();
        unit.ShowBossSpeech("�������̱�!");
        player = GameObject.FindGameObjectWithTag("Player");
        unit.animator.SetBool("IsStanding", true);
    }

    public void Update(Unit unit)
    {
        //�÷��̾�������� �Ÿ��� ����ؼ� �� ����
        float distance = Vector3.Distance(unit.transform.position, player.transform.position);

        //���� �Ÿ��� �����Ÿ����� �־����� idle�� �ٲ۴�
        if (distance > unit.detectRange)
        {
            unit.ChangeState(new BossIdleState());
        }
    }

    public void Exit(Unit unit)
    {
        unit.HideBossSpeech();
    }

}
