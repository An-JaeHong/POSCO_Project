using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 상태를 나타내는 클래스 -> 추상함수로 선언해서 관리할 것임.
public abstract class PlayerStateBase
{
    protected Player player;
    //아래 두개는 필요 없을 수도 있다.
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

    //상호작용을 위해 일단 넣어둠
    public abstract void HandleCollision(Collision collision);
}
