using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.Events
{
    public class DetailSavedEvent : PubSubEvent<DetailSavedEventArgs>
    {

    }

    public class DetailSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
        public string ViewModelName { get; set; }
    }
}
