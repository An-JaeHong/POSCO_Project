using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    Skill2, //-> 확장성을 위해 남겨두었지만 사용은 하지 않음
}
[Serializable]
public class Monster : MonoBehaviour
{

    public string name;
    public float hp;
    public float maxHp;
    public int level;
    //이게 주는 경험치량
    [SerializeField] public int[] exp = { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95};
    //레벨 당 경험치
    public int levelPerExp;
    //이게 현재 경험치량
    public int currentExp;
    //100얻으면 2렙, 220얻으면 3렙 이런식임. 그래서 17렙까지 진화해서 진화한얘들도 레벨업을 하려면 이런식으로 해야할듯 -> 16칸
    public int[] expToNextLevelArr = { 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400};
    
       // if(currentExp > expToNextLevelArr[level])
    //여기있는 참조를 MonsterData에서 해야함
    public float hpAmount { get { return hp / maxHp; } }
    
    public float damage;
    public Element element;

    public bool isEnemy;
    public Animator animator;

    //가지고 있는 스킬 배열
    public SkillData[] skills;

    //선택된 스킬
    public SkillData selectedSkill;

    private GameObject playedParticle;

    //몇초동안 파티클 실행할껀지
    public float playParticleDuration;

    //공격 타입 -> 나중에 기본경격인지, 스킬인지 확인하게끔 필요한 변수
    public AttackType attackType;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        maxHp = hp;
        selectedSkill = null;
        attackType = AttackType.None;
        levelPerExp = exp[level - 1];
        print($"{levelPerExp}");

        //레벨당 경험치는 어짜피 적만 주는거기 때문에 처음부터 설정해두면 좋을 듯
    }

    public void OnEnable()
    {
    }

    private void Start()
    {
        //maxHp = hp;
        //selectedSkill = null;
        //attackType = AttackType.None;
    }

    //Monster클래스가 MonoBehaviour를 상속받고 있어서 new로 할당 불가
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
        print($"{name}가 맞았다. 남은체력 : {hp}");
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

    //스킬을 선택하는 함수
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

    //첫번째 스킬 애니메이션 실행하는 함수
    public void FirstSkillAnimation()
    {
        //파티클이 존재하면
        if (selectedSkill.particle != null)
        {
            //공격하는 애니메이션이랑 파티클 소환은 따로따로
            animator.SetTrigger("OnSkill1");

            //공격 타입에 따라 파티클 소환되는 위치를 바꿔야한다. 예를들면 근접공격 -> 공격할위치, 원거리 공격 -> 본인위치
            playedParticle = Instantiate(selectedSkill.particle, transform.position, Quaternion.identity);

            //여기에서 파티클을 몇초간 실행할지 정해준다.
            Invoke("DestroySkillParticle", playParticleDuration);
        }

        print($"{name}이(가) 스킬 {selectedSkill.name}를 사용했습니다!");
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

    //경험치를 얻는 함수 -> 게임이 끝나면 할거임.
    public void GetExp(int exp)
    {
        currentExp += exp;
        print($"{name}이 {exp}의 경험치를 얻었습니다."); 

        //경험치를 많이 얻으면 계속 레벨업이 되어야함
        while (currentExp >= expToNextLevelArr[level - 1])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentExp -= expToNextLevelArr[level - 1];
        level += 1;
        print($"{name}이 레벨업을 했습니다. 현재레벨 {level}");
    }
}
