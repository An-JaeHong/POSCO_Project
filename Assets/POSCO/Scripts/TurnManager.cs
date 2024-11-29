using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //�ܺο��� �����ϸ� �ȵǼ� Serializable�� �Ⱦ�
    public enum Turn
    {
        None,
        PlayerTurn,
        EnemyTurn,
    }

    //�ܺο����� ������ �����ؾ���
    private static TurnManager instance;
    public static TurnManager Instance { get { return instance; } }

    //�ܺο��� ���� �ٲ� �� ����� ��
    public Turn currentTurn { get; private set; }

    //��
    private Queue<Monster> turnQueue = new Queue<Monster>();
    //���� ���� ������ �ִ� ĳ����
    private Monster currentTurnMonster;

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

    //GameManager���� ����
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
        }

    }

    //�÷��̾ ���� ���ε� �Ѹ� �����ϰ� �ؾ���
    private IEnumerator HandlePlayerTurn(Monster player)
    {
        print($"{player.name}�� ������ �ұ�?");

        //�ǳ��� ������
        UIPopup.Instance.ChooseBattleStateCanvasOpen(player);
        yield return new WaitUntil(() => GameManager.Instance.IsPlayerActionComplete);
        turnQueue.Enqueue(player); //�ٽ� Queue�� �־��� �׷��� ��� �ݺ�
    }

    //Action�� �Ķ���͸� �ϳ��� �Ѱܹ��� �� �־ ����ü�� �̿��� �����ϴ� ��, �����ϴ� ��� �Ѵ� ����
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

    //�� ���� ����Ǹ� �ҷ��� Action
    public event Action<ActionData> enemyAttack;
    private IEnumerator HandleEnemyTurn(CreatureInfo enemy)
    {
        print($"{enemy.name}��(��) �ൿ ��...");
        yield return new WaitForSeconds(1f);

        //ü���� 0���� ���� ��� ã��
        List<CreatureInfo> targetList = playerCreatureList.FindAll(c => c.hp > 0);

        if (targetList.Count > 0)
        {
            CreatureInfo target = targetList[UnityEngine.Random.Range(0, targetList.Count)];
            ActionData actionData = new ActionData(enemy, target);

            //����ִ� event ����
            enemyAttack?.Invoke(actionData);
            yield return new WaitUntil(() => InGameManager.Instance.IsEnemyActionComplete);
        }
        turnQueue.Enqueue(enemy);
    }

}
