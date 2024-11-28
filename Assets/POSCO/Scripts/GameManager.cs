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
        //CameraManager.Instance.HandleCamera(CAMERATYPE.BATTLEMAP);

        playerMonsterInBattleList = playerMonsterList;
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(enemyMoster);
        }
        print(playerMonsterInBattleList[0].name);
        print(enemyMonsterInBattleList[0].name);
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
