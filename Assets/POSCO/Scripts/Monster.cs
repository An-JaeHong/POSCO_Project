using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    Skill2, //-> Ȯ�强�� ���� ���ܵξ����� ����� ���� ����
}
[Serializable]
public class Monster : MonoBehaviour
{
    public string name;
    public float hp;
    public float maxHp;
    public int level;
    //�̰� �ִ� ����ġ��
    public int[] expArr = { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95};
    //���� �� ����ġ
    public int levelPerExp;
    //�̰� ���� ����ġ��
    public int currentExp;
    //100������ 2��, 220������ 3�� �̷�����. �׷��� 17������ ��ȭ�ؼ� ��ȭ�Ѿ�鵵 �������� �Ϸ��� �̷������� �ؾ��ҵ� -> 16ĭ
    public int[] expToNextLevelArr = { 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200 };
    
       // if(currentExp > expToNextLevelArr[level])
    //�����ִ� ������ MonsterData���� �ؾ���
    public float hpAmount { get { return hp / maxHp; } }
    
    public float damage;

    //�Ӽ�
    public Element element;

    public bool isEnemy;
    public Animator animator;

    //������ �ִ� ��ų �迭 -> �ν����ͻ󿡼� ��ũ���ͺ� ������Ʈ�� SkillData�� �־��ش�.
    public SkillData[] skillDataArr;

    //���õ� ��ų
    public Skill selectedSkill;

    //private GameObject playedParticle;

    //���ʵ��� ��ƼŬ �����Ҳ���
    //public float playParticleDuration;

    //���� Ÿ�� -> ���߿� �⺻�������, ��ų���� Ȯ���ϰԲ� �ʿ��� ����
    public AttackType attackType;

    //������ �ؽ�Ʈ ������
    public GameObject damageTextPrefab;
    
    private void Awake()
    {
        //������ ü�°� �������� �þ��.
        //hp += level * 2;
        //damage += level * 1;
        animator = GetComponent<Animator>();
        //selectedSkill = null;
        attackType = AttackType.None;
        //������ ����ġ�� �����Ҷ� �ʱ�ȭ
        levelPerExp = expArr[level - 1];
        print($"{levelPerExp}");
        //selectedSkill = skillDataArr[0];
        InitializeSkills();

        //������ ����ġ�� ��¥�� ���� �ִ°ű� ������ ó������ �����صθ� ���� ��
    }

    //�ϴ��� ��ų�� �ϳ��� �̷��� �ص�
    private void InitializeSkills()
    {
        if (skillDataArr.Length > 0)
        {
            //���⿡�� ��ų�� �������� ������Ѵ�
            selectedSkill = new Skill(skillDataArr[0]);
        }
    }    

