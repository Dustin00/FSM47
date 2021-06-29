# Welcome to FSM47

A simple Finite State Manager that uses .NET Framework 4.7

Library notes are mostly in FSM47.cs

Demo project has example notes in frmFSM47Player.cs

### Support
States, Events, Act() to invoke an Event, EntryActions and ExitActions.

### Goto and GoSub event support
You can use a standard "goto" style event flow, or a "gosub" to a state that you can then "SubReturn" back to the previous state.

### Event Paremters
You can declare an event that takes a parameter. To support any type of parameter this uses a json string for all event parameters.

Because of the json serialize/deserialize work, I would not recommend passing parameters for a high-volume event.

### EntryActions and ExitActions
EntryActions happen upon entering a State and can take json parameters and are triggered in order as specified in your constructor.

ExitActions happen when you leave a State and cannot take parameters.

### Of Note
After constructing your state machine, you can add more state/event combinations all you like. There is no way to remove.

## Add to your project

1. Add all FSM*.cs files to your Unity project scripts
2. Create and populate your State and Event classes with your data
	a. Most likely in your Scene's main script
3. Instantiate stateManager with new FSM47(States, Events)
	a. Note in the example I use Evnts instead of Events because Windows Form library collision
4. Add all your states and actions and flows to your stateManager
	a. See frmFSM47Player.cs constructor for example
5. stateManager.Build() when you are done designing your state flows
6. stateManager.Begin() when you want the state engine to run

## Possible TODOs

* Gated On/Goto:
	On(Event).If(test).Goto(State)
	But then I'll need a better example app

## Simple Sample State Machine

```c#

    private FSM47<States, FsmEvents> _StateManager = null;

    public enum States
    {
	  None,
	  Stopped,
	  Playing,
	  Paused,
	  Forwarding,
      Reversing,
      SubClearing // For Substates, I use the convention of "Sub" at the start of the name
    }

    public enum FsmEvents // Events already exists in too many .NET libraries, so I used this instead
    {
	  Stop,
	  Play,
	  Pause,
	  Next, 
      Last,
      ClearConsole,
      OK,
      OtherThing // Used to show .On(event).Do(SomeAction), but is never actually called by any .Act(event) used in this example
    }

    public frmFSM47Player()
    {
      InitializeComponent();

      _StateManager = new FSM47<States, FsmEvents>(States.None) // unspecified starting state makes None the default
        .In(States.None) // For the starting state, Go() will be triggered when you call .Begin()
          .Go(States.Stopped) // this demo only has a Go(), though
          .ExitAction(EnablePlayer); // will happen when you exit this state

      _StateManager // optionally, you can leave this out and chain all state construction together, including Begin()
        .In(States.Stopped)
          .EntryAction(StopEnterEvent)
          .ExitAction(StopExitEvent)
          .On(FsmEvents.Play).Goto(States.Playing)
          // You can jump to the SubClearing from multiple states.
          // You then use SubReturn to exit that Substate and return to whatever state you came from
          // In this example, you can Clear from Stopped, Playing, and Paused
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
		  // This indicates you will pass a json parameter with your event:
          .On(FsmEvents.SetVolume).WithJson().GoSub(States.UpdateVolume) // .Act(FsmEvents.SetVolume, jsonData)
        .In(States.Playing)
          .EntryAction(PlayEnterEvent)
          .ExitAction(PlayExitEvent)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Pause).Goto(States.Paused)
          .On(FsmEvents.Next).Goto(States.Forwarding)
          .On(FsmEvents.Last).Goto(States.Reversing)
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
          .On(FsmEvents.OtherThing).Do(PlayOtherThing) // Invoking a method instead of changing state
		  .On(FsmEvents.SetVolume).WithJson().Do(DoUpdateVolume) // Alternate json parameter support: can pass in a json object here, too
        .In(States.Paused)
          .EntryAction(PauseEnterEvent)
          .ExitAction(PauseExitEvent)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Play).Goto(States.Playing)
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
		  .On(FsmEvents.SetVolume).WithJson().GoSub(States.UpdateVolume)
        .In(States.Forwarding)
          .EntryAction(NextEvent)
          .Go(States.Playing)
        .In(States.Reversing)
          .EntryAction(LastEvent)
          .Go(States.Playing)
        .In(States.UpdateVolume).NeedParameters() // State knows a json parameter is needed
          .EntryAction(OnUpdateVolume)
          .On(FsmEvents.OK).SubReturn()
        .In(States.SubClearing)
          .EntryAction(OnClearConsole)
          .On(FsmEvents.OK).SubReturn() // SubReturn takes you back to whatever State you called .GoSub from

        .Build(); // finalize building FSM


      // Do any other initialization here

      // When ready, start the state engine with Begin()
      // You control this because maybe your starting state has a Go() on it but you may not be ready for it to be running yet
    }
```
