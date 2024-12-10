using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Monster/Skill")]
public class SkillData : ScriptableObject
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

    //public SkillData(SkillData original)
    //{
    //    skillName = original.skillName;
    //    particle = original.particle;
    //    skillDamage = original.skillDamage;
    //    skillCount = original.skillCount;
    //}
}
