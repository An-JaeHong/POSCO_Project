using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public enum SkillType
{
    None,
    Melee, //근접공격임
    Ranged,
}

public class Skill
{
    //스킬 이름
    public string skillName;
    //소환되는 파티클
    public GameObject particle;
    //스킬 데미지
    public float skillDamage;
    //스킬 속성 -> 필요 없을듯
    public Element skillElement;
    //스킬 사용가능 횟수
    public int skillCount;
    //스킬타입
    public SkillType skillType;
    //스킬 지속시간
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

            //파티클 삭제는 원본을 하면 오류가 나서 이렇게 받아줄 얘가 필요하다
            GameObject instantiatedParticle = null;
            if (skillType == SkillType.Ranged)
            {
                instantiatedParticle = GameObject.Instantiate(particle, attacker.transform.position, Quaternion.identity);
                attacker.StartParticleMovement(instantiatedParticle, target.transform.position, particleDuration, () =>
                {
                    //float finalDamage = CalculateDamage(attacker, target);
                    //target.TakeDamage(finalDamage);
                    //이러면 파티클이 다 이동한 후에 맞는 애니메이션 재생이 가능함
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

            //근접 공격 파티클은 여기서 삭제, 원거리 공격은 코루틴에서 삭제해준다
            if (instantiatedParticle != null && skillType == SkillType.Melee)
            {
                GameObject.Destroy(instantiatedParticle, particleDuration);
            }

            Debug.Log($"{skillName}을 사용했다!. 남은횟수 : {skillCount}");
            return true;
        }
        else
        {
            Debug.Log("스킬 사용 가능 횟수가 0회 이하입니다.");
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
