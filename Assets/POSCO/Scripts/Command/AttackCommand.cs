using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

//공격하는 행동
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
        #region ""가 ~~ 행동을 했다는 UI
        var buttons = new Dictionary<string, UnityAction>{};
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} 의 기본공격!",
            buttons
        );
        #endregion

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

        //이동 후 공격애니메이션 실행
        attacker.PlayAttackAnimation();
        //상성 확인
        //데미지 계산
        target.TakeDamage(attacker.damage);
        //피격 애니메이션 재생
        target.PlayAttackAnimation();

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        #region ""가 ""를 공격했다는 UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name}(이)가 {target.name}에게 {attacker.damage}의 데미지를 주었다!",
            buttons
        );
        #endregion

        //확인 버튼 누를때 까지 대기로 바꾸자 -> OnClickCheckButton
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

    public IEnumerator EnemyNormalAttackCoroutine()
    {
        #region ""가 ~~ 행동을 했다라는 UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name} 의 기본공격!",
            buttons
        );
        #endregion

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

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        #region ""가 ""를 공격했다! 라는 UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{target.name}에게 {attacker.damage}만큼의 데미지를 입혔다!",
            button
        );
        #endregion

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

    //첫번째 애니메이션 실행하는 코루틴
    public IEnumerator PlayerFirstSkillAttackCoroutine()
    {
        #region ""가 ""스킬을 시전했다! 라는 UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name}(이)가 {attacker.selectedSkill}(을)를 시전했다!",
            button
        );
        #endregion
        //첫번째 스킬 실행하는 애니메이션실행
        attacker.FirstSkillAnimation();

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        float skillDamage = attacker.selectedSkill.skillDamage;
        //효과가 굉장했다는 일단 false
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

        #region ""가 ""에게 ~~의 데미지를 입혔다! 라는 UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{target.name}에게 {attacker.selectedSkill.skillDamage}만큼의 데미지를 입혔다!",
            buttons
        );
        #endregion

        //2초후에 행동을 재개
        yield return new WaitForSeconds(2f);
        GameManager.Instance.isPlayerActionComplete = true;
    }


    public IEnumerator EnemyFirstSkillAttackCoroutine()
    {
        #region ""가 ""스킬을 시전했다! 라는 UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name}(이)가 {attacker.selectedSkill}(을)를 시전했다!",
            button
        );
        #endregion

        attacker.selectedSkill = attacker.skills[0];
        attacker.FirstSkillAnimation();
        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        float skillDamage = attacker.selectedSkill.skillDamage;
        //효과가 굉장했다는 일단 false
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

        #region ""가 ""에게 ~~의 데미지를 입혔다! 라는 UI
        var buttons = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{target.name}에게 {attacker.selectedSkill.skillDamage}만큼의 데미지를 입혔다!",
            buttons
        );
        #endregion

        //2초후에 행동을 재개
        yield return new WaitForSeconds(2f);
        GameManager.Instance.isEnemyActionComplete = true;
    }

    private void ShowEffectIsGreatPopup()
    {
        Debug.Log("ShowEffectivePopup called");
        var button = new Dictionary<string, UnityAction>()
        {
            {
                "확인", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                    GameManager.Instance.isPlayerActionComplete = true;
                }

            }
        };

        UIPopupManager.Instance.ShowPopup(
            "효과가 굉장했다!",
            button
            );

        Debug.Log("이미 버튼이 지나감");
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
