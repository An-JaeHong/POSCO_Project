using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ϴ� �ൿ
public class AttackCommand : ICommand
{
    private Monster attacker;
    private Monster target;

    public AttackCommand(Monster attacker, Monster target)
    {
        this.attacker=attacker;
        this.target=target;
    }

    public void Execute()
    {
        attacker.PlayerAttackAnimation();
        Debug.Log($"{attacker}�� �����ߴ�!");
        target.TakeDamage(attacker.damage);
        Debug.Log($"{target}�� ���ݹ޾Ҵ�!");
    }

    public void Undo()
    {
        
    }

}
