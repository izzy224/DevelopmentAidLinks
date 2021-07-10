using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.Events
{
    public class DetailDeletedEvent:PubSubEvent<DetailDeletedEventArgs>
    {

    }
    public class DetailDeletedEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
