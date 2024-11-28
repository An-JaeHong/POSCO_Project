using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContactEnemyState : PlayerStateBase
{
    private Monster enemyMonster;

    public PlayerContactEnemyState(Player player, Monster enemy) : base(player)
    {
        this.enemyMonster = enemy;
    }

    public override void Enter()
    {
        //���� ������ ���� �����ٴ� UI�� �������
        uiPopup.EnemyContactCanvasOpen();
        //���� ���� GameManager���� �÷��̾��� ������ ���� ������ ����ȭ
        gameManager.SetMonsterInformation(player.selectedMonsterList, enemyMonster);
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        uiPopup.
    }

    public override void HandleCollision(Collision collision)
    {
        throw new System.NotImplementedException();
    }

}
