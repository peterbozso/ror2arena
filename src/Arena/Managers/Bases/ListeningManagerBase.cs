namespace Arena.Managers.Bases;

internal abstract class ListeningManagerBase : ManagerBase
{
    protected bool IsListening { get; set; }

    public void Start()
    {
        if (IsListening)
        {
            return;
        }

        StartListening();
        IsListening = true;
    }

    public void Stop()
    {
        if (!IsListening)
        {
            return;
        }

        StopListening();
        IsListening = false;
    }

    protected abstract void StartListening();

    protected abstract void StopListening();
}
