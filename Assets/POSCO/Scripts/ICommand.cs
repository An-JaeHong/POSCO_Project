using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Command Pattern�� �� ����
public interface ICommand
{
    //�����ų �Լ�
    public void PlayerNormalAttackExecute();

    public void PlayerSkillAttackExecute();

    public void EnemyAttackExecute();

    //�ڷ� ���� �Լ��ε�, �� ������ ���� �� ������ Ȥ�� �𸣴� �ۼ�
    public void Undo();
}
