using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Domain.Entity
{
    public class RecabSystemConfig :BaseEntity
    {
        [Required]
        public bool AutomaticConfirmCreatedAD { get; set; }

        [Required]
        public bool AutomaticConfirmUpdateAD { get; set; }

        [Required]
        public int ADSPictureInFirstTime { get; set; }


        [Required]
        public int NewUesrVoucher { get; set; }

        [Required]
        public int NewDealershipVoucher { get; set; }

    }
}
