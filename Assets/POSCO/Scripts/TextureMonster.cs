using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonster : MonoBehaviour
{
    public Element element;
    public string name;

    private Monster monster;

    //이름이 같은 몬스터 찾기
    public void FindSameMonster(Monster monster)
    {
        if (monster.name == this.name) // 이름으로 확인
        {
            this.monster = monster;
            this.element = monster.element; // 몬스터 속성 복사
        }
    }

}
