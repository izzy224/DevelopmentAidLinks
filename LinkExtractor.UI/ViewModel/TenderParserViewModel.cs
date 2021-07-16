using HtmlAgilityPack;
using LinkExtractor.UI.DataServices.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spire.Doc;
using Spire.Doc.Documents;

namespace LinkExtractor.UI.ViewModel
{
    public class TenderParserViewModel : ViewModelBase, ITenderParserViewModel
    {
        private string _address;
        private ITenderRepository _tenderRepository;
        
        public TenderParserViewModel(ITenderRepository tenderRepository)
        {
            _tenderRepository = tenderRepository;
            Address = "https://www.developmentaid.org/#!/tenders/search?statuses=3&modifiedAfter=2021-06-29";
        }
        public string Address 
        { 
            get { return _address; }
            set 
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public async void AddTenders(string html)
        {
            //TODO : Maybe turn this code async
            html = html.Replace("</option>", "");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlTenders = htmlDoc.DocumentNode.SelectNodes("//*[@class=\"search-card__title ng-binding ng-scope\"]").ToList();
            var urlList = new List<string>();
            foreach (var htmlTender in htmlTenders)
            {
                //MessageBox.Show(htmlTender.Attributes["ng-href"].Value);
                urlList.Add(htmlTender.Attributes["ng-href"].Value);
            }
            CreateFile(urlList);
        }

        private void CreateFile(List<String> urlList)
        {
            //TODO : Maybe turn this code async
            using(Document doc = new Document())
            {
                Section section = doc.AddSection();
                Paragraph para = section.AddParagraph();

                foreach(var url in urlList)
                {
                    para.AppendText("https://www.developmentaid.org/"+url+'\n');
                }
                doc.SaveToFile(@"D:\DevelopmentAid\doc.docx", FileFormat.Docx);
            }
        }
    }
}
