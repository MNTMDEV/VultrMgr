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
    public sealed partial class CloudPage : Page
    {
        public CloudPage()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(Cloud_Index));
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string ivkName = args.InvokedItem.ToString();
            switch(ivkName)
            {
                case "首页":
                    contentFrame.Navigate(typeof(Cloud_Index));
                    break;
                case "ISO镜像":
                    break;
                case "快照":
                    break;
                case "云硬盘":
                    break;
                case "部署服务器":
                    break;
            }
        }
    }
}
