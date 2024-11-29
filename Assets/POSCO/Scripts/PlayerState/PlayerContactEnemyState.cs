using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContactEnemyState : PlayerStateBase
{
    //만난 몬스터
    private Monster enemyMonster;

    public PlayerContactEnemyState(Player player, Monster enemy) : base(player)
    {
        this.enemyMonster = enemy;
    }

    public override void Enter()
    {
        //적과 만나는 순간은 만날 수 없어야한다.
        player.canMove = false;
        //적과 만나면 적과 만났다는 UI를 띄워야함
        uiPopup.EnemyContactCanvasOpen();
        //만난 순간 GameManager에게 플레이어의 정보와 적의 정보를 동기화
        gameManager.SetMonsterInformation(player.selectedMonsterList, enemyMonster);
    }

    //Update에는 딱히 쓰는게 없다
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        //적과 헤어지면 움직일 수 있다.
        player.canMove = true;
        uiPopup.enemyContactCanvas.SetActive(false);
    }

    //이것도 딱히 쓰는게 없다.
    public override void HandleCollision(Collision collision)
    {
        
    }

}
