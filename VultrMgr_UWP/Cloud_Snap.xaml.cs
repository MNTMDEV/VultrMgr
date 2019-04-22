﻿using System;
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
    public sealed partial class Cloud_Snap : Page
    {
        public ObservableCollection<SnapInfo> Recordings { get; set; }

        public Cloud_Snap()
        {
            this.InitializeComponent();
            Recordings = new ObservableCollection<SnapInfo>();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HttpAdapter adapter = new HttpAdapter();
            bool bValid = await adapter.KeyValidAsync();
            List<SnapInfo> infoRes = null;
            if (bValid)
            {
                //加载
                infoRes = await adapter.GetSnapList();
                int cnt = 0;
                foreach (SnapInfo item in infoRes)
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

        /// <summary>
        /// 销毁快照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Destroy_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btnOper = (HyperlinkButton)sender;
            string snapid = btnOper.Tag.ToString();
            int nRet = await MessageAdapter.ShowYesNoAsync("该操作将删除此快照\r\n是否进行操作?");
            if (nRet == 1)
            {
                HttpAdapter adapter = new HttpAdapter();
                bool ret = await adapter.SnapDestroy(snapid);
                if (ret)
                {
                    await MessageAdapter.ShowMsgDlgAsync("删除成功");
                }
                else
                {
                    await MessageAdapter.ShowMsgDlgAsync("删除失败");
                }
            }
        }
    }
}
