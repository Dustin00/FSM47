﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FSM;

/// A demonstration of the FSM35 library

namespace FSM35Player
{
  public partial class frmFSM35Player : Form
  {
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

    public frmFSM35Player()
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

    private void frmFSM35Player_Load(object sender, EventArgs e)
    {
      for (int n = 0; n <= 6; n++)
      {
        lstTracks.Items.Add("Track " + n);
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
      lstConsole.Items.Clear();
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
        lstConsole.Items.Add("Selected track " + lstTracks.SelectedItem);
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
