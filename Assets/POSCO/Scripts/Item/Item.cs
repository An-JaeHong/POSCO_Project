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
    // ������ ���
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

 

    // ������ ��� �޼���
    public void Use(int number, Monster monster)
    {
        ItemInfo itemToUse = items[number];
        monster.hp += itemToUse.healingAmount;
        monster.hp = Mathf.Clamp(monster.hp, 0, monster.maxHp); // �ִ� HP�� �ʰ����� �ʵ��� Ŭ����
    
        print($"{itemToUse.itemName} ���, ���� HP: {monster.hp}");
    }


}
