using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Command Pattern�� �� ����
public interface ICommand
{
    //�����ų �Լ�
    public void PlayerAttackExecute();

    public void EnemyAttackExecute();

    //�ڷ� ���� �Լ��ε�, �� ������ ���� �� ������ Ȥ�� �𸣴� �ۼ�
    public void Undo();
}
