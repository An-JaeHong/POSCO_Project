using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Monster/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public GameObject particle;
    public float skillDamage;
    public Element skillElement;
    //스킬 사용가능 횟수
    public int skillCount;

    public SkillData(SkillData original)
    {
        skillName = original.skillName;
        particle = original.particle;
        skillDamage = original.skillDamage;
        skillCount = original.skillCount;
    }
}
