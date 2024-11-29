using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        InitializeTurnQueue();
        StartCoroutine(HandleTurn());
    }

    //GameManager에서 연결 -> 배틀에 들어온 몬스터들을 받아온다.
    public void SetMonsterInfomation(List<Monster> playerMonsterList, List<Monster> enemyMonsterList)
    {
        this.playerMonsterList = playerMonsterList;
        this.enemyMonsterList = enemyMonsterList;
    }

    //턴 초기화 -> 씬이 하나이니까 전투가 다 끝나면 초기화 하는 방식으로 해야할 듯
    private void InitializeTurnQueue()
    {
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
    }

    //Monster의 턴이 바뀔때 마다 GameManger와 UIPopup에 정보를 넘겨줄 수 있어야한다.
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
            monsterTurnChange?.Invoke(currentTurnMonster);

            //플레이어라면
            if (!currentTurnMonster.isEnemy)
            {
                yield return StartCoroutine(HandlePlayerTurn(currentTurnMonster));
            }
            //적이라면
            else
            {
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
        //print($"{player.name}는 무엇을 할까?");

        //판넬을 보여줌
        //UIPopup.Instance.ChooseBattleStateCanvasOpen();
        yield return new WaitUntil(() => GameManager.Instance.isPlayerActionComplete);

        turnQueue.Enqueue(player); //다시 Queue에 넣어줌 그래야 계속 반복
        //다시 플레이어 액션 실행
        GameManager.Instance.isPlayerActionComplete = false;
    }

    //Action은 파라미터를 하나만 넘겨받을 수 있어서 구조체를 이용해 공격하는 자, 공격하는 대상 둘다 받음
    public struct ActionData
    {
        public Monster attacker;
        public Monster target;

        public ActionData(Monster attacker, Monster target)
        {
            this.attacker = attacker;
            this.target = target;
        }
    }

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
            //살아있는 얘 중에서 Random으로 타겟을 지정
            Monster targetMonster = targetMonsterList[UnityEngine.Random.Range(0, targetMonsterList.Count)];

            //그다음 event로 받아온 변수 초기화
            ActionData actionData = new ActionData(enemy, targetMonster);
            //담겨있던 event 실행
            //enemyAttack?.Invoke(actionData);
            yield return new WaitUntil(() => GameManager.Instance.isEnemyActionComplete);
        }
        turnQueue.Enqueue(enemy);
    }
    
    //전투가 끝났는지 확인
    public bool CheckBattleEnd()
    {
        bool allPlayerMonstersDead = playerMonsterList.All(m => m.hp <= 0);
        bool allEnemiesDead = enemyMonsterList.All(m => m.hp <= 0);

        if (allPlayerMonstersDead || allEnemiesDead)
        {
            return true;
        }

        return false;
    }

    ////GameManager, UIpop에서 쓸 현재 공격하는 몬스터 정보를 넘겨줌
    //public void SetCurrentTurnMonster(Monster currentMonster)
    //{
    //    currentMonster = currentTurnMonster;
    //}
}
