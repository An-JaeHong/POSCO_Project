using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


    //전투에 들어가는 몬스터 리스트
    public List<Monster> playerMonsterInBattleList = new List<Monster>();
    public List<Monster> enemyMonsterInBattleList = new List<Monster>();

    //전투에서 몬스터가 생성될 포지션리스트
    public List<Transform> playerBattlePosList = new List<Transform>();
    public List<Transform> enemyBattlePosList = new List<Transform>();

    //턴이 끝났는지 알 수 있는 변수
    public bool isPlayerActionComplete = false;
    public bool isEnemyActionComplete = false;

    //현재 턴의 몬스터
    public Monster currentTurnMonster = new Monster();

    public Monster contactedFieldMonster;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        //턴 바뀔때마다 함수를 실행한다.
        TurnManager.Instance.monsterTurnChange += SetCurrentTurnMonster;
    }

    //적 정보 넘겨받기
    public void SetMonsterInformation(Player player, Monster enemyMoster)
    {
        playerMonsterInBattleList = player.selectedMonsterList;
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(enemyMoster);

            //로그 찍어보는 용도
            print(playerMonsterInBattleList[i].transform.position);
            print(enemyMonsterInBattleList[i].transform.position);
        }

        //턴 매니저에게 몬스터 정보 넘겨주기 왜 이때하냐? 플레이어가 만난 후에 지금 플레이어의 정보와 만난 몬스터의 정보를 받고 그다음에 GameManager가 Turn한테 넘겨주는게 맞아서
        //TurnManager.Instance.SetMonsterInfomation(playerMonsterInBattleList, enemyMonsterInBattleList);
        SetMonsterOnBattlePosition();
    }

    //플레이어와 적 지정된 포지션에 생성하기
    public void SetMonsterOnBattlePosition()
    {
        List<Monster> temp1 = new List<Monster>(playerMonsterInBattleList);
        List<Monster> temp2 = new List<Monster>(enemyMonsterInBattleList);
        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            temp1[i] = Instantiate(playerMonsterInBattleList[i], playerBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            //temp.animator = FindObjectOfType<Animator>();
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            temp2[i] = Instantiate(enemyMonsterInBattleList[i], enemyBattlePosList[i].transform.position, Quaternion.Euler(0, -90f, 0));
            //temp.animator = FindObjectOfType<Animator>();
        }

        TurnManager.Instance.SetMonsterInfomation(temp1, temp2);

        //테스트용
        for (int i = 0; i < 3; i++)
        {
            print($"temp1의 포지션 : {temp1[i].transform.position}");
            print($"temp2의 포지션 : {temp2[i].transform.position}");
        }

    }

    //현재 턴인 몬스터를 불러온다.
    public void SetCurrentTurnMonster(Monster currentTurnMonster)
    {
        this.currentTurnMonster = currentTurnMonster;

        //테스트용
        print($"From GameManager : {this.currentTurnMonster.transform.position}");
    }

    //플레이어가 공격하는 행동에 돌입
    public void ExecutePlayerNormalAttackAction(Monster attacker, Monster target)
    {
        //생성자로 생성해주고
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        //print($"currentTurnMonster의 위치 : {currentTurnMonster.transform.position}");
        //print($"currentTurnMonster의 의 이름 : {currentTurnMonster.name}");
        //print($"currentTurnMonster의 타입 : {currentTurnMonster.GetType()}");
        //print($"target의 위치 : {target.transform.position}");
        //print($"target의 이름 : {target.name}");

        //테스트용
        foreach (var playerMonster in playerBattlePosList)
        {
            print($"플레이어 몬스터 위치 : {playerMonster.transform.position}");
        }

        foreach (var enemyMonster in enemyBattlePosList)
        {
            print($"적 몬스터 위치 : {enemyMonster.transform.position}");
        }

        //공격페이즈 돌입
        attackCommand.PlayerNormalAttackExecute();
        UIPopupManager.Instance.ClosePopup();
        //턴끝
        //isPlayerActionComplete = true;
        //StartCoroutine(PlayerAttackAnimation(attacker, target));
    }

    //private IEnumerator PlayerAttackAnimation(Monster attacker, Monster target)
    //{
    //    //attacker.playerattackanimation();

    //    //animatorstateinfo stateinfo = attacker.getcomponent<animator>().getcurrentanimatorstateinfo(0);

    //    //// 애니메이션 길이만큼 대기
    //    //yield return new waitforseconds(stateinfo.length);

    //    AttackCommand attackCommand = new AttackCommand(attacker, target);
    //    attackCommand.Execute();
    //    isPlayerActionComplete = true;
    //    yield return null;
    //}

    public void ExecutePlayerSkillAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.PlayerSkillAttackExecute();
        UIPopupManager.Instance.ClosePopup();
    }

    //위와 같다
    public void ExecuteEnemyAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.EnemyAttackExecute();
    }
}
