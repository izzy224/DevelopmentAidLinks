using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.Events
{
    public class TenderRequestEvent : PubSubEvent<TenderRequestEventArgs>
    {
    }

    public class TenderRequestEventArgs
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string FileName { get; set; }
        //Maybe later add filters
    }
}
