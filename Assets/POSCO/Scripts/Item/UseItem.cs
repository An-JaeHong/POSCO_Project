using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public Monster monster; // 플레이어 스크립트 참조
    public Item item; // 사용할 아이템

    // 버튼 클릭 시 호출되는 메서드
    public void OnUseItem()
    {
        if (item != null)
        {
            item.Use(monster);
        }
    }

}
