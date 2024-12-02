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
            temp1[i] = Instantiate(playerMonsterInBattleList[i], playerBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            //temp.animator = FindObjectOfType<Animator>();
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            temp2[i] = Instantiate(enemyMonsterInBattleList[i], enemyBattlePosList[i].transform.position, Quaternion.Euler(0, -90f, 0));
            //temp.animator = FindObjectOfType<Animator>();
        }

        TurnManager.Instance.SetMonsterInfomation(temp1, temp2);

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

        //�׽�Ʈ��
        print($"From GameManager : {this.currentTurnMonster.transform.position}");
    }

    //�÷��̾ �����ϴ� �ൿ�� ����
    public void ExecutePlayerNormalAttackAction(Monster attacker, Monster target)
    {
        //�����ڷ� �������ְ�
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        //print($"currentTurnMonster�� ��ġ : {currentTurnMonster.transform.position}");
        //print($"currentTurnMonster�� �� �̸� : {currentTurnMonster.name}");
        //print($"currentTurnMonster�� Ÿ�� : {currentTurnMonster.GetType()}");
        //print($"target�� ��ġ : {target.transform.position}");
        //print($"target�� �̸� : {target.name}");

        //�׽�Ʈ��
        foreach (var playerMonster in playerBattlePosList)
        {
            print($"�÷��̾� ���� ��ġ : {playerMonster.transform.position}");
        }

        foreach (var enemyMonster in enemyBattlePosList)
        {
            print($"�� ���� ��ġ : {enemyMonster.transform.position}");
        }

        //���������� ����
        attackCommand.PlayerNormalAttackExecute();
        UIPopupManager.Instance.ClosePopup();
        //�ϳ�
        //isPlayerActionComplete = true;
        //StartCoroutine(PlayerAttackAnimation(attacker, target));
    }

    //private IEnumerator PlayerAttackAnimation(Monster attacker, Monster target)
    //{
    //    //attacker.playerattackanimation();

    //    //animatorstateinfo stateinfo = attacker.getcomponent<animator>().getcurrentanimatorstateinfo(0);

    //    //// �ִϸ��̼� ���̸�ŭ ���
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

    //���� ����
    public void ExecuteEnemyAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.EnemyAttackExecute();
    }
}
