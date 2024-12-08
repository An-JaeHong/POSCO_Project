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

    #region 전투에서 생성될 포지션 리스트
    //전투에서 몬스터가 생성될 포지션리스트
    public List<Transform> playerBattlePosList = new List<Transform>();
    public List<Transform> enemyBattlePosList = new List<Transform>();
    //보스맵에서 생성될 포지션리스트
    public List<Transform> PlayerBossBattlePosList = new List<Transform>();
    public List<Transform> EnemyBossBattlePosList = new List<Transform>();
    #endregion

    //실제로 소환되는 몬스터들의 Prefab리스트 -> 나중에 한번에 삭제하기 쉽게 하기 위한 리스트
    private List<GameObject> instantiatedMonsterList = new List<GameObject>();

    //깊은 복사로 저장할 원래 몬스터의 정보
    private List<MonsterDeepCopy> originEnemyMonsterDataList = new List<MonsterDeepCopy>();

    //실제 소환되는 몬스터들 -> Monster형태, GameObject 형태는 instantiateMonsterList이다.
    List<Monster> spawnedPlayerMonsterList = new List<Monster>();
    List<Monster> spawnedEnemyMonsterList = new List<Monster>();

    //턴이 끝났는지 알 수 있는 변수
    public bool isPlayerActionComplete = false;
    public bool isEnemyActionComplete = false;

    //현재 턴의 몬스터
    public Monster currentTurnMonster;

    //밖에서 만난 몬스터
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
        //턴 바뀔때마다 함수를 실행한다. -> 근데 굳이 필요가 있나 싶다. TurnManager.Instance.currentTurnMonster로 충분할 것 같다.
        TurnManager.Instance.monsterTurnChange += SetCurrentTurnMonster;
    }

    //만난 몬스터들 정보 넘겨받는 함수
    public void SetNormalMonsterInformation(List<Monster> playerselectedMonster, Unit unit)
    {
        //플레이어 몬스터 리스트는 얕은 복사로 받아온다. -> 정보가 변하면 안되기 때문에.
        playerMonsterInBattleList = playerselectedMonster;

        //일단 적 몬스터 리스트도 얕은 복사로 받아온 다음에 마지막에 정보를 초기화 해준다.
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(unit.ownedMonsterList[i]);
            //테스트 용
            print($"맵에 생성된 적 몬스터들의 레벨 : {enemyMonsterInBattleList[i].level}");
        }

        //새로 들어온 몬스터 정보는 일단 초기화
        originEnemyMonsterDataList.Clear();

        //여기에서 깊은 복사로 원래 몬스터 정보를 보관해준다.
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
                Skills = unit.ownedMonsterList[i].skills,
                
            };
            originEnemyMonsterDataList.Add(temp);
        }

        SetMonsterOnBattlePosition();
    }

    public void SetBossInformation(List<Monster> playerselectedMonster, Unit boss)
    {
        playerMonsterInBattleList = playerselectedMonster;

        //일단 적 몬스터 리스트도 얕은 복사로 받아온 다음에 마지막에 정보를 초기화 해준다.
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(boss.ownedMonsterList[i]);
        }

        //새로 들어온 몬스터 정보는 일단 초기화
        originEnemyMonsterDataList.Clear();

        //여기에서 깊은 복사로 원래 몬스터 정보를 보관해준다.

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
                    Skills = boss.ownedMonsterList[i].skills,
                };
            originEnemyMonsterDataList.Add(temp);
        }

        SetMonsterOnBossMapPosition();
    }

    //List<GameObject> enemyMonsterObj = new List<GameObject>();
    //플레이어와 적 지정된 포지션에 생성하기
    public void SetMonsterOnBattlePosition()
    {
        //실제 소환되는 몬스터들 -> Monster형태, GameObject 형태는 instantiateMonsterList이다.
        spawnedPlayerMonsterList = new List<Monster>();
        spawnedEnemyMonsterList = new List<Monster>();
        
        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, playerBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiatedMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, enemyBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = enemyMonsterObj.GetComponent<Monster>();
            spawnedEnemyMonsterList.Add(tempMonster);

            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiatedMonsterList.Add(enemyMonsterObj);
        }

        //그 후에 소환된 Monster형태의 몬스터 리스트를 TurnManager에게 넘겨준다.
        TurnManager.Instance.SetMonsterInfomation(spawnedPlayerMonsterList, spawnedEnemyMonsterList);
    }

    //보스맵에서 생성되는 함수
    public void SetMonsterOnBossMapPosition()
    {
        //실제 소환되는 몬스터들 -> Monster형태, GameObject 형태는 instantiateMonsterList이다.
        spawnedPlayerMonsterList = new List<Monster>();
        spawnedEnemyMonsterList = new List<Monster>();

        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, PlayerBossBattlePosList[i].transform.position, Quaternion.Euler(0, -90f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiatedMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, EnemyBossBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = enemyMonsterObj.GetComponent<Monster>();
            spawnedEnemyMonsterList.Add(tempMonster);


            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiatedMonsterList.Add(enemyMonsterObj);
        }

        //그 후에 소환된 Monster형태의 몬스터 리스트를 TurnManager에게 넘겨준다.
        TurnManager.Instance.SetMonsterInfomation(spawnedPlayerMonsterList, spawnedEnemyMonsterList);
    }

    //현재 턴인 몬스터를 불러온다.
    public void SetCurrentTurnMonster(Monster currentTurnMonster)
    {
        this.currentTurnMonster = currentTurnMonster;
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

        ////테스트용
        //foreach (var playerMonster in playerBattlePosList)
        //{
        //    print($"플레이어 몬스터 위치 : {playerMonster.transform.position}");
        //}

        //foreach (var enemyMonster in enemyBattlePosList)
        //{
        //    print($"적 몬스터 위치 : {enemyMonster.transform.position}");
        //}

        //공격페이즈 돌입
        attackCommand.PlayerNormalAttackExecute();

        //UIPopupManager.Instance.ClosePopup();
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

    public void ExecutePlayerFirstSkillAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.PlayerFristSkillAttackExecute();
        //UIPopupManager.Instance.ClosePopup();
    }

    //위와 같다
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

    //싸운 몬스터 오브젝트들 제거해주는 함수
    public void ClearAllBattleMapMonsterObj()
    {
        foreach(GameObject instantiateMonster in instantiatedMonsterList)
        {
            Destroy(instantiateMonster);
        }
        instantiatedMonsterList.Clear();
    }

    //플레이어의 몬스터도 데이터를 초기화 해주어야한다.
    public void InitializePlayerMonsterData()
    {
        for (int i = 0; i < spawnedPlayerMonsterList.Count; i++)
        {
            MonsterDataManager.Instance.selectedMonsterDataList[i].hp = spawnedPlayerMonsterList[i].hp;
            MonsterDataManager.Instance.selectedMonsterDataList[i].name = spawnedPlayerMonsterList[i].name;
            MonsterDataManager.Instance.selectedMonsterDataList[i].damage = spawnedPlayerMonsterList[i].damage;
            MonsterDataManager.Instance.selectedMonsterDataList[i].element = spawnedPlayerMonsterList[i].element;
            MonsterDataManager.Instance.selectedMonsterDataList[i].isEnemy = spawnedPlayerMonsterList[i].isEnemy;
            MonsterDataManager.Instance.selectedMonsterDataList[i].skills = spawnedPlayerMonsterList[i].skills;
            //MonsterDataManager.Instance.selectedMonsterDataList[i].selectedSkill.skillCount = spawnedPlayerMonsterList[i].selectedSkill.skillCount;
            MonsterDataManager.Instance.selectedMonsterDataList[i].level = spawnedPlayerMonsterList[i].level;
            MonsterDataManager.Instance.selectedMonsterDataList[i].currentExp = spawnedPlayerMonsterList[i].currentExp;
        }
    }

    //몬스터 정보는 저장해 놨던 처음 상태로 되돌려준다
    public void InitializeUnitMonsterData(Unit unit)
    {
        //담아놨던 originEnemyMonster의 정보를 담아준다.
        for(int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            unit.ownedMonsterList[i].hp = originEnemyMonsterDataList[i].Hp;
            unit.ownedMonsterList[i].name = originEnemyMonsterDataList[i].Name;
            unit.ownedMonsterList[i].damage = originEnemyMonsterDataList[i].Damage;
            unit.ownedMonsterList[i].element = originEnemyMonsterDataList[i].Element;
            unit.ownedMonsterList[i].isEnemy = originEnemyMonsterDataList[i].IsEnemy;
            unit.ownedMonsterList[i].skills = originEnemyMonsterDataList[i].Skills;

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

    //전투가 끝나고나서 전투의 상태를 초기화 해주는 함수
    public void InitializeBattleState()
    {
        //playerMonsterInBattleList.Clear();
        //enemyMonsterInBattleList.Clear();

        ClearAllBattleMapMonsterObj();
        //InitializeMonsterInfo();

        //행동 완료를 false로 바꿔준다.
        isPlayerActionComplete = false;
        isEnemyActionComplete = false;
    }
}
