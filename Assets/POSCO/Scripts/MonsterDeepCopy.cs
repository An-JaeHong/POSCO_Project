using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//깊은 복사를 위한 클래스
public class MonsterDeepCopy : MonoBehaviour
{
    public string Name { get; set; }
    public float Hp { get; set; }
    public float Damage { get; set; }
    public Element Element { get; set; }
    public bool IsEnemy { get; set; }
    public SkillData[] Skills { get; set; }
}
