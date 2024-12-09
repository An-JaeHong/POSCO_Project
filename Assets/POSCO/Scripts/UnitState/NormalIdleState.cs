using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

//일반 몬스터 Idle 상태 -> MonoBehaviour은 객체를 만들어야해서 인터페이스로 만든다.
public class NormalIdleState : IUnitState
{
    Vector3 destination;
    private float waitTime = 2f; //대기시간
    private float waitTimer = 0;
    private bool isWaiting = false;
    private bool hasRandomPosition = true;

    //public NormalChaseState(Unit unit) { }

    public void Enter(Unit unit)
    {
        Debug.Log("Idle상태로 들어옴");
        //여기에서 일반 몬스터가 주위를 돌면서 있는 함수를 실행. 한 3초 움직였다가 2초 멈추는 움직이면 좋을듯
        //X축은 Unit의 포지션에서 +-unit.moveRange 값으로 움직인다.
        unit.isMove = true;
        destination = unit.SetRandomPosition();
        //unit.InitMoveSpeed();
        unit.moveSpeed = 1f;

    //들어가면 일단 느낌표는 숨긴다.
    unit.HideExclamationMark();
    }

    private float updateInterval = 0.5f; // 업데이트 주기
    private float updateTimer = 0f;

    public void Update(Unit unit)
    {
        //updateTimer += Time.deltaTime;
        //if (updateTimer >= updateInterval)
        //{
        //    updateTimer = 0f;
        //    PerformUpdate(unit);
        //}
        //2초 기다림
        if (isWaiting)
        {
            Debug.Log("다시 움직임");
            waitTimer -= Time.deltaTime;
            //2초 후에는 다시 움직일 수 있게끔 해야한다.
            if (waitTimer <= 0)
            {
                isWaiting = false;
                destination = unit.SetRandomPosition();
            }
        }
        //다시 움직임
        else
        {
            //처음 랜덤 찍은 위치로 움직이면 잠시 멈춘 후 다시 움직임
            unit.UnitMove(destination);
            unit.UnitRotation(destination);
            //Debug.Log($"랜덤으로 주어진 좌표 : {destination}");
            if (Vector3.Distance(unit.transform.position, destination) < 0.1f)
            {
                //여기서 잠시 멈춰주는 함수 넣어주자.
                Debug.Log("이동중");
                isWaiting = true;
                waitTimer = waitTime;
            }
        }

        //Debug.Log($"현재 NormalIdleState에 Update로 들어옴");
        //여기에서 플레이어를 발견하면 느낌표가 뜨면서 NormalChaseState로 바뀌어야함
        if (DetectPlayer(unit))
        {
            unit.ChangeState(new NormalChaseState());
            Debug.Log("IdleState에서 ChaseState로 감");

            Debug.Log("감지중");
            return;
        }
    }
   
    //private void PerformUpdate(Unit unit)
    //{
        
    //    //2초 기다림
    //    if (isWaiting)
    //    {
    //        Debug.Log("다시 움직임");
    //        waitTimer -= Time.deltaTime;
    //        //2초 후에는 다시 움직일 수 있게끔 해야한다.
    //        if (waitTimer <= 0)
    //        {
    //            isWaiting = false;
    //            destination = unit.SetRandomPosition();
    //        }
    //    }
    //    //다시 움직임
    //    else
    //    {
    //        //처음 랜덤 찍은 위치로 움직이면 잠시 멈춘 후 다시 움직임
    //        unit.UnitMove(destination);
    //        unit.UnitRotation(destination);
    //        //Debug.Log($"랜덤으로 주어진 좌표 : {destination}");
    //        if (Vector3.Distance(unit.transform.position, destination) < 0.1f)
    //        {
    //            //여기서 잠시 멈춰주는 함수 넣어주자.
    //            Debug.Log("이동중");
    //            isWaiting = true;
    //            waitTimer = waitTime;
    //        }
    //    }

    //    //Debug.Log($"현재 NormalIdleState에 Update로 들어옴");
    //    //여기에서 플레이어를 발견하면 느낌표가 뜨면서 NormalChaseState로 바뀌어야함
    //    if (DetectPlayer(unit))
    //    {
    //        unit.ChangeState(new NormalChaseState());
    //        Debug.Log("IdleState에서 ChaseState로 감");

    //        Debug.Log("감지중");
    //        return;
    //    }
    //}

    private bool DetectPlayer(Unit unit)
    {
        //플레이어의 태그가 Player로 되어있어야 한다
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //테스트
        if (player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
            return false;
        }

        Debug.Log("before UnitSight(unit, player)");
        //플레이어가 있으면
        if (UnitSight(unit, player))
        {
            Debug.Log("플레이어 감지됨");
            return true;
        }

        return false;
    }
    
    //Vector3 directionToPlayer = Vector3.zero;
    private bool UnitSight(Unit unit, GameObject player)
    {
        //플레이어쪽을 보는 방향
        Vector3 directionToPlayer = (player.transform.position + new Vector3(0, 0.5f, 0) - unit.transform.position).normalized;
        //각도 = (유닛이 앞을 보는 방향, 플레이어를 보는 방향)
        float angle = Vector3.Angle(unit.transform.forward, directionToPlayer);

        Debug.Log("1");
        //시야각 안에 있고
        if (angle < unit.sightAngle / 2)
        {
            Debug.Log($"2 : {directionToPlayer}, {unit.detectRange}");
            //방향 만족하고
            if (Physics.Raycast(unit.transform.position, directionToPlayer, out RaycastHit hit, unit.detectRange, LayerMask.GetMask("Player")))
            {
                Debug.Log($"hit : {hit}");
                Debug.Log($"hit2 : {hit.collider.name}");
                //플레이어 만족하면 true
                if (hit.collider.gameObject == player)
                {
                    Debug.Log($"현재 닿은 오브젝트 : {hit.collider.gameObject}");
                    return true;
                }
            }
        }
        return false;
    }
    

    public void Exit(Unit unit)
    {
        //죽으면 나오나? 일반 상태가 끝나는건 죽는 것 말고는 없는 것 같은데...
    }

}
