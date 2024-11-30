using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBattleState : PlayerStateBase
{
    public PlayerBattleState(Player player) : base(player) { }

    public override void Enter()
    {
        //������ ����
        //1. �ʵ忡 �ִ� �÷��̾� ������ ����
        //2. ī�޶� ��Ʋ������ ��ü
        //3. ���� ������ ������ �÷��̾��� ���� ������ GameManager���� �Ѱ���
        //4. �����ϴ� UI����
        player.canMove = false;
        CameraManager.Instance.HandleCamera(CameraType.BattleMap);
        gameManager.SetMonsterOnBattlePosition();

        var buttons = new Dictionary<string, UnityAction>
        {
            {
                "DoAttack", () =>
                {
                    DoAttack();
                }
            },
            {
                "DoHeal", () =>
                {
                    DoHeal();
                }
            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"",
            buttons
            );
    }

    private void DoAttack()
    {
        Debug.Log($"FromDoAttack -> {GameManager.Instance.currentTurnMonster}�� ���ݹ޴���");
        GameManager.Instance.ExecutePlayerAttackAction(GameManager.Instance.currentTurnMonster);
    }

    private void DoHeal()
    {

    }

    //update�� �������� �ʿ����.
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

    //���⼭�� �ʿ����.
    public override void HandleCollision(Collision collision)
    {
        
    }
}
