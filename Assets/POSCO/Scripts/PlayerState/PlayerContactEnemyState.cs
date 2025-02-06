using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class PlayerContactEnemyState : PlayerStateBase
{
    //���� ����
    private Unit unit;

    public PlayerContactEnemyState(Player player, Unit unit) : base(player)
    {
        //������ ����ȭ
        this.unit = unit;
    }

    public override void Enter()
    {
        //�׽�Ʈ
        Debug.Log("ContactEnemyEnter�� �ٽ� ����");

        //���� ������ ������ ���� �� ������Ѵ�.
        player.canMove = false;
        //unit.isMove = false;
        //���� ������ ���� �����ٴ� UI�� �������
        //uiPopup.EnemyContactCanvasOpen();
        //���� ���� GameManager���� �÷��̾��� ������ ���� ������ ����ȭ

        //������ ��ư�� ���� ����
        var buttons = new Dictionary<string, UnityAction>
        {
            { 
                "�ο��!", () =>
                {
                    StartBattle(); 
                } 
            },
            { 
                "��������", () =>
                { 
                    RunAway();
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"{unit.name} (��)�� ��Ÿ���� ������ �ұ�?",
            buttons
        );
    }

    private void StartBattle()
    {
        //���� �÷��̾ ����ִ� ���Ϳ� ���� ������ ������ �Ѱ���
        gameManager.SetNormalMonsterInformation(MonsterDataManager.Instance.selectedMonsterDataList, unit);
        //foreach(var selectedMonster in player.selectedMonsterList)
        //{

        //    Debug.Log($"StartBattle���� �Ѿ�� �÷��̾ ������ ���� ��ġ : {selectedMonster.transform.position}");
        //}

        //�÷��̾��� ������, �������� ������ �Ѱ���
        player.ChangeState(new PlayerNormalBattleState(player, unit));

        Debug.Log("������ ����");
    }

    private void RunAway()
    {
        player.ChangeState(new PlayerIdleState(player));
        Debug.Log("������");

        unit.ChangeState(new NormalIdleState());

        unit.isMove = true;
        Debug.Log("���Ͱ� Idle ���·� ��ȯ��");
    }

    //Update���� ���� ���°� ����
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        //���� ������� ������ �� �ִ�.
        //player.canMove = true;
        //uiPopup.enemyContactCanvas.SetActive(false);
        UIPopupManager.Instance.ClosePopup();
    }

    //�̰͵� ���� ���°� ����.
    public override void HandleCollision(Collision collision)
    {
        
    }

}
