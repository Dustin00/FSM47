using System;

namespace FSM
{
  public class FSMEvent<EventEnum>
    where EventEnum : Enum
  {
    private int _ID = 0;
    private EventEnum _Name;

    public FSMEvent(EventEnum eventName)
    {
      _Name = eventName;
      _ID = _Name.ToString().GetDeterministicHashCode();
    }

    public EventEnum Name
    {
      get
      {
        return _Name;
      }
    }

    public int ID
    {
      get
      {
        return _ID;
      }
    }
  }
}
