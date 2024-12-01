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
    public Animator animator;
     
    private void Awake()
    {
        print("1");
        animator = GetComponent<Animator>();  
        if (animator==null)
        {
            print($"{name} 없음");
        }
        else
        {
            print($"{name} 있음");
        }
        ////테스터
        //animator = GetComponentInChildren<Animator>();
        //if (animator == null)
        //{
        //    Debug.LogError($"Animator component is missing on {gameObject.name}");
        //}
        //else
        //{
        //    Debug.Log($"Animator component found on {gameObject.name}");
        //    if (animator.runtimeAnimatorController == null)
        //    {
        //        Debug.LogError($"Animator Controller is missing on {gameObject.name}");
        //    }
        //    else
        //    {
        //        Debug.Log($"Animator Controller is assigned on {gameObject.name}");
        //    }
        //}
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

        //StartCoroutine(PlayerAttackAnimationCoroutine());
    }

    private IEnumerator PlayerAttackAnimationCoroutine()
    {
        yield return null;
        animator.SetTrigger("OnAttack");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
    }


    private void OnDead()
    {
        animator.SetBool("IsDead", true);
    }


}
