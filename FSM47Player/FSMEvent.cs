using System;
using System.Diagnostics; //todo: remove when Debug is done

namespace FSM
{
  public class FSMEvent<EventEnum>
    where EventEnum : Enum
  {
    private int _ID = 0; //todo: still needed?
    private EventEnum _Name;

    public FSMEvent(EventEnum eventName)
    {
      _Name = eventName;
      _ID = _Name.ToString().GetDeterministicHashCode();
    }

    //public override string ToString()
    //{
    //  return _Name;
    //}

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
