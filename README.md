## Welcome to FSM47

A simple Finite State Manager that uses .NET Framework 4.7

Library notes are mostly in FSM47.cs

Demo project has example notes in frmFSM47Player.cs

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

* Parameters for Actions
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
	  Reversing
    }

    public enum FsmEvents // Events already exists in too many .NET libraries, so I used this instead
    {
	  Stop,
	  Play,
	  Pause,
	  Next, 
	  Last
    }

    public frmFSM47Player()
    {
      InitializeComponent();

      _StateManager = new FSM47<States, FsmEvents>(States.None) // unspecified starting state makes None the default
        .In(States.None) // For the starting state, both EntryAction() and Go() will be triggered when you call FSM.Begin()
          .Go(States.Stopped) // this demo only has a Go(), though
          .ExitAction(EnablePlayer); // will happen when you exit this state

      _StateManager // optionally, you can leave this out and chain all state construction together, including Begin()
        .In(States.Stopped)
          .EntryAction(StopEnterEvent)
          .ExitAction(StopExitEvent)
          .On(FsmEvents.Play).Goto(States.Playing)
        .In(States.Playing)
          .EntryAction(PlayEnterEvent)
          .ExitAction(PlayExitEvent)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Pause).Goto(States.Paused)
          .On(FsmEvents.Next).Goto(States.Forwarding)
          .On(FsmEvents.Last).Goto(States.Reversing)
        .In(States.Paused)
          .EntryAction(PauseEnterEvent)
          .ExitAction(PauseExitEvent)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Play).Goto(States.Playing)
        .In(States.Forwarding)
          .EntryAction(NextEvent)
          .Go(States.Playing)
        .In(States.Reversing)
          .EntryAction(LastEvent)
          .Go(States.Playing)

        .Build(); // finalize building FSM


      // Do any other initialization here

      // When ready, start the state engine with Begin()
      // You control this because maybe your starting state has a Go() on it but you may not be ready for it to be running yet
    }
```
