using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Monster/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public GameObject particle;
    public float skillDamage;

    //��ų ��밡�� Ƚ��
    public int skillCount;
}
