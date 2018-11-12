using Fint.Event.Model;
using FINT.Model.Felles;
using FINT.Model.Felles.Kompleksedatatyper;
using FINT.Model.Resource;
using FINT.Model.Utdanning.Elev;
using System.Collections.Generic;

namespace Fint.Sse.Adapter.Services
{
    public class ElevService : IElevService
    {
        void IElevService.GetAllBasisgruppe(Event<object> serverSideEvent)
        {
            BasisgruppeResource basisgruppe = new BasisgruppeResource
            {
                Beskrivelse = "1STA",
                Navn = "1STA",
                SystemId = new Identifikator
                {
                    Identifikatorverdi = "BG_2018_2019_1STA"
                },
                Periode = new List<Periode> { new Periode {
                    Start = new System.DateTime(2018,08,01).ToUniversalTime()
                } }
            };
            basisgruppe.AddElevforhold(Link.with(typeof(Elevforhold), "systemid", "EF001"));
            serverSideEvent.Data.Add(basisgruppe);
        }

        void IElevService.GetAllElev(Event<object> serverSideEvent)
        {
            ElevResource elev = new ElevResource
            {
                SystemId = new Identifikator
                {
                    Identifikatorverdi = "E001"
                }
            };
            elev.AddElevforhold(Link.with(typeof(Elevforhold), "systemid", "EF001"));
            elev.AddPerson(Link.with(typeof(Person), "fodselsnummer", "12345678901"));
            serverSideEvent.Data.Add(elev);
        }

        void IElevService.GetAllElevforhold(Event<object> serverSideEvent)
        {
            ElevforholdResource elevforhold = new ElevforholdResource
            {
                SystemId = new Identifikator { Identifikatorverdi = "EF001" },
                Beskrivelse = "Elevforhold #1"
            };
            elevforhold.AddElev(Link.with(typeof(Elev), "systemid", "E001"));
            elevforhold.AddBasisgruppe(Link.with(typeof(Basisgruppe), "systemid", "BG_2018_2019_1STA"));
            elevforhold.AddKontaktlarergruppe(Link.with(typeof(Kontaktlarergruppe), "systemid", "KG_2018_2019_1STAA"));
            serverSideEvent.Data.Add(elevforhold);
        }

        void IElevService.GetAllKontaktlarergruppe(Event<object> serverSideEvent)
        {
            KontaktlarergruppeResource kontaktlarergruppe = new KontaktlarergruppeResource
            {
                SystemId = new Identifikator
                {
                    Identifikatorverdi = "KG_2018_2019_1STAA"
                },
                Beskrivelse = "1STAA",
                Navn = "1STAA",
                Periode = new List<Periode> { new Periode {
                    Start = new System.DateTime(2018,08,01).ToUniversalTime()
                } }
            };
            kontaktlarergruppe.AddBasisgruppe(Link.with(typeof(Basisgruppe), "systemid", "BG_2018_2019_1STA"));
            kontaktlarergruppe.AddElevforhold(Link.with(typeof(Elevforhold), "systemid", "EF001"));
            kontaktlarergruppe.AddUndervisningsforhold(Link.with(typeof(Undervisningsforhold), "systemid", "UF001"));
            serverSideEvent.Data.Add(kontaktlarergruppe);
        }
    }
}