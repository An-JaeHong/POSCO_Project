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
    //�ܺο����� ������ �����ؾ���
    private static TurnManager instance;
    public static TurnManager Instance { get { return instance; } }

    ////�ܺο��� �����ϸ� �ȵǼ� Serializable�� �Ⱦ�
    //public enum Turn
    //{
    //    None,
    //    PlayerTurn,
    //    EnemyTurn,
    //}

    //��
    public Queue<Monster> turnQueue = new Queue<Monster>();
    //���� ���� ������ �ִ� ĳ����
    public Monster currentTurnMonster;
    //�ԷµǾ��ִ� Ŀ���
    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    //GameManager���� �Ѱܹ��� MonsterList
    public List<Monster> playerMonsterList = new List<Monster>();
    public List<Monster> enemyMonsterList = new List<Monster>();

    public bool isTurnDone = false;

    //��� �Ʊ��� ���� ���Ͱ� �׾�����
    public bool allPlayerMonstersDead = false;
    public bool allEnemyMonstersDead = false;


    //���� ������ ����Ǵ� �׼�
    public event Action OnBattleEnd;

    ////���� �ٲ�� ����Ǵ� �̺�Ʈ���� ��ƵѲ���
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
        //�����ϸ� �� �ʱ�ȭ �� ����
        //InitializeTurnQueue();
        //StartCoroutine(HandleTurn());
    }

    //GameManager���� ���� -> ��Ʋ�� ���� ���͵��� �޾ƿ´�.
    public void SetMonsterInfomation(List<Monster> playerMonsterList, List<Monster> enemyMonsterList)
    {
        this.playerMonsterList = playerMonsterList;
        this.enemyMonsterList = enemyMonsterList;

        //�׽�Ʈ��
        //foreach (Monster playerMonster in this.playerMonsterList)
        //{
        //    print($"TurnManager���� ���� : {playerMonster.transform.position}");
        //}
        ////�׽�Ʈ��
        //for (int i = 0; i < 3; i++)
        //{
        //    if (this.playerMonsterList[i].animator != null)
        //    {
        //        print("animator �� �پ��ֱ�����");
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
        //    print("���׻���� ���� �ִϸ����� �پ��־��!");
        //}
        //else
        //{
        //    print("������ �Ⱥ���");
        //}
    }

    //�� �ʱ�ȭ -> ���� �ϳ��̴ϱ� ������ �� ������ �ʱ�ȭ �ϴ� ������� �ؾ��� ��
    public void InitializeTurnQueue()
    {
        //�ʱ�ȭ�Ҷ�, ���� ���͵��� isEnemy�� true�� ������ش�. �̷����� ���� �� �۵��Ѵ�. -> ���� ������ ������ �ʴ´�.
        foreach (Monster enemyMonster in enemyMonsterList)
        {
            enemyMonster.isEnemy = true;
        }
        //�����Ҷ� ���� �� �ʱ�ȭ
        turnQueue.Clear();

        foreach (Monster playerCreature in playerMonsterList)
        {
            //��� �ִ� ��鸸 Queue�� ��������
            if (playerCreature.hp > 0)
            {
                turnQueue.Enqueue(playerCreature);
            }
        }
        foreach (Monster enemyCreature in enemyMonsterList)
        {
            //��� �ִ� ��鸸 Queue�� ��������
            if (enemyCreature.hp > 0)
            {
                turnQueue.Enqueue(enemyCreature);
            }
        }
        //�̷��ԵǸ� �÷��̾�1, 2, 3, ��1, 2, 3 ������ Queue�� ������ �ȴ�.

        StartCoroutine(HandleTurn());
    }

    public event Action<Monster> monsterTurnChange;

    private IEnumerator HandleTurn()
    {
        while (true)
        {
            //���� 0 �̸� �� �ʱ�ȭ �� ��� ����
            if (turnQueue.Count == 0)
            {
                InitializeTurnQueue();
                yield return null;
                continue;
            }

            currentTurnMonster = turnQueue.Dequeue();

            if (currentTurnMonster.hp <= 0)
            {
                yield return null;
                continue;
            }

            print($"{currentTurnMonster.name}�� ��!");

            monsterTurnChange?.Invoke(currentTurnMonster);

            if (!currentTurnMonster.isEnemy)
            {
                yield return StartCoroutine(HandlePlayerTurn(currentTurnMonster));
            }
            else
            {
                UIPopupManager.Instance.ClosePopup();
                yield return StartCoroutine(HandleEnemyTurn(currentTurnMonster));
            }

            //�� �� ������ �����ϸ� ��
            if (CheckBattleEnd())
            {
                break;
            }
        }

    }

    //�÷��̾ ���� ���ε� �Ѹ�� �����ϰ� �ؾ���
    private IEnumerator HandlePlayerTurn(Monster player)
    {
        //isPlayerActionComplete�� true�� �ɶ����� ��ٸ���.
        yield return new WaitUntil(() => GameManager.Instance.isPlayerActionComplete);

        turnQueue.Enqueue(player); //�ٽ� Queue�� �־��� �׷��� ��� �ݺ�
        //�ٽ� �÷��̾� �׼� ����
        GameManager.Instance.isPlayerActionComplete = false;
    }

    private IEnumerator HandleEnemyTurn(Monster enemy)
    {
        print($"{enemy.name}��(��) �ൿ ��...");
        yield return new WaitForSeconds(1f);

        List<Monster> targetMonsterList = playerMonsterList.FindAll(M => M.hp > 0);

        if (targetMonsterList.Count > 0)
        {
            Monster targetMonster = targetMonsterList[UnityEngine.Random.Range(0, targetMonsterList.Count)];

            print($"���� �������� �� ������ �̸� : {currentTurnMonster}");
            print($"���� Ÿ���� �� �÷��̾� ������ �̸� : {targetMonster}");
            print($"���� �������� �� ������ ��ų : {currentTurnMonster.selectedSkill}");

            GameManager.Instance.ExecuteEnemyAttackAction(currentTurnMonster, targetMonster);
            yield return new WaitUntil(() => GameManager.Instance.isEnemyActionComplete);
            GameManager.Instance.isEnemyActionComplete = false;
        }
        turnQueue.Enqueue(enemy);
    }
    
    //������ �������� Ȯ��
    public bool CheckBattleEnd()
    {
        allPlayerMonstersDead = playerMonsterList.All(m => m.hp <= 0);
        allEnemyMonstersDead = enemyMonsterList.All(m => m.hp <= 0);

        if (allPlayerMonstersDead == true || allEnemyMonstersDead == true)
        {
            OnBattleEnd?.Invoke();
            return true;
        }
        return false;
    }

    //���� ������ ����Ǵ� �׼�
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

    ////GameManager, UIpop���� �� ���� �����ϴ� ���� ������ �Ѱ���
    //public void SetCurrentTurnMonster(Monster currentMonster)
    //{
    //    currentMonster = currentTurnMonster;
    //}
}
