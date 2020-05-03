## Welcome to FSM35

A simple Finite State Manager that uses .NET Framework 3.5 (current Unity3D C# framework)

Library notes are mostly in FSM35.cs

Demo project has example notes in frmFSM35Player.cs

## Add to your project

1. Add all FSM*.cs files to your Unity project scripts
2. Create and populate your State and Event classes with your data
	a. Most likely in your Scene's main script
3. Instantiate stateManager with new FSM35(States.Items, Events.Items)
	a. Note in the example I use Evnts instead of Events because Windows Form library collision
4. Add all your states and actions and flows to your stateManager
	a. See frmFSM35Player.cs constructor for example
5. stateManager.Build() when you are done designing your state flows
6. stateManager.Begin() when you want the state engine to run

## Possible TODOs

* Parameters for Actions
* Gated On/Goto:
	On(Event).If(test).Goto(State)
	But then I'll need a better example app

## Simple Sample State Machine

```c#

    private FSM35 _StateManager = null;

    public static class States
    {
      public const string None = "None";
      public const string Stopped = "Stopped";
      public const string Playing = "Playing";
      public const string Paused = "Paused";
      public const string Forwarding = "Forwarding";
      public const string Reversing = "Reversing";

      public static List<string> Items
      {
        get
        {
          return new List<string>()
          {
            // First state listed is the default FSM's starting State
            // In FSM constructor, you can specify your starting State if you don't want the default
            None, Stopped, Playing, Paused, Forwarding, Reversing
          };
        }
      }
    }

    public static class Evnts // name this whatever you want -- Events already exists in too many .NET libraries, so I used this instead
    {
      public const string Stop = "Stop";
      public const string Play = "Play";
      public const string Pause = "Pause";
      public const string Next = "Next";
      public const string Last = "Last";

      public static List<string> Items
      {
        get
        {
          return new List<string>()
          {
            Stop, Play, Pause, Next, Last
          };
        }
      }
    }

    public frmFSM35Player()
    {
      ...

      _StateManager = new FSM35(States.Items, Evnts.Items) // unspecified starting state makes None the default
        .In(States.None) // For the starting state, both EntryAction() and Go() will be triggered when you call FSM.Begin()
          .Go(States.Stopped) // this demo only has a Go(), though
          .ExitAction(EnablePlayer); // will happen when you exit this state

      _StateManager // optionally, you can leave this out and chain all state construction together, including Begin()
        .In(States.Stopped)
          .EntryAction(StopEnterEvent)
          .ExitAction(StopExitEvent)
          .On(Evnts.Play).Goto(States.Playing)
        .In(States.Playing)
          .EntryAction(PlayEnterEvent)
          .ExitAction(PlayExitEvent)
          .On(Evnts.Stop).Goto(States.Stopped)
          .On(Evnts.Pause).Goto(States.Paused)
          .On(Evnts.Next).Goto(States.Forwarding)
          .On(Evnts.Last).Goto(States.Reversing)
        .In(States.Paused)
          .EntryAction(PauseEnterEvent)
          .ExitAction(PauseExitEvent)
          .On(Evnts.Stop).Goto(States.Stopped)
          .On(Evnts.Play).Goto(States.Playing)
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
