using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineStarter : MonoBehaviour
{
    //�ܺο����� ������ �����ؾ���
    private static CoroutineStarter instance;
    public static CoroutineStarter Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }
    public void StartPlayerNormalAttackCoroutine(AttackCommand attackCoroutine)
    {
        StartCoroutine(attackCoroutine.PlayerNormalAttackCoroutine());
    }

    public void StartEnemyAttackCoroutine(AttackCommand attackCoroutine)
    {
        StartCoroutine(attackCoroutine.EnemyAttackCoroutine());
    }

    public void StartPlayerSkillAttackCoroutine(AttackCommand attackCoroutine)
    {
        StartCoroutine(attackCoroutine.PlayerSkillAttackCoroutine());
    }
}
