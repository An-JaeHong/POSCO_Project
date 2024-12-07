using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public int healingAmount;
    private UIInventory uiInventory;

    private void Start()
    {
        uiInventory = FindObjectOfType<UIInventory>();
    }

    public Item(string name, int healing)
    {
        itemName = name;
        healingAmount = healing;
    }

    // ������ ��� �޼���
    public void Use(Monster monster)
    {
        monster.hp += healingAmount;
        monster.hp = Mathf.Clamp(monster.hp, 0, monster.maxHp); // �ִ� HP�� �ʰ����� �ʵ��� Ŭ����
    
        print($"{itemName} ���! ���� HP: {monster.hp}");
    }



}
