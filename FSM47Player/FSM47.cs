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
  public class FSM47<StateEnum, EventEnum>
    where StateEnum : Enum
    where EventEnum : Enum
  {
    const int INSTANT_ACTION = -1;

    Dictionary<EventEnum, FSMEvent<EventEnum>> _Events = null;
    FSMEvent<EventEnum> _BuildEvent = null;
    Dictionary<StateEnum, FSMState<StateEnum>> _States = null;
    FSMState<StateEnum> _BuildState = null;
    FSMAction<StateEnum, EventEnum> _BuildAction = null;
    FSMDo<StateEnum, EventEnum> _BuildDo = null;
    private Dictionary<ActionKey, FSMAction<StateEnum, EventEnum>> _Actions = null;
    private Dictionary<ActionKey, FSMDo<StateEnum, EventEnum>> _Do = null;
    FSMState<StateEnum> _CurrentState = null;
    private List<StateAction> _EntryAction = null;
    private List<StateAction> _ExitAction = null;
    private static Queue<EventEnum> _Queue = new Queue<EventEnum>();
    private bool _isInAction = false;
    private EventEnum _isInActionName;

    public FSM47(StateEnum startingState) //, List<string> states, List<string> events, string startingState = "")
    {
      if (Enum.GetNames(typeof(StateEnum)).Length < 2)
      {
        throw new InvalidOperationException("You must have at least 2 states");
      }

      if (Enum.GetNames(typeof(EventEnum)).Length < 2)
      {
        throw new InvalidOperationException("You must have at least 1 event");
      }

      _States = new Dictionary<StateEnum, FSMState<StateEnum>>();
      _Events = new Dictionary<EventEnum, FSMEvent<EventEnum>>();
      _Actions = new Dictionary<ActionKey, FSMAction<StateEnum, EventEnum>>();
      _Do = new Dictionary<ActionKey, FSMDo<StateEnum, EventEnum>>();

      foreach (StateEnum state in (StateEnum[])Enum.GetValues(typeof(StateEnum)))
      {
        var newState = new FSMState<StateEnum>(state);
        _States.Add(state, newState);
      }
      _CurrentState = _States[startingState];

      foreach (EventEnum eventName in (EventEnum[])Enum.GetValues(typeof(EventEnum)))
      {
        var newEvent = new FSMEvent<EventEnum>(eventName);
        _Events.Add(eventName, newEvent);
      }

      _EntryAction = new List<StateAction>();
      _ExitAction = new List<StateAction>();
    }

    public FSM47<StateEnum, EventEnum> InitialState(StateEnum stateName)
    {
      if (_States != null)
      {
        throw new InvalidOperationException("Starting State already added, use Goto to add more states.");
      }

      _States = new Dictionary<StateEnum, FSMState<StateEnum>>();

      _BuildState = new FSMState<StateEnum>(stateName);
      _CurrentState = _BuildState;

      return Goto(stateName);
    }

    public FSM47<StateEnum, EventEnum> Build()
    {
      FinalizePreviousActions();

      return this;
    }

    public FSM47<StateEnum, EventEnum> Begin()
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

    public FSM47<StateEnum, EventEnum> In(StateEnum stateName)
    {
      if (!_States.ContainsKey(stateName))
      {
        throw new InvalidOperationException($"Unable to find key {stateName} in States.");
      }

      FinalizePreviousActions();

      _BuildState = _States[stateName];

      _BuildEvent = null;

      return this;
    }

    public FSM47<StateEnum, EventEnum> Goto(StateEnum stateName)
    {
      if (_BuildState == null) // from In
      {
        throw new InvalidOperationException($"Unknown build state for [{stateName}]. Use In() first.");
      }
      if (_BuildEvent == null) // from On
      {
        throw new InvalidOperationException($"Unknown Event for Goto({stateName}) in State [{_BuildState.Name}]. Use On() first.");
      }

      var newState = _States[stateName];

      _BuildAction = new FSMAction<StateEnum, EventEnum>(_BuildState, _BuildEvent, newState);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = _BuildEvent.ID };
      _Actions.Add(key, _BuildAction);

      _BuildEvent = null;

      return this;
    }

    public FSM47<StateEnum, EventEnum> Do(StateAction stateAction)
    {
      if (_BuildState == null) // from In
      {
        throw new InvalidOperationException($"Unknown build state for [{stateAction.Method}]. Use In() first.");
      }
      if (_BuildEvent == null) // from On
      {
        throw new InvalidOperationException($"Unknown Event for Goto({stateAction.Method}) in State [{_BuildState.Name}]. Use On() first.");
      }

      //_BuildAction = new FSMAction<StateEnum, EventEnum>(_BuildState, _BuildEvent, newState);
      _BuildDo = new FSMDo<StateEnum, EventEnum>(_BuildState, _BuildEvent, stateAction);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = _BuildEvent.ID };
      //_Actions.Add(key, _BuildAction);
      _Do.Add(key, _BuildDo);

      _BuildEvent = null;

      return this;
    }


    public FSM47<StateEnum, EventEnum> Go(StateEnum stateName)
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

      _BuildAction = new FSMAction<StateEnum, EventEnum>(_BuildState, null, newState);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = INSTANT_ACTION };
      _Actions.Add(key, _BuildAction);

      _BuildEvent = null;

      return this;
    }

    public FSM47<StateEnum, EventEnum> At(StateEnum stateName)
    {
      if (!_States.ContainsKey(stateName))
      {
        throw new ArgumentException($"Statename key doesn't exist: {stateName}");
      }

      _BuildState = _States[stateName];

      return this;
    }

    public FSM47<StateEnum, EventEnum> On(EventEnum eventName)
    {
      if (!_Events.ContainsKey(eventName))
      {
        throw new InvalidOperationException($"Event named {eventName} not declared in class constructor.");
      }

      _BuildEvent = _Events[eventName];

      return this;
    }

    public FSM47<StateEnum, EventEnum> EntryAction(StateAction stateAction)
    {
      _EntryAction.Add(stateAction);
      return this;
    }

    public FSM47<StateEnum, EventEnum> ExitAction(StateAction stateAction)
    {
      _ExitAction.Add(stateAction);
      return this;
    }

    public void QueueAct(EventEnum eventType)
    {
      _Queue.Enqueue(eventType);
    }

    public void Act(EventEnum eventType)
    {
      if (!_Events.ContainsKey(eventType))
      {
        throw new InvalidOperationException($"Event list is missing: {eventType}");
      }
      if (_isInAction)
      {
        throw new InvalidOperationException(
          $"Cannot start a new Act '{eventType}' when already evaluating '{_isInActionName}' -- QueueAct() it instead");
      }

      var fsmEvent = _Events[eventType];

      bool isDo = false;
      foreach (var key in _Do.Keys)
      {
        if (key.SourceStateID == _CurrentState.ID && key.SourceEventID == fsmEvent.ID)
        {
          _Do[key].Action();
          isDo = true;
        }
      }

      if (isDo) return;

      _isInAction = true;
      _isInActionName = fsmEvent.Name;

      FSMAction<StateEnum, EventEnum> transition = null;
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

      if (_Queue.Count > 0)
      {
        EventEnum action = _Queue.Dequeue();
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

    private void EnterState(FSMState<StateEnum> nextState)
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
      FSMAction<StateEnum, EventEnum> instantAction = null;
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
