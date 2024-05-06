using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StateMachine<T>
{
    public T owner; // 스테이트 머신 사용자 객체

    public enum StateTransitionMethod { PopNPush, JustPush, ReturnToPrev, ClearNPush }

    public Stack<State<T>> stateStack = new Stack<State<T>>();
    public int StackCount { get => stateStack.Count; }
    Type defaultState;

    private bool isStarted = false;
    private bool isActive = true;

    public StateMachine(State<T> initialState)
    {
        stateStack.Push(initialState);
        defaultState = initialState.GetType();
    }

    public void Run()
    {
        if (!isActive) return;

        if (!isStarted) // 업데이트 문 이후에 최초 1회만 Enter를 수행함
        {
            stateStack.Peek().Enter();
            isStarted = true;
        }
        else // 그 이후 업데이트 문에선 Execute만 호출됨
        {
            stateStack.Peek().Execute();
        }

    }

    public bool IsStateType(System.Type type) => stateStack.Peek().GetType() == type;

    public void ChangeState(State<T> nextState, StateTransitionMethod method) // 스테이트와 스테이트 머신에 사용할 방식을 입력받습니다.
    {
        if (!isActive) return;

        switch (method)
        {
            case StateTransitionMethod.PopNPush:
                {
                    State<T> prevState = stateStack.Peek();
                    prevState.Exit();
                    stateStack.Pop();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }

            case StateTransitionMethod.JustPush:
                {
                    State<T> prevState = stateStack.Peek();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }

            case StateTransitionMethod.ReturnToPrev:
                {
                    State<T> currState = stateStack.Peek();
                    currState.Exit();
                    stateStack.Pop();
                    State<T> prevState = stateStack.Peek();
                    prevState.Enter();
                    break;
                }

            case StateTransitionMethod.ClearNPush:
                {
                    State<T> currState = stateStack.Peek();
                    currState.Exit();
                    stateStack.Clear();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }
        }

    }

}