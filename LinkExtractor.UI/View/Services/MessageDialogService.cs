using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LinkExtractor.UI.View.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var res = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return res == MessageBoxResult.OK ? MessageDialogResult.Ok : MessageDialogResult.Cancel;
        }
    }
    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }
}
