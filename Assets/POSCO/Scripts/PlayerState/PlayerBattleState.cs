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
        //������ ����
        //1. �ʵ忡 �ִ� �÷��̾� ������ ����
        //2. ī�޶� ��Ʋ������ ��ü
        //3. ���� ������ ������ �÷��̾��� ���� ������ GameManager���� �Ѱ���
        //4. �����ϴ� UI����
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

    //�����ϱ⸦ ������ ���� ���������� ������ �� �־���Ѵ�.
    private void DoAttack()
    {
        //GameManager.Instance.ExecutePlayerAttackAction(GameManager.Instance.currentTurnMonster);
        ChooseTarget();
    }

    //���� ��� ����
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
        Debug.Log("ù��° Ÿ���� �����!");
        Monster target = TurnManager.Instance.enemyMonsterList[0];
        if (target.hp <= 0)
        {
            Debug.Log("�̹� ���� �����̴�. �ٸ� ���͸� ���!");
        }
        else
        {
            GameManager.Instance.ExecutePlayerAttackAction(target);
        }
    }

    private void DoAttackSecondTarget()
    {
        Debug.Log("�ι�° Ÿ���� �����!");
        Monster target = TurnManager.Instance.enemyMonsterList[1];
        if (target.hp <= 0)
        {
            Debug.Log("�̹� ���� �����̴�. �ٸ� ���͸� ���!");
        }
        else
        {
            GameManager.Instance.ExecutePlayerAttackAction(target);
        }
    }
    
    private void DoAttackThirdTarget()
    {
        Debug.Log("����° Ÿ���� �����!");
        Monster target = TurnManager.Instance.enemyMonsterList[2];
        if (target.hp <= 0)
        {
            Debug.Log("�̹� ���� �����̴�. �ٸ� ���͸� ���!");
        }
        else
        {
            GameManager.Instance.ExecutePlayerAttackAction(target);
        }
    }

    private void DoHeal()
    {

    }

    //update�� �������� �ʿ����.
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

    //���⼭�� �ʿ����.
    public override void HandleCollision(Collision collision)
    {
        
    }
}
