using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격하는 행동
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
        Debug.LogError($"{attacker.transform.position}");
        Debug.Log($"{attacker}가 공격했다!");
        target.TakeDamage(attacker.damage);
        Debug.Log($"{target}이 공격받았다!");
    }

    public void Undo()
    {
        
    }

}
