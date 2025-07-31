using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyGateway.Core.Model
{
    // курс валюты
    public sealed class CurrencyRate
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public int Nominal { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
