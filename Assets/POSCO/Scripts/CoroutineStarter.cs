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

    public void StartEnemyNormalAttackCoroutine(AttackCommand attackCoroutine)
    {
        StartCoroutine(attackCoroutine.EnemyNormalAttackCoroutine());
    }

    public void StartPlayerFirstSkillAttackCoroutine(AttackCommand attackCoroutine)
    {
        StartCoroutine(attackCoroutine.PlayerFirstSkillAttackCoroutine());
    }

    public void StartEnemyFirstSkillAttackCoroutine(AttackCommand attackCoroutine)
    {
        StartCoroutine(attackCoroutine.EnemyFirstSkillAttackCoroutine());
    }
}
