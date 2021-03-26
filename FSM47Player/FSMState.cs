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

    public StateEnum Name
    {
      get
      {
        return _Name;
      }
    }

    public int ID
    {
      get
      {
        return _ID;
      }
    }

    private List<StateAction> _EntryAction = new List<StateAction>();
    public List<StateAction> EntryAction
    {
      get { return _EntryAction; }
    }
    public void AddEntryAction(StateAction action)
    {
      _EntryAction.Add(action);
    }

    private List<StateAction> _ExitAction = new List<StateAction>();
    public List<StateAction> ExitAction
    {
      get { return _ExitAction; }
    }
    public void AddExitAction(StateAction action)
    {
      _ExitAction.Add(action);
    }
  }
}
