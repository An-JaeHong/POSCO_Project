using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonster : MonoBehaviour
{
    public Element element;
    public string name;
    public float hp;
    public float maxHp; 
    private Monster monster;
    public float hpAmount { get { return hp / maxHp; } }
    //�̸��� ���� ���� ã��
    public void FindSameMonster(Monster monster)
    {
        if (monster.name == this.name) // �̸����� Ȯ��
        {
            this.monster = monster;
            this.element = monster.element; // ���� �Ӽ� ����
            this.hp = monster.hp;
            this.maxHp = monster.maxHp;
        }
    }
    private void Start()
    {
        //FindSameMonster(monster);
        maxHp = hp;
    }
}


