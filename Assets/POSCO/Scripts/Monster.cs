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
        animator.SetTrigger("TakeDamage");
    }

    public void Heal(float healAmount)
    {

    }

    public void PlayerAttackAnimation()
    {
        animator.SetTrigger("OnAttack");
    }

    private void OnDead()
    {
        animator.SetBool("IsDead", true);
    }


}
