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
        this.enemyMonster = enemy;
    }

    public override void Enter()
    {
        //���� ������ ������ ���� �� ������Ѵ�.
        player.canMove = false;
        //���� ������ ���� �����ٴ� UI�� �������
        //uiPopup.EnemyContactCanvasOpen();
        //���� ���� GameManager���� �÷��̾��� ������ ���� ������ ����ȭ

        //������ ��ư�� ���� ����
        var buttons = new Dictionary<string, UnityAction>
        {
            { "Fight", () =>
                {
                    StartBattle(); 
                } 
            },
            { "RunAway", () =>
                { 
                    RunAway();
                }
            }
        };

        foreach (var button in buttons)
        {
            if (button.Value == null)
            {
                Debug.LogError($"Callback for button {button.Key} is null");
            }
        }

        UIPopupManager.Instance.ShowPopup(
            $"{enemyMonster.name} is appear!, what do you do?",
            buttons
        );

        UIPopupManager.Instance.ShowPopup(
            $"{enemyMonster.name} is appear!, what do you do?",
            buttons
            );
    }

    private void StartBattle()
    {
        gameManager.SetMonsterInformation(player.selectedMonsterList, enemyMonster);

        player.ChangeState(new PlayerBattleState(player));

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
        player.canMove = true;
        //uiPopup.enemyContactCanvas.SetActive(false);
        UIPopupManager.Instance.ClosePopup();
    }

    //�̰͵� ���� ���°� ����.
    public override void HandleCollision(Collision collision)
    {
        
    }

}
