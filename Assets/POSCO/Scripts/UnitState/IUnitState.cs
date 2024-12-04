using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitState
{
    public void Enter(Unit unit);
    public void Update(Unit unit);
    public void Exit(Unit unit);
}
