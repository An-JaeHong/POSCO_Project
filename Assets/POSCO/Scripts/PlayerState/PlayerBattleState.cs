using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBattleState : PlayerStateBase
{
    public PlayerBattleState(Player player) : base(player) { }

    public override void Enter()
    {
        //전투에 들어가면
        //1. 필드에 있는 플레이어 움직임 정지
        //2. 카메라 배틀맵으로 교체
        //3. 만난 몬스터의 정보와 플레이어의 몬스터 정보를 GameManager에게 넘겨줌
        //4. 선택하는 UI생성
        player.canMove = false;
        CameraManager.Instance.HandleCamera(CameraType.BattleMap);
        gameManager.SetMonsterOnBattlePosition();

        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "DoAttack", () =>
                {
                    DoAttack();
                }
            },
            {
                "DoHeal", () =>
                {
                    DoHeal();
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"asdasdasdasdasd",
            buttons
            );
    }

    //공격하기를 누르면 누굴 공격할지를 선택할 수 있어야한다.
    private void DoAttack()
    {
        //GameManager.Instance.ExecutePlayerAttackAction(GameManager.Instance.currentTurnMonster);
        ChooseTarget();
    }

    //공격 대상 선택
    private void ChooseTarget()
    {

        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "First", () =>
                {
                    DoAttackFirstTarget();
                }
            },
            {
                "Second", () =>
                {
                    DoAttackSecondTarget();
                }
            },
            {
                "Third", () =>
                {
                    DoAttackThirdTarget();
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"ChooseTarget!",
            buttons
            );
    }

    private void DoAttackFirstTarget()
    {
        Debug.Log("첫번째 타겟을 골랐다!");
        Monster target = TurnManager.Instance.enemyMonsterList[0];
        if (target.hp <= 0)
        {
            Debug.Log("이미 죽은 몬스터이다. 다른 몬스터를 골라!");
        }
        else
        {
            GameManager.Instance.ExecutePlayerAttackAction(target);
        }
    }

    private void DoAttackSecondTarget()
    {
        Debug.Log("두번째 타겟을 골랐다!");
        Monster target = TurnManager.Instance.enemyMonsterList[1];
        if (target.hp <= 0)
        {
            Debug.Log("이미 죽은 몬스터이다. 다른 몬스터를 골라!");
        }
        else
        {
            GameManager.Instance.ExecutePlayerAttackAction(target);
        }
    }
    
    private void DoAttackThirdTarget()
    {
        Debug.Log("세번째 타겟을 골랐다!");
        Monster target = TurnManager.Instance.enemyMonsterList[2];
        if (target.hp <= 0)
        {
            Debug.Log("이미 죽은 몬스터이다. 다른 몬스터를 골라!");
        }
        else
        {
            GameManager.Instance.ExecutePlayerAttackAction(target);
        }
    }

    private void DoHeal()
    {

    }

    //update는 아직까진 필요없다.
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        player.canMove = true;
        //uiPopup.chooseBattleStateCanvas.SetActive(false);
        //uiPopup.chooseTargetCanvas.SetActive(false);
        CameraManager.Instance.HandleCamera(CameraType.FieldMap);
    }

    //여기서는 필요없다.
    public override void HandleCollision(Collision collision)
    {
        
    }
}
