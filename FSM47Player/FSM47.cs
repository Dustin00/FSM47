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
  public enum EventStyle
  {
    Standard,
    GoSub,
    Return
  }

  public class FSM47<StateEnum, EventEnum>
    where StateEnum : Enum
    where EventEnum : Enum
  {
    const int INSTANT_ACTION = -1;

    Dictionary<EventEnum, FSMEvent<EventEnum>> _Events = null;
    FSMEvent<EventEnum> _BuildEvent = null;
    Dictionary<StateEnum, FSMState<StateEnum>> _States = null;
    FSMState<StateEnum> _BuildState = null;
    private Queue<FSMState<StateEnum>> _SubStateHistory = new Queue<FSMState<StateEnum>>();
    FSMAction<StateEnum, EventEnum> _BuildAction = null;
    FSMDo<StateEnum, EventEnum> _BuildDo = null;
    private Dictionary<ActionKey, FSMAction<StateEnum, EventEnum>> _Actions = null;
    private Dictionary<ActionKey, FSMDo<StateEnum, EventEnum>> _Do = null;
    FSMState<StateEnum> _CurrentState = null;
    private List<Action<string>> _EntryAction = null;
    private List<Action> _ExitAction = null;
    private static Queue<EventEnum> _Queue = new Queue<EventEnum>();
    private bool _IsInAction = false;
    private EventEnum _IsInActionName;
    private bool _IsWithJson;
    private bool _IsNeedParameters;

    public FSM47(StateEnum startingState)
    {
      if (Enum.GetNames(typeof(StateEnum)).Length < 2)
      {
        throw new InvalidOperationException("You must have at least 2 states");
      }

      if (Enum.GetNames(typeof(EventEnum)).Length == 0)
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

      _EntryAction = new List<Action<string>>();
      _ExitAction = new List<Action>();
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
      CheckForInstantAction(String.Empty);

      return this;
    }

    public void FinalizePreviousActions()
    {
      if (_BuildState != null)
      {
        if (_EntryAction.Count > 0)
        {
          foreach (var action in _EntryAction)
            _BuildState.AddEntryAction(action, _IsNeedParameters);
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

      _IsNeedParameters = false;

      return this;
    }

    public FSM47<StateEnum, EventEnum> NeedParameters()
    {
      _IsNeedParameters = true;

      return this;
    }

    public FSM47<StateEnum, EventEnum> Goto(StateEnum stateName, EventStyle eventStyle = EventStyle.Standard)
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

      _BuildAction = new FSMAction<StateEnum, EventEnum>(_BuildState, _BuildEvent, newState, eventStyle, _IsWithJson);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = _BuildEvent.ID };
      _Actions.Add(key, _BuildAction);

      _BuildEvent = null;

      return this;
    }

    public FSM47<StateEnum, EventEnum> GoSub(StateEnum state)
    {
      if (!_States.ContainsKey(state))
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }

      // add the Goto to the state we can GoSub to
      Goto(state, EventStyle.GoSub);

      // // then add the return event for destination state to return to this state
      // var stashState = _BuildState;
      // _BuildState = _States[stateName];
      // Goto(stashState.Name, StateEventType.Return);

      return this;
    }

    public FSM47<StateEnum, EventEnum> SubReturn()
    {
      if (_BuildState == null) // from In
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      if (_BuildEvent == null) // from On
      {
        throw new InvalidOperationException("Unknown Event Name. Use On() first.");
      }

      _BuildAction = new FSMAction<StateEnum, EventEnum>(_BuildState, _BuildEvent, null, EventStyle.Return, _IsWithJson);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = _BuildEvent.ID };
      _Actions.Add(key, _BuildAction);

      _BuildEvent = null;

      return this;
    }

    public FSM47<StateEnum, EventEnum> Do(Action action)
    {
      void StringAction(string s) => action();
      Do(StringAction);
      return this;
    }

    public FSM47<StateEnum, EventEnum> Do(Action<string> stateAction)
    {
      if (_BuildState == null) // from In
      {
        throw new InvalidOperationException($"Unknown build state for [{stateAction.Method}]. Use In() first.");
      }
      if (_BuildEvent == null) // from On
      {
        throw new InvalidOperationException($"Unknown Event for Goto({stateAction.Method}) in State [{_BuildState.Name}]. Use On() first.");
      }

      _BuildDo = new FSMDo<StateEnum, EventEnum>(_BuildState, _BuildEvent, stateAction, _IsWithJson);
      ActionKey key = new ActionKey() { SourceStateID = _BuildState.ID, SourceEventID = _BuildEvent.ID };
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

      _BuildAction = new FSMAction<StateEnum, EventEnum>(_BuildState, null, newState, EventStyle.Standard, _IsWithJson);
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
      _IsWithJson = false;

      return this;
    }

    public FSM47<StateEnum, EventEnum> WithJson()
    {
      _IsWithJson = true;

      return this;
    }

    public FSM47<StateEnum, EventEnum> EntryAction(Action action)
    {
      void StringAction(string s) => action();
      _EntryAction.Add(StringAction);
      return this;
    }

    public FSM47<StateEnum, EventEnum> EntryAction(Action<string> action)
    {
      _EntryAction.Add(action);
      return this;
    }

    public FSM47<StateEnum, EventEnum> ExitAction(Action action)
    {
      _ExitAction.Add(action);
      return this;
    }

    public void QueueAct(EventEnum eventType)
    {
      _Queue.Enqueue(eventType);
    }

    public void Act(EventEnum eventType, string json = "")
    {
      if (!_Events.ContainsKey(eventType))
      {
        throw new InvalidOperationException($"Event list is missing: {eventType}");
      }
      if (_IsInAction)
      {
        throw new InvalidOperationException(
          $"Cannot start a new Act '{eventType}' when already evaluating '{_IsInActionName}' -- QueueAct() it instead");
      }

      var fsmEvent = _Events[eventType];

      bool isDo = false;
      foreach (var key in _Do.Keys)
      {
        if (key.SourceStateID == _CurrentState.ID && key.SourceEventID == fsmEvent.ID)
        {
          if (_Do[key].IsWithJson && json == string.Empty)
            throw new InvalidOperationException($"Act '{eventType}' requires a json object, but was given none.");
          _Do[key].Action(json);
          isDo = true;
        }
      }

      if (isDo) return;

      _IsInAction = true;
      _IsInActionName = fsmEvent.Name;

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

      // if Action was marked as WithJson, throw error of no json was included
      if (transition.IsWithJson && json == string.Empty)
        throw new InvalidOperationException($"In '{_CurrentState.Name}' On '{fsmEvent.Name}' requires a json object, but was given none.");
      
      ExitState();

      FSMState<StateEnum> finalState;
      switch (transition.StateEventType())
      {
        case EventStyle.GoSub:
          _SubStateHistory.Enqueue(_CurrentState);
          finalState = transition.FinalState;
          break;
        case EventStyle.Return:
          finalState = _SubStateHistory.Dequeue();
          break;
        case EventStyle.Standard:
          finalState = transition.FinalState;
          break;
        default:
          throw new InvalidOperationException($"Unknown StateEventType: {transition.StateEventType()}");
      }

      EnterState(finalState, json);

      FinishAct();
    }

    private void FinishAct()
    {
      _IsInAction = false;

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

    private void EnterState(FSMState<StateEnum> nextState, string json)
    {
      if (nextState.EntryAction != null)
      {
        foreach (var action in nextState.EntryAction)
        {
          if (action.isWithJson && json == string.Empty)
            throw new InvalidOperationException($"Act '{action.Action}' requires a json object, but was given none.");

          if (!action.isWithJson && json != string.Empty)
            throw new InvalidOperationException($"Act '{action.Action}' is not expecting a json object, but was given: {json}");

          action.Action(json);
        }
      }

      _CurrentState = nextState;

      CheckForInstantAction(json);
    }

    private void CheckForInstantAction(string json)
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
      EnterState(instantAction.FinalState, json);
    }
  }
}
