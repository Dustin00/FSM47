namespace FSM
{
  public class FSMEvent
  {
    private int _ID = 0;
    private string _Name = null;

    public FSMEvent(string eventName)
    {
      _Name = eventName;
      _ID = _Name.GetDeterministicHashCode();
    }

    public override string ToString()
    {
      return _Name;
    }

    public string Name
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
