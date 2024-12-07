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

    // 아이템 사용 메서드
    public void Use(Monster monster)
    {
        monster.hp += healingAmount;
        monster.hp = Mathf.Clamp(monster.hp, 0, monster.maxHp); // 최대 HP를 초과하지 않도록 클램프
    
        print($"{itemName} 사용! 현재 HP: {monster.hp}");
    }



}
