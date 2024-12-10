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

        float stopDistance = 3.3f; //멈추는 거리

        Vector3 directionToTarget = (targetPosition - currentPlayerPosition).normalized;
        Vector3 stopPosition = targetPosition - directionToTarget * stopDistance;

        //시간 더해줄 변수
        float moveTime = 0;
        //움직이는 시간
        float moveDuration = 1f;
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(currentPlayerPosition, stopPosition, moveTime / moveDuration);
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
        target.PlayTakeDamageAnimation();

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
            attacker.transform.position = Vector3.Lerp(stopPosition, currentPlayerPosition, moveTime / moveDuration);
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

        float stopDistance = 3.3f; //멈추는 거리

        Vector3 directionToTarget = (targetPosition - currentPlayerPosition).normalized;
        Vector3 stopPosition = targetPosition - directionToTarget * stopDistance;

        //시간 더해줄 변수
        float moveTime = 0;
        //움직이는 시간
        float moveDuration = 1f;
        while (moveTime < moveDuration)
        {
            attacker.transform.position = Vector3.Lerp(currentPlayerPosition, stopPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //다시 시간 초기화
        moveTime = 0;

        //이동 후 공격
        attacker.PlayAttackAnimation();
        target.TakeDamage(attacker.damage);
        target.PlayTakeDamageAnimation();

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
            attacker.transform.position = Vector3.Lerp(stopPosition, currentPlayerPosition, moveTime / moveDuration);
            moveTime += Time.deltaTime;
            yield return null;
        }

        //공격이 끝나면 PlayerAction이 끝났다는걸 알려줘야함
        GameManager.Instance.isEnemyActionComplete = true;
    }

    private bool isClickCheckButton = false;
    //첫번째 애니메이션 실행하는 코루틴
    public IEnumerator PlayerFirstSkillAttackCoroutine()
    {
        isClickCheckButton = false; // 확인했다는 버튼을 눌렀는가

        #region ""가 ""스킬을 시전했다! 라는 UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name}(이)가 {attacker.selectedSkill}(을)를 시전했다!",
            button
        );
        #endregion
        //첫번째 스킬 실행하는 애니메이션실행
        attacker.PlayFirstSkillAnimation();
        bool isEffectIsGreat = false;
        attacker.UseSkill(target, out isEffectIsGreat);

        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        //float skillDamage = attacker.selectedSkill.skillDamage;
        ////효과가 굉장했다는 일단 false
        ////bool isEffectIsGreat = false;

        //if ((attacker.element == Element.Fire && target.element == Element.Grass) ||
        //    (attacker.element == Element.Grass && target.element == Element.Water) ||
        //    (attacker.element == Element.Water && target.element == Element.Fire))
        //{
        //    skillDamage *= 1.5f;
        //    isEffectIsGreat = true;
        //}

        //target.TakeDamage(skillDamage);
        attacker.StartParticleMovement(attacker.selectedSkill.particle, target.transform.position, attacker.selectedSkill.particleDuration, () =>
        {
            if (isEffectIsGreat)
            {
                ShowEffectIsGreatPopup();
                //GameManager.Instance.isEnemyActionComplete = true;
            }
            else
            {
                #region ""가 ""에게 ~~의 데미지를 입혔다! 라는 UI
                var buttons = new Dictionary<string, UnityAction>
        {
            {
                "확인", ()=>
                {
                    UIPopupManager.Instance.ClosePopup();
                    isClickCheckButton = true;
                }
            }
        };
                UIPopupManager.Instance.ShowPopup(
                    $"{target.name}에게 {attacker.selectedSkill.skillDamage}만큼의 데미지를 입혔다!",
                    buttons
                );
            }
            #endregion
            //isClickCheckButton = true;
        });
        //if (isEffectIsGreat)
        //{
        //    ShowEffectIsGreatPopup();
        //    //GameManager.Instance.isEnemyActionComplete = true;
        //}
        //else
        //{
        //    #region ""가 ""에게 ~~의 데미지를 입혔다! 라는 UI
        //    var buttons = new Dictionary<string, UnityAction>
        //{
        //    {
        //        "확인", ()=>
        //        {
        //            UIPopupManager.Instance.ClosePopup();
        //            isClickCheckButton = true;
        //        }
        //    }
        //};
        //    UIPopupManager.Instance.ShowPopup(
        //        $"{target.name}에게 {attacker.selectedSkill.skillDamage}만큼의 데미지를 입혔다!",
        //        buttons
        //    );
        //}
        //#endregion
        //isClickCheckButton = true;

        //2초후에 행동을 재개
        yield return new WaitUntil(() => isClickCheckButton == true);
        GameManager.Instance.isPlayerActionComplete = true;
    }


    public IEnumerator EnemyFirstSkillAttackCoroutine()
    {
        isClickCheckButton = false; // 확인했다는 버튼을 눌렀는가
        #region ""가 ""스킬을 시전했다! 라는 UI
        var button = new Dictionary<string, UnityAction> { };
        UIPopupManager.Instance.ShowPopup(
            $"{attacker.name}(이)가 {attacker.selectedSkill}(을)를 시전했다!",
            button
        );
        #endregion

        //스킬 선택하고 -> 이미 생성되는걸로 바꿔서 상관 없을듯

        //attacker.selectedSkill = attacker.skillDataArr[0];
        //스킬 애니메이션 재생
        attacker.PlayFirstSkillAnimation();
        bool isEffectIsGreat;
        attacker.UseSkill(target, out isEffectIsGreat);
        //애니메이션 재생하는 변수
        AnimatorStateInfo stateInfo = attacker.animator.GetCurrentAnimatorStateInfo(0);
        //애니메이션 재생하는 변수의 길이만큼 기다려준다 즉, 애니메이션이 끝날때 까지 기다림
        //여기에다가 투사체가 타겟에게 가는 것 까지 하면 될 듯
        yield return new WaitForSeconds(stateInfo.length);

        //float skillDamage = attacker.selectedSkill.skillDamage;
        ////효과가 굉장했다는 일단 false
        ////bool isEffectIsGreat = false;

        //if ((attacker.element == Element.Fire && target.element == Element.Grass) ||
        //    (attacker.element == Element.Grass && target.element == Element.Water) ||
        //    (attacker.element == Element.Water && target.element == Element.Fire))
        //{
        //    skillDamage *= 1.5f;
        //    isEffectIsGreat = true;
        //}

        //target.TakeDamage(skillDamage);

        if (isEffectIsGreat)
        {
            ShowEffectIsGreatPopup();
            //GameManager.Instance.isEnemyActionComplete = true;
        }

        else
        {
            #region ""가 ""에게 ~~의 데미지를 입혔다! 라는 UI
            var buttons = new Dictionary<string, UnityAction> 
            {
                {
                    "확인", ()=>
                    {
                        UIPopupManager.Instance.ClosePopup();
                        isClickCheckButton = true;
                    }
                }
            };
            UIPopupManager.Instance.ShowPopup(
                $"{target.name}에게 {attacker.selectedSkill.skillDamage}만큼의 데미지를 입혔다!",
                buttons
            );
            #endregion
        }

        //2초후에 행동을 재개
        yield return new WaitUntil(() => isClickCheckButton == true);
        //isClickCheckButton = false;
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
                    isClickCheckButton = true;
                }

            }
        };

        UIPopupManager.Instance.ShowPopup(
            $"효과가 굉장했다!\n{target.name}에게 {attacker.selectedSkill.skillDamage * 1.5f}만큼의 데미지를 입혔다!",
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
