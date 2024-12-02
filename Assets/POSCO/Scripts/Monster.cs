using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[Serializable]
public enum Element
{
    None,
    Fire,
    Water,
    Grass,
}

public enum AttackType
{
    None,
    NormalAttack,
    Skill1,
    Skill2,
}

public class Monster : MonoBehaviour
{

    public string name;
    public float hp;
    public float damage;
    public Element element;

    public bool isEnemy;
    public Animator animator;

    //가지고 있는 스킬 배열
    public SkillData[] skills;
    //선택된 스킬
    private SkillData selectedSkill;

    private GameObject playedParticle;

    //공격 타입 -> 나중에 기본경격인지, 스킬인지 확인하게끔 필요한 변수
    public AttackType attackType;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();  
    }

    private void Start()
    {
        selectedSkill = null;
        attackType = AttackType.None;
    }

    public void TakeDamage(float damage)
    {
        animator.SetTrigger("OnTakeDamage");
        hp -= damage;
        print($"{name}가 맞았다. 남은체력 : {hp}");
        if (hp <= 0)
        {
            hp = 0;
            OnDead();
        }
    }

    public void Heal(float healAmount)
    {

    }

    public void SetSkillNum(int skillNum)
    {
        //입력된 숫자가 0보다 작거나, 입력된 숫자가 보유한 스킬의 숫자보다 크면 return
        if (skillNum < 0 || skillNum >= skills.Length)
        {
            print("스킬 입력이 잘못되었습니다");
            return;
        }

        //스킬이 선택됨
        selectedSkill = skills[skillNum];
    }

    public void PlayerSkillAnimation()
    {
        if (selectedSkill.particle != null)
        {
            animator.SetTrigger("OnSkill1");
            playedParticle = Instantiate(selectedSkill.particle, transform.position, Quaternion.identity);
        }

        print($"{name}이(가) 스킬 {selectedSkill.name}를 사용했습니다!");

    }

    public void InitalizeSkill()
    {
        if (selectedSkill.particle == null || selectedSkill == null)
        {
            return;
        }
        //attackType = AttackType.None;
        //selectedSkill = null;
        Destroy(selectedSkill.particle);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("OnNarmalAttack");
        print($"{name}가 공격했다!");

        //StartCoroutine(PlayAttackAnimationCoroutine());
    }

    //private IEnumerator PlayAttackAnimationCoroutine()
    //{
    //    yield return null;
    //    animator.SetTrigger("OnNarmalAttack");
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    yield return new WaitForSeconds(stateInfo.length);
    //}

    private void OnDead()
    {
        animator.SetBool("IsDead", true);
        print($"{name}가 죽었다.");
    }


}
