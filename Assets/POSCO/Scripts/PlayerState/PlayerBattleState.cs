using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleState : PlayerStateBase
{
    public PlayerBattleState(Player player) : base(player) { }

    public override void Enter()
    {
        //적과 전투에 들어가면 플레이어는 움직일 수 없다.
        player.canMove = false;
        //들어가면 카메라를 배틀맵으로 움직인다.
        CameraManager.Instance.HandleCamera(CameraType.BattleMap);
        //그다음 배틀맵 UI를 띄운다 -> TurnManager에서 할거임
        //uiPopup.ChooseBattleStateCanvasOpen();
        gameManager.SetMonsterOnBattlePosition();
    }
    
    //update는 아직까진 필요없다.
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        player.canMove = true;
        //uiPopup.chooseBattleStateCanvas.SetActive(false);
        //uiPopup.chooseTargetCanvas.SetActive(false);
        CameraManager.Instance.HandleCamera(CameraType.FieldMap);
    }

    //여기서는 필요없다.
    public override void HandleCollision(Collision collision)
    {
        
    }
}
