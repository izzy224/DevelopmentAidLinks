using CefSharp;
using LinkExtractor.UI.Events;
using LinkExtractor.UI.ViewModel;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows;

namespace LinkExtractor.UI
{
    /// <summary>
    /// Interaction logic for TenderParser.xaml
    /// </summary>
    public partial class TenderParser : Window
    {
        private ITenderParserViewModel _viewModel;
        private IEventAggregator _eventAggregator;
        public TenderRequestEventArgs Args
        { 
            get
            {
                return _args;
            }
            set
            {
                _args = value;
                StartParse(_args);
            }
        }

        private TenderRequestEventArgs _args;

        public TenderParser(ITenderParserViewModel viewModel, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _eventAggregator = eventAggregator;
            DataContext = _viewModel;
            _eventAggregator.GetEvent<TenderRequestEvent>().Subscribe(StartParse, true);

            wb.FrameLoadEnd += WebBrowserFrameLoadEnded;
            this.Loaded += TenderParser_Loaded;
        }

        private void TenderParser_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private async void WebBrowserFrameLoadEnded(object sender, FrameLoadEndEventArgs e)
        {
                
             if(e.Frame.IsMain)
                {
                await Task.Delay(3000);// - In case it is not loaded
//                wb.ViewSource();
                var html = await wb.GetSourceAsync();
                _viewModel.AddTenders(html);
                }
        }

        private void StartParse(TenderRequestEventArgs args)
        {
            _viewModel.StartParse(args);
        }

        //TODO : Maybe turn this code async or at least multi-thread


    }
}
