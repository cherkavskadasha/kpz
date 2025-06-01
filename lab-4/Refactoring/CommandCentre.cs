using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    class CommandCentre : IAirTrafficMediator
    {
        private readonly List<Runway> _runways = new List<Runway>();
        private readonly List<Aircraft> _aircrafts = new List<Aircraft>();

        public CommandCentre(IEnumerable<Runway> runways)
        {
            _runways.AddRange(runways);
        }

        public void RegisterAircraft(Aircraft aircraft)
        {
            _aircrafts.Add(aircraft);
        }

        public void RequestLanding(Aircraft aircraft)
        {
            Console.WriteLine($"Aircraft {aircraft.Name} requesting landing...");

            var availableRunway = _runways.FirstOrDefault(r => r.IsFree);
            if (availableRunway != null)
            {
                Console.WriteLine($"Aircraft {aircraft.Name} is landing on runway {availableRunway.Id}.");
                availableRunway.IsBusyWithAircraft = aircraft;
                availableRunway.HighLightRed();
            }
            else
            {
                Console.WriteLine($"No available runways. Aircraft {aircraft.Name} must wait.");
            }
        }

        public void RequestTakeOff(Aircraft aircraft)
        {
            Console.WriteLine($"Aircraft {aircraft.Name} requesting takeoff...");

            var runway = _runways.FirstOrDefault(r => r.IsBusyWithAircraft == aircraft);
            if (runway != null)
            {
                runway.IsBusyWithAircraft = null;
                runway.HighLightGreen();
                Console.WriteLine($"Aircraft {aircraft.Name} has taken off from runway {runway.Id}.");
            }
            else
            {
                Console.WriteLine($"Aircraft {aircraft.Name} is not on any runway.");
            }
        }
    }
}
