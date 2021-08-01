using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
        private MetroWindow MetroWindow { get { return (MetroWindow)App.Current.MainWindow; } }
        public async Task ShowInfoDialogAsync(string text)
        {
            await MetroWindow.ShowMessageAsync("Info",text);
        }

        public async Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title)
        {
            var res = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            return res == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative ? MessageDialogResult.Ok : MessageDialogResult.Cancel;
        }
    }
    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }
}
