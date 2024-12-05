using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    //player와 연결
    public PlayerIdleState(Player player) : base(player) { }

    public override void Enter()
    {
        //움직일 수 있어야한다
        player.canMove = true;
        //UI를 다 꺼야한다. -> 이건 나중에 Inventory랑은 다른 UI들이니 헷갈리지 말자.
        //uiPopup.AllCanvasClose();
    }

    public override void Update()
    {
        //여기에 Player의 움직임을 관리할 메서드 생성
        player.HandleMovement();
    }

    public override void Exit()
    {
        //움직임 제한
        player.canMove = false;
    }

    public override void HandleCollision(Collision collision)
    {
        if (player.selectedMonsterList.Count < 3)
        {
            Debug.Log("플레이어 포켓몬이 3마리 이하입니다");
            return;
        }
        foreach(Monster selectedMonster in player.selectedMonsterList)
        {
            if (selectedMonster.hp <= 0)
            {
                Debug.Log("체력이 0 이하인 몬스터가 있다");
                return;
            }
        }

        //만난적의 정보가 PlayerContactEnemyState에 넘어감
        if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        {
            if (unit.CompareTag("Unit"))
            {
                player.ChangeState(new PlayerContactEnemyState(player, unit));
                //GameManager.Instance.contactedFieldMonster = unit.GetComponent<Monster>().gameObject;
            }

            else if (unit.CompareTag("Boss"))
            {
                player.ChangeState(new PlayerContactBossState(player, unit));
            }
        }
    }
}
