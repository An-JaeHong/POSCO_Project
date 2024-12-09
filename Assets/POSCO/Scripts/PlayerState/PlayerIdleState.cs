using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
        //3���� ���ϸ� �ƿ�
        if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
        {
            //player.canMove = false;
            //Debug.Log("�÷��̾� ���ϸ��� 3���� �����Դϴ�");
            //var button = new Dictionary<string, UnityAction>()
            //    {
            //        {
            //            "Ȯ��", () =>
            //            {
            //                UIPopupManager.Instance.ClosePopup();
            //                player.canMove = true;
            //            }
            //        }
            //    };
            //UIPopupManager.Instance.ShowPopup(
            //    $"3������ ���͸� �������ּ���.",
            //    button
            //    );
            return;
        }

        //3�����ε�
        //if (MonsterDataManager.Instance.selectedMonsterDataList.Count == 3)
        //{
        //    //ü���� 0 ���ϰ� �Ѹ����� ������ out
        //    foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
        //    {
        //        if (selectedMonsterData.hp <= 0)
        //        {
        //            player.canMove = false;
        //            Debug.Log($"{selectedMonsterData.name}�� ü���� 0 �����Դϴ�.");
        //            //���⿡ ü���� 0 ������ ���Ͱ� �ִٰ� UI �߸� �� ��
        //            var button = new Dictionary<string, UnityAction>()
        //            {
        //                {
        //                    "Ȯ��", () =>
        //                    {
        //                        UIPopupManager.Instance.ClosePopup();
        //                        player.canMove = true;
        //                    }
        //                }
        //            };
        //            UIPopupManager.Instance.ShowPopup(
        //                $"ü���� 0 ������ ���Ͱ� �����մϴ�.",
        //                button
        //                );
        //            return;
        //        }
        //    }
        //    foreach (Monster selectedMonster in player.selectedMonsterList)
        //    {
        //        if (selectedMonster.hp <= 0)
        //        {
        //            Debug.Log("ü���� 0 ������ ���Ͱ� �ִ�");
        //            return;
        //        }
        //    }


        //�������� ������ PlayerContactEnemyState�� �Ѿ
        if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        {
            if (unit.CompareTag("Unit"))
            {
                //if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
                //{
                //    Debug.Log("������ ������ 3���� �����̴�.");
                //    return;
                //}
                //if (MonsterDataManager.Instance.selectedMonsterDataList.Count == 3)
                //{
                //    foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
                //    {
                //        if (selectedMonsterData.hp <= 0)
                //        {
                //            Debug.Log("ü���� 0 ������ ���Ͱ� �����մϴ�");
                //            return;
                //        }
                //    }
                player.ChangeState(new PlayerContactEnemyState(player, unit));

                //������ ���� �ı����Ѿ��ؼ� GameManager���� �������ش�
                GameManager.Instance.contactedFieldMonster = unit.GetComponent<Unit>().gameObject;
                //}
            }

            else if (unit.CompareTag("Boss"))
            {
                player.ChangeState(new PlayerContactBossState(player, unit));
            }
        }
        // }
        //foreach(Monster selectedMonster in player.selectedMonsterList)
        //{
        //    if (selectedMonster.hp <= 0)
        //    {
        //        Debug.Log("ü���� 0 ������ ���Ͱ� �ִ�");
        //        return;
        //    }
        //}

        ////�������� ������ PlayerContactEnemyState�� �Ѿ
        //if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        //{
        //    if (unit.CompareTag("Unit"))
        //    {
        //        player.ChangeState(new PlayerContactEnemyState(player, unit));

        //        //������ ���� �ı����Ѿ��ؼ� GameManager���� �������ش�
        //        GameManager.Instance.contactedFieldMonster = unit.GetComponent<Unit>().gameObject;
        //    }

        //    else if (unit.CompareTag("Boss"))
        //    {
        //        player.ChangeState(new PlayerContactBossState(player, unit));
        //    }
        //}
    }
}
