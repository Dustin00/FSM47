using System;
using System.Collections.Generic;

namespace FSM
{
  public class FSMState<StateEnum>
    where StateEnum : Enum
  {
    private int _ID = 0;
    private StateEnum _Name;

    public FSMState(StateEnum stateName)
    {
      _Name = stateName;
      _ID = _Name.ToString().GetDeterministicHashCode();
    }

    public StateEnum Name => _Name;

    public int ID => _ID;

    private List<WithJsonAction> _EntryAction = new List<WithJsonAction>();
    public List<WithJsonAction> EntryAction => _EntryAction;

    public void AddEntryAction(Action<string> action, bool isWithJson)
    {
      _EntryAction.Add(new WithJsonAction() { Action = action, isWithJson = isWithJson});
    }

    private List<Action> _ExitAction = new List<Action>();
    public List<Action> ExitAction => _ExitAction;

    public void AddExitAction(Action action) => _ExitAction.Add(action);
  }
}
