using LinkExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.Wrapper
{

    public class EmployeeWrapper : ModelWrapper<Employee>
    {
        public EmployeeWrapper(Employee model) : base(model)
        {

        }

        public int Id { get { return Model.Id; } }

        public string Name
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }




        public string Surname
        {
            get
            { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }


        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
                switch(propertyName)
                {
                    case nameof(Name):
                        if(string.IsNullOrWhiteSpace(Name))
                        {
                            yield return "The character string is null, empty or consists of white space characters";
                        }
                        break;
                }
        }

    }
}
