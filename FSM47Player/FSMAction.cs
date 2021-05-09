using System;

namespace FSM
{
  public delegate void StateAction();

  class ActionKey
  {
    public int SourceStateID;
    public int SourceEventID;
  }

  public class FSMAction<StateEnum, EventEnum>
    where StateEnum : Enum
    where EventEnum : Enum
  {
    private FSMState<StateEnum> _InitialState;
    private FSMEvent<EventEnum> _Event;
    private FSMState<StateEnum> _FinalState;
    private EventStyle _EventStyle;

    public FSMAction(FSMState<StateEnum> initialState, 
      FSMEvent<EventEnum> fsmEvent, 
      FSMState<StateEnum> finalState,
      EventStyle eventStyle)
    {
      _InitialState = initialState;
      _Event = fsmEvent;
      _FinalState = finalState;
      _EventStyle = eventStyle;
    }

    public override string ToString()
    {
      return $"{_InitialState.Name} to {_FinalState.Name}";
    }

    public EventStyle StateEventType()
    {
      return _EventStyle;
    }

    public FSMState<StateEnum> InitialState
    {
      get { return _InitialState; }
    }

    public FSMEvent<EventEnum> Event
    {
      get { return _Event; }
    }

    public FSMState<StateEnum> FinalState
    {
      get { return _FinalState; }
    }
  }
}
