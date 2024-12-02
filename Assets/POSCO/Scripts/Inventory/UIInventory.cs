using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    //�÷��̾ ����ִ� �������� ���� 
    public List<Monster> playerMonsterList = new List<Monster>();
    //���õ� ���Ͱ� �ӽ÷� ����Ǵ� ���� 
    public List<Monster> TempSelectedMonsterList = new List<Monster>();


    // �κ��丮�� ��� ���� ��ü ����Ʈ
    public List<GameObject> textureMonsterPrefabsList;
    // �κ��丮�� ��� ���� ����Ʈ
    public List<GameObject> texturePlayerMonsterList; 

    public GameObject[] cameraForMonster;

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


