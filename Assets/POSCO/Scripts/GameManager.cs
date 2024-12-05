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
    //�����ʿ��� ������ �����Ǹ���Ʈ
    public List<Transform> BossBattlePlayerPosList = new List<Transform>();
    public List<Transform> BossBattlePosList = new List<Transform>();

    //������ ��ȯ�Ǵ� ���͵��� Prefab����Ʈ
    private List<GameObject> instantiateMonsterList = new List<GameObject>();
    //���� ����� ������ ���� ������ ����
    private List<MonsterDeepCopy> originEnemyMonster = new List<MonsterDeepCopy>();

    //���� �������� �� �� �ִ� ����
    public bool isPlayerActionComplete = false;
    public bool isEnemyActionComplete = false;

    //���� ���� ����
    public Monster currentTurnMonster;

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
    public void SetMonsterInformation(Player player, Unit unit)
    {
        //�÷��̾� ���� ����Ʈ�� ���� ����� �޾ƿ´�. -> ������ ���ϸ� �ȵǱ� ������.
        playerMonsterInBattleList = player.selectedMonsterList;

        //�ϴ� �� ���� ����Ʈ�� ���� ����� �޾ƿ� ������ �������� ������ �ʱ�ȭ ���ش�.
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(unit.ownedMonsterList[i]);
        }

        //���� ���� ���� ������ �ϴ� �ʱ�ȭ
        originEnemyMonster.Clear();

        //���⿡�� ���� ����� ���� ���� ������ �������ش�.
        for (int i = 0; i < 3; i++)
        {
            MonsterDeepCopy temp =
            new MonsterDeepCopy
            {
                Name = unit.ownedMonsterList[i].name,
                Hp = unit.ownedMonsterList[i].hp,
                Damage = unit.ownedMonsterList[i].damage,
                Element = unit.ownedMonsterList[i].element,
                IsEnemy = unit.ownedMonsterList[i].isEnemy,
                Skills = unit.ownedMonsterList[i].skills,
            };
            originEnemyMonster.Add(temp);
        }

        SetMonsterOnBattlePosition();
    }

    public void SetBossInformation(Player player, Unit boss)
    {
        playerMonsterInBattleList = player.selectedMonsterList;

        //�ϴ� �� ���� ����Ʈ�� ���� ����� �޾ƿ� ������ �������� ������ �ʱ�ȭ ���ش�.
        enemyMonsterInBattleList = boss.ownedMonsterList;

        //���� ���� ���� ������ �ϴ� �ʱ�ȭ
        originEnemyMonster.Clear();

        //���⿡�� ���� ����� ���� ���� ������ �������ش�.

        for (int i = 0; i < 3; i++)
        {
            MonsterDeepCopy temp =
                new MonsterDeepCopy
                {
                    Name = boss.ownedMonsterList[i].name,
                    Hp = boss.ownedMonsterList[i].hp,
                    Damage = boss.ownedMonsterList[i].damage,
                    Element = boss.ownedMonsterList[i].element,
                    IsEnemy = boss.ownedMonsterList[i].isEnemy,
                    Skills = boss.ownedMonsterList[i].skills,
                };
            originEnemyMonster.Add(temp);
        }

        SetMonsterOnBossMapPosition();
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

    //�����ʿ��� �����Ǵ� �Լ�
    public void SetMonsterOnBossMapPosition()
    {
        //���� ��ȯ�Ǵ� ���͵� -> Monster����, GameObject ���´� instantiateMonsterList�̴�.
        List<Monster> spawnedPlayerMonsterList = new List<Monster>();
        List<Monster> spawnedEnemyMonsterList = new List<Monster>();

        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, BossBattlePlayerPosList[i].transform.position, Quaternion.Euler(0, -90f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            //���߿� �ѹ��� �����ϱ� ���ϰ� �Ϸ��� ����Ʈ�� �߰��Ѵ�.
            instantiateMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, BossBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
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

        //UIPopupManager.Instance.ClosePopup();
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
        for(int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            enemyMonsterInBattleList[i].hp = originEnemyMonster[i].Hp;
            enemyMonsterInBattleList[i].name = originEnemyMonster[i].Name;
            enemyMonsterInBattleList[i].damage = originEnemyMonster[i].Damage;
            enemyMonsterInBattleList[i].element = originEnemyMonster[i].Element;
            enemyMonsterInBattleList[i].isEnemy = originEnemyMonster[i].IsEnemy;
            enemyMonsterInBattleList[i].skills = originEnemyMonster[i].Skills;
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
