using CefSharp;
using LinkExtractor.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void TenderParser_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadAsync or something
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += new EventHandler(timer_Tick);
            /*CefSharp.Wpf.ChromiumWebBrowser*/
            //wb = new
            //CefSharp.Wpf.ChromiumWebBrowser("https://www.developmentaid.org/#!/tenders/search?statuses=3&modifiedAfter=2021-06-29");
            wb.Address = "https://www.developmentaid.org/#!/tenders/search?statuses=3&modifiedAfter=2021-06-29";
        
        }

        private void wb_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();

            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            string html = GetHtml();
            MessageBox.Show(html);
        }

        private string GetHtml()
        {
            wb.GetBrowser().MainFrame.ViewSource();
            Task<String> taskHtml = wb.GetBrowser().MainFrame.GetSourceAsync();
            return taskHtml.Result;
        }
    }
}
