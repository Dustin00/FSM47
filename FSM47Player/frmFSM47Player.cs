using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FSM;

/// A demonstration of the FSM47 library

namespace FSM47Player
{
  public partial class frmFSM47Player : Form
  {
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

      _StateManager = new FSM47<States, FsmEvents>(States.None) // must specify starting state
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
        .In(States.Playing)
          .EntryAction(PlayEnterEvent)
          .ExitAction(PlayExitEvent)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Pause).Goto(States.Paused)
          .On(FsmEvents.Next).Goto(States.Forwarding)
          .On(FsmEvents.Last).Goto(States.Reversing)
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
          .On(FsmEvents.OtherThing).Do(PlayOtherThing) // Invoking a method instead of changing state
        .In(States.Paused)
          .EntryAction(PauseEnterEvent)
          .ExitAction(PauseExitEvent)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Play).Goto(States.Playing)
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
        .In(States.Forwarding)
          .EntryAction(NextEvent)
          .Go(States.Playing)
        .In(States.Reversing)
          .EntryAction(LastEvent)
          .Go(States.Playing)
        .In(States.SubClearing)
          .EntryAction(OnClearConsole)
          .On(FsmEvents.OK).SubReturn() // SubReturn takes you back to whatever State you called .GoSub from

        .Build(); // finalize building FSM

      // Do any other initialization here

      // When ready, start the state engine with Begin()
      // You control this because maybe your starting state has a Go() on it but you may not be ready for it to be running yet
    }

    private void frmFSM47Player_Load(object sender, EventArgs e)
    {
      for (int n = 0; n <= 3; n++)
      {
        lstTracks.Items.Add($"Track {n}");
      }

      _StateManager.Begin();
    }

    private void EnablePlayer()
    {
      pnlControls.Enabled = true;
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      _StateManager.Act(FsmEvents.Stop);
    }

    private void btnPlay_Click(object sender, EventArgs e)
    {
      _StateManager.Act(FsmEvents.Play);
    }

    private void btnPause_Click(object sender, EventArgs e)
    {
      _StateManager.Act(FsmEvents.Pause);
    }

    private void btnLast_Click(object sender, EventArgs e)
    {
      _StateManager.Act(FsmEvents.Last);
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      _StateManager.Act(FsmEvents.Next);
    }

    private void btnClearConsole_Click(object sender, EventArgs e)
    {
      _StateManager.Act(FsmEvents.ClearConsole);
    }

    private void OnClearConsole()
    {
      lstConsole.Items.Clear();

      // Because this is invoked by entering a State EntryAction, you are "in flight" of processing an Action
      // So instead of .Act, you must use .QueueAct to indicate the next action
      _StateManager.QueueAct(FsmEvents.OK);
    }

    private void StopEnterEvent()
    {
      lstConsole.Items.Add("Enter Stopped");
      txtState.Text = "Stopped";
      lstTracks.SelectedItem = null;
      btnPlay.Enabled = true;
      btnStop.Enabled = false;
      btnPause.Enabled = false;
      btnLast.Enabled = false;
      btnNext.Enabled = false;
    }

    private void StopExitEvent()
    {
      lstConsole.Items.Add("Exit Stopped");
      btnPlay.Enabled = false;
    }

    private void PlayOtherThing()
    {
      // This is called by the FSMDo(Action) to show it in code, but is not implemented to do anything in this sample app
    }

    private void PlayEnterEvent()
    {
      if (-1 == lstTracks.SelectedIndex)
      {
        lstTracks.SelectedIndex = 0;
      }
      lstConsole.Items.Add($"Enter Playing for track {lstTracks.SelectedItem}");
      txtState.Text = "Playing";
      btnStop.Enabled = true;
      btnPause.Enabled = true;
      btnNext.Enabled = true;
      btnLast.Enabled = true;
    }

    private void PlayExitEvent()
    {
      lstConsole.Items.Add("Exit Playing");
      btnStop.Enabled = false;
      btnPause.Enabled = false;
      btnNext.Enabled = false;
      btnLast.Enabled = false;
    }

    private void PauseEnterEvent()
    {
      lstConsole.Items.Add("Enter Paused");
      txtState.Text = "Paused";
      btnStop.Enabled = true;
      btnPlay.Enabled = true;
    }

    private void PauseExitEvent()
    {
      lstConsole.Items.Add("Exit Paused");
      btnStop.Enabled = false;
      btnPlay.Enabled = false;
    }

    private void LastEvent()
    {
      lstConsole.Items.Add("Previous Track");
      if (lstTracks.SelectedIndex > 0)
      {
        lstTracks.SelectedIndex = lstTracks.SelectedIndex - 1;
        lstConsole.Items.Add($"Selected track {lstTracks.SelectedItem}");
      }
      else
      {
        lstConsole.Items.Add("At start of track list");
      }
    }

    private void NextEvent()
    {
      lstConsole.Items.Add("Next Track");
      if (lstTracks.SelectedIndex < lstTracks.Items.Count - 1)
      {
        lstTracks.SelectedIndex = lstTracks.SelectedIndex + 1;
        lstConsole.Items.Add($"Selected track {lstTracks.SelectedItem}");
      }
      else
      {
        lstConsole.Items.Add("At end of track list");
      }
    }
  }
}
