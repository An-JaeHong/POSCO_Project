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
        //배틀 종료되면 호출하는 함수
        TurnManager.Instance.OnBattleEnd += HandleBossBattleEnd;
        player.canMove = false;
        CameraManager.Instance.HandleCamera(CameraType.BossMap);
        TurnManager.Instance.InitializeTurnQueue();
    }

    private void OnMonsterTurnChange(Monster currentMonster)
    {
        //테스트용
        Debug.Log("현재 몬스터가 적이라서 뜨는 문구다");

        //플레이어 몬스터만 실행함
        if (!currentMonster.isEnemy)
        {
            //테스트용
            Debug.Log($"현재 몬스터는 누구인가? : {currentMonster}");
            Debug.Log($"현재 몬스터 위치는? : {currentMonster.transform.position}");

            ShowPlayerTurnPopup(currentMonster);
        }
    }

    //PlayerTurn일때 필요한 팝업창 띄우는 함수
    private void ShowPlayerTurnPopup(Monster currentMonster)
    {
        if (currentMonster.selectedSkill == null)
        {
            currentMonster.SetSkillNum(0);
        }

        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "기본공격", () =>
                {
                    DoNormalAttack();
                }
            },
            {
                $"{currentMonster.selectedSkill.skillName} \n 남은 횟수 : {currentMonster.selectedSkill.skillCount}", () =>
                {
                    //만약 스킬개수가 0보다 작으면 숫자 부족하다고 말해줘야함
                    if (currentMonster.selectedSkill.skillCount <= 0)
                    {
                        ShowSkillCountNotEnough(currentMonster);
                    }
                    else
                    {
                    TurnManager.Instance.currentTurnMonster.attackType = AttackType.Skill1;
                    ChooseTarget();
                    }
                }
            },
            {
                "회복하기", () =>
                {
                    DoHeal(currentMonster);
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"Lv : {currentMonster.level}. {currentMonster.name}의 턴이다 무엇을 할까?\n현재 체력 : {currentMonster.hp} / {currentMonster.maxHp}, 공격력 : {currentMonster.damage}. 속성 : {currentMonster.element}",
            buttons
            );
    }

    private void ShowSkillCountNotEnough(Monster currentMonster)
    {
        Debug.Log("스킬이 부족합니다");
        var button = new Dictionary<string, UnityAction>
        {
            {
                "확인", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    ShowPlayerTurnPopup(currentMonster);
                }
            }
        };
        UIPopupManager.Instance.ShowPopup(
            "스킬 사용 가능 횟수가 부족합니다.",
            button
            );
    }

    private void ShowHealItemCountNotEnough(Monster currentMonster)
    {
        Debug.Log("아이템 개수가 부족합니다");
        var button = new Dictionary<string, UnityAction>
        {
            {
                "확인", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    ShowPlayerTurnPopup(currentMonster);
                }
            }
        };
        UIPopupManager.Instance.ShowPopup(
            "아이템 개수가 부족합니다.",
            button
            );
    }

    //기본공격
    private void DoNormalAttack()
    {
        //현재 공격하는 Monster의 공격 타입을 기본공격 타입으로 바꾸고
        TurnManager.Instance.currentTurnMonster.attackType = AttackType.NormalAttack;
        //타겟을 고른다
        ChooseTarget();
    }

    private void DoSkillAttack()
    {
        ////스킬을 고르는 팝업이 떠야함
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
        switch (skillNum)
        {
            case 0:
                TurnManager.Instance.currentTurnMonster.attackType = AttackType.Skill1;
                break;
            case 1:
                TurnManager.Instance.currentTurnMonster.attackType = AttackType.Skill2;
                break;
        }

        //현재 몬스터의 스킬을 정해준다
        TurnManager.Instance.currentTurnMonster.SetSkillNum(skillNum);

        //그다음 타겟을 고르게 해야함
        ChooseTarget();
    }

    //공격 대상 선택
    private void ChooseTarget()
    {
        //살아있는 적을 리스트로 받는다
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
            string targetName = $"{aliveTargetList[i].name}({i+1}), 속성 : {aliveTargetList[i].element}\n체력 {aliveTargetList[i].hp}";

            buttons.Add(
                    targetName,
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
            $"누굴 공격할까?",
            buttons
            );
    }

    private void DoAttackTarget(int targetnum)
    {
        //기본공격일때
        if (TurnManager.Instance.currentTurnMonster.attackType == AttackType.NormalAttack)
        {
            //적이 다 죽으면 고를 수 없어야함
            if (targetnum < TurnManager.Instance.enemyMonsterList.Count)
            {
                Monster target = TurnManager.Instance.enemyMonsterList[targetnum];
                if (target.hp > 0)
                {
                    GameManager.Instance.ExecutePlayerNormalAttackAction(TurnManager.Instance.currentTurnMonster, target);
                }
                else
                {
                    Debug.Log("이미 쓰러진 몬스터입니다. 다른 몬스터를 선택해주세요");
                    ChooseTarget();
                }
            }
            //UIPopupManager.Instance.ClosePopup();
        }

        //스킬공격인데 1번 2번은 나중에 나누면 될 듯 하다
        else if (TurnManager.Instance.currentTurnMonster.attackType == AttackType.Skill1)
        {
            //적이 다 죽으면 고를 수 없어야함
            if (targetnum < TurnManager.Instance.enemyMonsterList.Count)
            {
                Monster target = TurnManager.Instance.enemyMonsterList[targetnum];
                if (target.hp > 0)
                {
                    GameManager.Instance.ExecutePlayerFirstSkillAttackAction(TurnManager.Instance.currentTurnMonster, target);
                }
                else
                {
                    Debug.Log("이미 쓰러진 몬스터입니다. 다른 몬스터를 선택해주세요");
                    ChooseTarget();
                }

            }
            //UIPopupManager.Instance.ClosePopup();
        }
    }

    //private void DoSkillAttackTarget(int targetnum)
    //{
    //    //적이 다 죽으면 고를 수 없어야함
    //    if (targetnum < TurnManager.Instance.enemyMonsterList.Count)
    //    {
    //        Monster target = TurnManager.Instance.enemyMonsterList[targetnum];
    //        if (target.hp > 0)
    //        {
    //            GameManager.Instance.ExecutePlayerSkillAttackAction(TurnManager.Instance.currentTurnMonster, target);
    //        }
    //        else
    //        {
    //            Debug.Log("이미 쓰러진 몬스터입니다. 다른 몬스터를 선택해주세요");
    //        }
    //    }
    //    UIPopupManager.Instance.ClosePopup();
    //}

    private void DoHeal(Monster currentTurnMonster)
    {
        var buttons = new Dictionary<string, UnityAction>
        {
            {
                $"하급 체력물약 ({player.uiInventory.potionNum[0]}개)\n체력의 20을 회복합니다", () =>
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
                $"중급 체력물약 ({player.uiInventory.potionNum[1]}개)\n체력의 40을 회복합니다", () =>
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
                $"고급 엘릭서 ({player.uiInventory.potionNum[2]}개)\n체력의 절반을 회복합니다", () =>
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
        //UI생성
        UIPopupManager.Instance.ShowPopup(
            $"어떤 물약을 사용하시겠습니까?",
            buttons
            );
    }

    //update는 아직까진 필요없다.
    public override void Update()
    {

    }

    //힐이 끝나면 확인을 해야한다.
    public void HealActionIsDone(Monster currentTurnMonster)
    {

        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "확인", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    GameManager.Instance.isPlayerActionComplete = true;
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"{currentTurnMonster.name}의 체력이 회복되었습니다.\n현재 체력 : {currentTurnMonster.hp}",
            buttons
            );
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

        GameManager.Instance.InitializePlayerMonsterData();

        //테스트 용
        foreach (Monster temp in MonsterDataManager.Instance.selectedMonsterDataList)
        {
            Debug.Log($"선택된 플레이어 몬스터의 {temp.name}의 남은 체력 : {temp.hp}");
        }
        foreach (Monster temp in MonsterDataManager.Instance.allMonsterDataList)
        {
            Debug.Log($"모든 플레이어 몬스터의 {temp.name}의 남은 체력 : {temp.hp}");
        }
        // 이 부분때문에 무한으로 즐김
        //player.ChangeState(new PlayerIdleState(player));
        Debug.Log("Exit 4");
    }

    private void HandleBossBattleEnd()
    {
        //상태 변환시키고
        player.ChangeState(new PlayerIdleState(player));

        CalculateExperience();
        //만약 모든 적이 죽으면 밖에 있는 몬스터를 삭제시켜야한다.
        if (TurnManager.Instance.allEnemyMonstersDead == true)
        {
            foreach (Monster playerMonster in MonsterDataManager.Instance.selectedMonsterDataList)
            {
                playerMonster.GetExp(totalExperience);
            }
            //밖에있는 몬스터는 삭제안함
            //GameObject.Destroy(GameManager.Instance.contactedFieldMonster);
        }
        else if (TurnManager.Instance.allPlayerMonstersDead == true)
        {
            Debug.Log("플레이어가 졌다");
        }

        //게임 끝나고나면 전투상태를 초기화 시켜줘야한다.
        GameManager.Instance.InitializeUnitMonsterData(boss);
        GameManager.Instance.InitializeBattleState();
    }

    public int totalExperience;
    public int CalculateExperience()
    {
        for (int i = 0; i < TurnManager.Instance.enemyMonsterList.Count; i++)
        {
            totalExperience += TurnManager.Instance.enemyMonsterList[i].levelPerExp;
        }
        return totalExperience;
    }

    //여기서는 필요없다.
    public override void HandleCollision(Collision collision)
    {

    }
}
