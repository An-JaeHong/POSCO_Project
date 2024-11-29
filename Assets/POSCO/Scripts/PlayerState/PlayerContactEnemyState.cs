using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        uiPopup.EnemyContactCanvasOpen();
        //���� ���� GameManager���� �÷��̾��� ������ ���� ������ ����ȭ
        gameManager.SetMonsterInformation(player.selectedMonsterList, enemyMonster);
    }

    //Update���� ���� ���°� ����
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        //���� ������� ������ �� �ִ�.
        player.canMove = true;
        uiPopup.enemyContactCanvas.SetActive(false);
    }

    //�̰͵� ���� ���°� ����.
    public override void HandleCollision(Collision collision)
    {
        
    }

}
