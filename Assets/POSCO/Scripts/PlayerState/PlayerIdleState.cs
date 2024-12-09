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
        //3마리 이하면 아웃
        if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
        {
            //player.canMove = false;
            //Debug.Log("플레이어 포켓몬이 3마리 이하입니다");
            //var button = new Dictionary<string, UnityAction>()
            //    {
            //        {
            //            "확인", () =>
            //            {
            //                UIPopupManager.Instance.ClosePopup();
            //                player.canMove = true;
            //            }
            //        }
            //    };
            //UIPopupManager.Instance.ShowPopup(
            //    $"3마리의 몬스터를 선택해주세요.",
            //    button
            //    );
            return;
        }

        //3마리인데
        //if (MonsterDataManager.Instance.selectedMonsterDataList.Count == 3)
        //{
        //    //체력이 0 이하가 한마리라도 있으면 out
        //    foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
        //    {
        //        if (selectedMonsterData.hp <= 0)
        //        {
        //            player.canMove = false;
        //            Debug.Log($"{selectedMonsterData.name}의 체력이 0 이하입니다.");
        //            //여기에 체력이 0 이하인 몬스터가 있다고 UI 뜨면 될 듯
        //            var button = new Dictionary<string, UnityAction>()
        //            {
        //                {
        //                    "확인", () =>
        //                    {
        //                        UIPopupManager.Instance.ClosePopup();
        //                        player.canMove = true;
        //                    }
        //                }
        //            };
        //            UIPopupManager.Instance.ShowPopup(
        //                $"체력이 0 이하인 몬스터가 존재합니다.",
        //                button
        //                );
        //            return;
        //        }
        //    }
        //    foreach (Monster selectedMonster in player.selectedMonsterList)
        //    {
        //        if (selectedMonster.hp <= 0)
        //        {
        //            Debug.Log("체력이 0 이하인 몬스터가 있다");
        //            return;
        //        }
        //    }


        //만난적의 정보가 PlayerContactEnemyState에 넘어감
        if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        {
            if (unit.CompareTag("Unit"))
            {
                //if (MonsterDataManager.Instance.selectedMonsterDataList.Count < 3)
                //{
                //    Debug.Log("몬스터의 개수가 3마리 이하이다.");
                //    return;
                //}
                //if (MonsterDataManager.Instance.selectedMonsterDataList.Count == 3)
                //{
                //    foreach (Monster selectedMonsterData in MonsterDataManager.Instance.selectedMonsterDataList)
                //    {
                //        if (selectedMonsterData.hp <= 0)
                //        {
                //            Debug.Log("체력이 0 이하인 몬스터가 존재합니다");
                //            return;
                //        }
                //    }
                player.ChangeState(new PlayerContactEnemyState(player, unit));

                //유닛은 지면 파괴시켜야해서 GameManager에게 연결해준다
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
        //        Debug.Log("체력이 0 이하인 몬스터가 있다");
        //        return;
        //    }
        //}

        ////만난적의 정보가 PlayerContactEnemyState에 넘어감
        //if (collision.collider.TryGetComponent<Unit>(out Unit unit))
        //{
        //    if (unit.CompareTag("Unit"))
        //    {
        //        player.ChangeState(new PlayerContactEnemyState(player, unit));

        //        //유닛은 지면 파괴시켜야해서 GameManager에게 연결해준다
        //        GameManager.Instance.contactedFieldMonster = unit.GetComponent<Unit>().gameObject;
        //    }

        //    else if (unit.CompareTag("Boss"))
        //    {
        //        player.ChangeState(new PlayerContactBossState(player, unit));
        //    }
        //}
    }
}
