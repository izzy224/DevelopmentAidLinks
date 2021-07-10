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
            _eventAggregator.GetEvent<DetailSavedEvent>().Subscribe(DetailSaved);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Subscribe(DetailDeleted);
        }



        public async Task LoadAsync()
        {
            var lookup = await _employeeLookupService.GetEmployeeLookupAsync();
            Employees.Clear();
            foreach (var item in lookup)
            {
                Employees.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,_eventAggregator, nameof(EmployeeDetailViewModel)));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Employees { get; }

        private void DetailDeleted(DetailDeletedEventArgs args)
        {
            switch(args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    {
                        var employee = Employees.SingleOrDefault(e => e.Id == args.Id);
                        if (employee != null)
                        {
                            Employees.Remove(employee);
                        }
                    }
                    break;
                
            }

        }

        private void DetailSaved(DetailSavedEventArgs args)
        {
            switch(args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    {
                        var lookupItem = Employees.SingleOrDefault(e => e.Id == args.Id);
                        if (lookupItem == null)
                        {
                            Employees.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, nameof(EmployeeDetailViewModel)));
                        }
                        else
                        {
                            lookupItem.DisplayMember = args.DisplayMember;
                        }
                    }break;
            }

        }

    }
}
