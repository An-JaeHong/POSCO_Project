using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public enum SkillType
{
    None,
    Melee, //����������
    Ranged,
}

public class Skill
{
    //��ų �̸�
    public string skillName;
    //��ȯ�Ǵ� ��ƼŬ
    public GameObject particle;
    //��ų ������
    public float skillDamage;
    //��ų �Ӽ� -> �ʿ� ������
    public Element skillElement;
    //��ų ��밡�� Ƚ��
    public int skillCount;
    //��ųŸ��
    public SkillType skillType;
    //��ų ���ӽð�
    public float particleDuration;

    public Skill(SkillData skillData)
    {
        skillName = skillData.skillName;
        particle = skillData.particle;
        skillDamage = skillData.skillDamage;
        skillElement = skillData.skillElement;
        skillCount = skillData.skillCount;
        skillType = skillData.skillType;
        particleDuration = skillData.particleDuration;
    }

    public bool UseSkill(Monster attacker, Monster target)
    {
        if (skillCount > 0)
        {
            skillCount--;

            //��ƼŬ ������ ������ �ϸ� ������ ���� �̷��� �޾��� �갡 �ʿ��ϴ�
            GameObject instantiatedParticle = null;
            if (skillType == SkillType.Ranged)
            {
                instantiatedParticle = GameObject.Instantiate(particle, attacker.transform.position, Quaternion.identity);
                attacker.StartParticleMovement(instantiatedParticle, target.transform.position, particleDuration, () =>
                {
                    //float finalDamage = CalculateDamage(attacker, target);
                    //target.TakeDamage(finalDamage);
                    //�̷��� ��ƼŬ�� �� �̵��� �Ŀ� �´� �ִϸ��̼� ����� ������
                    target.PlayTakeDamageAnimation();
                }
                );
            }
            else if (skillType == SkillType.Melee)
            {
                instantiatedParticle= GameObject.Instantiate(particle, target.transform.position, Quaternion.identity);
                //attacker.PlayFirstSkillAnimation();
                //float finalDamage = CalculateDamage(attacker, target);
                //target.TakeDamage(finalDamage);
                target.PlayTakeDamageAnimation();
            }

            //���� ���� ��ƼŬ�� ���⼭ ����, ���Ÿ� ������ �ڷ�ƾ���� �������ش�
            if (instantiatedParticle != null && skillType == SkillType.Melee)
            {
                GameObject.Destroy(instantiatedParticle, particleDuration);
            }

            Debug.Log($"{skillName}�� ����ߴ�!. ����Ƚ�� : {skillCount}");
            return true;
        }
        else
        {
            Debug.Log("��ų ��� ���� Ƚ���� 0ȸ �����Դϴ�.");
            return false;
        }
    }

    private float CalculateDamage(Monster attacker, Monster target, out bool isEffectGreat)
    {
        float finalDamage = skillDamage;
        isEffectGreat = false;
        if ((attacker.element == Element.Fire && target.element == Element.Grass) ||
            (attacker.element == Element.Grass && target.element == Element.Water) ||
            (attacker.element == Element.Water && target.element == Element.Fire))
        {
            finalDamage *= 1.5f;
            isEffectGreat = true;
        }
        return finalDamage;
    }
}
