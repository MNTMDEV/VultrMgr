using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VultrMgr
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Cloud_Index : Page
    {
        public ObservableCollection<ServerInfo> Recordings { get; set; }

        public Cloud_Index()
        {
            this.InitializeComponent();
            Recordings = new ObservableCollection<ServerInfo>();
        }

        /// <summary>
        /// 加载所有服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HttpAdapter adapter = new HttpAdapter();
            bool bValid = await adapter.KeyValidAsync();
            List<ServerInfo> infoRes = null;
            if (bValid)
            {
                //加载
                infoRes=await adapter.GetServerList();
                int cnt = 0;
                foreach(ServerInfo item in infoRes)
                {
                    item.Num = ++cnt;
                    this.Recordings.Add(item);
                }
                loadGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadBlock.Text = "加载失败,请检查网络是否正常连接以及密钥配置是否正确。";
            }
        }

        /// <summary>
        /// 进行关机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btnOper = (HyperlinkButton)sender;
            string subid = btnOper.Tag.ToString();
            HttpAdapter adapter = new HttpAdapter();
            bool ret=await adapter.ServerPowerOff(subid);
            if (ret)
            {
                await MessageAdapter.ShowMsgDlgAsync("关机成功");
            }
            else
            {
                await MessageAdapter.ShowMsgDlgAsync("关机失败");
            }
        }

        /// <summary>
        /// 执行重启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Restart_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btnOper = (HyperlinkButton)sender;
            string subid = btnOper.Tag.ToString();
            HttpAdapter adapter = new HttpAdapter();
            bool ret = await adapter.ServerRestart(subid);
            if (ret)
            {
                await MessageAdapter.ShowMsgDlgAsync("重启成功");
            }
            else
            {
                await MessageAdapter.ShowMsgDlgAsync("重启失败");
            }
        }

        private async void ReInstall_Click(object sender, RoutedEventArgs e)
        {
            //WARNING
            //await MessageAdapter.ShowMsgDlgAsync("抱歉,作者不敢测试此单元");
            HyperlinkButton btnOper = (HyperlinkButton)sender;
            string subid = btnOper.Tag.ToString();
            int nRet=await MessageAdapter.ShowYesNoAsync("该操作将重装服务器系统,所有数据将被清空\r\n是否进行操作?");
            if(nRet==1)
            {
                HttpAdapter adapter = new HttpAdapter();
                bool ret = await adapter.ServerReInstall(subid);
                if (ret)
                {
                    await MessageAdapter.ShowMsgDlgAsync("重装成功");
                }
                else
                {
                    await MessageAdapter.ShowMsgDlgAsync("重装失败");
                }
            }
        }

        private async void Destroy_Click(object sender, RoutedEventArgs e)
        {
            //WARNING
            //await MessageAdapter.ShowMsgDlgAsync("抱歉,作者不敢测试此单元");
            HyperlinkButton btnOper = (HyperlinkButton)sender;
            string subid = btnOper.Tag.ToString();
            int nRet = await MessageAdapter.ShowYesNoAsync("该操作将销毁服务器,请谨慎执行该操作\r\n是否进行操作?");
            if (nRet == 1)
            {
                HttpAdapter adapter = new HttpAdapter();
                bool ret = await adapter.ServerDestroy(subid);
                if (ret)
                {
                    await MessageAdapter.ShowMsgDlgAsync("销毁成功");
                }
                else
                {
                    await MessageAdapter.ShowMsgDlgAsync("销毁失败");
                }
            }
        }
        private void ViewDetail_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
