using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerIdleState : PlayerStateBase
{
    //player와 연결
    public PlayerIdleState(Player player) : base(player) { }

    public override void Enter()
    {
        //움직일 수 있어야한다
        player.canMove = true;
        //UI를 다 꺼야한다. -> 이건 나중에 Inventory랑은 다른 UI들이니 헷갈리지 말자.
        //uiPopup.AllCanvasClose();
    }

    public override void Update()
    {
        //여기에 Player의 움직임을 관리할 메서드 생성
        player.HandleMovement();
    }

    public override void Exit()
    {
        //움직임 제한
        player.canMove = false;
    }

    public override void HandleCollision(Collision collision)
    {
        if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        {
            // Unit의 태그가 "Unit"인지 확인
            if (unit.CompareTag("Unit"))
            {
                // 3마리 이하면 아웃
                if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
                {
                    player.canMove = false;
                    Debug.Log("플레이어 포켓몬이 3마리 이하입니다");

                    #region 몬스터의 개수가 부족하다는 UI
                    var button = new Dictionary<string, UnityAction>()
                {
                    {
                        "확인", () =>
                        {
                            UIPopupManager.Instance.ClosePopup();
                            player.canMove = true;
                        }
                    }
                };
                    UIPopupManager.Instance.ShowPopup(
                        $"3마리의 몬스터를 선택해주세요.",
                        button
                    );
                    #endregion
                    return;
                }

                // 3마리인데 체력이 0 이하가 한마리라도 있으면 out
                foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
                {
                    if (selectedMonsterData.hp <= 0)
                    {
                        player.canMove = false;
                        Debug.Log($"{selectedMonsterData.name}의 체력이 0 이하입니다.");

                        #region 체력이 0인 몬스터가 있다는 UI
                        var button = new Dictionary<string, UnityAction>()
                    {
                        {
                            "확인", () =>
                            {
                                UIPopupManager.Instance.ClosePopup();
                                player.canMove = true;
                            }
                        }
                    };
                        UIPopupManager.Instance.ShowPopup(
                            $"체력이 0 이하인 몬스터가 존재합니다.",
                            button
                        );
                        #endregion
                        return;
                    }
                }

                // 만난 적의 정보가 PlayerContactEnemyState에 넘어감
                player.ChangeState(new PlayerContactEnemyState(player, unit));
                GameManager.Instance.contactedFieldMonster = unit.gameObject;
            }
            else if (unit.CompareTag("Boss"))
            {
                if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
                {
                    player.canMove = false;
                    Debug.Log("플레이어 포켓몬이 3마리 이하입니다");
                    #region 몬스터의 개수가 부족하다는 UI
                    var button = new Dictionary<string, UnityAction>()
                {
                    {
                        "확인", () =>
                        {
                            UIPopupManager.Instance.ClosePopup();
                            player.canMove = true;
                        }
                    }
                };
                    UIPopupManager.Instance.ShowPopup(
                        $"3마리의 몬스터를 선택해주세요.",
                        button
                    );
                    #endregion
                    return;
                }

                // 3마리인데 체력이 0 이하가 한마리라도 있으면 out
                foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
                {
                    if (selectedMonsterData.hp <= 0)
                    {
                        player.canMove = false;
                        Debug.Log($"{selectedMonsterData.name}의 체력이 0 이하입니다.");
                        # region 체력이 0인 몬스터가 있다는 UI
                        var button = new Dictionary<string, UnityAction>()
                    {
                        {
                            "확인", () =>
                            {
                                UIPopupManager.Instance.ClosePopup();
                                player.canMove = true;
                            }
                        }
                    };
                        UIPopupManager.Instance.ShowPopup(
                            $"체력이 0 이하인 몬스터가 존재합니다.",
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
