using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VultrMgr
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AccountPage : Page
    {
        public AccountPage()
        {
            this.InitializeComponent();
            GlassGlur.InitGlass(glsBlur);
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HttpAdapter adapter = new HttpAdapter();
            bool bValid = await adapter.KeyValidAsync();
            if(bValid)
            {
                AccountInfo infoRes=await adapter.GetAccountInfo();
                email.Text = infoRes.Email;
                name.Text = infoRes.Name;
                balance.Text = infoRes.Balance;
                pending.Text = infoRes.Pending;
                pdate.Text = infoRes.PayDate;
                pamount.Text = infoRes.PayAmount;
            }
        }
    }
}
