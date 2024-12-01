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

    //���� ���� ����
    public Monster currentTurnMonster = new Monster();

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
        //�� �ٲ𶧸��� �Լ��� �����Ѵ�.
        TurnManager.Instance.monsterTurnChange += SetCurrentTurnMonster;
    }

    //�� ���� �Ѱܹޱ�
    public void SetMonsterInformation(Player player, Monster enemyMoster)
    {
        playerMonsterInBattleList = player.selectedMonsterList;
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(enemyMoster);

            //�α� ���� �뵵
            print(playerMonsterInBattleList[i].transform.position);
            print(enemyMonsterInBattleList[i].transform.position);
        }

        //�� �Ŵ������� ���� ���� �Ѱ��ֱ� �� �̶��ϳ�? �÷��̾ ���� �Ŀ� ���� �÷��̾��� ������ ���� ������ ������ �ް� �״����� GameManager�� Turn���� �Ѱ��ִ°� �¾Ƽ�
        //TurnManager.Instance.SetMonsterInfomation(playerMonsterInBattleList, enemyMonsterInBattleList);
        SetMonsterOnBattlePosition();
    }

    //�÷��̾�� �� ������ �����ǿ� �����ϱ�
    public void SetMonsterOnBattlePosition()
    {
        List<Monster> temp1 = new List<Monster>(playerMonsterInBattleList);
        List<Monster> temp2 = new List<Monster>(enemyMonsterInBattleList);
        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            temp1[i] = Instantiate(playerMonsterInBattleList[i], playerBattlePosList[i].transform.position, Quaternion.identity);
            //temp.animator = FindObjectOfType<Animator>();
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            temp2[i] = Instantiate(enemyMonsterInBattleList[i], enemyBattlePosList[i].transform.position, Quaternion.identity);
            //temp.animator = FindObjectOfType<Animator>();
        }
        TurnManager.Instance.SetMonsterInfomation(playerMonsterInBattleList, enemyMonsterInBattleList);

        //�׽�Ʈ��
        for (int i = 0; i < 3; i++)
        {
            print($"temp1�� ������ : {temp1[i].transform.position}");
            print($"temp2�� ������ : {temp2[i].transform.position}");
        }

    }

    //���� ���� ���͸� �ҷ��´�.
    public void SetCurrentTurnMonster(Monster currentTurnMonster)
    {
        this.currentTurnMonster = currentTurnMonster;
        //print($"���� ���� ������ �ִ� ���� �̸� : {this.currentTurnMonster}");
        //print($"���� ���� ������ �ִ� ���� ��ġ : {this.currentTurnMonster.transform.position}");
    }

    //�÷��̾ �����ϴ� �ൿ�� ����
    public void ExecutePlayerAttackAction(Monster attacker, Monster target)
    {
        //�����ڷ� �������ְ�
        AttackCommand attackCommand = new AttackCommand(currentTurnMonster, target);
        //print($"currentTurnMonster�� ��ġ : {currentTurnMonster.transform.position}");
        //print($"currentTurnMonster�� �� �̸� : {currentTurnMonster.name}");
        //print($"currentTurnMonster�� Ÿ�� : {currentTurnMonster.GetType()}");
        //print($"target�� ��ġ : {target.transform.position}");
        //print($"target�� �̸� : {target.name}");

        foreach (var playerMonster in playerBattlePosList)
        {
            print($"�÷��̾� ���� ��ġ : {playerMonster.transform.position}");
        }

        foreach (var enemyMonster in enemyBattlePosList)
        {
            print($"�� ���� ��ġ : {enemyMonster.transform.position}");
        }

        //���������� ����
        attackCommand.Execute();
        //�ϳ�
        isPlayerActionComplete = true;
        //StartCoroutine(PlayerAttackAnimation(attacker, target));
    }

    private IEnumerator PlayerAttackAnimation(Monster attacker, Monster target)
    {
        attacker.PlayerAttackAnimation();

        AnimatorStateInfo stateInfo = attacker.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        // �ִϸ��̼� ���̸�ŭ ���
        yield return new WaitForSeconds(stateInfo.length);

        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.Execute();
        isPlayerActionComplete = true;
    }

    //���� ����
    public void ExecuteEnemyAttackAction(Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(currentTurnMonster, target);
        attackCommand.Execute();
        isEnemyActionComplete = true;
    }
}
