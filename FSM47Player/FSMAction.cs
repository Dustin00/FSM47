using System;

namespace FSM
{
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
    private bool _IsWithJson;

    public FSMAction(FSMState<StateEnum> initialState, 
      FSMEvent<EventEnum> fsmEvent, 
      FSMState<StateEnum> finalState,
      EventStyle eventStyle,
      bool isWithJson)
    {
      _InitialState = initialState;
      _Event = fsmEvent;
      _FinalState = finalState;
      _EventStyle = eventStyle;
      _IsWithJson = isWithJson;
    }

    public override string ToString() => $"{_InitialState.Name} to {_FinalState.Name} isWithJson: {_IsWithJson}";

    public EventStyle StateEventType() => _EventStyle;
    public FSMState<StateEnum> InitialState => _InitialState;
    public FSMEvent<EventEnum> Event => _Event;
    public bool IsWithJson => _IsWithJson;
    public FSMState<StateEnum> FinalState => _FinalState;
  }
}
