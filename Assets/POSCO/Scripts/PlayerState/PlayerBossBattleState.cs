using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBossBattleState : PlayerStateBase
{
    private Unit boss;
    public PlayerBossBattleState(Player player, Unit boss) : base(player)
    {
        this.boss = boss;
    }

    public override void Enter()
    {

        TurnManager.Instance.monsterTurnChange += OnMonsterTurnChange;
        //��Ʋ ����Ǹ� ȣ���ϴ� �Լ�
        TurnManager.Instance.OnBattleEnd += HandleBossBattleEnd;
        player.canMove = false;
        CameraManager.Instance.HandleCamera(CameraType.BossMap);
        TurnManager.Instance.InitializeTurnQueue();
    }

    private void OnMonsterTurnChange(Monster currentMonster)
    {
        //�׽�Ʈ��
        Debug.Log("���� ���Ͱ� ���̶� �ߴ� ������");

        //�÷��̾� ���͸� ������
        if (!currentMonster.isEnemy)
        {
            //�׽�Ʈ��
            Debug.Log($"���� ���ʹ� �����ΰ�? : {currentMonster}");
            Debug.Log($"���� ���� ��ġ��? : {currentMonster.transform.position}");

            ShowPlayerTurnPopup(currentMonster);
        }
    }

    //PlayerTurn�϶� �ʿ��� �˾�â ���� �Լ�
    private void ShowPlayerTurnPopup(Monster currentMonster)
    {
        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "DoNormalAttack", () =>
                {
                    DoNormalAttack();
                }
            },
            {
                "DoSkillAttack", () =>
                {
                    DoSkillAttack();
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

    //�⺻����
    private void DoNormalAttack()
    {
        //���� �����ϴ� Monster�� ���� Ÿ���� �⺻���� Ÿ������ �ٲٰ�
        TurnManager.Instance.currentTurnMonster.attackType = AttackType.NormalAttack;
        //Ÿ���� ����
        ChooseTarget();
    }

    private void DoSkillAttack()
    {
        ////��ų�� ���� �˾��� ������
        //var buttons = new Dictionary<string, UnityAction>
        //{
        //    {
        //        $"{TurnManager.Instance.currentTurnMonster.skills[0].name}", () =>
        //        {
        //            SelectSkill(0);
        //        }
        //    },
        //    {
        //        $"{TurnManager.Instance.currentTurnMonster.skills[1].name}", () =>
        //        {
        //            SelectSkill(1);
        //        }
        //    },
        //};

        //UIPopupManager.Instance.ShowPopup(
        //    $"Choose Skill",
        //    buttons
        //    );
        SelectSkill(0);
    }

    private void SelectSkill(int skillNum)
    {
        switch (skillNum)
        {
            case 0:
                TurnManager.Instance.currentTurnMonster.attackType = AttackType.Skill1;
                break;
            case 1:
                TurnManager.Instance.currentTurnMonster.attackType = AttackType.Skill2;
                break;
        }

        //���� ������ ��ų�� �����ش�
        TurnManager.Instance.currentTurnMonster.SetSkillNum(skillNum);

        //�״��� Ÿ���� ���� �ؾ���
        ChooseTarget();
    }

    //���� ��� ����
    private void ChooseTarget()
    {
        //����ִ� ���� ����Ʈ�� �޴´�
        List<Monster> aliveTargetList = TurnManager.Instance.enemyMonsterList.Where(m => m.hp > 0).ToList();
        //List<string> targetNum = new List<string> { "First", "Second", "Third" };

        //switch (aliveTargetList.Count())
        //{
        //    case 1:
        //        targetNum = "First";
        //        break;
        //    case 2:
        //        targetNum = "Second";
        //        break;
        //    case 3:
        //        targetNum = "Third";
        //        break;
        //}

        var buttons = new Dictionary<string, UnityAction>();

        for (int i = 0; i < aliveTargetList.Count; i++)
        {
            int index = TurnManager.Instance.enemyMonsterList.IndexOf(aliveTargetList[i]);
            string targetNum = $"Target{index + 1}";

            buttons.Add(
                    targetNum,
                    () =>
                    {
                        DoAttackTarget(index);
                    }
            );
        }
        //var buttons = new Dictionary<string, UnityAction>
        //{
        //    {
        //        $"{targetNum}", () =>
        //        {
        //            DoAttackTarget(0);
        //        }
        //    },
        //    {
        //        $"{targetNum}", () =>
        //        {
        //            DoAttackTarget(1);
        //        }
        //    },
        //    {
        //        $"{targetNum}", () =>
        //        {
        //            DoAttackTarget(2);
        //        }
        //    }
        //};

        UIPopupManager.Instance.ShowPopup(
            $"ChooseTarget!",
            buttons
            );
    }

    private void DoAttackTarget(int targetnum)
    {
        //�⺻�����϶�
        if (TurnManager.Instance.currentTurnMonster.attackType == AttackType.NormalAttack)
        {
            //���� �� ������ �� �� �������
            if (targetnum < TurnManager.Instance.enemyMonsterList.Count)
            {
                Monster target = TurnManager.Instance.enemyMonsterList[targetnum];
                if (target.hp > 0)
                {
                    GameManager.Instance.ExecutePlayerNormalAttackAction(TurnManager.Instance.currentTurnMonster, target);
                }
                else
                {
                    Debug.Log("�̹� ������ �����Դϴ�. �ٸ� ���͸� �������ּ���");
                    ChooseTarget();
                }
            }
            //UIPopupManager.Instance.ClosePopup();
        }

        //��ų�����ε� 1�� 2���� ���߿� ������ �� �� �ϴ�
        else if (TurnManager.Instance.currentTurnMonster.attackType == AttackType.Skill1)
        {
            //���� �� ������ �� �� �������
            if (targetnum < TurnManager.Instance.enemyMonsterList.Count)
            {
                Monster target = TurnManager.Instance.enemyMonsterList[targetnum];
                if (target.hp > 0)
                {
                    GameManager.Instance.ExecutePlayerFirstSkillAttackAction(TurnManager.Instance.currentTurnMonster, target);
                }
                else
                {
                    Debug.Log("�̹� ������ �����Դϴ�. �ٸ� ���͸� �������ּ���");
                    ChooseTarget();
                }

            }
            //UIPopupManager.Instance.ClosePopup();
        }
    }

    //private void DoSkillAttackTarget(int targetnum)
    //{
    //    //���� �� ������ �� �� �������
    //    if (targetnum < TurnManager.Instance.enemyMonsterList.Count)
    //    {
    //        Monster target = TurnManager.Instance.enemyMonsterList[targetnum];
    //        if (target.hp > 0)
    //        {
    //            GameManager.Instance.ExecutePlayerSkillAttackAction(TurnManager.Instance.currentTurnMonster, target);
    //        }
    //        else
    //        {
    //            Debug.Log("�̹� ������ �����Դϴ�. �ٸ� ���͸� �������ּ���");
    //        }
    //    }
    //    UIPopupManager.Instance.ClosePopup();
    //}

    private void DoHeal()
    {

    }

    //update�� �������� �ʿ����.
    public override void Update()
    {

    }


    public override void Exit()
    {
        Debug.Log("Exit 1");

        //TurnManager.Instance.OnBattleEnd -= Exit;
        TurnManager.Instance.OnBattleEnd -= HandleBossBattleEnd;
        TurnManager.Instance.monsterTurnChange -= OnMonsterTurnChange;
        Debug.Log("Exit 2");
        //player.canMove = true;
        //uiPopup.chooseBattleStateCanvas.SetActive(false);
        //uiPopup.chooseTargetCanvas.SetActive(false);
        CameraManager.Instance.HandleCamera(CameraType.FieldMap);
        Debug.Log("Exit 3");
        UIPopupManager.Instance.ClosePopup();
        // �� �κж����� �������� ���
        //player.ChangeState(new PlayerIdleState(player));
        Debug.Log("Exit 4");
    }

    private void HandleBossBattleEnd()
    {
        //���� ��ȯ��Ű��
        player.ChangeState(new PlayerIdleState(player));

        //���� ��� ���� ������ �ۿ� �ִ� ���͸� �������Ѿ��Ѵ�. -> ������ �ƴ�
        if (TurnManager.Instance.allEnemyMonstersDead == true)
        {
            //GameObject.Destroy(GameManager.Instance.contactedFieldMonster);
        }
        else if (TurnManager.Instance.allPlayerMonstersDead == true)
        {
            Debug.Log("�÷��̾ ����");
        }

        //���� �������� �������¸� �ʱ�ȭ ��������Ѵ�.
        GameManager.Instance.InitializeUnitMonsterData(boss);
        GameManager.Instance.InitializeBattleState();
    }

    //���⼭�� �ʿ����.
    public override void HandleCollision(Collision collision)
    {

    }
}
