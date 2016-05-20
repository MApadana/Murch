using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Domain.Constant.Transaction
{
   public enum BankStatus
    {
        OK=0,
        Error=1,
        Init = 1000,
        WaitForPayment = 1001,
    }
}
