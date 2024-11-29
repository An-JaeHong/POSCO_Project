using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //외부에서 접근하면 안되서 Serializable로 안씀
    public enum Turn
    {
        None,
        PlayerTurn,
        EnemyTurn,
    }

    //외부에서도 접근이 가능해야함
    private static TurnManager instance;
    public static TurnManager Instance { get { return instance; } }

    //외부에서 턴을 바꿀 수 없어야 함
    public Turn currentTurn { get; private set; }

    //턴
    private Queue<Monster> turnQueue = new Queue<Monster>();
    //현재 턴을 가지고 있는 캐릭터
    private Monster currentTurnMonster;

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

    //GameManager에서 연결
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
        }

    }

    //플레이어에 관한 턴인데 한명씩 공격하게 해야함
    private IEnumerator HandlePlayerTurn(Monster player)
    {
        print($"{player.name}는 무엇을 할까?");

        //판넬을 보여줌
        UIPopup.Instance.ChooseBattleStateCanvasOpen(player);
        yield return new WaitUntil(() => GameManager.Instance.IsPlayerActionComplete);
        turnQueue.Enqueue(player); //다시 Queue에 넣어줌 그래야 계속 반복
    }

    //Action은 파라미터를 하나만 넘겨받을 수 있어서 구조체를 이용해 공격하는 자, 공격하는 대상 둘다 받음
    public struct ActionData
    {
        public CreatureInfo attacker;
        public CreatureInfo target;

        public ActionData(CreatureInfo attacker, CreatureInfo target)
        {
            this.attacker = attacker;
            this.target = target;
        }
    }

    //적 턴이 실행되면 불러올 Action
    public event Action<ActionData> enemyAttack;
    private IEnumerator HandleEnemyTurn(CreatureInfo enemy)
    {
        print($"{enemy.name}이(가) 행동 중...");
        yield return new WaitForSeconds(1f);

        //체력이 0보다 높은 대상 찾기
        List<CreatureInfo> targetList = playerCreatureList.FindAll(c => c.hp > 0);

        if (targetList.Count > 0)
        {
            CreatureInfo target = targetList[UnityEngine.Random.Range(0, targetList.Count)];
            ActionData actionData = new ActionData(enemy, target);

            //담겨있던 event 실행
            enemyAttack?.Invoke(actionData);
            yield return new WaitUntil(() => InGameManager.Instance.IsEnemyActionComplete);
        }
        turnQueue.Enqueue(enemy);
    }

}
