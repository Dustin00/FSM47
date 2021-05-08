using System;

namespace FSM
{
	public class FSMDo<StateEnum, EventEnum>
		where StateEnum : Enum
		where EventEnum : Enum
	{
		private FSMState<StateEnum> _InitialState;
		private FSMEvent<EventEnum> _Event;
		private StateAction _StateAction;

		public FSMDo(FSMState<StateEnum> initialState, FSMEvent<EventEnum> fsmEvent, StateAction stateAction)
		{
			_InitialState = initialState;
			_Event = fsmEvent;
			_StateAction = stateAction;
		}

		public override string ToString()
		{
			return $"{_InitialState.Name} does {_StateAction}";
		}

		public FSMState<StateEnum> InitialState
		{
			get { return _InitialState; }
		}
		public FSMEvent<EventEnum> Event
		{
			get { return _Event; }
		}
		public StateAction Action
		{
			get { return _StateAction; }
		}
	}
}
