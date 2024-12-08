using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;




[Serializable]
public class ItemInfo 
{
    public string itemName;
    public int healingAmount;//���� 
    public float healingPercentage; // ���� ȸ����
  
    public ItemInfo(string name, int healing)
    {
        itemName = name;
        healingAmount = healing;
        healingPercentage = 0f;
    }
    public ItemInfo(string name, float healingPercentage)
    {
        itemName = name;
        healingAmount = 0; // �⺻��
        this.healingPercentage = healingPercentage;
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

        items = new List<ItemInfo>
        {
            new ItemInfo("Potion1", 20),
            new ItemInfo("Potion2", 40),
            new ItemInfo("Potion3", 0.5f)
        };

    }



    // ������ ��� �޼���
    public void Use(int number, Monster monster)
    {
        if (uiInventory.potionNum[number] == 0)
        { print("������ ������"); }
        else
        {
            ItemInfo itemToUse = items[number];
            int healingAmount;

            // ȸ���� ���
            if (itemToUse.healingAmount > 0)
            {
                healingAmount = itemToUse.healingAmount; // ���� ȸ���� ���
            }
            else
            {
                healingAmount = Mathf.FloorToInt(monster.maxHp * itemToUse.healingPercentage); // ������ ���� ȸ���� ���
            }


            monster.hp += itemToUse.healingAmount;
            monster.hp = Mathf.Clamp(monster.hp, 0, monster.maxHp); // �ִ� HP�� �ʰ����� �ʵ��� Ŭ����

            Debug.Log($"{itemToUse.itemName} ���, ���� HP: {monster.hp}");
            uiInventory.potionNum[number]--;
        }
    }

    public ItemInfo GetItemInfo(int index)
    {
        return items[index];
    }

}
