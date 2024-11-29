using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        InitializeTurnQueue();
        StartCoroutine(HandleTurn());
    }

    //GameManager���� ���� -> ��Ʋ�� ���� ���͵��� �޾ƿ´�.
    public void SetMonsterInfomation(List<Monster> playerMonsterList, List<Monster> enemyMonsterList)
    {
        this.playerMonsterList = playerMonsterList;
        this.enemyMonsterList = enemyMonsterList;
    }

    //�� �ʱ�ȭ -> ���� �ϳ��̴ϱ� ������ �� ������ �ʱ�ȭ �ϴ� ������� �ؾ��� ��
    private void InitializeTurnQueue()
    {
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
    }

    //Monster�� ���� �ٲ� ���� GameManger�� UIPopup�� ������ �Ѱ��� �� �־���Ѵ�.
    public event Action<Monster> monsterTurnChange;
    //�������� �� ����
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

            //0�� �ƴ϶�� �̾Ƽ� ��
            currentTurnMonster = turnQueue.Dequeue();

            //�ٵ� �̾� ��µ� �� ���� Creature�� ü���� 0 ���϶�� �׳� ����
            if (currentTurnMonster.hp <= 0)
            {
                yield return null;
                continue;
            }

            print($"{currentTurnMonster.name}�� ��!");
            monsterTurnChange?.Invoke(currentTurnMonster);

            //�÷��̾���
            if (!currentTurnMonster.isEnemy)
            {
                yield return StartCoroutine(HandlePlayerTurn(currentTurnMonster));
            }
            //���̶��
            else
            {
                yield return StartCoroutine(HandleEnemyTurn(currentTurnMonster));
            }

            //�� �� ������ �����ϸ� ��
            if (CheckBattleEnd())
            {
                break;
            }
        }

    }

    //�÷��̾ ���� ���ε� �Ѹ� �����ϰ� �ؾ���
    private IEnumerator HandlePlayerTurn(Monster player)
    {
        //print($"{player.name}�� ������ �ұ�?");

        //�ǳ��� ������
        //UIPopup.Instance.ChooseBattleStateCanvasOpen();
        yield return new WaitUntil(() => GameManager.Instance.isPlayerActionComplete);

        turnQueue.Enqueue(player); //�ٽ� Queue�� �־��� �׷��� ��� �ݺ�
        //�ٽ� �÷��̾� �׼� ����
        GameManager.Instance.isPlayerActionComplete = false;
    }

    //Action�� �Ķ���͸� �ϳ��� �Ѱܹ��� �� �־ ����ü�� �̿��� �����ϴ� ��, �����ϴ� ��� �Ѵ� ����
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

    //�� ���� ����Ǹ� �ҷ��� Action
    //public event Action<ActionData> enemyAttack;
    private IEnumerator HandleEnemyTurn(Monster enemy)
    {
        print($"{enemy.name}��(��) �ൿ ��...");
        yield return new WaitForSeconds(1f);

        //ü���� 0���� ���� ��� ã��
        List<Monster> targetMonsterList = playerMonsterList.FindAll(M => M.hp > 0);

        //�Ѹ����� ������ ����
        if (targetMonsterList.Count > 0)
        {
            //����ִ� �� �߿��� Random���� Ÿ���� ����
            Monster targetMonster = targetMonsterList[UnityEngine.Random.Range(0, targetMonsterList.Count)];

            //�״��� event�� �޾ƿ� ���� �ʱ�ȭ
            ActionData actionData = new ActionData(enemy, targetMonster);
            //����ִ� event ����
            //enemyAttack?.Invoke(actionData);
            yield return new WaitUntil(() => GameManager.Instance.isEnemyActionComplete);
        }
        turnQueue.Enqueue(enemy);
    }
    
    //������ �������� Ȯ��
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

    ////GameManager, UIpop���� �� ���� �����ϴ� ���� ������ �Ѱ���
    //public void SetCurrentTurnMonster(Monster currentMonster)
    //{
    //    currentMonster = currentTurnMonster;
    //}
}
