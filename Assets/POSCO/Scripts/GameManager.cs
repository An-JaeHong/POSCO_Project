using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


    //������ ���� ���� ����Ʈ
    public List<Monster> playerMonsterInBattleList = new List<Monster>();
    public List<Monster> enemyMonsterInBattleList = new List<Monster>();

    //�������� ���Ͱ� ������ �����Ǹ���Ʈ
    public List<Transform> playerBattlePosList = new List<Transform>();
    public List<Transform> enemyBattlePosList = new List<Transform>();

    //���� �������� �� �� �ִ� ����
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

    //�� ���� �Ѱܹޱ�
    public void SetMonsterInformation(List<Monster> playerMonsterList, Monster enemyMoster)
    {
        playerMonsterInBattleList = playerMonsterList;
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(enemyMoster);

            //�α� ���� �뵵
            print(playerMonsterInBattleList[i].name);
            print(enemyMonsterInBattleList[i].name);
        }

        //�� �Ŵ������� ���� ���� �Ѱ��ֱ�
        TurnManager.Instance.SetMonsterInfomation(playerMonsterInBattleList, enemyMonsterInBattleList);
    }

    //�÷��̾�� �� ������ �����ǿ� �����ϱ�
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
