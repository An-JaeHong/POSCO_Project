using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //������ �ִ� ��ų �迭
    public SkillData[] skills;
    //���õ� ��ų
    private SkillData selectedSkill;

    private GameObject playedParticle;

    //���� Ÿ�� -> ���߿� �⺻�������, ��ų���� Ȯ���ϰԲ� �ʿ��� ����
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
    }

    public void Heal(float healAmount)
    {

    }

    public void SetSkillNum(int skillNum)
    {
        //�Էµ� ���ڰ� 0���� �۰ų�, �Էµ� ���ڰ� ������ ��ų�� ���ں��� ũ�� return
        if (skillNum < 0 || skillNum >= skills.Length)
        {
            print("��ų �Է��� �߸��Ǿ����ϴ�");
            return;
        }

        //��ų�� ���õ�
        selectedSkill = skills[skillNum];
    }

    public void PlayerSkillAnimation()
    {
        if (selectedSkill.particle != null)
        {
            animator.SetTrigger("OnSkill1");
            playedParticle = Instantiate(selectedSkill.particle, transform.position, Quaternion.identity);
        }

        print($"{gameObject.name}��(��) ��ų {selectedSkill.name}�� ����߽��ϴ�!");

    }

    public void InitalizeSkill()
    {
        if (selectedSkill.particle == null)
        {
            return;
        }
        attackType = AttackType.None;
        selectedSkill = null;
        Destroy(playedParticle);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("OnNarmalAttack");

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
    }


}
