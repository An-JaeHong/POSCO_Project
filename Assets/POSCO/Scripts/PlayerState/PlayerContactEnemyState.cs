using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class PlayerContactEnemyState : PlayerStateBase
{
    //만난 유닛
    private Unit unit;

    public PlayerContactEnemyState(Player player, Unit unit) : base(player)
    {
        //만난적 동기화
        this.unit = unit;
    }

    public override void Enter()
    {
        //테스트
        Debug.Log("ContactEnemyEnter에 다시 들어옴");

        //적과 만나는 순간은 만날 수 없어야한다.
        player.canMove = false;
        //unit.isMove = false;
        //적과 만나면 적과 만났다는 UI를 띄워야함
        //uiPopup.EnemyContactCanvasOpen();
        //만난 순간 GameManager에게 플레이어의 정보와 적의 정보를 동기화

        //생성할 버튼에 대한 정보
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
            $"{unit.name} is appear!, what do you do?",
            buttons
        );
    }

    private void StartBattle()
    {
        //현재 플레이어가 들고있는 몬스터와 만난 몬스터의 정보를 넘겨줌
        gameManager.SetNormalMonsterInformation(player, unit);
        //foreach(var selectedMonster in player.selectedMonsterList)
        //{

        //    Debug.Log($"StartBattle에서 넘어온 플레이어가 선택한 몬스터 위치 : {selectedMonster.transform.position}");
        //}

        //플레이어의 정보와, 만난적의 정보를 넘겨줌
        player.ChangeState(new PlayerNormalBattleState(player, unit));

        Debug.Log("전투에 들어옴");
    }

    private void RunAway()
    {
        player.ChangeState(new PlayerIdleState(player));
        Debug.Log("도망감");

        unit.ChangeState(new NormalIdleState());

        unit.isMove = true;
        Debug.Log("몬스터가 Idle 상태로 전환됨");
    }

    //Update에는 딱히 쓰는게 없다
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        //적과 헤어지면 움직일 수 있다.
        //player.canMove = true;
        //uiPopup.enemyContactCanvas.SetActive(false);
        UIPopupManager.Instance.ClosePopup();
    }

    //이것도 딱히 쓰는게 없다.
    public override void HandleCollision(Collision collision)
    {
        
    }

}
