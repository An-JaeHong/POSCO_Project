using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

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

    //�⺻���ݽ� �����ϴ� �Լ�
    public void PlayerNormalAttackExecute()
    {
        //attacker.PlayerAttackAnimation();
        //Debug.Log($"{attacker}�� �����ߴ�!");

        //target.TakeDamage(attacker.damage);
        //Debug.Log($"{target}�� ���ݹ޾Ҵ�!");

        CoroutineStarter.Instance.StartPlayerNormalAttackCoroutine(this);

        //�ӽ�
        TurnManager.Instance.OnBattleEnd += Undo;
    }

    //��ų���ݽ� �����ϴ� �Լ�
    public void PlayerFristSkillAttackExecute()
    {
        CoroutineStarter.Instance.StartPlayerFirstSkillAttackCoroutine(this);
    }

    public void EnemyNormalAttackExecute()
    {
        CoroutineStarter.Instance.StartEnemyNormalAttackCoroutine(this);
    }

    public void EnemyFristSkillAttackExecute()
    {
        CoroutineStarter.Instance.StartEnemyFirstSkillAttackCoroutine(this);
    }

    public IEnumerator PlayerNormalAttackCoroutine()
    {
        #region ""�� ~~ �ൿ�� �ߴٴ� UI
        var buttons = new Dictionary<string, UnityAction>{};
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} Attack!",
            buttons
        );
        #endregion

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

        //�̵� �� ���ݾִϸ��̼� ����
        attacker.PlayAttackAnimation();
        //�� Ȯ��
        //������ ���
        target.TakeDamage(attacker.damage);
        //�ǰ� �ִϸ��̼� ���
        target.PlayAttackAnimation();

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        #region ""�� ""�� �����ߴٴ� UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} take attack {target.name}!",
            buttons
        );
        #endregion

        //Ȯ�� ��ư ������ ���� ���� �ٲ��� -> OnClickCheckButton
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

    public IEnumerator EnemyNormalAttackCoroutine()
    {
        #region ""�� ~~ �ൿ�� �ߴٶ�� UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} Attack!",
            buttons
        );
        #endregion

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

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        #region ""�� ""�� �����ߴ�! ��� UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} take attack {target.name}!",
            button
        );
        #endregion

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

    //ù��° �ִϸ��̼� �����ϴ� �ڷ�ƾ
    public IEnumerator PlayerFirstSkillAttackCoroutine()
    {
        #region ""�� ""��ų�� �����ߴ�! ��� UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} use {attacker.selectedSkill}!",
            button
        );
        #endregion
        //ù��° ��ų �����ϴ� �ִϸ��̼ǽ���
        attacker.FirstSkillAnimation();

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        #region ""�� ""���� ~~�� �������� ������! ��� UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{target.name} take {attacker.selectedSkill.skillDamage}damage!",
            buttons
        );
        #endregion

        //2���Ŀ� �ൿ�� �簳
        yield return new WaitForSeconds(2f);
        GameManager.Instance.isPlayerActionComplete = true;
    }


    public IEnumerator EnemyFirstSkillAttackCoroutine()
    {
        #region ""�� ""��ų�� �����ߴ�! ��� UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} use {attacker.selectedSkill}!",
            button
        );
        #endregion
        attacker.selectedSkill = attacker.skills[0];
        attacker.FirstSkillAnimation();
        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        #region ""�� ""���� ~~�� �������� ������! ��� UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{target.name} take {attacker.selectedSkill.skillDamage}damage!",
            buttons
        );
        #endregion

        //2���Ŀ� �ൿ�� �簳
        yield return new WaitForSeconds(2f);
        GameManager.Instance.isEnemyActionComplete = true;
    }

    //���� ������ ������ �Լ�
    public void Undo()
    {
        //if (TurnManager.Instance.allPlayerMonstersDead == true)
        //{
        //    Debug.Log("�Ʊ� ���� ����");
        //}
        //else if (TurnManager.Instance.allEnemyMonstersDead == true)
        //{
        //    Debug.Log("�� ���� ����");
        //}
    }
}
