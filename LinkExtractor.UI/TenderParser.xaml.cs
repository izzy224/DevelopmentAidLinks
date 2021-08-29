using CefSharp;
using LinkExtractor.UI.Events;
using LinkExtractor.UI.ViewModel;
using Prism.Events;
using System;
using System.Collections.Generic;
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
        //private IEventAggregator _eventAggregator;
        private int _requestedQuantity;
        private List<int> _pagesExecuted;
        public List<TenderRequestEventArgs> Args
        {
            get
            {
                return _args;
            }
            set
            {
                _args.Clear();
                foreach (var x in value)
                {
                    _args.Add(x);
                }
                if (_args[0].Type == RequestType.Parse)
                    StartParse(_args);
                else
                    StartLogin();
            }
        }
        public int RequestedQuantity
        {
            get
            {
                return _requestedQuantity;
            }
            set
            {
                _requestedQuantity = value;
            }
        }


        private List<TenderRequestEventArgs> _args;

        public TenderParser(ITenderParserViewModel viewModel, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _args = new List<TenderRequestEventArgs>();
            _pagesExecuted = new List<int>();
            //_eventAggregator = eventAggregator;
            DataContext = _viewModel;
            //_eventAggregator.GetEvent<TenderRequestEvent>().Subscribe(StartParse, true);

            wb.BrowserSettings.ApplicationCache = CefState.Enabled;

            wb.FrameLoadEnd += WebBrowserFrameLoadEnded;
            this.Loaded += TenderParser_Loaded;
        }

        private void TenderParser_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private async void WebBrowserFrameLoadEnded(object sender, FrameLoadEndEventArgs e)
        {
            if (_args[0].Type == RequestType.Parse)
            {
                if (e.Frame.IsMain)
                {
                    if (!_pagesExecuted.Contains(_viewModel.GetCurrentPage()))
                    {                   
                        await Task.Delay(7000);// - In case it is not loaded
                        _pagesExecuted.Add(_viewModel.GetCurrentPage());
                        //wb.ViewSource();
                        var html = await wb.GetSourceAsync();
                        await Task.Run(() => _viewModel.AddTenders(html));
                    }
                }
            }
        }

        private void StartParse(List<TenderRequestEventArgs> args)
        {
            _viewModel.SetupArgs(args, RequestedQuantity);
            _viewModel.StartParse();
        }

        //TODO : Maybe turn this code async or at least multi-thread

        private void StartLogin()
        {
            this.Width = 1280;
            this.Height = 720;
            this.Visibility = Visibility.Visible;
            this.wb.Visibility = Visibility.Visible;
            _viewModel.StartLogin();
        }
    }
}
