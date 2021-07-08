using LinkExtractor.Models;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.Events;
using LinkExtractor.UI.Wrapper;
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
        private EmployeeWrapper _employee;

        public EmployeeDetailViewModel(IEmployeeDataService dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int employeeId)
        {
            var employee = await _dataService.GetByIdAsync(employeeId);

            Employee = new EmployeeWrapper(employee);
            Employee.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Employee.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public EmployeeWrapper Employee
        {
            get { return _employee; }
            private set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Employee.Model);
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
            //Check if employee has changes
            return Employee!=null && !Employee.HasErrors;
        }

        private async void OnOpenFriendDetailView(int employeeId)
        {
            await LoadAsync(employeeId);
        }



    }
}
