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


}
