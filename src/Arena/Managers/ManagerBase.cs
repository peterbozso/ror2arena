namespace Arena.Managers;

internal abstract class ManagerBase
{
    protected bool IsListening { get; set; }

    protected virtual void StartListening() => IsListening = true;

    protected virtual void StopListening() => IsListening = false;

    public void Destroy()
    {
        if (IsListening)
        {
            StopListening();
        }
    }
}
