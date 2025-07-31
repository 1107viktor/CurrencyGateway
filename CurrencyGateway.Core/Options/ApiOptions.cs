using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyGateway.Core.Options
{
    public class CbrApiOptions
    {
        public const string SectionName = "CbrApi";

        public string BaseAddress { get; set; } 
        public string Endpoint { get; set; }
    }
}
