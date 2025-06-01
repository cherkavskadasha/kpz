using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    class Program
    {
        static void Main(string[] args)
        {
            var runway1 = new Runway();
            var runway2 = new Runway();

            var commandCentre = new CommandCentre(new[] { runway1, runway2 });

            var aircraft1 = new Aircraft("Boeing-737", commandCentre);
            var aircraft2 = new Aircraft("Airbus-A320", commandCentre);

            commandCentre.RegisterAircraft(aircraft1);
            commandCentre.RegisterAircraft(aircraft2);

            aircraft1.Land();
            aircraft2.Land();
            aircraft1.TakeOff();
            aircraft2.Land();
        }
    }
}
