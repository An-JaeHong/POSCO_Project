using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//깊은 복사를 위한 클래스
public class MonsterDeepCopy
{
    public string Name { get; set; }
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    public int Level { get; set; }
    public int LevelPerExp { get; set; }
    public float Damage { get; set; }
    public Element Element { get; set; }
    public bool IsEnemy { get; set; }
    public SkillData[] Skills { get; set; }

    public SkillData SelectedSkill { get; set; }

    public MonsterDeepCopy() { }

    public MonsterDeepCopy(Monster original)
    {
        Name = original.name;
        Hp = original.hp;
        MaxHp = original.maxHp;
        Level = original.level;
        Damage = original.damage;
        Element = original.element;
        IsEnemy =  original.isEnemy;
        Skills = original.skills != null? (SkillData[])original.skills.Clone() : null;
        SelectedSkill.skillCount = original.selectedSkill.skillCount;

        //if (original.Skills != null)
        //{
        //    Skills = new SkillData[original.Skills.Length];
        //    for (int i = 0; i < original.Skills.Length; i++)
        //    {
        //        Skills[i] = new SkillData(original.Skills[i]);
        //    }
        //}
    }

    public MonsterDeepCopy(MonsterDeepCopy original)
    {
        Name = original.Name;
        Hp = original.Hp;
        MaxHp = original.MaxHp;
        MaxHp = original.MaxHp;
        Level = original.Level;
        Damage = original.Damage;
        Element = original.Element;
        IsEnemy = original.IsEnemy;
        Skills = original.Skills != null ? (SkillData[])original.Skills.Clone() : null;
    }
}
