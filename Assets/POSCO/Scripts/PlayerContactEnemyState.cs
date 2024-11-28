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
        //적과 만나면 적과 만났다는 UI를 띄워야함
        uiPopup.EnemyContactCanvasOpen();
        //만난 순간 GameManager에게 플레이어의 정보와 적의 정보를 동기화
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
