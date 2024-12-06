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
    //이름이 같은 몬스터 찾기
    public void FindSameMonster(Monster monster)
    {
        if (monster.name == this.name) // 이름으로 확인
        {
            this.monster = monster;
            this.element = monster.element; // 몬스터 속성 복사
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


