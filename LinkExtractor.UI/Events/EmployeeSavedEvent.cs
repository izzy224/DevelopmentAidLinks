using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.Events
{
    public class EmployeeSavedEvent : PubSubEvent<EmployeeSavedEventArgs>
    {

    }

    public class EmployeeSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
