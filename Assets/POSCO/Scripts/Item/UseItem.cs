using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public Monster monster; // �÷��̾� ��ũ��Ʈ ����
    public Item item; // ����� ������
    public int number;
    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnUseItem()
    {
        if (item != null)
        {
            item.Use(number,monster);
        }
    }

}
