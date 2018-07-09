namespace MiamiOps
{
    public interface IStuff
    {
        void WalkOn(Round ctx);

        string Name { get; }
        Vector Position { get; }
        bool IsAlive { get; }
        string Status { get; }
    } 

    public interface IStuffFactory
    {
        IStuff Create();
        IStuff CreateToCheat(Vector place);
        string Name { get; }
    }
}
