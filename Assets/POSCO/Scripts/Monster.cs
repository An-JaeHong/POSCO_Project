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
    Skill2, //-> Ȯ�强�� ���� ���ܵξ����� ����� ���� ����
}
[Serializable]
public class Monster : MonoBehaviour
{

    public string name;
    public float hp;
    public float maxHp;
    public float hpAmount { get { return hp / maxHp; } }
    
    public float damage;
    public Element element;

    public bool isEnemy;
    public Animator animator;

    //������ �ִ� ��ų �迭
    public SkillData[] skills;

    //���õ� ��ų
    public SkillData selectedSkill;

    private GameObject playedParticle;

    //���ʵ��� ��ƼŬ �����Ҳ���
    public float playParticleDuration;

    //���� Ÿ�� -> ���߿� �⺻�������, ��ų���� Ȯ���ϰԲ� �ʿ��� ����
    public AttackType attackType;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        maxHp = hp;
        selectedSkill = null;
        attackType = AttackType.None;
    }

    //MonsterŬ������ MonoBehaviour�� ��ӹް� �־ new�� �Ҵ� �Ұ�
    //public Monster DeepCopy()
    //{

    //    Monster newMonster = ScriptableObject.CreateInstance<Monster>();
    //    newMonster.name = this.name;
    //    newMonster.hp = this.hp;
    //    newMonster.maxHp = this.maxHp;
    //    newMonster.damage = this.damage;
    //    newMonster.element = this.element;
    //    newMonster.isEnemy = this.isEnemy;
    //    newMonster.skills = (SkillData[])this.skills.Clone();

    //    return newMonster;
    //}

    public MonsterDeepCopy DeepCopy()
    {
        return new MonsterDeepCopy(this);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        print($"{name}�� �¾Ҵ�. ����ü�� : {hp}");
        if (hp <= 0)
        {
            hp = 0;
            OnDead();
        }
    }

    public void PlayTakeDamageAnimation()
    {
        animator.SetTrigger("OnTakeDamage");
    }

    public void Heal(float healAmount)
    {

    }

    //��ų�� �����ϴ� �Լ�
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

    //ù��° ��ų �ִϸ��̼� �����ϴ� �Լ�
    public void FirstSkillAnimation()
    {
        //��ƼŬ�� �����ϸ�
        if (selectedSkill.particle != null)
        {
            //�����ϴ� �ִϸ��̼��̶� ��ƼŬ ��ȯ�� ���ε���
            animator.SetTrigger("OnSkill1");

            //���� Ÿ�Կ� ���� ��ƼŬ ��ȯ�Ǵ� ��ġ�� �ٲ���Ѵ�. ������� �������� -> ��������ġ, ���Ÿ� ���� -> ������ġ
            playedParticle = Instantiate(selectedSkill.particle, transform.position, Quaternion.identity);

            //���⿡�� ��ƼŬ�� ���ʰ� �������� �����ش�.
            Invoke("DestroySkillParticle", playParticleDuration);
        }

        print($"{name}��(��) ��ų {selectedSkill.name}�� ����߽��ϴ�!");
    }

    public void DestroySkillParticle()
    {
        Destroy(playedParticle);
    }

    public void SecondSkillAniamtion()
    {

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
        print($"{name}�� �����ߴ�!");

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
        print($"{name}�� �׾���.");
    }


}
