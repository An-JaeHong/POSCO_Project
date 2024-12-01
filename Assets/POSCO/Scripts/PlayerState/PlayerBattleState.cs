using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBattleState : PlayerStateBase
{
    //�׽�Ʈ ��
    private List<Monster> playerMonsterList = new List<Monster>();

    private Monster enemyMonster;
    public PlayerBattleState(Player player, Monster enemy) : base(player) 
    {
        //�� ����ȭ
        enemyMonster = enemy;
    }

    public override void Enter()
    {
        TurnManager.Instance.monsterTurnChange += OnMonsterTurnChange;
        //������ ����
        //1. �ʵ忡 �ִ� �÷��̾� ������ ����
        //2. ī�޶� ��Ʋ������ ��ü
        //3. ���� ������ ������ �÷��̾��� ���� ������ GameManager���� �Ѱ���
        //4. �����ϴ� UI����
        player.canMove = false;
        CameraManager.Instance.HandleCamera(CameraType.BattleMap);
        //gameManager.SetMonsterOnBattlePosition();

        //�÷��̾� ���϶� ����ִ� �˾�
        //ShowPlayerTurnPopup();

        //�׽�Ʈ
        playerMonsterList = GameManager.Instance.playerMonsterInBattleList;
        foreach(var playerMonster in playerMonsterList)
        {
            Debug.Log($"���� �̷��� �Ѱ� ������ �ߴ� ��ġ : {playerMonster.transform.position}");
        }
    }

    //������ ���� �ٲ� ���� �ƴ϶�� �˾��� �����
    private void OnMonsterTurnChange(Monster currentMonster)
    {
        if (!currentMonster.isEnemy)
        {
            //�׽�Ʈ��
            Debug.Log($"���� ���ʹ� �����ΰ�? : {currentMonster}");
            Debug.Log($"���� ���� ��ġ��? : {currentMonster.transform.position}");

            ShowPlayerTurnPopup(currentMonster);
        }
    }

    private void ShowPlayerTurnPopup(Monster currentMonster)
    {
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
            $"{currentMonster.name}'s Turn. What do you do?",
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
                    DoAttackTarget(0);
                }
            },
            {
                "Second", () =>
                {
                    DoAttackTarget(1);
                }
            },
            {
                "Third", () =>
                {
                    DoAttackTarget(2);
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"ChooseTarget!",
            buttons
            );
    }

    private void DoAttackTarget(int targetnum)
    {
        //���� �� ������ �� �� �������
        if (targetnum < TurnManager.Instance.enemyMonsterList.Count)
        {
            Monster target = TurnManager.Instance.enemyMonsterList[targetnum];
            if (target.hp > 0)
            {
                GameManager.Instance.ExecutePlayerAttackAction(TurnManager.Instance.currentTurnMonster, target);
            }
            else
            {
                Debug.Log("�̹� ������ �����Դϴ�. �ٸ� ���͸� �������ּ���");
            }
        }
        UIPopupManager.Instance.ClosePopup();
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