    //������ �´� ������ �Է��ؾ��Ѵ�.
    public void InitializeLevelInfo(int level)
    {
        this.level = level;
        hp += level * 2;
        damage += level * 1;
        maxHp = hp;
        levelPerExp = expArr[level - 1];
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

    public void InitializeFrom(Monster source)
    {
        this.name = source.name;
        this.hp = source.hp;
        this.maxHp = source.maxHp;
        this.level = source.level;
        this.damage = source.damage;
        this.element = source.element;
        this.isEnemy = source.isEnemy;
        this.levelPerExp = source.levelPerExp;
        this.skillDataArr = (SkillData[])source.skillDataArr.Clone();
        this.selectedSkill.skillCount = source.selectedSkill.skillCount;
    }

    public MonsterDeepCopy DeepCopy()
    {
        return new MonsterDeepCopy(this);
    }

    public void TakeDamage(float damage)
    {
        //float finalDamage = damage;
        //bool isEffectIsGreat = false;

        //if (isSkillAttack)
        //{
        //    if((attackerElement == Element.Fire && element == Element.Grass) ||
        //        (attackerElement == Element.Grass && element == Element.Water) ||
        //        (attackerElement == Element.Water && element == Element.Fire))
        //    {
        //        finalDamage *= 1.5f;
        //        isEffectIsGreat = true;
        //        //���⿡ ������ ȿ���� �����ߴ� UI�� �߰� �ϸ� ���ڴ�
        //    }
        //}

        hp -= damage;
        ShowDamageText(damage);
        print($"{name}�� �¾Ҵ�. ����ü�� : {hp}");

        //if (isEffectIsGreat)
        //{
        //    ShowEffectIsGreatPopup();
        //}

        if (hp <= 0)
        {
            hp = 0;
            OnDead();
        }
    }

    private void ShowDamageText(float damage)
    {
        print("������ �������");
        if (damageTextPrefab != null)
        {
            GameObject damageTextObj = Instantiate(damageTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            TextMeshProUGUI damageText = damageTextObj.GetComponentInChildren<TextMeshProUGUI>();
            if (damageText != null)
            {
                damageText.text = damage.ToString();
                print($"{damage}�� ������ �ؽ�Ʈ�� ���;���");
            }
            Destroy(damageText, 1.5f);
        }
    }

    //private void ShowEffectIsGreatPopup()
    //{
    //    Debug.Log("ShowEffectivePopup called");
    //    GameManager.Instance.isPlayerActionComplete = false;
    //    var button = new Dictionary<string, UnityAction>()
    //    {
    //        {
    //            "Ȯ��", () =>
    //            {
    //                UIPopupManager.Instance.ClosePopup();
    //                GameManager.Instance.isPlayerActionComplete = true;
    //            }

    //        }
    //    };

    //    UIPopupManager.Instance.ShowPopup(
    //        "ȿ���� �����ߴ�!",
    //        button
    //        );

    //    Debug.Log("�̹� ��ư�� ������");
    //}

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
        if (skillNum < 0 || skillNum >= skillDataArr.Length)
        {
            print("��ų �Է��� �߸��Ǿ����ϴ�");
            return;
        }

        //��ų�� ���õ�
        selectedSkill = new Skill(skillDataArr[skillNum]);
    }

    public void UseSkill(Monster target)
    {
        if (selectedSkill != null)
        {
            if (selectedSkill.UseSkill(this, target))
            {
                print($"{name}�� {selectedSkill.skillName}�� ����߽��ϴ�");
            }
        }
    }

    public void StartParticleMovement(GameObject particle, Vector3 targetPosition, float duration, Action onComplete)
    {
        if (particle != null)
        {
            StartCoroutine(MoveParticle(particle, targetPosition, duration, onComplete));
        }
    }

    private IEnumerator MoveParticle(GameObject particle, Vector3 targetPosition, float duration, Action onComplete)
    {
        print("MoveParticle�� �����");
        if (particle == null)
        {
            yield break;
        }
        float currentTime = 0f;
        Vector3 startingPosition = particle.transform.position;

        //�Ⱓ���� �ð� �����̵ȴ�
        while (currentTime < duration)
        {
            if (particle == null)
            {
                yield break;
            }

            particle.transform.position = Vector3.Lerp(startingPosition, targetPosition, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        if (particle != null)
        {
            particle.transform.position = targetPosition;

            //GameObject.Destroy(particle);
        }

        if (Application.isPlaying)
        {

            GameObject.Destroy(particle);
        }
        else
        {
            GameObject.DestroyImmediate(particle, true);
        }
        //����ü�� �� �����ϰ� Invoke���ش�
        onComplete?.Invoke(); // �ݹ� ȣ��
    }

    //AttackCommand���� Instantiate�� ���ؼ� ���⼭ ����� �Լ��� �����ߴ�
    public GameObject CreateParticleInstance()
    {
        return Instantiate(selectedSkill.particle, transform.position, Quaternion.identity);
    }

    //ù��° ��ų �ִϸ��̼� �����ϴ� �Լ�
    public void PlayFirstSkillAnimation()
    {
        ////��ƼŬ�� �����ϸ�
        //if (selectedSkill.particle != null)
        //{
        //�����ϴ� �ִϸ��̼��̶� ��ƼŬ ��ȯ�� ���ε���
        animator.SetTrigger("OnSkill1");

        //    //���� Ÿ�Կ� ���� ��ƼŬ ��ȯ�Ǵ� ��ġ�� �ٲ���Ѵ�. ������� �������� -> ��������ġ, ���Ÿ� ���� -> ������ġ
        //    playedParticle = Instantiate(selectedSkill.particle, transform.position, Quaternion.identity);

        //    //���⿡�� ��ƼŬ�� ���ʰ� �������� �����ش�.
        //    Invoke("DestroySkillParticle", playParticleDuration);
        //}

        //print($"{name}��(��) ��ų {selectedSkill.name}�� ����߽��ϴ�!");
    }

    public void DestroySkillParticle()
    {
        //Destroy(playedParticle);
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

    //����ġ�� ��� �Լ� -> ������ ������ �Ұ���.
    public void GetExp(int exp)
    {
        currentExp += exp;
        print($"{name}�� {exp}�� ����ġ�� ������ϴ�."); 

        //����ġ�� ���� ������ ��� �������� �Ǿ����
        while (currentExp >= expToNextLevelArr[level - 1])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentExp -= expToNextLevelArr[level - 1];
        level += 1;
        maxHp += 2;
        //������ �ϸ� ü�� ȸ��
        hp = maxHp;
        damage += 1;
        print($"{name}�� �������� �߽��ϴ�. ���緹��: {level}, ü��: {hp}, ������: {damage}");
        PopupQueueManager.Instance.EnqueuePopup(
            $"{name}�� �������� �߽��ϴ�! ���緹�� : {level}, ü�� : {hp}, ������ {damage}.");

        //var button = new Dictionary<string, UnityAction>
        //{
        //    {
        //        "Ȯ��", () =>
        //        {
        //            UIPopupManager.Instance.ClosePopup();
        //        }
        //    }
        //};
        //UIPopupManager.Instance.ShowPopup(
        //    $"{name}�� �������� �߽��ϴ�! ���緹�� : {level}, ü�� : {hp}, ������ {damage}.",
        //    button
        //    );
    }

    public void ShowLevelUpPanel()
    {
        var button = new Dictionary<string, UnityAction>
        {
            {
                "Ȯ��", () =>
                {
                    UIPopupManager.Instance.ClosePopup();
                }
            }
        };
        UIPopupManager.Instance.ShowPopup(
            $"{name}�� �������� �߽��ϴ�! ���緹�� : {level}, ü�� : {hp}, ������ {damage}.",
            button
            );
    }

    //private void ShowSkillCountNotEnough(Monster currentMonster)
    //{
    //    Debug.Log("��ų�� �����մϴ�");
    //    var button = new Dictionary<string, UnityAction>
    //    {
    //        {
    //            "Ȯ��", () =>
    //            {
    //                UIPopupManager.Instance.ClosePopup();
    //                ShowPlayerTurnPopup(currentMonster);
    //            }
    //        }
    //    };
    //    UIPopupManager.Instance.ShowPopup(
    //        "��ų ��� ���� Ƚ���� �����մϴ�.",
    //        button
    //        );
    //}

}
