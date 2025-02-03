using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Monster/Skill")]
public class SkillData : ScriptableObject
{
    //��ų �̸�
    public string skillName;
    //��ȯ�Ǵ� ��ƼŬ
    public GameObject particle;
    //��ų ������
    public float skillDamage;
    //��ų �Ӽ�
    public Element skillElement;
    //��ų ��밡�� Ƚ��
    public int skillCount;
    //��ųŸ��
    public SkillType skillType;
    //��ų ���ӽð�
    public float particleDuration;

    //public SkillData(SkillData original)
    //{
    //    skillName = original.skillName;
    //    particle = original.particle;
    //    skillDamage = original.skillDamage;
    //    skillCount = original.skillCount;
    //}

    public SkillData(SkillData original)
    {
        skillName = original.skillName;
        particle = original.particle; // GameObject�� �������̹Ƿ� �ʿ信 ���� ���� ����� ����
        skillDamage = original.skillDamage;
        skillCount = original.skillCount;
        particleDuration = original.particleDuration;
        skillType = original.skillType;
        skillElement = original.skillElement;
    }
}
