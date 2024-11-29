using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Element
{
    None,
    Fire,
    Water,
    Grass,
}

public class Monster : MonoBehaviour
{

    public string name;
    public float hp;
    public float damage;
    public Element element;

    public bool isEnemy;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {

    }

    public void Heal(float healAmount)
    {

    }

    public void PlayerAttackAnimation()
    {

    }

    private void OnDead()
    {

    }


}
