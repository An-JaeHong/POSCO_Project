using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ϲ� ���� Idle ����
public class NormalIdleState : IUnitState
{
    public void Enter(Unit unit)
    {
        //���⿡�� �Ϲ� ���Ͱ� ������ ���鼭 �ִ� �Լ��� ����.
    }

    public void Update(Unit unit)
    {
        //���⿡�� �÷��̾ �߰��ϸ� ����ǥ�� �߸鼭 NormalChaseState�� �ٲ�����
    }

    public void Exit(Unit unit)
    {
        //������ ������? �Ϲ� ���°� �����°� �״� �� ����� ���� �� ������...
    }

}
