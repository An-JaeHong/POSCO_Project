using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerContactBossState : PlayerStateBase
{
    private Unit boss;
    public PlayerContactBossState(Player player, Unit boss) : base(player)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        Debug.Log("PlayerContactBossState�� �ٽ� ����");

        player.canMove = false;

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
            $"{boss.name} is appear!, what do you do?",
            buttons
        );
    }

    private void StartBattle()
    {
        gameManager.SetBossInformation(player, boss);
        player.ChangeState(new PlayerBossBattleState(player, boss));
    }

    private void RunAway()
    {
        player.ChangeState(new PlayerIdleState(player));
        Debug.Log("������");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        UIPopupManager.Instance.ClosePopup();
    }

    public override void HandleCollision(Collision collision)
    {

    }
}

