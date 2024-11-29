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
    private Monster currentTurnMonster = new Monster();

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
    public void SetMonsterInformation(List<Monster> playerMonsterList, Monster enemyMoster)
    {
        playerMonsterInBattleList = playerMonsterList;
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(enemyMoster);

            //로그 찍어보는 용도
            print(playerMonsterInBattleList[i].name);
            print(enemyMonsterInBattleList[i].name);
        }

        //턴 매니저에게 몬스터 정보 넘겨주기
        TurnManager.Instance.SetMonsterInfomation(playerMonsterInBattleList, enemyMonsterInBattleList);     
    }

    //플레이어와 적 지정된 포지션에 생성하기
    public void SetMonsterOnBattlePosition()
    {
        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            Monster temp = Instantiate(playerMonsterInBattleList[i], playerBattlePosList[i].transform.position, Quaternion.identity);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            Monster temp = Instantiate(enemyMonsterInBattleList[i], enemyBattlePosList[i].transform.position, Quaternion.identity);
        }
    }

    //현재 턴인 몬스터를 불러온다.
    public void SetCurrentTurnMonster(Monster currentTurnMonster)
    {
        this.currentTurnMonster = currentTurnMonster;
    }

    //플레이어가 공격하는 행동에 돌입
    public void ExecutePlayerAttackAction(Monster target)
    {
        //생성자로 생성해주고
        AttackCommand attackCommand = new AttackCommand(currentTurnMonster, target);
        //공격페이즈 돌입
        attackCommand.Execute();
        //턴끝
        isPlayerActionComplete = true;
    }

    //위와 같다
    public void ExecuteEnemyAttackAction(Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(currentTurnMonster, target);
        attackCommand.Execute();
        isEnemyActionComplete = true;
    }
}
