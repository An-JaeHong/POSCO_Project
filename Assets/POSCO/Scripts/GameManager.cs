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
    //보스맵에서 생성될 포지션리스트
    public List<Transform> BossBattlePlayerPosList = new List<Transform>();
    public List<Transform> BossBattlePosList = new List<Transform>();

    //실제로 소환되는 몬스터들의 Prefab리스트
    private List<GameObject> instantiateMonsterList = new List<GameObject>();
    //깊은 복사로 저장할 원래 몬스터의 정보
    private List<MonsterDeepCopy> originEnemyMonster = new List<MonsterDeepCopy>();

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
    public void SetMonsterInformation(Player player, Unit unit)
    {
        //플레이어 몬스터 리스트는 얕은 복사로 받아온다. -> 정보가 변하면 안되기 때문에.
        playerMonsterInBattleList = player.selectedMonsterList;

        //일단 적 몬스터 리스트도 얕은 복사로 받아온 다음에 마지막에 정보를 초기화 해준다.
        for (int i = 0; i < 3; i++)
        {
            enemyMonsterInBattleList.Add(unit.ownedMonsterList[i]);
        }

        //새로 들어온 몬스터 정보는 일단 초기화
        originEnemyMonster.Clear();

        //여기에서 깊은 복사로 원래 몬스터 정보를 보관해준다.
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

        //일단 적 몬스터 리스트도 얕은 복사로 받아온 다음에 마지막에 정보를 초기화 해준다.
        enemyMonsterInBattleList = boss.ownedMonsterList;

        //새로 들어온 몬스터 정보는 일단 초기화
        originEnemyMonster.Clear();

        //여기에서 깊은 복사로 원래 몬스터 정보를 보관해준다.

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
    //플레이어와 적 지정된 포지션에 생성하기
    public void SetMonsterOnBattlePosition()
    {
        //실제 소환되는 몬스터들 -> Monster형태, GameObject 형태는 instantiateMonsterList이다.
        List<Monster> spawnedPlayerMonsterList = new List<Monster>();
        List<Monster> spawnedEnemyMonsterList = new List<Monster>();

        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, playerBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiateMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, enemyBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = enemyMonsterObj.GetComponent<Monster>();
            spawnedEnemyMonsterList.Add(tempMonster);

            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiateMonsterList.Add(enemyMonsterObj);
        }

        //그 후에 소환된 Monster형태의 몬스터 리스트를 TurnManager에게 넘겨준다.
        TurnManager.Instance.SetMonsterInfomation(spawnedPlayerMonsterList, spawnedEnemyMonsterList);
    }

    //보스맵에서 생성되는 함수
    public void SetMonsterOnBossMapPosition()
    {
        //실제 소환되는 몬스터들 -> Monster형태, GameObject 형태는 instantiateMonsterList이다.
        List<Monster> spawnedPlayerMonsterList = new List<Monster>();
        List<Monster> spawnedEnemyMonsterList = new List<Monster>();

        for (int i = 0; i < playerMonsterInBattleList.Count; i++)
        {
            GameObject playerMonsterObj = Instantiate(playerMonsterInBattleList[i].gameObject, BossBattlePlayerPosList[i].transform.position, Quaternion.Euler(0, -90f, 0));
            Monster tempMonster = playerMonsterObj.GetComponent<Monster>();
            spawnedPlayerMonsterList.Add(tempMonster);

            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiateMonsterList.Add(playerMonsterObj);
        }
        for (int i = 0; i < enemyMonsterInBattleList.Count; i++)
        {
            GameObject enemyMonsterObj = Instantiate(enemyMonsterInBattleList[i].gameObject, BossBattlePosList[i].transform.position, Quaternion.Euler(0, 90f, 0));
            Monster tempMonster = enemyMonsterObj.GetComponent<Monster>();
            spawnedEnemyMonsterList.Add(tempMonster);

            //나중에 한번에 삭제하기 편하게 하려고 리스트에 추가한다.
            instantiateMonsterList.Add(enemyMonsterObj);
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

        //테스트용
        foreach (var playerMonster in playerBattlePosList)
        {
            print($"플레이어 몬스터 위치 : {playerMonster.transform.position}");
        }

        foreach (var enemyMonster in enemyBattlePosList)
        {
            print($"적 몬스터 위치 : {enemyMonster.transform.position}");
        }

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
        UIPopupManager.Instance.ClosePopup();
    }

    //위와 같다
    public void ExecuteEnemyAttackAction(Monster attacker, Monster target)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, target);
        attackCommand.EnemyAttackExecute();
    }

    //싸운 몬스터 오브젝트들 제거해주는 함수
    public void ClearBattleMonsters()
    {
        foreach(GameObject instantiateMonster in instantiateMonsterList)
        {
            Destroy(instantiateMonster);
        }
        instantiateMonsterList.Clear();
    }

    //몬스터 정보는 초기화 해준다.
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

    //전투가 끝나고나서 전투의 상태를 초기화 해주는 함수
    public void InitializeBattleState()
    {
        //playerMonsterInBattleList.Clear();
        //enemyMonsterInBattleList.Clear();

        ClearBattleMonsters();
        InitializeMonsterInfo();

        //행동 완료를 false로 바꿔준다.
        isPlayerActionComplete = false;
        isEnemyActionComplete = false;
    }
}
