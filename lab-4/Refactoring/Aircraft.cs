using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    class Aircraft
    {
        public string Name { get; }
        public bool IsTakingOff { get; set; }
        private readonly IAirTrafficMediator _mediator;

        public Aircraft(string name, IAirTrafficMediator mediator)
        {
            Name = name;
            _mediator = mediator;
        }

        public void Land()
        {
            _mediator.RequestLanding(this);
        }

        public void TakeOff()
        {
            _mediator.RequestTakeOff(this);
        }
    }

}
