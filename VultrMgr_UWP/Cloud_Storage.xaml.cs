using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class Cloud_Storage : Page
    {
        public ObservableCollection<StorageInfo> Recordings { get; set; }

        public Cloud_Storage()
        {
            this.InitializeComponent();
            Recordings = new ObservableCollection<StorageInfo>();
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HttpAdapter adapter = new HttpAdapter();
            bool bValid = await adapter.KeyValidAsync();
            List<StorageInfo> infoRes = null;
            if (bValid)
            {
                //加载
                infoRes = await adapter.GetStorageList();
                int cnt = 0;
                foreach (StorageInfo item in infoRes)
                {
                    item.Count = ++cnt;
                    this.Recordings.Add(item);
                }
                loadGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadBlock.Text = "加载失败,请检查网络是否正常连接以及密钥配置是否正确。";
            }
        }
    }
}
