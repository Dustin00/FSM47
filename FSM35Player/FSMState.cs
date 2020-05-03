using System.Collections.Generic;
using System.Diagnostics;

namespace FSM
{
  public class FSMState
  {
    private int _ID = 0;
    private string _Name = null;

    public FSMState(string stateName)
    {
      _Name = stateName;
      _ID = _Name.GetDeterministicHashCode();
      Debug.WriteLine(_Name + ": " + _ID);
    }

    public override string ToString()
    {
      return _Name;
    }

    public string Name
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
