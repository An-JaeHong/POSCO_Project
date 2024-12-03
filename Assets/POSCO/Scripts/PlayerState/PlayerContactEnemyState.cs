using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class PlayerContactEnemyState : PlayerStateBase
{
    //���� ����
    private Monster enemyMonster;

    public PlayerContactEnemyState(Player player, Monster enemy) : base(player)
    {
        //������ ����ȭ
        this.enemyMonster = enemy;
    }

    public override void Enter()
    {
        //�׽�Ʈ
        Debug.Log("ContactEnemyEnter�� �ٽ� ����");

        //���� ������ ������ ���� �� ������Ѵ�.
        player.canMove = false;
        //���� ������ ���� �����ٴ� UI�� �������
        //uiPopup.EnemyContactCanvasOpen();
        //���� ���� GameManager���� �÷��̾��� ������ ���� ������ ����ȭ

        //������ ��ư�� ���� ����
        var buttons = new Dictionary<string, UnityAction>
        {
            { 
                "Fight", () =>
                {
                    StartBattle(); 
                } 
            },
            { 
                "RunAway", () =>
                { 
                    RunAway();
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"{enemyMonster.name} is appear!, what do you do?",
            buttons
        );
    }

    private void StartBattle()
    {
        //���� �÷��̾ ����ִ� ���Ϳ� ���� ������ ������ �Ѱ���
        gameManager.SetMonsterInformation(player, enemyMonster);
        //foreach(var selectedMonster in player.selectedMonsterList)
        //{

        //    Debug.Log($"StartBattle���� �Ѿ�� �÷��̾ ������ ���� ��ġ : {selectedMonster.transform.position}");
        //}

        //�÷��̾��� ������, �������� ������ �Ѱ���
        player.ChangeState(new PlayerBattleState(player, enemyMonster));

        Debug.Log("������ ����");
    }

    private void RunAway()
    {
        player.ChangeState(new PlayerIdleState(player));
        Debug.Log("������");
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
