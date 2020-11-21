using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary
{
    public enum State
    {
        New,
        Completed,
        Canceled,
        AwaitingPayment,
        Pending,
        AwaitingPickup
    }
}
