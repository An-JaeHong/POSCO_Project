using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonster : MonoBehaviour
{
    public Element element;
    public string name;

    private Monster monster;

    //�̸��� ���� ���� ã��
    public void FindSameMonster(Monster monster)
    {
        if (monster.name == this.name) // �̸����� Ȯ��
        {
            this.monster = monster;
            this.element = monster.element; // ���� �Ӽ� ����
        }
    }

}
