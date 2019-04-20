using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace VultrMgr
{
    public class UserConfig
    {
        /// <summary>
        /// 未加密的ApiKey
        /// </summary>
        public string ApiKey { get; private set; }
        /// <summary>
        /// AES加密的ApiKey
        /// </summary>
        public string ApiKeyHash { get; private set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// 密码的SHA1哈希值
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// ApiKey是否被加密
        /// </summary>
        public bool isEncrypt { get; private set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public bool isLogin { get; private set; }
        private string AesKey;
        /// <summary>
        /// 存入对象的存储对象
        /// </summary>
        private ApplicationDataContainer container=null;

        /// <summary>
        /// 通过该方法可以获取用户配置
        /// </summary>
        /// <param name="container">Application的存储设置对象</param>
        public void GetConfig(ApplicationDataContainer container)
        {
            this.container=container;
            ApiKeyHash = getString("ApiKey");
            UserName = getString("UserName");
            Password = getString("Password");
            isEncrypt = getBool("Encrypt");
        }

        /// <summary>
        /// 获取字符串格式的存储内容
        /// </summary>
        /// <param name="column">要查询的列</param>
        /// <returns>结果</returns>
        private string getString(string column)
        {
            if (container == null)
                return "";
            object obj = container.Values[column];
            if (obj == null)
                return "";
            return obj.ToString();
        }

        /// <summary>
        /// 获取布尔型值
        /// </summary>
        /// <param name="column">要查询的列</param>
        /// <returns>结果</returns>
        private bool getBool(string column)
        {
            if (container == null)
                return false;
            object obj = container.Values[column];
            if (obj == null)
                return false;
            return (bool)obj;
        }

        /// <summary>
        /// 获取布尔型值2
        /// </summary>
        /// <param name="column">要查询的列</param>
        /// <param name="flag">如果查询不到，默认的返回值</param>
        /// <returns>结果</returns>
        private bool getBool(string column,bool flag)
        {
            if (container == null)
                return flag;
            object obj = container.Values[column];
            if (obj == null)
                return flag;
            return (bool)obj;
        }

        /// <summary>
        /// 登录函数
        /// </summary>
        /// <param name="usn">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public bool Login(string usn,string pwd)
        {
            //必须先获取配置
            if (container == null)
                return false;
            try
            {
                if (!isEncrypt)
                {
                    //无验证情况
                    //输入了不该输入的usn和密码
                    if (!String.IsNullOrEmpty(usn))
                        return false;
                    if (!String.IsNullOrEmpty(pwd))
                        return false;
                    //ApiKey就是原文
                    this.ApiKey = this.ApiKeyHash;
                }
                else //验证身份
                {
                    //验证用户
                    if (usn != this.UserName)
                        return false;
                    //SHA1校验 密码
                    string pwdHash = SHA1Crypto.EncryptString(pwd);
                    if (pwdHash != this.Password)
                    {
                        //密码错误
                        return false;
                    }
                    //ApiKey不为空
                    if(!String.IsNullOrEmpty(this.ApiKeyHash))
                    {
                        string strApiKey = AesCrypto.DecryptString(this.ApiKeyHash, MD5Crypto.EncryptString(pwd));
                        if (String.IsNullOrEmpty(strApiKey))
                        {
                            //说明ApiKey不是用该口令生成的
                            //纠正错误值
                            container.Values["ApiKey"] = "";
                        }
                        this.ApiKey = strApiKey;
                    }
                    else
                    {
                        this.ApiKey = "";
                    }
                }
            }
            catch(Exception)
            {
                return false;
            }
            AesKey = MD5Crypto.EncryptString(pwd);
            isLogin = true;
            return true;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="api">ApiKey</param>
        /// <param name="usn">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public bool SaveConfig(string api,string usn,string pwd)
        {
            //必须先加载配置
            if (container == null)
                return false;
            try
            {
                //修改登录认证方式
                if(String.IsNullOrEmpty(usn))
                {
                    //不认证
                    container.Values["Encrypt"] = false;
                    container.Values["UserName"] = null;
                    container.Values["Password"] = null;
                    container.Values["ApiKey"] = api;
                }
                else
                {
                    //认证
                    container.Values["Encrypt"] = true;
                    container.Values["UserName"] = usn;
                    //非空表示更改密码
                    if (!String.IsNullOrEmpty(pwd))
                    {
                        container.Values["Password"] = SHA1Crypto.EncryptString(pwd);
                        container.Values["ApiKey"] = AesCrypto.EncryptString(api, MD5Crypto.EncryptString(pwd));
                        //还需要更新AesKey
                        AesKey = MD5Crypto.EncryptString(pwd);
                    }
                    else
                    {
                        // 用原密码进行Aes加密
                        container.Values["ApiKey"] = AesCrypto.EncryptString(api, this.AesKey);
                    }
                }
                //重新加载配置
                GetConfig(this.container);
                //还需要更新明文的ApiKey
                this.ApiKey = api;
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        public bool ResetConfig()
        {
            if (container == null)
                return false;
            try
            {
                container.Values["Encrypt"] = false;
                container.Values["UserName"] = null;
                container.Values["Password"] = null;
                container.Values["ApiKey"] = null;
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public UserConfig()
        {
            ApiKey = "";
            ApiKeyHash = "";
            UserName = "";
            Password = "";
            isEncrypt = false;
            isLogin = false;
            AesKey = "";
        }
    }
}
