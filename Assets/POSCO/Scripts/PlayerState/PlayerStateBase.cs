using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� ���¸� ��Ÿ���� Ŭ���� -> �߻��Լ��� �����ؼ� ������ ����.
public abstract class PlayerStateBase
{
    protected Player player;
    protected GameManager gameManager;

    public PlayerStateBase(Player player)
    {
        this.player = player;
        this.gameManager = GameManager.Instance;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

    //��ȣ�ۿ��� ���� �ϴ� �־��
    public abstract void HandleCollision(Collision collision);
}
