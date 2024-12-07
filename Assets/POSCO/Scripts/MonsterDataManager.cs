using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    private static MonsterDataManager instance;
    public static MonsterDataManager Instance { get { return instance; } }

    //�÷��̾��� ��� ���� �����͸� ������ ����Ʈ
    public List<Monster> allMonsterDataList = new List<Monster>();
    //���õ� ���� ������ ����Ʈ
    public List<Monster> selectedMonsterDataList = new List<Monster>();

    private Player player;

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

    void Start()
    {
        //�÷��̾� ����
        allMonsterDataList.Clear();
        selectedMonsterDataList.Clear();
        player = FindAnyObjectByType<Player>();
        BringPlayerAllMonsterData();

        //�÷��̾ 3������ ���͸� �����ϸ� �Լ��� ����ȴ�.
        player.onClickSelectButton += InitializeSelectedPlayerMonsterData;
    }

    //���� �÷��̾ ����ִ� ���� �������� ������ �� �ҷ��´�
    private void BringPlayerAllMonsterData()
    {
        foreach(GameObject monsterObj in player.playerMonsterPrefabList)
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster monster))
            {
                allMonsterDataList.Add(monster);
            }
        }
    }

    //select�ϴ� ��ư�� ������ �̺�Ʈ�� �߰����ָ� ���� �� -> �̰� allMonsterDataList���� �����;���
    public void InitializeSelectedPlayerMonsterData()
    {
        //���� ���õȰ� �ϴ� ����� -> �ٵ� �̰� ��ư�� ������ ��Ŀ��� ������.
        selectedMonsterDataList.Clear();

        //�÷��̾��� ���� ������ 3���� �Ǹ�
        if (player.selectedMonsterList.Count == 3)
        {
            //��� ���� �������� ����Ʈ���� 
            foreach(Monster selectedMonster in player.selectedMonsterList)
            {
                Monster matchedMonster = allMonsterDataList.Find(monster => monster.name == selectedMonster.name);
                if (matchedMonster != null)
                {
                    selectedMonsterDataList.Add(matchedMonster);
                }
            }
        }
    }

    //selectButton ���� -> �÷��̾ ���� ������ ���� ����Ʈ�� �״�� �޾ƿ� (�����͸�)
    //-> �������� �� �޾ƿ� ����Ʈ�� ������ ���� -> �׷��� �÷��̾� ������ ���� ü�µ� ����ִ� �����̰�, ���� ���õ� ����ü�µ�
    //-> ����ִ� �����̴�. -> ������ ������ �� �������� ������ 5�� �ִ� ������ �Ѱ��ش�. �� �׷��� 5������ �̾ƿ��� ������ �ؾ߰ڳ�
    //-> �Ѱ��ٶ� ���õ� ���� ����Ʈ�� �״�� ��������, ���� ü���� 0���� ������ ����Ʈ���� ���ִ� ������?
    //-> ��� �ʱ�ȭ ��ư ������ ����Ʈ�� �ʱ�ȭ �ؾ��ҵ�
    //-> �ƴ� �÷��̾��� ���õ� ������ ���ڰ� 3������ �׶� �ʱ�ȭ�� �����ص� �ɵ� ���� �̰� ������? �׷��� 3���� ���ϸ�
    //-> �ƿ� ������ �ȳѾ���°��� �׸��� ������ �÷��̾��� ���� ������ 3���� ���ϸ� �ȵȴٴ� ��� �߰� �׷� ��¥�� ������ �ȵ���
    //��� ������
}
