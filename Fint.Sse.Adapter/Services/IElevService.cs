using Fint.Event.Model;

namespace Fint.Sse.Adapter.Services
{
    public interface IElevService
    {
        void GetAllElev(Event<object> serverSideEvent);
        void GetAllElevforhold(Event<object> serverSideEvent);
        void GetAllBasisgruppe(Event<object> serverSideEvent);
        void GetAllKontaktlarergruppe(Event<object> serverSideEvent);
    }
}