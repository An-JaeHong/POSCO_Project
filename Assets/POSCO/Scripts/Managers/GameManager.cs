using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GameObject damageTextPrefab;

    //������ ���� ���� ����Ʈ
    public List<Monster> playerMonsterInBattleList = new List<Monster>();
    public List<Monster> enemyMonsterInBattleList = new List<Monster>();

    #region �������� ������ ������ ����Ʈ
    //�������� ���Ͱ� ������ �����Ǹ���Ʈ
    public List<Transform> playerBattlePosList = new List<Transform>();
    public List<Transform> enemyBattlePosList = new List<Transform>();
    //�����ʿ��� ������ �����Ǹ���Ʈ
    public List<Transform> PlayerBossBattlePosList = new List<Transform>();
    public List<Transform> EnemyBossBattlePosList = new List<Transform>();
    #endregion

    //������ ��ȯ�Ǵ� ���͵��� Prefab����Ʈ -> ���߿� �ѹ��� �����ϱ� ���� �ϱ� ���� ����Ʈ
    private List<GameObject> instantiatedMonsterList = new List<GameObject>();

    //���� ����� ������ ���� ������ ����
    private List<MonsterDeepCopy> originEnemyMonsterDataList = new List<MonsterDeepCopy>();

    //���� ��ȯ�Ǵ� ���͵� -> Monster����, GameObject ���´� instantiateMonsterList�̴�.
    List<Monster> spawnedPlayerMonsterList = new List<Monster>();
    List<Monster> spawnedEnemyMonsterList = new List<Monster>();

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
    public void SetNormalMonsterInformation(List<Monster> playerselectedMonster, Unit unit)
    {
        //�÷��̾� ���� ����Ʈ�� ���� ����� �޾ƿ´�. -> ������ ���ϸ� �ȵǱ� ������.
        playerMonsterInBattleList = playerselectedMonster;

        //�ϴ� �� ���� ����Ʈ�� ���� ����� �޾ƿ� ������ �������� ������ �ʱ�ȭ ���ش�.
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(unit.ownedMonsterList[i]);
            //�׽�Ʈ ��
            print($"�ʿ� ������ �� ���͵��� ���� : {enemyMonsterInBattleList[i].level}");
        }

        //���� ���� ���� ������ �ϴ� �ʱ�ȭ
        originEnemyMonsterDataList.Clear();

        //���⿡�� ���� ����� ���� ���� ������ �������ش�.
        for (int i = 0; i < 3; i++)
        {
            MonsterDeepCopy temp =
            new MonsterDeepCopy
            {
                Name = unit.ownedMonsterList[i].name,
                Hp = unit.ownedMonsterList[i].hp,
                MaxHp = unit.ownedMonsterList[i].maxHp,
                Level = unit.ownedMonsterList[i].level,
                LevelPerExp = unit.ownedMonsterList[i].levelPerExp,
                Damage = unit.ownedMonsterList[i].damage,
                Element = unit.ownedMonsterList[i].element,
                IsEnemy = unit.ownedMonsterList[i].isEnemy,
                SkillDataArr = unit.ownedMonsterList[i].skillDataArr,
                
            };
            originEnemyMonsterDataList.Add(temp);
        }

        SetMonsterOnBattlePosition();
    }

    public void SetBossInformation(List<Monster> playerselectedMonster, Unit boss)
    {
        playerMonsterInBattleList = playerselectedMonster;

        //�ϴ� �� ���� ����Ʈ�� ���� ����� �޾ƿ� ������ �������� ������ �ʱ�ȭ ���ش�.
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(boss.ownedMonsterList[i]);
        }

        //���� ���� ���� ������ �ϴ� �ʱ�ȭ
        originEnemyMonsterDataList.Clear();

        //���⿡�� ���� ����� ���� ���� ������ �������ش�.

        for (int i = 0; i < 3; i++)
        {
            MonsterDeepCopy temp =
                new MonsterDeepCopy
                {
                    Name = boss.ownedMonsterList[i].name,
                    Hp = boss.ownedMonsterList[i].hp,
                    MaxHp = boss.ownedMonsterList[i].maxHp,
                    Level = boss.ownedMonsterList[i].level,
                    LevelPerExp = boss.ownedMonsterList[i].levelPerExp,
                    Damage = boss.ownedMonsterList[i].damage,
                    Element = boss.ownedMonsterList[i].element,
                    IsEnemy = boss.ownedMonsterList[i].isEnemy,
                    SkillDataArr = boss.ownedMonsterList[i].skillDataArr,
                };
            originEnemyMonsterDataList.Add(temp);
        }

        SetMonsterOnBossMapPosition();
    }

    //List<GameObject> enemyMonsterObj = new List<GameObject>();
    //�÷��̾�� �� ������ �����ǿ� �����ϱ�
    public void SetMonsterOnBattlePosition()
    {
        //���� ��ȯ�Ǵ� ���͵� -> Monster����, GameObject ���´� instantiateMonsterList�̴�.
        spawnedPlayerMonsterList = new List<Monster>();
        spawnedEnemyMonsterList = new List<Monster>();
        
        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, playerBattlePosList[i].transform.position, Quaternion.Euler(0, -90f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            #region
            //DamageDisplay damageDisplay = playerMonsterObj.AddComponent<DamageDisplay>();
            //damageDisplay.damageTextPrefab = damageTextPrefab;

            //Canvas canvas = damageDisplay.GetComponentInChildren<Canvas>();
            //CameraManager.Instance.SetCanvasEventCamera(canvas);

            //if (canvas != null)
            //{
            //    Camera mainCamera = CameraManager.Instance.currentCamera;
            //    canvas.transform.LookAt(mainCamera.transform);
            //    canvas.transform.Rotate(0, 180, 0); // LookAt���� ���� ������ ��� ����
            //}
            #endregion

            //���߿� �ѹ��� �����ϱ� ���ϰ� �Ϸ��� ����Ʈ�� �߰��Ѵ�.
            instantiatedMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, enemyBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = enemyMonsterObj.GetComponent<Monster>();
            spawnedEnemyMonsterList.Add(tempMonster);

            #region
            //DamageDisplay damageDisplay = enemyMonsterObj.AddComponent<DamageDisplay>();
            //damageDisplay.damageTextPrefab = damageTextPrefab;

            //Canvas canvas = damageDisplay.GetComponentInChildren<Canvas>();
            //CameraManager.Instance.SetCanvasEventCamera(canvas);

            //if (canvas != null)
            //{
            //    Camera mainCamera = CameraManager.Instance.currentCamera;
            //    canvas.transform.LookAt(mainCamera.transform);
            //    canvas.transform.Rotate(0, 180, 0); // LookAt���� ���� ������ ��� ����
            //}
            #endregion

            //���߿� �ѹ��� �����ϱ� ���ϰ� �Ϸ��� ����Ʈ�� �߰��Ѵ�.
            instantiatedMonsterList.Add(enemyMonsterObj);
        }

        //�� �Ŀ� ��ȯ�� Monster������ ���� ����Ʈ�� TurnManager���� �Ѱ��ش�.
        TurnManager.Instance.SetMonsterInfomation(spawnedPlayerMonsterList, spawnedEnemyMonsterList);
    }

    //�����ʿ��� �����Ǵ� �Լ�
    public void SetMonsterOnBossMapPosition()
    {
        //���� ��ȯ�Ǵ� ���͵� -> Monster����, GameObject ���´� instantiateMonsterList�̴�.
        spawnedPlayerMonsterList = new List<Monster>();
        spawnedEnemyMonsterList = new List<Monster>();

        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, PlayerBossBattlePosList[i].transform.position, Quaternion.Euler(0, -80f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            #region
            DamageDisplay damageDisplay = playerMonsterObj.AddComponent<DamageDisplay>();
            damageDisplay.damageTextPrefab = damageTextPrefab;
            //playerMonsterObj.AddComponent<DamageDisPlay>();
            Canvas canvas = damageDisplay.GetComponentInChildren<Canvas>();
            CameraManager.Instance.SetCanvasEventCamera(canvas);
            #endregion

            //���߿� �ѹ��� �����ϱ� ���ϰ� �Ϸ��� ����Ʈ�� �߰��Ѵ�.
            instantiatedMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, EnemyBossBattlePosList[i].transform.position, Quaternion.Euler(0, 97f, 0));
            Monster tempMonster = enemyMonsterObj.GetComponent<Monster>();
            spawnedEnemyMonsterList.Add(tempMonster);

            #region
            DamageDisplay damageDisplay = enemyMonsterObj.AddComponent<DamageDisplay>();
            damageDisplay.damageTextPrefab = damageTextPrefab;

            Canvas canvas = damageDisplay.GetComponentInChildren<Canvas>();
            CameraManager.Instance.SetCanvasEventCamera(canvas);
            #endregion

            //enemyMonsterObj.AddComponent<DamageDisPlay>();

            //���߿� �ѹ��� �����ϱ� ���ϰ� �Ϸ��� ����Ʈ�� �߰��Ѵ�.
            instantiatedMonsterList.Add(enemyMonsterObj);
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

        ////�׽�Ʈ��
        //foreach (var playerMonster in playerBattlePosList)
        //{
        //    print($"�÷��̾� ���� ��ġ : {playerMonster.transform.position}");
        //}

        //foreach (var enemyMonster in enemyBattlePosList)
        //{
        //    print($"�� ���� ��ġ : {enemyMonster.transform.position}");
        //}

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
        //UIPopupManager.Instance.ClosePopup();
    }

    //���� ����
    public void ExecuteEnemyAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);

        int enemyAttackType = Random.Range(0, 2);
        switch(enemyAttackType)
        {
            case 0:
                attackCommand.EnemyNormalAttackExecute();
                break;
            case 1:
                attackCommand.EnemyFristSkillAttackExecute();
                break;
        }
    }

    //�ο� ���� ������Ʈ�� �������ִ� �Լ�
    public void ClearAllBattleMapMonsterObj()
    {
        foreach(GameObject instantiateMonster in instantiatedMonsterList)
        {
            Destroy(instantiateMonster);
        }
        instantiatedMonsterList.Clear();
    }

    //�÷��̾��� ���͵� �����͸� �ʱ�ȭ ���־���Ѵ�.
    public void InitializePlayerMonsterData()
    {
        for (int i = 0; i < spawnedPlayerMonsterList.Count; i++)
        {
            MonsterDataManager.Instance.selectedMonsterDataList[i].hp = spawnedPlayerMonsterList[i].hp;
            MonsterDataManager.Instance.selectedMonsterDataList[i].name = spawnedPlayerMonsterList[i].name;
            MonsterDataManager.Instance.selectedMonsterDataList[i].damage = spawnedPlayerMonsterList[i].damage;
            MonsterDataManager.Instance.selectedMonsterDataList[i].element = spawnedPlayerMonsterList[i].element;
            MonsterDataManager.Instance.selectedMonsterDataList[i].isEnemy = spawnedPlayerMonsterList[i].isEnemy;
            MonsterDataManager.Instance.selectedMonsterDataList[i].skillDataArr = spawnedPlayerMonsterList[i].skillDataArr;
            //MonsterDataManager.Instance.selectedMonsterDataList[i].selectedSkill.skillCount = spawnedPlayerMonsterList[i].selectedSkill.skillCount;
            MonsterDataManager.Instance.selectedMonsterDataList[i].level = spawnedPlayerMonsterList[i].level;
            MonsterDataManager.Instance.selectedMonsterDataList[i].currentExp = spawnedPlayerMonsterList[i].currentExp;
        }
    }

    //���� ������ ������ ���� ó�� ���·� �ǵ����ش�
    public void InitializeUnitMonsterData(Unit unit)
    {
        //��Ƴ��� originEnemyMonster�� ������ ����ش�.
        for(int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            unit.ownedMonsterList[i].hp = originEnemyMonsterDataList[i].Hp;
            unit.ownedMonsterList[i].name = originEnemyMonsterDataList[i].Name;
            unit.ownedMonsterList[i].damage = originEnemyMonsterDataList[i].Damage;
            unit.ownedMonsterList[i].element = originEnemyMonsterDataList[i].Element;
            unit.ownedMonsterList[i].isEnemy = originEnemyMonsterDataList[i].IsEnemy;
            unit.ownedMonsterList[i].skillDataArr = originEnemyMonsterDataList[i].SkillDataArr;

            //enemymonsterinbattlelist[i].hp = originenemymonster[i].hp;
            //enemymonsterinbattlelist[i].name = originenemymonster[i].name;
            //enemymonsterinbattlelist[i].damage = originenemymonster[i].damage;
            //enemymonsterinbattlelist[i].element = originenemymonster[i].element;
            //enemymonsterinbattlelist[i].isenemy = originenemymonster[i].isenemy;
            //enemymonsterinbattlelist[i].skills = originenemymonster[i].skills;
        }
        enemyMonsterInBattleList.Clear();
        //playerMonsterInBattleList.Clear();
    }

    //������ ��������� ������ ���¸� �ʱ�ȭ ���ִ� �Լ�
    public void InitializeBattleState()
    {
        //playerMonsterInBattleList.Clear();
        //enemyMonsterInBattleList.Clear();

        ClearAllBattleMapMonsterObj();
        //InitializeMonsterInfo();

        //�ൿ �ϷḦ false�� �ٲ��ش�.
        isPlayerActionComplete = false;
        isEnemyActionComplete = false;
    }
}
