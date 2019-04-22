using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
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
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
            GlassGlur.InitGlass(glsBlur);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //读取属性值
            appName.Text = Package.Current.DisplayName;
            appVersion.Text = string.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);
            appPub.Text = Package.Current.PublisherDisplayName;
            appDescription.Text = Package.Current.Description;
            appInsDate.Text = Package.Current.InstalledDate.ToString();
            //读取更新记录
            XDocument xd = XDocument.Load("Static/UpdateRecord.xml");
            List<XElement> list=xd.Descendants("version").ToList();
            foreach(XElement elem in list)
            {
                string Name=elem.Attribute("name").Value;
                TextBlock nBlock = new TextBlock();
                nBlock.Text = "V"+Name;
                nBlock.Margin = new Thickness(0, 15, 0, 0);
                nBlock.FontWeight = FontWeights.Bold;
                upRcd.Children.Add(nBlock);
                List<XElement> item=elem.Descendants("upitem").ToList();
                int cnt = 0;
                foreach(XElement node in item)
                {
                    string Text = node.Attribute("text").Value;
                    TextBlock tBlock = new TextBlock();
                    cnt++;
                    tBlock.Text = cnt+"."+Text;
                    tBlock.Margin = new Thickness(0, 5, 0, 0);
                    upRcd.Children.Add(tBlock);
                }
            }
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btnOper = (HyperlinkButton)sender;
            Uri uri = new Uri(btnOper.Content.ToString());
            bool success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if(!success)
            {
                await MessageAdapter.ShowMsgDlgAsync("打开浏览器失败");
            }
        }
    }
}
