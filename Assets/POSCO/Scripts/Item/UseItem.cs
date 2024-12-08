using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public Item item; // 사용할 아이템
    private MonsterDataManager monsterDataManager;


    private void Start()
    {
        monsterDataManager = MonsterDataManager.Instance;
    }
    // 버튼 클릭 시 호출되는 메서드
    public void OnUseItem(int monsterIndex, int itemIndex)
    {
        Monster selectedMonster = monsterDataManager.selectedMonsterDataList[monsterIndex];
        item.Use(itemIndex, selectedMonster);

    }

}
