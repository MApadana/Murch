using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Authentication.Content.Resources
{
    public static class BadRequestSignatures
    {
        public static string[] badRequestKeywords = {
            //Xss Attacks
            "<script>",
            "</script>",
        };
    }
}
