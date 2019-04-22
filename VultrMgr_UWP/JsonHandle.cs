using Imagine.Uwp.Json;
using Imagine.Uwp.Json.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VultrMgr
{
    class JsonHandle
    {
        public JsonHandle()
        {
            
        }

        /// <summary>
        /// Json标准化([]变{})
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string JsonLize(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;
            if ((str[0] == '['))
            {
                str = "{" + str.Substring(1);
            }
            if ((str[str.Length - 1] == ']'))
            {
                str = str.Substring(0, str.Length-1) + "}";
            }
            return str;
        }

        /// <summary>
        /// 分离Json各项
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> SplitJsonString(string str)
        {
            str=str.Trim();
            if((!str.StartsWith("{")&&str.EndsWith("}")))
            {
                return null;
            }
            str=str.Substring(1, str.Length - 2).Trim();
            List<string> ret = new List<string>();
            int ind;
            while (str.Length > 0)
            {
                ind = FindFirstEnd(str);
                if (ind == -1)
                    return null;
                ret.Add(str.Substring(0, ind + 1).Trim());
                str = str.Substring(ind+1).Trim();
                if (str.StartsWith(","))
                    str = str.Substring(1).Trim();
            }
            return ret;
        }

        /// <summary>
        /// 提取出json的值为json的字符串的值
        /// </summary>
        /// <param name="list"></param>
        private static void ExtractJsonValue(List<string> list)
        {
            for(int i=0; i<list.Count ;i++)
            {
                int ind = list[i].IndexOf(":");
                list[i] = list[i].Substring(ind+1);
            }
        }

        /// <summary>
        /// 找到第一个json的结尾
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static int FindFirstEnd(string str)
        {
            int nLeft = 0;
            for(int i=0; i<str.Length ;i++)
            {
                char ch = str.ElementAt(i);
                if (ch == '{')
                    nLeft++;
                else if (ch == '}')
                {
                    nLeft--;
                    if (nLeft < 0)
                        return -1;
                }
                else if((nLeft==0)&&(ch==','))
                {
                    return i-1;
                }
            }
            if (nLeft != 0)
                return -1;
            return str.Length-1;
        }

        //实现

        /// <summary>
        /// 生成服务器信息对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<ServerInfo> GenerateServerInfo(List<string> list)
        {
            List<ServerInfo> info = new List<ServerInfo>();
            foreach(string item in list)
            {
                ServerInfo elem=Json.Parse<ServerInfo>(item);
                info.Add(elem);
            }
            return info;
        }

        /// <summary>
        /// 处理服务器信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<ServerInfo> ServerListJson(string str)
        {
            List<string> list = SplitJsonString(str);
            ExtractJsonValue(list);
            List<ServerInfo> listRes=GenerateServerInfo(list);
            return listRes;
        }

        /// <summary>
        /// 生成ISO信息对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<IsoInfo> GenerateIsoInfo(List<string> list)
        {
            List<IsoInfo> info = new List<IsoInfo>();
            foreach (string item in list)
            {
                IsoInfo elem = Json.Parse<IsoInfo>(item);
                info.Add(elem);
            }
            return info;
        }

        /// <summary>
        /// 处理ISO信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<IsoInfo> IsoListJson(string str)
        {
            List<string> list = SplitJsonString(str);
            ExtractJsonValue(list);
            List<IsoInfo> listRes = GenerateIsoInfo(list);
            return listRes;
        }

        /// <summary>
        /// 生成快照信息对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<SnapInfo> GenerateSnapInfo(List<string> list)
        {
            List<SnapInfo> info = new List<SnapInfo>();
            foreach (string item in list)
            {
                SnapInfo elem = Json.Parse<SnapInfo>(item);
                info.Add(elem);
            }
            return info;
        }

        /// <summary>
        /// 处理快照信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<SnapInfo> SnapListJson(string str)
        {
            List<string> list = SplitJsonString(str);
            ExtractJsonValue(list);
            List<SnapInfo> listRes = GenerateSnapInfo(list);
            return listRes;
        }

        /// <summary>
        /// 处理账号信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static AccountInfo AccountInfoJson(string strAccount,string strUser)
        {
            AccountInfo listRes = Json.Parse<AccountInfo>(strAccount);
            AccountInfo listRes2 =Json.Parse<AccountInfo>(strUser);
            listRes.Email = listRes2.Email;
            listRes.Name = listRes2.Name;
            return listRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<StorageInfo> GenerateStorageInfo(List<string> list)
        {
            List<StorageInfo> info = new List<StorageInfo>();
            foreach (string item in list)
            {
                StorageInfo elem=Json.Parse<StorageInfo>(item);
                info.Add(elem);
            }
            return info;
        }

        /// <summary>
        /// 处理云硬盘信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<StorageInfo> StorageListJson(string str)
        {
            str = JsonLize(str);
            List<string> list = SplitJsonString(str);
            List<StorageInfo> listRes = GenerateStorageInfo(list);
            return listRes;
        }
    }
    
}
