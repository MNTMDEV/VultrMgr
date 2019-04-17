using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace VultrMgr
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Application对象带上用户配置对象，以便被各个窗体使用
        /// </summary>
        private UserConfig userConfig=null;

        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            InitUserConfig();
            //InitBack();
        }

        /// <summary>
        /// 加载用户配置项
        /// </summary>
        private void InitUserConfig()
        {
            userConfig = new UserConfig();
            ApplicationDataContainer container = ApplicationData.Current.RoamingSettings;
            userConfig.GetConfig(container);
        }

        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <returns></returns>
        public UserConfig GetUserConfig()
        {
            return userConfig;
        }

        /// <summary>
        /// 用于启动后台任务
        /// </summary>
        private async void InitBack()
        {
            BackgroundExecutionManager.RemoveAccess();
            var requestStatus = await BackgroundExecutionManager.RequestAccessAsync();
            try
            {
                var builder = new BackgroundTaskBuilder();
                builder.Name = "Vultr Status Message";
                builder.SetTrigger(new TimeTrigger(15, false)); //new TimeTrigger(15, true)
                                                                // Do not set builder.TaskEntryPoint for in-process background tasks
                                                                // Here we register the task and work will start based on the time trigger.
                BackgroundTaskRegistration task = builder.Register();
            }
            catch(Exception)
            {
                ;
            }
        }

        /// <summary>
        /// 后台任务处理事件
        /// </summary>
        /// <param name="args"></param>
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            IBackgroundTaskInstance taskInstance = args.TaskInstance;
            //DoYourBackgroundWork(taskInstance);
            //ShowMsg();
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                //if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                //{
                //    //TODO: 从之前挂起的应用程序加载状态
                //}

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    // 需要确定是否需要登录
                    if (!userConfig.isEncrypt)
                    {
                        userConfig.Login(null, null);
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    }
                    else
                        rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }

        /// <summary>
        /// 推送消息函数
        /// </summary>
        private void ShowMsg()
        {
            string title = "<TITLE>";
            string content = "<CONTENT>";
            //string image = "https://picsum.photos/360/202?image=883";
            //string logo = "ms-appdata:///local/Andrew.jpg";

            //toast界面设置
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title
                        },
                        new AdaptiveText()
                        {
                            Text = content
                        },
                        //new AdaptiveImage()
                        //{
                        //    Source = image
                        //}
                    }
                    //,
                    //AppLogoOverride = new ToastGenericAppLogo()
                    //{
                    //    Source = logo,
                    //    HintCrop = ToastGenericAppLogoCrop.Circle
                    //}
                }
            };


            // In a real app, these would be initialized with actual data
            int conversationId = 56482;

            // 动态控件 e.g.按钮等
            ToastActionsCustom actions = new ToastActionsCustom()
            {
                //Inputs =
                //{
                //    new ToastTextBox("tbReply")
                //    {
                //        PlaceholderContent = "Type a response"
                //    }
                //},
                //Buttons =
                //{
                //    new ToastButton("Reply", new QueryString()
                //    {
                //        { "action", "reply" },
                //        { "conversationId", conversationId.ToString() }
                //    }.ToString())
                //    {
                //    ActivationType = ToastActivationType.Background,
                //    ImageUri = "Assets/Reply.png",
                //    // Reference the text box's ID in order to
                //    // place this button next to the text box
                //    TextBoxId = "tbReply"
                //    },
                //    new ToastButton("Like", new QueryString()
                //    {
                //    { "action", "like" },
                //    { "conversationId", conversationId.ToString() }
                //    }.ToString())
                //    {
                //    ActivationType = ToastActivationType.Background
                //    },
                //    new ToastButton("View", new QueryString()
                //    {
                //    { "action", "viewImage" },
                //    { "imageUrl", image }
                //    }.ToString())
                //}
            };

            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()
            };

            //通过内容创建toast对象
            var toast = new ToastNotification(toastContent.GetXml());
            //过期时间
            toast.ExpirationTime = DateTime.Now.AddDays(2);
            //toast属性
            toast.Tag = "18365";
            toast.Group = "wallPosts";
            //发送通知
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
