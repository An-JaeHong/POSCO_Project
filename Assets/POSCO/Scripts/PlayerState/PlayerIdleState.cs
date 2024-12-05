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
        if (player.selectedMonsterList.Count < 3)
        {
            Debug.Log("�÷��̾� ���ϸ��� 3���� �����Դϴ�");
            return;
        }
        foreach(Monster selectedMonster in player.selectedMonsterList)
        {
            if (selectedMonster.hp <= 0)
            {
                Debug.Log("ü���� 0 ������ ���Ͱ� �ִ�");
                return;
            }
        }

        //�������� ������ PlayerContactEnemyState�� �Ѿ
        if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        {
            if (unit.CompareTag("Unit"))
            {
                player.ChangeState(new PlayerContactEnemyState(player, unit));
                //GameManager.Instance.contactedFieldMonster = unit.GetComponent<Monster>().gameObject;
            }

            else if (unit.CompareTag("Boss"))
            {
                player.ChangeState(new PlayerContactBossState(player, unit));
            }
        }
    }
}
