using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    private State currentState;

    //whenever we change a state, call exit and enter 
    public void ChangeState(State newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
        
    }

    //calls OnUpdate on the state themself
    public void update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}
