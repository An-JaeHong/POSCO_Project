using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

//�����ϴ� �ൿ
public class AttackCommand
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
            $"{attacker.name} �� �⺻����!",
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
            $"{attacker.name}(��)�� {target.name}���� {attacker.damage}�� �������� �־���!",
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
            $"{attacker.name} �� �⺻����!",
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
            $"{target.name}���� {attacker.damage}��ŭ�� �������� ������!",
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
            $"{attacker.name}(��)�� {attacker.selectedSkill}(��)�� �����ߴ�!",
            button
        );
        #endregion
        //ù��° ��ų �����ϴ� �ִϸ��̼ǽ���
        attacker.FirstSkillAnimation();

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        float skillDamage = attacker.selectedSkill.skillDamage;
        //ȿ���� �����ߴٴ� �ϴ� false
        bool isEffectIsGreat = false;

        if ((attacker.element == Element.Fire && target.element == Element.Grass) ||
            (attacker.element == Element.Grass && target.element == Element.Water) ||
            (attacker.element == Element.Water && target.element == Element.Fire))
        {
            skillDamage *= 1.5f;
            isEffectIsGreat = true;
        }

        target.TakeDamage(skillDamage);

        if(isEffectIsGreat)
        {
            ShowEffectIsGreatPopup();
            //GameManager.Instance.isEnemyActionComplete = true;
        }

        #region ""�� ""���� ~~�� �������� ������! ��� UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{target.name}���� {attacker.selectedSkill.skillDamage}��ŭ�� �������� ������!",
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
            $"{attacker.name}(��)�� {attacker.selectedSkill}(��)�� �����ߴ�!",
            button
        );
        #endregion

        attacker.selectedSkill = attacker.skills[0];
        attacker.FirstSkillAnimation();
        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        float skillDamage = attacker.selectedSkill.skillDamage;
        //ȿ���� �����ߴٴ� �ϴ� false
        bool isEffectIsGreat = false;

        if ((attacker.element == Element.Fire && target.element == Element.Grass) ||
            (attacker.element == Element.Grass && target.element == Element.Water) ||
            (attacker.element == Element.Water && target.element == Element.Fire))
        {
            skillDamage *= 1.5f;
            isEffectIsGreat = true;
        }

        target.TakeDamage(skillDamage);

        if (isEffectIsGreat)
        {
            ShowEffectIsGreatPopup();
            //GameManager.Instance.isEnemyActionComplete = true;
        }

        #region ""�� ""���� ~~�� �������� ������! ��� UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{target.name}���� {attacker.selectedSkill.skillDamage}��ŭ�� �������� ������!",
            buttons
        );
        #endregion

        //2���Ŀ� �ൿ�� �簳
        yield return new WaitForSeconds(2f);
        GameManager.Instance.isEnemyActionComplete = true;
    }

    private void ShowEffectIsGreatPopup()
    {
        Debug.Log("ShowEffectivePopup called");
        var button = new Dictionary<string, UnityAction>()
        {
            {
                "Ȯ��", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    GameManager.Instance.isPlayerActionComplete = true;
                }

            }
        };

        UIPopupManager.Instance.ShowPopup(
            "ȿ���� �����ߴ�!",
            button
            );

        Debug.Log("�̹� ��ư�� ������");
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
