using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//공격하는 행동
public class AttackCommand : ICommand
{
    private Monster attacker;
    private Monster target;

    private CoroutineStarter coroutineStarter;

    public AttackCommand(Monster attacker, Monster target)
    {
        this.attacker=attacker;
        this.target=target;
    }

    //기본공격시 실행하는 함수
    public void PlayerNormalAttackExecute()
    {
        //attacker.PlayerAttackAnimation();
        //Debug.Log($"{attacker}가 공격했다!");

        //target.TakeDamage(attacker.damage);
        //Debug.Log($"{target}이 공격받았다!");

        CoroutineStarter.Instance.StartPlayerNormalAttackCoroutine(this);

        //임시
        TurnManager.Instance.OnBattleEnd += Undo;
    }

    //스킬공격시 실행하는 함수
    public void PlayerFristSkillAttackExecute()
    {
        CoroutineStarter.Instance.PlayerFirstSkillAttackCoroutine(this);
    }

    public void EnemyAttackExecute()
    {
        CoroutineStarter.Instance.StartEnemyAttackCoroutine(this);
    }

    public IEnumerator PlayerNormalAttackCoroutine()
    {
        //기존 위치 저장
        Vector3 currentPlayerPosition = attacker.transform.position;
        Vector3 targetPosition = target.transform.position;

        //시간 더해줄 변수
        float moveTime = 0;
        //움직이는 시간
        float moveDuration = 1f;
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(currentPlayerPosition, targetPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //다시 시간 초기화
        moveTime = 0;

        //이동 후 공격
        attacker.PlayAttackAnimation();
        target.TakeDamage(attacker.damage);

        //돌아오기까지 2초 대기
        yield return new WaitForSeconds(2f);
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(targetPosition, currentPlayerPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //공격이 끝나면 PlayerAction이 끝났다는걸 알려줘야함
        GameManager.Instance.isPlayerActionComplete = true;
    }


    //첫번째 애니메이션 실행하는 코루틴
    public IEnumerator FirstSkillAttackCoroutine()
    {
        //첫번째 스킬 실행하는 애니메이션실행
        attacker.FirstSkillAnimation();

        //2초후에 행동을 재개
        yield return new WaitForSeconds(2f);
        GameManager.Instance.isPlayerActionComplete = true;
    }


    public IEnumerator EnemyAttackCoroutine()
    {
        //기존 위치 저장
        Vector3 currentPlayerPosition = attacker.transform.position;
        Vector3 targetPosition = target.transform.position;

        //시간 더해줄 변수
        float moveTime = 0;
        //움직이는 시간
        float moveDuration = 1f;
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(currentPlayerPosition, targetPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //다시 시간 초기화
        moveTime = 0;

        //이동 후 공격
        attacker.PlayAttackAnimation();
        target.TakeDamage(attacker.damage);

        //돌아오기까지 2초 대기
        yield return new WaitForSeconds(2f);
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(targetPosition, currentPlayerPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //공격이 끝나면 PlayerAction이 끝났다는걸 알려줘야함
        GameManager.Instance.isEnemyActionComplete = true;
    }

    //전투 끝나면 실행할 함수
    public void Undo()
    {
        //if (TurnManager.Instance.allPlayerMonstersDead == true)
        //{
        //    Debug.Log("아군 몬스터 전멸");
        //}
        //else if (TurnManager.Instance.allEnemyMonstersDead == true)
        //{
        //    Debug.Log("적 몬스터 전멸");
        //}
    }
}
