using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;




[Serializable]
public class ItemInfo
{
    public string itemName;
    public int healingAmount;
    public ItemInfo(string name, int healing)
    {
        itemName = name;
        healingAmount = healing;
    }

}
public class Item : MonoBehaviour
{
    // 아이템 목록
    public List<ItemInfo> items;
    public Text itemInfo;

    private UIInventory uiInventory;

    private void Start()
    {
        uiInventory = FindObjectOfType<UIInventory>();
        items = new List<ItemInfo>
        {
            new ItemInfo("Health Potion", 20),
            new ItemInfo("Mana Potion", 15),
            new ItemInfo("Elixir", 30)
        };
    }

 

    // 아이템 사용 메서드
    public void Use(int number, Monster monster)
    {
        ItemInfo itemToUse = items[number];
        monster.hp += itemToUse.healingAmount;
        monster.hp = Mathf.Clamp(monster.hp, 0, monster.maxHp); // 최대 HP를 초과하지 않도록 클램프
    
        print($"{itemToUse.itemName} 사용, 현재 HP: {monster.hp}");
    }


}
