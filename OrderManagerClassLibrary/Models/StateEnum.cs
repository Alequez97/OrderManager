using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagerClassLibrary.Models
{
    public enum StateEnum
    {
        New,
        Completed,
        Canceled,
        AwaitingPayment,
        Pending,
        AwaitingPickup
    }
}
