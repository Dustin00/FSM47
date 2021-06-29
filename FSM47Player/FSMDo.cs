using System;

namespace FSM
{
	public class FSMDo<StateEnum, EventEnum>
		where StateEnum : Enum
		where EventEnum : Enum
	{
		private FSMState<StateEnum> _InitialState;
		private FSMEvent<EventEnum> _Event;
		private Action<string> _StateAction;
		private bool _IsWithJson;

		public FSMDo(FSMState<StateEnum> initialState, 
			FSMEvent<EventEnum> fsmEvent, 
			Action<string> stateAction,
			bool isWithJson)
		{
			_InitialState = initialState;
			_Event = fsmEvent;
			_StateAction = stateAction;
			_IsWithJson = isWithJson;
		}

		public override string ToString() => $"{_InitialState.Name} does {_StateAction} isWithJson: {_IsWithJson}";

		public FSMState<StateEnum> InitialState => _InitialState;
		public FSMEvent<EventEnum> Event => _Event;
		public bool IsWithJson => _IsWithJson;
		public Action<string> Action => _StateAction;
	}
}
