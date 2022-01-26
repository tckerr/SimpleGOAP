using PlainGOAP.Engine;

namespace PlainGOAP.Tests.Data
{
   public interface IFixtureDataFactory<T1, T2>
    {
        SearchParameters<T1, T2> Create();
    }
}
