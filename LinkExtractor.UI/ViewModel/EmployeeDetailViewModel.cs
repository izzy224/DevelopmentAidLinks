using LinkExtractor.Models;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.DataServices.Repositories;
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
        private IEmployeeRepository _employeeRepository;
        private IEventAggregator _eventAggregator;
        private EmployeeWrapper _employee;

        public EmployeeDetailViewModel(IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator)
        {
            _employeeRepository = employeeRepository;
            _eventAggregator = eventAggregator;


            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            Employee = new EmployeeWrapper(employee);
            Employee.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _employeeRepository.HasChanges();
                }
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

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand SaveCommand { get; }

        private async void OnSaveExecute()
        {
            await _employeeRepository.SaveAsync();
            HasChanges = _employeeRepository.HasChanges();
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
            return Employee != null && !Employee.HasErrors && HasChanges;
        }




    }
}
