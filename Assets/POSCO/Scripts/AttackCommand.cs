using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ϴ� �ൿ
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

    public void PlayerAttackExecute()
    {
        //attacker.PlayerAttackAnimation();
        //Debug.Log($"{attacker}�� �����ߴ�!");

        //target.TakeDamage(attacker.damage);
        //Debug.Log($"{target}�� ���ݹ޾Ҵ�!");

        CoroutineStarter.Instance.StartPlayerAttackCoroutine(this);
    }

    public void EnemyAttackExecute()
    {
        CoroutineStarter.Instance.StartEnemyAttackCoroutine(this);
    }

    public void Undo()
    {
        
    }
    
    public IEnumerator PlayerAttackCoroutine()
    {
        //���� ��ġ ����
        Vector3 currentPlayerPosition = attacker.transform.position;
        Vector3 targetPosition = target.transform.position;

        //�ð� ������ ����
        float moveTime = 0;
        //�����̴� �ð�
        float moveDuration = 1f;
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(currentPlayerPosition, targetPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //�ٽ� �ð� �ʱ�ȭ
        moveTime = 0;

        //�̵� �� ����
        attacker.PlayAttackAnimation();
        target.TakeDamage(attacker.damage);

        //���ƿ������ 2�� ���
        yield return new WaitForSeconds(2f);
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(targetPosition, currentPlayerPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //������ ������ PlayerAction�� �����ٴ°� �˷������
        GameManager.Instance.isPlayerActionComplete = true;
    }

    public IEnumerator EnemyAttackCoroutine()
    {
        //���� ��ġ ����
        Vector3 currentPlayerPosition = attacker.transform.position;
        Vector3 targetPosition = target.transform.position;

        //�ð� ������ ����
        float moveTime = 0;
        //�����̴� �ð�
        float moveDuration = 1f;
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(currentPlayerPosition, targetPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //�ٽ� �ð� �ʱ�ȭ
        moveTime = 0;

        //�̵� �� ����
        attacker.PlayAttackAnimation();
        target.TakeDamage(attacker.damage);

        //���ƿ������ 2�� ���
        yield return new WaitForSeconds(2f);
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(targetPosition, currentPlayerPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //������ ������ PlayerAction�� �����ٴ°� �˷������
        GameManager.Instance.isEnemyActionComplete = true;
    }

}
