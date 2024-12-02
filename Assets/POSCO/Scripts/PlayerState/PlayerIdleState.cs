using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    //player�� ����
    public PlayerIdleState(Player player) : base(player) { }

    public override void Enter()
    {
        //������ �� �־���Ѵ�
        player.canMove = true;
        //UI�� �� �����Ѵ�. -> �̰� ���߿� Inventory���� �ٸ� UI���̴� �򰥸��� ����.
        //uiPopup.AllCanvasClose();
    }

    public override void Update()
    {
        //���⿡ Player�� �������� ������ �޼��� ����
        player.HandleMovement();
    }

    public override void Exit()
    {
        //������ ����
        player.canMove = false;
    }

    public override void HandleCollision(Collision collision)
    {
        Debug.Log("HandleCollision st");
        if (player.selectedMonsterList.Count < 3)
        {
            Debug.Log("�÷��̾� ���ϸ��� 3���� �����Դϴ�");
            return;
        }
        Debug.Log("HandleCollision 2");

        //�������� ������ PlayerContactEnemyState�� �Ѿ
        if (collision.collider.TryGetComponent<Monster>(out Monster enemy))
        {
            if (enemy.CompareTag("Enemy"))
            {
                player.ChangeState(new PlayerContactEnemyState(player, enemy));
            }
        }
    }

}
