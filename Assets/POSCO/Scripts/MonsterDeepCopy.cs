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
}
