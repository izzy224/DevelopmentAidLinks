using HtmlAgilityPack;
using LinkExtractor.UI.DataServices.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spire.Doc;
using Spire.Doc.Documents;
using LinkExtractor.Models;
using LinkExtractor.UI.Events;
using System.Threading.Tasks;
using Prism.Events;

namespace LinkExtractor.UI.ViewModel
{
    public class TenderParserViewModel : ViewModelBase, ITenderParserViewModel
    {
        private string _address;
        private int _employeeWorkshiftId;
        private ITenderRepository _tenderRepository;
        private List<Tender> Tenders;
        private int _requestedQuantity;
        private string _fileName;



        public TenderParserViewModel(ITenderRepository tenderRepository)
        {
            _tenderRepository = tenderRepository;
            Tenders = new List<Tender>();
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
        public int EmployeeWorkshiftId 
        {
            get { return _employeeWorkshiftId; }
            set
            {
                _employeeWorkshiftId = value;
            }
        }


        public async void AddTenders(string html)
        {
            //TODO : Maybe turn this code async -----------------------------
            html = html.Replace("</option>", "");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlTenders = htmlDoc.DocumentNode.SelectNodes("//*[@class=\"search-card__title ng-binding ng-scope\"]").ToList();
            var urlList = new List<string>();

            foreach (var htmlTender in htmlTenders)
            {
                urlList.Add(htmlTender.Attributes["ng-href"].Value);
            }

            Tenders.Clear();
            foreach(var url in urlList)
            {
                if(!(await _tenderRepository.HasUrlAsync(url))) 
                {
                    // TODO: Add new if - Tenders.Count + the extracted tenders <= requested quantity else - break
                    Tenders.Add(new Tender() { Url = url, EmployeeWorkshiftId = this.EmployeeWorkshiftId}); 
                }
                
                
                //This can be done without the list, so maybe review later
            }
            if(Tenders.Count>0)
            {
                await _tenderRepository.AddListAsync(Tenders);
                await _tenderRepository.SaveAsync();
            }
            CreateFile(urlList);//TODO : Request url list from database, so every url gets included, not only this cycle
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
                doc.SaveToFile(@$"D:\DevelopmentAid\{_fileName}.docx", FileFormat.Docx);
                //TODO : Add path from json file, also implement the menu item for choosing the path
            }
        }

        public void StartParse(TenderRequestEventArgs args)
        {
            EmployeeWorkshiftId = args.Id;
            _requestedQuantity = args.Quantity;
            _fileName = args.FileName;
        }
    }
}
