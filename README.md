# Welcome to FSM35

A simple Finite State Manager that uses .NET Framework 3.5 (current Unity3D C# framework)

Library notes are mostly in FSM35.cs

Demo project has example notes in frmFSM35Player.cs

To add to your Unity Project:

1. Add all FSM*.cs files to your Unity project scripts
2. Create and populate your State and Event classes with your data
	a. Most likely in your Scene's main script
3. Instantiate stateManager with new FSM35(States.Items, Events.Items)
	a. Note in the example I use Evnts instead of Events because Windows Form library collision
4. Add all your states and actions and flows to your stateManager
	a. See frmFSM35Player.cs constructor for example
5. stateManager.Build() when you are done designing your state flows
6. stateManager.Begin() when you want the state engine to run

TODOs:

* Parameters for Actions
* Gated On/Goto:
	On(Event).If(test).Goto(State)
	But then I'll need a better example app

