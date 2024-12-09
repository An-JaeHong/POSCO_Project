using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNormalBattleState : PlayerStateBase
{
    //�׽�Ʈ ��
    private List<Monster> playerMonsterList = new List<Monster>();

    private Unit unit;
    public PlayerNormalBattleState(Player player, Unit unit) : base(player) 
    {
        //�� ����ȭ
        this.unit = unit;
    }

    //private void TmpBattleEnd()
    //{

    //        player.ChangeState(new PlayerIdleState(player));
    //}

    public override void Enter()
    {
        //�� �ٲ� ���� ȣ���ϴ� �Լ�
        TurnManager.Instance.monsterTurnChange += OnMonsterTurnChange;
        //��Ʋ ����Ǹ� ȣ���ϴ� �Լ�
        TurnManager.Instance.OnBattleEnd += HandleBattleEnd;
        //TurnManager.Instance.OnBattleEnd += TmpBattleEnd;
        //������ ����
        //1. �ʵ忡 �ִ� �÷��̾� ������ ����
        //2. ī�޶� ��Ʋ������ ��ü
        //3. ���� ������ ������ �÷��̾��� ���� ������ GameManager���� �Ѱ���
        //4. �����ϴ� UI����
        player.canMove = false;
        CameraManager.Instance.HandleCamera(CameraType.BattleMap);
        TurnManager.Instance.InitializeTurnQueue();
        //gameManager.SetMonsterOnBattlePosition();

        //�÷��̾� ���϶� ����ִ� �˾�
        //ShowPlayerTurnPopup();

        //�׽�Ʈ
        playerMonsterList = GameManager.Instance.playerMonsterInBattleList;

        //�׽�Ʈ
        foreach(var playerMonster in playerMonsterList)
        {
            Debug.Log($"���� �̷��� �Ѱ� ������ �ߴ� ��ġ : {playerMonster.transform.position}");
        }
    }

    //������ ���� �ٲ� ���� �ƴ϶�� �˾��� �����
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
        //��ų�� �ϳ��� �̷��� ���� �����ش�
        if (currentMonster.selectedSkill == null)
        {
            currentMonster.SetSkillNum(0);
        }
        
        //�׽�Ʈ
        if (currentMonster == null)
        {
            Debug.LogError("currentMonster is null");
            return;
        }
        if (currentMonster.selectedSkill == null)
        {
            Debug.LogError("currentMonster.selectedSkill is null");
            return;
        }
        Debug.Log($"Current Monster: {currentMonster.name}, Skill: {currentMonster.selectedSkill.skillName}, Skill Count: {currentMonster.selectedSkill.skillCount}");
    


        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "�⺻����", () =>
                {
                    DoNormalAttack();
                }
            },
            {
                $"{currentMonster.selectedSkill.skillName} \n ���� Ƚ�� : {currentMonster.selectedSkill.skillCount}", () =>
                {
                    //���� ��ų������ 0���� ������ ���� �����ϴٰ� ���������
                    if (currentMonster.selectedSkill.skillCount <= 0)
                    {
                        ShowSkillCountNotEnough(currentMonster);
                    }
                    else
                    {
                        //��ų Ÿ�� �����ְ�
                    TurnManager.Instance.currentTurnMonster.attackType = AttackType.Skill1;
                        //��ų ���Ƚ�� �ٿ��ְ�
                    currentMonster.selectedSkill.skillCount -= 1;
                        //Ÿ�� ����
                    ChooseTarget();
                    }
                }
            },
            {
                "ȸ���ϱ�", () =>
                {
                    DoHeal(currentMonster);
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"Lv : {currentMonster.level}. {currentMonster.name}�� ���̴� ������ �ұ�?\n���� ü�� : {currentMonster.hp} / {currentMonster.maxHp}, ���ݷ� : {currentMonster.damage}",
            buttons
            );
    }

    private void ShowSkillCountNotEnough(Monster currentMonster)
    {
        Debug.Log("��ų�� �����մϴ�");
        var button = new Dictionary<string, UnityAction>
        {
            {
                "Ȯ��", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    ShowPlayerTurnPopup(currentMonster);
                }
            }
        };
        UIPopupManager.Instance.ShowPopup(
            "��ų ��� ���� Ƚ���� �����մϴ�.",
            button
            );
    }

    private void ShowHealItemCountNotEnough(Monster currentMonster)
    {
        Debug.Log("������ ������ �����մϴ�");
        var button = new Dictionary<string, UnityAction>
        {
            {
                "Ȯ��", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    ShowPlayerTurnPopup(currentMonster);
                }
            }
        };
        UIPopupManager.Instance.ShowPopup(
            "������ ������ �����մϴ�.",
            button
            );
    }

    //�⺻����
    private void DoNormalAttack()
    {
        //if (TurnManager.Instance.currentTurnMonster.selectedSkill.skillCount >= 1)
        //{
            //���� �����ϴ� Monster�� ���� Ÿ���� �⺻���� Ÿ������ �ٲٰ�
            TurnManager.Instance.currentTurnMonster.attackType = AttackType.NormalAttack;
            //Ÿ���� ����
            ChooseTarget();
        //}
        //else
        //{
        //    #region ��ųī��Ʈ�� �����ϴٴ� UI�� ������
        //    var buttons = new Dictionary<string, UnityAction> { };
        //    UIPopupManager.Instance.ShowPopup(
        //    $"{TurnManager.Instance.currentTurnMonster.selectedSkill} skillcount is 0",
        //    buttons
        //    );
        //    #endregion
        //}
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
        ChooseTarget();
    }

    private void SelectSkill(int skillNum)
    {
        switch(skillNum)
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

            var buttons = new Dictionary<string, UnityAction>();

        for (int i = 0; i < aliveTargetList.Count; i++)
        {
            int index = TurnManager.Instance.enemyMonsterList.IndexOf(aliveTargetList[i]);
            string targetName = $"{aliveTargetList[i].name}({index + 1}), ü�� {aliveTargetList[i].hp}";

            buttons.Add(
                    targetName,
                    () =>
                    {
                        DoAttackTarget(index);
                    }
            );
        }
        UIPopupManager.Instance.ShowPopup(
            $"���� �����ұ�?",
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

    private void DoHeal(Monster currentTurnMonster)
    {
        //��ư ����
        var buttons = new Dictionary<string, UnityAction>
        {
            {
                $"�ϱ� ü�¹��� ({player.uiInventory.potionNum[0]}��)\nü���� 20�� ȸ���մϴ�", () =>
                {
                    if (player.uiInventory.potionNum[0] <= 0)
                    {
                        ShowHealItemCountNotEnough(currentTurnMonster);
                    }
                    else
                    {
                    player.UseItem(0, currentTurnMonster);
                    HealActionIsDone(currentTurnMonster);
                    }
                }
            },
            {
                $"�߱� ü�¹��� ({player.uiInventory.potionNum[1]}��)\nü���� 40�� ȸ���մϴ�", () =>
                {
                    if (player.uiInventory.potionNum[1] <= 0)
                    {
                        ShowHealItemCountNotEnough(currentTurnMonster);
                    }
                    else
                    {
                    player.UseItem(1, currentTurnMonster);
                    HealActionIsDone(currentTurnMonster);
                    }
                }
            },
            {
                $"��� ������ ({player.uiInventory.potionNum[2]}��)\nü���� ������ ȸ���մϴ�", () =>
                {
                    if (player.uiInventory.potionNum[2] <= 0)
                    {
                        ShowHealItemCountNotEnough(currentTurnMonster);
                    }
                    else
                    {
                    player.UseItem(2, currentTurnMonster);
                    HealActionIsDone(currentTurnMonster);
                    }
                }
            }
        };
        //UI����
        UIPopupManager.Instance.ShowPopup(
            $"� ������ ����Ͻðڽ��ϱ�?",
            buttons
            );
    }

    //update�� �������� �ʿ����.
    public override void Update()
    {
        
    }

    //���� ������ Ȯ���� �ؾ��Ѵ�.
    public void HealActionIsDone(Monster currentTurnMonster)
    {

        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "Ȯ��", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    GameManager.Instance.isPlayerActionComplete = true;
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"{currentTurnMonster.name}�� ü���� ȸ���Ǿ����ϴ�.\n���� ü�� : {currentTurnMonster.hp}",
            buttons
            );
    }

    
    public override void Exit()
    {
        Debug.Log("Exit 1");

        //TurnManager.Instance.OnBattleEnd -= Exit;
        TurnManager.Instance.OnBattleEnd -= HandleBattleEnd;
        TurnManager.Instance.monsterTurnChange -= OnMonsterTurnChange;
        Debug.Log("Exit 2");
        //player.canMove = true;
        //uiPopup.chooseBattleStateCanvas.SetActive(false);
        //uiPopup.chooseTargetCanvas.SetActive(false);
        CameraManager.Instance.HandleCamera(CameraType.FieldMap);
        Debug.Log("Exit 3");

        //������ ������ ��� UI�� �ݾƾ��Ѵ�
        UIPopupManager.Instance.ClosePopup();

        GameManager.Instance.InitializePlayerMonsterData();

        //�׽�Ʈ ��
        foreach (Monster temp in MonsterDataManager.Instance.selectedMonsterDataList)
        {
            Debug.Log($"���õ� �÷��̾� ������ {temp.name}�� ���� ü�� : {temp.hp}");
        }
        foreach (Monster temp in MonsterDataManager.Instance.allMonsterDataList)
        {
            Debug.Log($"��� �÷��̾� ������ {temp.name}�� ���� ü�� : {temp.hp}");
        }
        // �� �κж����� �������� ���
        //player.ChangeState(new PlayerIdleState(player));
        Debug.Log("Exit 4");
    }

    private void HandleBattleEnd()
    {
        //���� ��ȯ��Ű��
        player.ChangeState(new PlayerIdleState(player));

        CalculateExperience();
        //���� ��� ���� ������ �ۿ� �ִ� ���͸� �������Ѿ��Ѵ�.
        if (TurnManager.Instance.allEnemyMonstersDead == true)
        {
            foreach (Monster playerMonster in MonsterDataManager.Instance.selectedMonsterDataList)
            {
                playerMonster.GetExp(totalExperience);
            }
            //�ۿ��ִ� ���ʹ� ����
            GameObject.Destroy(GameManager.Instance.contactedFieldMonster);
        }
        else if (TurnManager.Instance.allPlayerMonstersDead == true)
        {
            Debug.Log("�÷��̾ ����");
        }

        //���� �������� �������¸� �ʱ�ȭ ��������Ѵ�.
        GameManager.Instance.InitializeUnitMonsterData(unit);
        GameManager.Instance.InitializeBattleState();
    }

    public int totalExperience;

    //�� ����ġ�� ����ϴ� �Լ�
    public int CalculateExperience()
    {
        for (int i = 0; i < TurnManager.Instance.enemyMonsterList.Count; i++)
        {
            totalExperience += TurnManager.Instance.enemyMonsterList[i].levelPerExp;
        }
        return totalExperience;
    }

    //���⼭�� �ʿ����.
    public override void HandleCollision(Collision collision)
    {
        
    }
}
