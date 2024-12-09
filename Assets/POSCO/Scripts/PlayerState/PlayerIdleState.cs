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
        if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        {
            // Unit�� �±װ� "Unit"���� Ȯ��
            if (unit.CompareTag("Unit"))
            {
                // 3���� ���ϸ� �ƿ�
                if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
                {
                    player.canMove = false;
                    Debug.Log("�÷��̾� ���ϸ��� 3���� �����Դϴ�");

                    #region ������ ������ �����ϴٴ� UI
                    var button = new Dictionary<string, UnityAction>()
                {
                    {
                        "Ȯ��", () =>
                        {
                            UIPopupManager.Instance.ClosePopup();
                            player.canMove = true;
                        }
                    }
                };
                    UIPopupManager.Instance.ShowPopup(
                        $"3������ ���͸� �������ּ���.",
                        button
                    );
                    #endregion
                    return;
                }

                // 3�����ε� ü���� 0 ���ϰ� �Ѹ����� ������ out
                foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
                {
                    if (selectedMonsterData.hp <= 0)
                    {
                        player.canMove = false;
                        Debug.Log($"{selectedMonsterData.name}�� ü���� 0 �����Դϴ�.");

                        #region ü���� 0�� ���Ͱ� �ִٴ� UI
                        var button = new Dictionary<string, UnityAction>()
                    {
                        {
                            "Ȯ��", () =>
                            {
                                UIPopupManager.Instance.ClosePopup();
                                player.canMove = true;
                            }
                        }
                    };
                        UIPopupManager.Instance.ShowPopup(
                            $"ü���� 0 ������ ���Ͱ� �����մϴ�.",
                            button
                        );
                        #endregion
                        return;
                    }
                }

                // ���� ���� ������ PlayerContactEnemyState�� �Ѿ
                player.ChangeState(new PlayerContactEnemyState(player, unit));
                GameManager.Instance.contactedFieldMonster = unit.gameObject;
            }
            else if (unit.CompareTag("Boss"))
            {
                if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
                {
                    player.canMove = false;
                    Debug.Log("�÷��̾� ���ϸ��� 3���� �����Դϴ�");
                    #region ������ ������ �����ϴٴ� UI
                    var button = new Dictionary<string, UnityAction>()
                {
                    {
                        "Ȯ��", () =>
                        {
                            UIPopupManager.Instance.ClosePopup();
                            player.canMove = true;
                        }
                    }
                };
                    UIPopupManager.Instance.ShowPopup(
                        $"3������ ���͸� �������ּ���.",
                        button
                    );
                    #endregion
                    return;
                }

                // 3�����ε� ü���� 0 ���ϰ� �Ѹ����� ������ out
                foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
                {
                    if (selectedMonsterData.hp <= 0)
                    {
                        player.canMove = false;
                        Debug.Log($"{selectedMonsterData.name}�� ü���� 0 �����Դϴ�.");
                        # region ü���� 0�� ���Ͱ� �ִٴ� UI
                        var button = new Dictionary<string, UnityAction>()
                    {
                        {
                            "Ȯ��", () =>
                            {
                                UIPopupManager.Instance.ClosePopup();
                                player.canMove = true;
                            }
                        }
                    };
                        UIPopupManager.Instance.ShowPopup(
                            $"ü���� 0 ������ ���Ͱ� �����մϴ�.",
                            button
                        );
                        #endregion
                        return;
                    }
                }
                player.ChangeState(new PlayerContactBossState(player, unit));
            }
        }
    }
}
