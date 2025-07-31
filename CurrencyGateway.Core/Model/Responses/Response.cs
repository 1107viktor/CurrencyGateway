using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace CurrencyGateway.Core.Model.Responses
{
    [XmlRoot(ElementName = "ValCurs")]
    public sealed class CbrResponse
    {
        [XmlAttribute(AttributeName = "Date")]
        public string DateString { get; set; }

        [XmlIgnore]
        public DateTime Date => DateTime.ParseExact(
            DateString,
            "dd.MM.yyyy",
            CultureInfo.InvariantCulture
        );

        [XmlElement(ElementName = "Valute")]
        public List<Currency> Currencies { get; set; }
    }
}
