using LinkExtractor.Models;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkExtractor.UI.ViewModel
{
    public class EmployeeDetailViewModel : ViewModelBase, IEmployeeDetailViewModel
    {
        private IEmployeeDataService _dataService;
        private IEventAggregator _eventAggregator;

        public EmployeeDetailViewModel(IEmployeeDataService dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Employee);
            _eventAggregator.GetEvent<EmployeeSavedEvent>()
                .Publish(new EmployeeSavedEventArgs
                {
                    Id = Employee.Id,
                    DisplayMember = Employee.Name + ' ' + Employee.Surname
                }
                );
        }

        private bool OnSaveCanExecute()
        {
            //Check if it is valid
            return true;
        }

        private async void OnOpenFriendDetailView(int employeeId)
        {
            await LoadAsync(employeeId);
        }

        public async Task LoadAsync(int employeeId)
        {
            Employee = await _dataService.GetByIdAsync(employeeId);
        }

        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            private set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

    }
}
