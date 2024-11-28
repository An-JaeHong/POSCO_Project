using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    //player¿Í ¿¬°á
    public PlayerIdleState(Player player) : base(player) { }

    public override void Enter()
    {
        player.canMove = true;
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void HandleCollision()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
