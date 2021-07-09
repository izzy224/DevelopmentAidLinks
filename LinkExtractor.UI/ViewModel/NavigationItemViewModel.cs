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
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            _eventAggregator = eventAggregator;
            OpenEmployeeDetailViewCommand = new DelegateCommand(OnOpenEmployeeDetailView);
            
        }



        public int Id { get; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set { _displayMember = value; OnPropertyChanged(); }
        }

        public ICommand OpenEmployeeDetailViewCommand { get; }

        private IEventAggregator _eventAggregator;

        private void OnOpenEmployeeDetailView()
        {
            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Publish(Id);
        }
    }
}
