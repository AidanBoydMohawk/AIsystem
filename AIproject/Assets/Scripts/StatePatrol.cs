using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol : State
{
    public StatePatrol(AIController ai) : base(ai) { }
    public override void Enter()
    {
        Debug.Log("Enterting Patrol Sate");
    }
    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
    public override void Update()
    {
       if(ai.CanSeePlayer())
       {
            ai.ChangeState(new StateChase(ai));

       }
       else
       {
            ai.Patrol();
       }
    }
}
