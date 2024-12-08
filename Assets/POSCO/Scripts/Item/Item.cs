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
    public int healingAmount;//고정 
    public float healingPercentage; // 비율 회복량
  
    public ItemInfo(string name, int healing)
    {
        itemName = name;
        healingAmount = healing;
        healingPercentage = 0f;
    }
    public ItemInfo(string name, float healingPercentage)
    {
        itemName = name;
        healingAmount = 0; // 기본값
        this.healingPercentage = healingPercentage;
    }

}
public class Item
{
    // 아이템 목록
    public List<ItemInfo> items;
    public Text itemInfo;

    private UIInventory uiInventory;

    private void Start()
    {

        items = new List<ItemInfo>
        {
            new ItemInfo("Potion1", 20),
            new ItemInfo("Potion2", 40),
            new ItemInfo("Potion3", 0.5f)
        };
    }

 

    // 아이템 사용 메서드
    public void Use(int number, Monster monster)
    {
        ItemInfo itemToUse = items[number];
        monster.hp += itemToUse.healingAmount;
        monster.hp = Mathf.Clamp(monster.hp, 0, monster.maxHp); // 최대 HP를 초과하지 않도록 클램프
    
        Debug.Log($"{itemToUse.itemName} 사용, 현재 HP: {monster.hp}");

    }

}
