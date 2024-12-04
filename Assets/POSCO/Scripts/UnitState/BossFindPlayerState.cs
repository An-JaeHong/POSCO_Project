using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFindPlayerState : IUnitState
{
    public void Enter(Unit unit)
    {
        unit.ShowExclamationMark();
    }

    public void Update(Unit unit)
    {
        
    }

    public void Exit(Unit unit)
    {
        
    }

}
