using System;

public interface IObjectiveEndAction
{
    public void EndExecute();
    public event EventHandler OnExecutionEnd;
}