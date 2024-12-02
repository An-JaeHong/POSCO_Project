using System.Collections;
using System.Collections.Generic;
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

    public void PlayerNormalAttackExecute()
    {
        //attacker.PlayerAttackAnimation();
        //Debug.Log($"{attacker}가 공격했다!");

        //target.TakeDamage(attacker.damage);
        //Debug.Log($"{target}이 공격받았다!");

        CoroutineStarter.Instance.StartPlayerNormalAttackCoroutine(this);
    }

    public void PlayerSkillAttackExecute()
    {
        CoroutineStarter.Instance.StartPlayerSkillAttackCoroutine(this);
    }

    public void EnemyAttackExecute()
    {
        CoroutineStarter.Instance.StartEnemyAttackCoroutine(this);
    }

    public void Undo()
    {
        
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


    //재홍님 여기에다가 스킬 구현하시면 됩니다.
    public IEnumerator PlayerSkillAttackCoroutine()
    {
        yield return null;
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

}
