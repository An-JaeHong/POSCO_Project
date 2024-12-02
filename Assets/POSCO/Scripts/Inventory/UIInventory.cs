using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    //�÷��̾ ����ִ� �������� ���� (Ȯ���� ���� public���� �����ص� ������ ������ Ȯ�εǸ� private����)
    public List<Monster> playerMonsterList = new List<Monster>();
    //���õ� ���Ͱ� �ӽ÷� ����Ǵ� ���� (Ȯ���� ���� public���� �����ص� ������ ������ Ȯ�εǸ� private����)
    public List<Monster> TempSelectedMonsterList = new List<Monster>();

    // �κ��丮�� ��� ���� ��ü ����Ʈ(public ->private)
    public List<GameObject> textureMonsterPrefabsList;
    // �κ��丮�� ��� ���� ����Ʈ(public ->private)
    public List<GameObject> texturePlayerMonsterList; 

    

    public GameObject MonsterCardPrefab;

    private Player player;

    private void Start()
    {

        //player�� ���� ���� ���������
        BringPlaterMonsterList();
    }

    private void Update()
    {
        
    }

    //GameObject-> Monster ��ȯ �� playerMonsterList�� ���� 

    private void BringPlaterMonsterList()
    {
        foreach (GameObject monsterObj in player.playerMonsterPrefabList)
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster monster))
            {
                playerMonsterList.Add(monster);
            }
        }
    }







}


