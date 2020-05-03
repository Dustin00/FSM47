namespace FSM
{
  public delegate void StateAction();

  class ActionKey
  {
    public int SourceStateID;
    public int SourceEventID;
  }

  public class FSMAction
  {
    private FSMState _InitialState;
    private FSMEvent _Event;
    private FSMState _FinalState;

    public FSMAction(FSMState initialState, FSMEvent fsmEvent, FSMState finalState)
    {
      _InitialState = initialState;
      _Event = fsmEvent;
      _FinalState = finalState;
    }

    public override string ToString()
    {
      return _InitialState.Name + " to " + _FinalState.Name;
    }

    public FSMState InitialState
    {
      get { return _InitialState; }
    }
    public FSMEvent Event
    {
      get { return _Event; }
    }
    public FSMState FinalState
    {
      get { return _FinalState; }
    }
  }
}
