namespace PoGoBot.Logic.Interfaces
{
    public interface IFilter<T>
    {
        bool Enabled { get; }
        T Process(T input);
    }
}