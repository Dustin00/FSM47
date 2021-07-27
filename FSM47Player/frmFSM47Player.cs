using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
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
      UpdateVolume,
      SubClearing // For Substates, I use the convention of "Sub" at the start of the name
    }

    public enum FsmEvents // Events already exists in too many .NET libraries, so I used this instead
    {
      Stop,
      Play,
      Pause,
      Next,
      Last,
      SetVolume,
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
          .EntryAction(OnStopEnter)
          .ExitAction(OnStopExit)
          .On(FsmEvents.Play).Goto(States.Playing)
          // You can jump to the SubClearing from multiple states.
          // You then use SubReturn to exit that Substate and return to whatever state you came from
          // In this example, you can Clear from Stopped, Playing, and Paused
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
          .On(FsmEvents.SetVolume).WithJson().GoSub(States.UpdateVolume)
        .In(States.Playing)
          .EntryAction(OnPlayEnter)
          .ExitAction(OnPlayExit)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Pause).Goto(States.Paused)
          .On(FsmEvents.Next).Goto(States.Forwarding)
          .On(FsmEvents.Last).Goto(States.Reversing)
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
          .On(FsmEvents.OtherThing).Do(PlayOtherThing) // Invoking a method instead of changing state
          .On(FsmEvents.SetVolume).WithJson().Do(DoUpdateVolume) // Alternately: can pass in a json object here, too
        .In(States.Paused)
          .EntryAction(OnPauseEnter)
          .ExitAction(OnPauseExit)
          .On(FsmEvents.Stop).Goto(States.Stopped)
          .On(FsmEvents.Play).Goto(States.Playing)
          .On(FsmEvents.ClearConsole).GoSub(States.SubClearing)
          .On(FsmEvents.SetVolume).WithJson().GoSub(States.UpdateVolume)
        .In(States.Forwarding)
          .EntryAction(OnForwardingEnter)
          .Go(States.Playing)
        .In(States.Reversing)
          .EntryAction(OnReversingEnter)
          .Go(States.Playing)
        .In(States.UpdateVolume).NeedParameters()
          .EntryAction(OnUpdateVolume)
          .On(FsmEvents.OK).SubReturn()
        .In(States.SubClearing)
          .EntryAction(OnSubClearingEnter)
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

    private void btnVolume0_Click(object sender, EventArgs e)
    {
      SetVolume(0);
    }

    private void btnVolume33_Click(object sender, EventArgs e)
    {
      SetVolume(33);
    }
    
    private void btnVolume66_Click(object sender, EventArgs e)
    {
      SetVolume(66);
    }
    
    private void btnVolume100_Click(object sender, EventArgs e)
    {
      SetVolume(100);
    }

    private void SetVolume(int amount)
    {
      VolumeJson volumeJson = new VolumeJson() {Amount = amount};
      string json = JsonSerializer.Serialize(volumeJson);
      //_StateManager.Act(FsmEvents.SetVolume); // As this is marked WithJson in the construction block, this will cause a runtime error
      // include the className parameter in addition to the json string
      // to allow multiple classes to be passed through the event
      // the receiving method will need to check the className to deserialize each class supported
      _StateManager.Act(FsmEvents.SetVolume, nameof(VolumeJson), json);
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

    private void OnSubClearingEnter()
    {
      lstConsole.Items.Clear();

      // Because this is invoked by entering a State EntryAction, you are "in flight" of processing an Action
      // So instead of .Act, you must use .QueueAct to indicate the next action
      _StateManager.QueueAct(FsmEvents.OK);
    }

    private void OnStopEnter()
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

    private void OnStopExit()
    {
      lstConsole.Items.Add("Exit Stopped");
      btnPlay.Enabled = false;
    }

    private void PlayOtherThing()
    {
      // This is called by the FSMDo(Action) to show it in code, but is not implemented to do anything in this sample app
    }

    private void OnPlayEnter()
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

    private void OnPlayExit()
    {
      lstConsole.Items.Add("Exit Playing");
      btnStop.Enabled = false;
      btnPause.Enabled = false;
      btnNext.Enabled = false;
      btnLast.Enabled = false;
    }

    private void OnPauseEnter()
    {
      lstConsole.Items.Add("Enter Paused");
      txtState.Text = "Paused";
      btnStop.Enabled = true;
      btnPlay.Enabled = true;
    }

    private void OnPauseExit()
    {
      lstConsole.Items.Add("Exit Paused");
      btnStop.Enabled = false;
      btnPlay.Enabled = false;
    }

    private void OnUpdateVolume(string className, string json)
    {
      UpdateVolume(className, json);
      _StateManager.QueueAct(FsmEvents.OK);
    }
    
    private void DoUpdateVolume(string className, string json)
    {
      UpdateVolume(className, json);
    }

    private void UpdateVolume(string className, string json)
    {
      // if multiple classes were being passed in, check the className here for how to deserialize/react in the code
      var volumeJson = JsonSerializer.Deserialize<VolumeJson>(json);
      lstConsole.Items.Add($"Volume set to: {volumeJson.Amount}");
    }
    
    private void OnReversingEnter()
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

    private void OnForwardingEnter()
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
