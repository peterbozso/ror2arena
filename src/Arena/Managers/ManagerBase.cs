namespace Arena.Managers;

internal abstract class ManagerBase
{
    // Subclasses who subscribe to events should override this method and clean up after themselves.
    public virtual void Destroy() { }
}
