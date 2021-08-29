using HtmlAgilityPack;
using LinkExtractor.UI.DataServices.Repositories;
using System.Collections.Generic;
using System.Linq;
using LinkExtractor.Models;
using LinkExtractor.UI.Events;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using LiteMiner.classes;
using System.Threading.Tasks;
using LinkExtractor.UI.View.Services;

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
        private string _mail;
        private string _dateFrom;
        private IMessageDialogService _messageDialogService;
        private int _currentPage;
        private List<string> _urlList;
        private List<TenderRequestEventArgs> _args;

        public TenderParserViewModel(ITenderRepository tenderRepository, IMessageDialogService messageDialogService)
        {
            _tenderRepository = tenderRepository;
            _messageDialogService = messageDialogService;
            Tenders = new List<Tender>();
            _urlList = new List<string>();
            _args = new List<TenderRequestEventArgs>();
            _currentPage = 1;
        }

        public int GetCurrentPage()
        {
            return _currentPage;
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
            try
            {
                //TODO : Maybe turn this code async -----------------------------
                html = html.Replace("</option>", "");
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var htmlTenders = htmlDoc.DocumentNode.SelectNodes("//*[@class=\"search-card__title ng-binding ng-scope\"]").ToList();


                //var urlList = new List<string>();
                //urlList changed to _urlList
                foreach (var arg in _args)
                {
                    await _tenderRepository.DeleteByEWIdAsync(arg.Id);
                }


                foreach (var htmlTender in htmlTenders)
                {
                    LanguageDetector ld = new LanguageDetector();

                    if (!(await _tenderRepository.HasUrlAsync(htmlTender.Attributes["ng-href"].Value)))
                    {
                        if (ld.Detect(htmlTender.InnerText) == "en")
                            _urlList.Add(htmlTender.Attributes["ng-href"].Value);
                    }
                }
                if (_urlList.Count < _requestedQuantity)
                {
                    _currentPage++;
                    StartParse();
                }
                else
                {
                    int x = 0;
                    Tenders.Clear();
                    foreach (var arg in _args)
                    {
                        List<string> localUrlList = new List<string>();
                        for (int i = x; i < x + arg.Quantity; i++)
                        {
                            if (!(await _tenderRepository.HasUrlAsync(_urlList[i])))
                            {
                                Tenders.Add(new Tender() { Url = _urlList[i], EmployeeWorkshiftId = arg.Id });
                                localUrlList.Add(_urlList[i]);
                            }
                        }
                        x += arg.Quantity;
                        CreateFile(localUrlList, arg);//TODO : Request url list from database, so every url gets included, not only this cycle
                    }



                    //foreach(var url in _urlList)
                    //{
                    //    if(!(await _tenderRepository.HasUrlAsync(url))) 
                    //    {
                    //        // TODO: Add new if - Tenders.Count + the extracted tenders <= requested quantity else - break
                    //        Tenders.Add(new Tender() { Url = url, EmployeeWorkshiftId = this.EmployeeWorkshiftId}); 
                    //    }


                    //    //This can be done without the list, so maybe review later
                    //}



                    if (Tenders.Count > 0)
                    {
                        await _tenderRepository.AddListAsync(Tenders);
                        await _tenderRepository.SaveAsync();
                    }

                }
            }
            catch(Exception e)
            {
                await _messageDialogService.ShowInfoDialogAsync("An error happened, contact Vasea:\n" + e);
            }
        }

        private void CreateFile(List<string> urlList, TenderRequestEventArgs args)
        {
            //TODO : Maybe turn this code async
            using(Document doc = new Document())
            {
                Section section = doc.AddSection();
                Paragraph para = section.AddParagraph();

                int x = 0;
                foreach(var url in urlList)
                {
                    x++;
                    para.AppendText($"{x}. "+"https://www.developmentaid.org/"+url+"\n\n");
                }
                doc.SaveToFile(@$"D:\DevelopmentAid\{args.FileName}.docx", FileFormat.Docx);

                //TODO : Add path from json file, also implement the menu item for choosing the path
                //TODO : Maybe execute a script from powershell - - -
                
                //using(MailMessage message = new MailMessage())
                //{
                //    using(SmtpClient smtp = new SmtpClient())
                //    {
                //        message.From = new MailAddress("linktogo365@gmail.com");
                //        message.To.Add(new MailAddress(_mail));
                //        System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                //        contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                //        contentType.Name = _fileName+".docx";
                //        message.Attachments.Add(new Attachment(@$"D:\DevelopmentAid\{_fileName}.docx", contentType));  //TODO : Path from json file
                //        message.Subject = "Today's tenders";
                //        smtp.Port = 587;
                //        smtp.Host = "smtp.gmail.com";
                //        smtp.EnableSsl = true;
                //        smtp.UseDefaultCredentials = false;
                //        smtp.Credentials = new NetworkCredential("linktogo365@gmail.com", "Linktogoapp@@");
                //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        smtp.Send(message);
                //    }
                //}
            }
        }

        public void StartParse()
        {
            //foreach(var arg in args)
            //{
            //EmployeeWorkshiftId = arg.Id;
            //_requestedQuantity = quantity;
            //_fileName = arg.FileName;
            //_mail = arg.Email;
            //_dateFrom = arg.DateFrom;
            //Address = @$"https://www.developmentaid.org/#!/tenders/search?statuses=3&modifiedAfter={_dateFrom}";
            
            //Page 2: https://www.developmentaid.org/#!/tenders/search?showAdvancedFilters=1&pageSize=100&pageNr=2&statuses=3&modifiedAfter={_dateFrom}
            //}
                Address = @$"https://www.developmentaid.org/#!/tenders/search?showAdvancedFilters=1&pageSize=100&pageNr={_currentPage}&statuses=3&modifiedAfter={_dateFrom}";

        }
        public void StartLogin()
        {
            Address = @$"https://www.developmentaid.org/#!/authentication/login";
        }
        public void SetupArgs(List<TenderRequestEventArgs> args, int quantity)
        {
            _requestedQuantity = quantity;
            _args.Clear();
            foreach(var arg in args)
            {
                _args.Add(arg);
            }
            if (args[0].Type == RequestType.Parse)
            {
                _dateFrom = args[0].DateFrom;
            }

        }
    }
}
