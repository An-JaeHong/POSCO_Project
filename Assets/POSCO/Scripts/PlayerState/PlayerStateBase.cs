using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� ���¸� ��Ÿ���� Ŭ���� -> �߻��Լ��� �����ؼ� ������ ����.
public abstract class PlayerStateBase
{
    protected Player player;
    //�Ʒ� �ΰ��� �ʿ� ���� ���� �ִ�.
    protected GameManager gameManager;
    protected UIPopup uiPopup;

    public PlayerStateBase(Player player)
    {
        this.player = player;
        this.gameManager = GameManager.Instance;
        //this.uiPopup = UIPopup.Instance;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

    //��ȣ�ۿ��� ���� �ϴ� �־��
    public abstract void HandleCollision(Collision collision);
}
