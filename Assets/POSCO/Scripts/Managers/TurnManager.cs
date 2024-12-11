using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class TurnManager : MonoBehaviour
{
    //외부에서도 접근이 가능해야함
    private static TurnManager instance;
    public static TurnManager Instance { get { return instance; } }

    ////외부에서 접근하면 안되서 Serializable로 안씀
    //public enum Turn
    //{
    //    None,
    //    PlayerTurn,
    //    EnemyTurn,
    //}

    //턴
    public Queue<Monster> turnQueue = new Queue<Monster>();
    //현재 턴을 가지고 있는 캐릭터
    public Monster currentTurnMonster;
    //입력되어있는 커멘드
    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    //GameManager에서 넘겨받을 MonsterList
    public List<Monster> playerMonsterList = new List<Monster>();
    public List<Monster> enemyMonsterList = new List<Monster>();

    public bool isTurnDone = false;

    //모든 아군과 적의 몬스터가 죽었는지
    public bool allPlayerMonstersDead = false;
    public bool allEnemyMonstersDead = false;


    //전투 끝나면 실행되는 액션
    public event Action OnBattleEnd;

    ////턴이 바뀌면 실행되는 이벤트들을 모아둘꺼임
    //public event Action<Turn> onTurnChanged;

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

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //시작하면 턴 초기화 후 실행
        //InitializeTurnQueue();
        //StartCoroutine(HandleTurn());
    }

    //GameManager에서 연결 -> 배틀에 들어온 몬스터들을 받아온다.
    public void SetMonsterInfomation(List<Monster> playerMonsterList, List<Monster> enemyMonsterList)
    {
        this.playerMonsterList = playerMonsterList;
        this.enemyMonsterList = enemyMonsterList;

        //테스트용
        //foreach (Monster playerMonster in this.playerMonsterList)
        //{
        //    print($"TurnManager에서 실행 : {playerMonster.transform.position}");
        //}
        ////테스트용
        //for (int i = 0; i < 3; i++)
        //{
        //    if (this.playerMonsterList[i].animator != null)
        //    {
        //        print("animator 다 붙어있구만유");
        //    }
        //}

        //else
        //{
        //    for(int i = 0; i < 3; i++)
        //    {
        //        this.playerMonsterList[i].animator = playerMonsterList[i].GetComponent<Animator>();
        //        this.enemyMonsterList[i].animator = enemyMonsterList[i].GetComponent<Animator>();
        //    }
        //}
        //if (this.playerMonsterList[0].animator != null)
        //{
        //    print("동네사람들 여기 애니메이터 붙어있어요!");
        //}
        //else
        //{
        //    print("아직도 안붙음");
        //}
    }

    //턴 초기화 -> 씬이 하나이니까 전투가 다 끝나면 초기화 하는 방식으로 해야할 듯
    public void InitializeTurnQueue()
    {
        //초기화할때, 만난 몬스터들의 isEnemy를 true로 만들어준다. 이래야지 턴이 잘 작동한다. -> 조금 마음에 들지는 않는다.
        foreach (Monster enemyMonster in enemyMonsterList)
        {
            enemyMonster.isEnemy = true;
        }
        //시작할때 마다 턴 초기화
        turnQueue.Clear();

        foreach (Monster playerCreature in playerMonsterList)
        {
            //살아 있는 얘들만 Queue에 넣을거임
            if (playerCreature.hp > 0)
            {
                turnQueue.Enqueue(playerCreature);
            }
        }
        foreach (Monster enemyCreature in enemyMonsterList)
        {
            //살아 있는 얘들만 Queue에 넣을거임
            if (enemyCreature.hp > 0)
            {
                turnQueue.Enqueue(enemyCreature);
            }
        }
        //이렇게되면 플레이어1, 2, 3, 적1, 2, 3 순서로 Queue가 형성이 된다.

        StartCoroutine(HandleTurn());
    }

    //Monster의 턴이 바뀔떄 마다 호출되는 이벤트
    public event Action<Monster> monsterTurnChange;
    //본격적인 턴 관리
    private IEnumerator HandleTurn()
    {
        while (true)
        {
            //턴이 0 이면 턴 초기화 후 계속 진행
            if (turnQueue.Count == 0)
            {
                InitializeTurnQueue();
                yield return null;
                continue;
            }

            //0이 아니라면 뽑아서 씀
            currentTurnMonster = turnQueue.Dequeue();

            //근데 뽑아 썼는데 그 턴의 Creature가 체력이 0 이하라면 그냥 진행
            if (currentTurnMonster.hp <= 0)
            {
                yield return null;
                continue;
            }

            print($"{currentTurnMonster.name}의 턴!");

            //테스트용 
            //print($"현재턴인 몬스터의 위치 : {currentTurnMonster.transform.position}");

            monsterTurnChange?.Invoke(currentTurnMonster);

            //플레이어라면
            if (!currentTurnMonster.isEnemy)
            {
                yield return StartCoroutine(HandlePlayerTurn(currentTurnMonster));
            }
            //적이라면
            else
            {
                //팝업 닫아주고
                UIPopupManager.Instance.ClosePopup();
                yield return StartCoroutine(HandleEnemyTurn(currentTurnMonster));
            }

            //둘 중 한팀이 전멸하면 끝
            if (CheckBattleEnd())
            {
                break;
            }
        }

    }

    //플레이어에 관한 턴인데 한명씩 공격하게 해야함
    private IEnumerator HandlePlayerTurn(Monster player)
    {
        //isPlayerActionComplete가 true가 될때까지 기다린다.
        yield return new WaitUntil(() => GameManager.Instance.isPlayerActionComplete);

        turnQueue.Enqueue(player); //다시 Queue에 넣어줌 그래야 계속 반복
        //다시 플레이어 액션 실행
        GameManager.Instance.isPlayerActionComplete = false;
    }

    //Action은 파라미터를 하나만 넘겨받을 수 있어서 구조체를 이용해 공격하는 자, 공격하는 대상 둘다 받음
    //public struct ActionData
    //{
    //    public Monster attacker;
    //    public Monster target;

    //    public ActionData(Monster attacker, Monster target)
    //    {
    //        this.attacker = attacker;
    //        this.target = target;
    //    }
    //}

    //적 턴이 실행되면 불러올 Action
    //public event Action<ActionData> enemyAttack;
    private IEnumerator HandleEnemyTurn(Monster enemy)
    {
        print($"{enemy.name}이(가) 행동 중...");
        yield return new WaitForSeconds(1f);

        //체력이 0보다 높은 대상 찾기
        List<Monster> targetMonsterList = playerMonsterList.FindAll(M => M.hp > 0);

        //한마리라도 있으면 실행
        if (targetMonsterList.Count > 0)
        {
            //살아있는 플레이어 몬스터 중에서 Random으로 타겟을 지정
            Monster targetMonster = targetMonsterList[UnityEngine.Random.Range(0, targetMonsterList.Count)];

            //그다음 event로 받아온 변수 초기화
            //ActionData actionData = new ActionData(enemy, targetMonster);
            //enemyAttack?.Invoke(actionData);
            //담겨있던 event 실행
            //enemyAttack?.Invoke(actionData);

            //테스트
            print($"현재 공격중인 적 몬스터의 이름 : {currentTurnMonster}");
            print($"현재 타겟이 된 플레이어 몬스터의 이름 : {targetMonster}");
            print($"현재 공격중인 적 몬스터의 스킬 : {currentTurnMonster.selectedSkill}");

            GameManager.Instance.ExecuteEnemyAttackAction(currentTurnMonster, targetMonster);
            yield return new WaitUntil(() => GameManager.Instance.isEnemyActionComplete);
            GameManager.Instance.isEnemyActionComplete = false;
        }
        turnQueue.Enqueue(enemy);
    }
    
    //전투가 끝났는지 확인
    public bool CheckBattleEnd()
    {
        allPlayerMonstersDead = playerMonsterList.All(m => m.hp <= 0);
        allEnemyMonstersDead = enemyMonsterList.All(m => m.hp <= 0);

        ////플레이어 몬스터가 전멸할때
        //if (allPlayerMonstersDead)
        //{
        //    AllPlayerMonsterDead();
        //    print("플레이어 몬스터 전멸");
        //    return true;
        //}
        ////적 몬스터가 전멸할때
        //else if (allEnemyMonstersDead)
        //{
        //    AllEnemyMonsterDead();
        //    print("적 몬스터 전멸");
        //    return true;
        //}
        
        //return false;
        if (allPlayerMonstersDead == true || allEnemyMonstersDead == true)
        {
            //AllPlayerMonsterDead();
            //AllEnemyMonsterDead();

            //전투가 종료되면 불러올 함수들
            OnBattleEnd?.Invoke();
            return true;
        }
        return false;
    }

    //전투 끝나면 실행되는 액션
    //public event Action OnBattleEnd;

    //public GameObject contactedFieldMonster;

    //public void AllPlayerMonsterDead()
    //{
    //    //Destroy(contactedFieldMonster);
    //    OnBattleEnd?.Invoke();
    //}

    //public void AllEnemyMonsterDead()
    //{
    //    Destroy(contactedFieldMonster);
    //    OnBattleEnd?.Invoke();
    //}

    ////GameManager, UIpop에서 쓸 현재 공격하는 몬스터 정보를 넘겨줌
    //public void SetCurrentTurnMonster(Monster currentMonster)
    //{
    //    currentMonster = currentTurnMonster;
    //}
}
