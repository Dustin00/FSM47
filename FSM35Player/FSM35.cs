using System;
using System.Collections.Generic;

/// A simple Finite State Manager that uses .NET Framework 3.5 (current Unity3D C# framework)
/// Uses Enum-like classes to enumerate States and Events
/// Chain State-Event construction for a human-readable state engine progression
/// 
/// Inspired by https://github.com/appccelerate/statemachine in case you can't tell
/// 
/// todo: Parameters for Actions
/// todo: Gated On/Goto:
///     On(Event).If(test).Goto(State)
/// But then I'll need a better example app

namespace FSM
{
  public class FSM35
  {
    const int INSTANT_ACTION = -1;

    Dictionary<string, FSMEvent> _Events = null;
    FSMEvent _BuildEvent = null;
    Dictionary<string, FSMState> _States = null;
    FSMState _BuildState = null;
    FSMAction _BuildAction = null;
    private Dictionary<ActionKey, FSMAction> _Actions = null;
    FSMState _CurrentState = null;
    private List<StateAction> _EntryAction = null;
    private List<StateAction> _ExitAction = null;
    private static Queue<string> _Queue = new Queue<string>();
    private bool _isInAction = false;
    private string _isInActionName = string.Empty;

    public FSM35(List<string> states, List<string> events, string startingState = "")
    {
      if (states.Count < 2)
      {
        throw new InvalidOperationException("You must have at least 2 states");
      }

      if (events.Count == 0)
      {
        throw new InvalidOperationException("You must have at least 1 event");
      }

      _States = new Dictionary<string, FSMState>();
      _Events = new Dictionary<string, FSMEvent>();
      _Actions = new Dictionary<ActionKey, FSMAction>();

      foreach (var state in states)
      {
        var newState = new FSMState(state);
        _States.Add(state, newState);
      }
      _CurrentState = (startingState == string.Empty)
          ? _CurrentState = _States[states[0]]
          : _CurrentState = _States[startingState];

      foreach (var eventName in events)
      {
        var newEvent = new FSMEvent(eventName);
        _Events.Add(eventName, newEvent);
      }

      _EntryAction = new List<StateAction>();
      _ExitAction = new List<StateAction>();
    }

    public FSM35 InitialState(string stateName)
    {
      if (_States != null)
      {
        throw new InvalidOperationException("Starting State already added, use Goto to add more states.");
      }

      _States = new Dictionary<string, FSMState>();

      _BuildState = new FSMState(stateName);
      _CurrentState = _BuildState;

      return Goto(stateName);
    }

    public FSM35 Build()
    {
      FinalizePreviousActions();

      return this;
    }

    public FSM35 Begin()
    {
      CheckForInstantAction();

      return this;
    }

    public void FinalizePreviousActions()
    {
      if (_BuildState != null)
      {
        if (_EntryAction.Count > 0)
        {
          foreach (var action in _EntryAction)
            _BuildState.AddEntryAction(action);
        }
        _EntryAction.Clear();
        if (_ExitAction.Count > 0)
        {
          foreach (var action in _ExitAction)
            _BuildState.AddExitAction(action);
        }
        _ExitAction.Clear();
      }
    }

    public FSM35 In(string stateName)
    {
      if (!_States.ContainsKey(stateName))
      {
        throw new InvalidOperationException("Unable to find key " + stateName + " in States.");
      }

      FinalizePreviousActions();

      _BuildState = _States[stateName];

      _BuildEvent = null;

      return this;
    }

    public FSM35 Goto(string stateName)
    {
      if (_BuildState == null) // from In
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      if (_BuildEvent == null) // from On
      {
        throw new InvalidOperationException("Unknown Event Name. Use On() first.");
      }

      var newState = _States[stateName];

      _BuildAction = new FSMAction(_BuildState, _BuildEvent, newState);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = _BuildEvent.ID };
      _Actions.Add(key, _BuildAction);

      _BuildEvent = null;

      return this;
    }

    public FSM35 Go(string stateName)
    {
      if (_BuildState == null) // from In
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      if (_BuildEvent != null) // from On
      {
        throw new InvalidOperationException("Already has an Event. Use Go() instead of On().");
      }

      var newState = _States[stateName];

      _BuildAction = new FSMAction(_BuildState, null, newState);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = INSTANT_ACTION };
      _Actions.Add(key, _BuildAction);

      _BuildEvent = null;

      return this;
    }

    public FSM35 At(string stateName)
    {
      if (!_States.ContainsKey(stateName))
      {
        throw new ArgumentException(string.Format("Statename key doesn't exist: {0}", stateName));
      }

      _BuildState = _States[stateName];

      return this;
    }

    public FSM35 On(string eventName)
    {
      if (string.IsNullOrEmpty(eventName))
      {
        throw new InvalidOperationException("Parameter eventName may not be null or empty.");
      }

      if (!_Events.ContainsKey(eventName))
      {
        throw new InvalidOperationException("Event named " + eventName + " not declared in class constructor.");
      }

      _BuildEvent = _Events[eventName];

      return this;
    }

    public FSM35 EntryAction(StateAction stateAction)
    {
      _EntryAction.Add(stateAction);
      return this;
    }

    public FSM35 ExitAction(StateAction stateAction)
    {
      _ExitAction.Add(stateAction);
      return this;
    }

    public void QueueAct(string eventName)
    {
      _Queue.Enqueue(eventName);
    }

    public void Act(string eventName)
    {
      if (!_Events.ContainsKey(eventName))
      {
        throw new InvalidOperationException(string.Format("Event list is missing: {0}", eventName));
      }
      if (_isInAction)
      {
        throw new InvalidOperationException(string.Format("Cannot start a new Act '{0}' when already evaluating '{1}' -- QueueAct() it instead", eventName, _isInActionName));
      }

      var fsmEvent = _Events[eventName];

      _isInAction = true;
      _isInActionName = fsmEvent.Name;

      FSMAction transition = null;
      foreach (var key in _Actions.Keys)
      {
        if (key.SourceStateID == _CurrentState.ID && key.SourceEventID == fsmEvent.ID)
        {
          transition = _Actions[key];
          break;
        }
      }
      if (transition == null)
      {
        FinishAct();
        return; // Invalid Event for the Current State
      }

      ExitState();

      EnterState(transition.FinalState);

      FinishAct();
    }

    private void FinishAct()
    {
      _isInAction = false;
      _isInActionName = string.Empty;

      if (_Queue.Count > 0)
      {
        string action = _Queue.Dequeue();
        Act(action);
      }
    }

    private void ExitState()
    {
      if (_CurrentState.ExitAction != null)
      {
        foreach (var action in _CurrentState.ExitAction)
        {
          action();
        }
      }
    }

    private void EnterState(FSMState nextState)
    {
      if (nextState.EntryAction != null)
      {
        foreach (var action in nextState.EntryAction)
        {
          action();
        }
      }

      _CurrentState = nextState;

      CheckForInstantAction();
    }

    private void CheckForInstantAction()
    {
      FSMAction instantAction = null;
      foreach (var key in _Actions.Keys)
      {
        if (key.SourceStateID == _CurrentState.ID && key.SourceEventID == INSTANT_ACTION)
        {
          instantAction = _Actions[key];
          break;
        }
      }

      if (instantAction == null)
        return;

      ExitState();
      EnterState(instantAction.FinalState);
    }
  }
}
