public enum NodeState
{
    Success,
    Failure,
    Running
}

public abstract class Node
{
    public abstract NodeState Run();
}