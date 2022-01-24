namespace PlainGOAP.Engine
{
    public interface IAction<TKey, TValue>
    {
        string Name { get; }
        int Cost { get; }
        Fact<TKey, TValue>[] Prerequisites { get; }
        Fact<TKey, TValue>[] Effects { get; }
        void Execute();
    }
}
