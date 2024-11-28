using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleState : PlayerStateBase
{
    public PlayerBattleState(Player player) : base(player) { }

    public override void Enter()
    {
        //���� ������ ���� �÷��̾�� ������ �� ����.
        player.canMove = false;
        //���� ī�޶� ��Ʋ������ �����δ�.
        CameraManager.Instance.HandleCamera(CameraType.BattleMap);
        //�״��� ��Ʋ�� UI�� ����
        uiPopup.BattleCanvasOpen();
        gameManager.SetMonsterOnBattlePosition();
    }
    
    //update�� �������� �ʿ����.
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        player.canMove = true;
        uiPopup.battleCanvas.SetActive(false);
        CameraManager.Instance.HandleCamera(CameraType.FieldMap);
    }

    //���⼭�� �ʿ����.
    public override void HandleCollision(Collision collision)
    {
        
    }
}
