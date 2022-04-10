namespace Arena.Managers.Bases;

internal abstract class ListeningManagerBase : ManagerBase
{
    protected bool IsListening { get; set; }

    protected virtual void StartListening() => IsListening = true;

    protected virtual void StopListening() => IsListening = false;

    public void StopIfListening()
    {
        if (IsListening)
        {
            StopListening();
        }
    }
}
