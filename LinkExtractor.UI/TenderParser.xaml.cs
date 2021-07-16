using CefSharp;
using LinkExtractor.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LinkExtractor.UI
{
    /// <summary>
    /// Interaction logic for TenderParser.xaml
    /// </summary>
    public partial class TenderParser : Window
    {
        ITenderParserViewModel _viewModel;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public TenderParser(ITenderParserViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += TenderParser_Loaded;
            wb.FrameLoadEnd += new EventHandler<CefSharp.FrameLoadEndEventArgs>(wb_FrameLoadEnd);
        }

        //maybe async await.. somehow
        private void TenderParser_Loaded(object sender, RoutedEventArgs e)
        {

            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += new EventHandler(timer_Tick);

        }

        private void wb_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();

            timer.Start();
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            var html = await GetHtml();
            MessageBox.Show(html);
            
            Thread t = new Thread(() => _viewModel.AddTenders(html)); ;
            t.Start();
        }

        private async Task<string> GetHtml()
        {
            wb.GetBrowser().MainFrame.ViewSource();
            string taskHtml = await wb.GetBrowser().MainFrame.GetSourceAsync();
            return taskHtml;
        }
    }
}
