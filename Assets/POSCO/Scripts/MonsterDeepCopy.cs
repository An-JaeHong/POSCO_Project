using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���縦 ���� Ŭ����
public class MonsterDeepCopy : MonoBehaviour
{
    public string Name { get; set; }
    public float Hp { get; set; }
    public float Damage { get; set; }
    public Element Element { get; set; }
    public bool IsEnemy { get; set; }
    public SkillData[] Skills { get; set; }

    public MonsterDeepCopy() { }

    public MonsterDeepCopy(MonsterDeepCopy original)
    {
        Name = original.Name;
        Hp = original.Hp;
        Damage = original.Damage;
        Element = original.Element;
        IsEnemy =  original.IsEnemy;
        Skills = original.Skills;

        if (original.Skills != null)
        {
            Skills = new SkillData[original.Skills.Length];
            for (int i = 0; i < original.Skills.Length; i++)
            {
                Skills[i] = new SkillData(original.Skills[i]);
            }
        }
    }
}
