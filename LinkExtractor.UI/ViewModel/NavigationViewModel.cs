using LinkExtractor.Models;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.DataServices.Lookups;
using LinkExtractor.UI.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IEmployeeLookupDataService _employeeLookupService;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(IEmployeeLookupDataService employeeLookupService,
            IEventAggregator eventAggregator)
        {
            _employeeLookupService = employeeLookupService;
            _eventAggregator = eventAggregator;
            Employees = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<EmployeeSavedEvent>().Subscribe(EmployeeSaved);
            _eventAggregator.GetEvent<EmployeeDeletedEvent>().Subscribe(EmployeeDeleted);
        }



        public async Task LoadAsync()
        {
            var lookup = await _employeeLookupService.GetEmployeeLookupAsync();
            Employees.Clear();
            foreach (var item in lookup)
            {
                Employees.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,_eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Employees { get; }

        private void EmployeeDeleted(int employeeId)
        {
            var employee = Employees.SingleOrDefault(e => e.Id == employeeId);
            if(employee!=null)
            {
                Employees.Remove(employee);
            }
        }

        private void EmployeeSaved(EmployeeSavedEventArgs obj)
        {
            var lookupItem = Employees.SingleOrDefault(e => e.Id == obj.Id);
            if (lookupItem == null)
            {
                Employees.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }

    }
}
