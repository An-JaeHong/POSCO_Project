using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    //�÷��̾��� ��� ���� �����͸� ������ ����Ʈ
    public List<Monster> allMonsterDataList = new List<Monster>();
    //���õ� ���� ������ ����Ʈ
    public List<Monster> selectedMonsterDataList = new List<Monster>();

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        //�÷��̾� ����
        player = FindAnyObjectByType<Player>();
        player.onClickSelectButton += InitializeSelectedPlayerMonsterData;
        BringPlayerAllMonsterData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            BringPlayerAllMonsterData();
        }
            //InitializeSelectedPlayerMonsterData();
        
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
            print($"{allMonsterDataList[0]}");
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
