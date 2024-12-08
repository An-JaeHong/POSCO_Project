using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public Item item; // ����� ������
    private MonsterDataManager monsterDataManager;


    private void Start()
    {
        monsterDataManager = MonsterDataManager.Instance;
    }
    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnUseItem(int monsterIndex, int itemIndex)
    {
        Monster selectedMonster = monsterDataManager.selectedMonsterDataList[monsterIndex];
        item.Use(itemIndex, selectedMonster);

    }

}
