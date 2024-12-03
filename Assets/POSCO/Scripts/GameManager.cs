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

    //������ ��ȯ�Ǵ� ���͵��� Prefab����Ʈ
    private List<GameObject> instantiateMonsterList = new List<GameObject>();
    //���� ����� ������ ���� ������ ����
    private MonsterDeepCopy originEnemyMonster = new MonsterDeepCopy();

    //���� �������� �� �� �ִ� ����
    public bool isPlayerActionComplete = false;
    public bool isEnemyActionComplete = false;

    //���� ���� ����
    public Monster currentTurnMonster = new Monster();

    //�ۿ��� ���� ����
    public GameObject contactedFieldMonster;

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
        //�� �ٲ𶧸��� �Լ��� �����Ѵ�. -> �ٵ� ���� �ʿ䰡 �ֳ� �ʹ�. TurnManager.Instance.currentTurnMonster�� ����� �� ����.
        TurnManager.Instance.monsterTurnChange += SetCurrentTurnMonster;
    }

    //���� ���͵� ���� �Ѱܹ޴� �Լ�
    public void SetMonsterInformation(Player player, Monster enemyMonster)
    {
        //�÷��̾� ���� ����Ʈ�� ���� ����� �޾ƿ´�. -> ������ ���ϸ� �ȵǱ� ������.
        playerMonsterInBattleList = player.selectedMonsterList;

        //�ϴ� �� ���� ����Ʈ�� ���� ����� �޾ƿ� ������ �������� ������ �ʱ�ȭ ���ش�.
        for(int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(enemyMonster);
        }

        //���� ���� ���� ������ �ϴ� �ʱ�ȭ
        originEnemyMonster = null;

        //���⿡�� ���� ����� ���� ���� ������ �������ش�.
        originEnemyMonster =
            new MonsterDeepCopy
            {
                Name = enemyMonster.name,
                Hp = enemyMonster.hp,
                Damage = enemyMonster.damage,
                Element = enemyMonster.element,
                IsEnemy = enemyMonster.isEnemy,
                Skills = enemyMonster.skills,
            };

        SetMonsterOnBattlePosition();
    }

    //List<GameObject> enemyMonsterObj = new List<GameObject>();
    //�÷��̾�� �� ������ �����ǿ� �����ϱ�
    public void SetMonsterOnBattlePosition()
    {
        //���� ��ȯ�Ǵ� ���͵� -> Monster����, GameObject ���´� instantiateMonsterList�̴�.
        List<Monster> spawnedPlayerMonsterList = new List<Monster>();
        List<Monster> spawnedEnemyMonsterList = new List<Monster>();

        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, playerBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            //���߿� �ѹ��� �����ϱ� ���ϰ� �Ϸ��� ����Ʈ�� �߰��Ѵ�.
            instantiateMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, enemyBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = enemyMonsterObj.GetComponent<Monster>();
            spawnedEnemyMonsterList.Add(tempMonster);

            //���߿� �ѹ��� �����ϱ� ���ϰ� �Ϸ��� ����Ʈ�� �߰��Ѵ�.
            instantiateMonsterList.Add(enemyMonsterObj);
        }

        //�� �Ŀ� ��ȯ�� Monster������ ���� ����Ʈ�� TurnManager���� �Ѱ��ش�.
        TurnManager.Instance.SetMonsterInfomation(spawnedPlayerMonsterList, spawnedEnemyMonsterList);
    }

    //���� ���� ���͸� �ҷ��´�.
    public void SetCurrentTurnMonster(Monster currentTurnMonster)
    {
        this.currentTurnMonster = currentTurnMonster;
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

    public void ExecutePlayerFirstSkillAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.PlayerFristSkillAttackExecute();
        UIPopupManager.Instance.ClosePopup();
    }

    //���� ����
    public void ExecuteEnemyAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.EnemyAttackExecute();
    }

    //�ο� ���� ������Ʈ�� �������ִ� �Լ�
    public void ClearBattleMonsters()
    {
        foreach(GameObject instantiateMonster in instantiateMonsterList)
        {
            Destroy(instantiateMonster);
        }
        instantiateMonsterList.Clear();
    }

    //���� ������ �ʱ�ȭ ���ش�.
    public void InitializeMonsterInfo()
    {
        foreach(Monster enemyMonsterInBattle in enemyMonsterInBattleList)
        {
            enemyMonsterInBattle.hp = originEnemyMonster.Hp;
            enemyMonsterInBattle.name = originEnemyMonster.Name;
            enemyMonsterInBattle.damage = originEnemyMonster.Damage;
            enemyMonsterInBattle.element = originEnemyMonster.Element;
            enemyMonsterInBattle.isEnemy = originEnemyMonster.IsEnemy;
            enemyMonsterInBattle.skills = originEnemyMonster.Skills;
        }
        enemyMonsterInBattleList.Clear();
        //playerMonsterInBattleList.Clear();
    }

    //������ �������� ������ ���¸� �ʱ�ȭ ���ִ� �Լ�
    public void InitializeBattleState()
    {
        //playerMonsterInBattleList.Clear();
        //enemyMonsterInBattleList.Clear();

        ClearBattleMonsters();
        InitializeMonsterInfo();

        //�ൿ �ϷḦ false�� �ٲ��ش�.
        isPlayerActionComplete = false;
        isEnemyActionComplete = false;
    }
}
