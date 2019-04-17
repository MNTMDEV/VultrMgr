using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
            GlassGlur.InitGlass(glsBlur);
        }

        private bool isEncrypt = false;

        /// <summary>
        /// 加载配置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSecurity();
        }

        /// <summary>
        /// 加载安全设置
        /// </summary>
        private void LoadSecurity()
        {
            UserConfig config = ((App)Application.Current).GetUserConfig();
            if (!config.isLogin)
                return;
            isEncrypt = config.isEncrypt;
            //加载配置
            apiKey.Password = config.ApiKey;
            userName.Text = config.UserName;
        }

        /// <summary>
        /// 重置按钮1点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Reset_1_Click(object sender, RoutedEventArgs e)
        {
            reset_1.IsEnabled = false;
            ContentDialog msgDlg = new ContentDialog()
            {
                Title = Package.Current.DisplayName,
                Content = "重置将会清除ApiKey以及认证信息\r\n是否重置?",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                FullSizeDesired = false,
            };

            msgDlg.PrimaryButtonClick += (_s, _e) => { };
            int nResult=(int)await msgDlg.ShowAsync();
            if(nResult==1)
            {
                UserConfig config = ((App)Application.Current).GetUserConfig();
                if (config.ResetConfig())
                {
                    await MessageAdapter.ShowMsgDlgAsync("重置成功");
                    apiKey.Password = "";
                    userName.Text = "";
                    password.Password = "";
                    passRepeat.Password = "";
                }
                else
                    await MessageAdapter.ShowMsgDlgAsync("重置失败");
            }
            reset_1.IsEnabled = true;
        }

        /// <summary>
        /// 保存按钮1点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_1_Click(object sender, RoutedEventArgs e)
        {
            UserConfig config = ((App)Application.Current).GetUserConfig();
            if (!config.isLogin)
                return;
            if(password.Password!=passRepeat.Password)
            {
                await MessageAdapter.ShowMsgDlgAsync("两次输入的密码不一致");
                return;
            }
            bool ret=config.SaveConfig(apiKey.Password, userName.Text, password.Password);
            if (ret)
                await MessageAdapter.ShowMsgDlgAsync("保存成功");
            else
                await MessageAdapter.ShowMsgDlgAsync("保存失败");
        }

        /// <summary>
        /// 测试ApiKey的有效性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Test_key_Click(object sender, RoutedEventArgs e)
        {
            test_key.IsEnabled = false;
            HttpAdapter adapter = new HttpAdapter();
            bool ret = await adapter.KeyValidAsync();
            if (ret)
                await MessageAdapter.ShowMsgDlgAsync("ApiKey有效");
            else
                await MessageAdapter.ShowMsgDlgAsync("ApiKey无效");
            test_key.IsEnabled = true;
        }
    }
}
