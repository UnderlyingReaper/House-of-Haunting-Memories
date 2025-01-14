public interface ILockable
{
    
    public bool CheckIfLocked();
    public void TryUnlock();
}