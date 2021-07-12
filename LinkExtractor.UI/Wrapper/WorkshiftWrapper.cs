using LinkExtractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.Wrapper
{
    public class WorkshiftWrapper : ModelWrapper<Workshift>
    {
        public WorkshiftWrapper(Workshift model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }
        public DateTime Date 
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); } 
        }

    }

}
