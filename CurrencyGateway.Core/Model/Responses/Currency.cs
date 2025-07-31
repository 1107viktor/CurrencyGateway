using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace CurrencyGateway.Core.Model.Responses
{
    public sealed class Currency
    {
        [XmlElement(ElementName = "CharCode")]
        public string CurrencyCode { get; set; }

        [XmlElement(ElementName = "Name")]
        public string CurrencyName { get; set; }

        [XmlElement(ElementName = "Nominal")]
        public int Nominal { get; set; }

        [XmlElement(ElementName = "Value")]
        public string ValueString { get; set; }

        [XmlIgnore]
        public decimal Value => decimal.Parse(
            ValueString.Replace(",", "."),  // Замена запятой на точку
            NumberStyles.Any,
            CultureInfo.InvariantCulture
        );
    }
}
