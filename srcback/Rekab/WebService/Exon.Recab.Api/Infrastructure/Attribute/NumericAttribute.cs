using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Infrastructure.Attribute
{
    public class NumericAttribute : ValidationAttribute
    {
        public NumericAttribute()
        { }

        public override bool IsValid(object value)
        {
            long num = 0;
           
            return long.TryParse(value.ToString(), out num);
        }


    }
}
