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
    //테스트 용
    private List<Monster> playerMonsterList = new List<Monster>();

    private Unit unit;
    public PlayerNormalBattleState(Player player, Unit unit) : base(player) 
    {
        //적 동기화
        this.unit = unit;
    }

    //private void TmpBattleEnd()
    //{

    //        player.ChangeState(new PlayerIdleState(player));
    //}

    public override void Enter()
    {
        //턴 바뀔때 마다 호출하는 함수
        TurnManager.Instance.monsterTurnChange += OnMonsterTurnChange;
        //배틀 종료되면 호출하는 함수
        TurnManager.Instance.OnBattleEnd += HandleBattleEnd;
        //TurnManager.Instance.OnBattleEnd += TmpBattleEnd;
        //전투에 들어가면
        //1. 필드에 있는 플레이어 움직임 정지
        //2. 카메라 배틀맵으로 교체
        //3. 만난 몬스터의 정보와 플레이어의 몬스터 정보를 GameManager에게 넘겨줌
        //4. 선택하는 UI생성
        player.canMove = false;
        CameraManager.Instance.HandleCamera(CameraType.BattleMap);
        TurnManager.Instance.InitializeTurnQueue();
        //gameManager.SetMonsterOnBattlePosition();

        //플레이어 턴일때 띄워주는 팝업
        //ShowPlayerTurnPopup();

        //테스트
        playerMonsterList = GameManager.Instance.playerMonsterInBattleList;

        //테스트
        foreach(var playerMonster in playerMonsterList)
        {
            Debug.Log($"현재 이렇게 넘겨 받으면 뜨는 위치 : {playerMonster.transform.position}");
        }
    }

    //몬스터의 턴이 바뀔때 적이 아니라면 팝업이 실행됨
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
        //스킬은 하나라서 이렇게 먼저 정해준다
        currentMonster.SetSkillNum(0);

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
                    currentMonster.selectedSkill.skillCount -= 1;
                    ChooseTarget();
                    }
                }
            },
            {
                "회복하기", () =>
                {
                    DoHeal();
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"{currentMonster.name}의 턴이다 무엇을 할까?",
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

    //기본공격
    private void DoNormalAttack()
    {
        //if (TurnManager.Instance.currentTurnMonster.selectedSkill.skillCount >= 1)
        //{
            //현재 공격하는 Monster의 공격 타입을 기본공격 타입으로 바꾸고
            TurnManager.Instance.currentTurnMonster.attackType = AttackType.NormalAttack;
            //타겟을 고른다
            ChooseTarget();
        //}
        //else
        //{
        //    #region 스킬카운트가 부족하다는 UI가 떠야함
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
        switch(skillNum)
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
            string targetName = $"{aliveTargetList[i].name}({index + 1})";

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

    private void DoHeal()
    {

    }

    //update는 아직까진 필요없다.
    public override void Update()
    {
        
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

        //전투가 끝나면 모든 UI를 닫아야한다
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

    private void HandleBattleEnd()
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
            //밖에있는 몬스터는 삭제
            GameObject.Destroy(GameManager.Instance.contactedFieldMonster);
        }
        else if (TurnManager.Instance.allPlayerMonstersDead == true)
        {
            Debug.Log("플레이어가 졌다");
        }

        //게임 끝나고나면 전투상태를 초기화 시켜줘야한다.
        GameManager.Instance.InitializeUnitMonsterData(unit);
        GameManager.Instance.InitializeBattleState();
    }

    public int totalExperience;

    //총 경험치를 계산하는 함수
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
