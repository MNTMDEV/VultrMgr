using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace VultrMgr
{
    class MessageAdapter
    {
        public async static Task ShowMsgDlgAsync(string content)
        {
            await new MessageDialog(content, Package.Current.DisplayName).ShowAsync();
        }

        public async static Task ShowMsgDlgAsync(string content,string title)
        {
            await new MessageDialog(content, title).ShowAsync();
        }

        public async static Task NoSupportAsync()
        {
            await ShowMsgDlgAsync("目前版本不支持该功能");
        }

        public async static Task<int> ShowYesNoAsync(string content)
        {
            ContentDialog msgDlg = new ContentDialog()
            {
                Title = Package.Current.DisplayName,
                Content = content,
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                FullSizeDesired = false,
            };

            msgDlg.PrimaryButtonClick += (_s, _e) => { };
            int nResult = (int)await msgDlg.ShowAsync();
            return nResult;
        }
    }
}
