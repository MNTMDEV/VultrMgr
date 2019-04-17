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

        public static List<ServerInfo> ServerListJson(string str)
        {
            List<string> list = SplitJsonString(str);
            ExtractJsonValue(list);
            List<ServerInfo> listRes=GenerateServerInfo(list);
            return listRes;
        }
    }
    /// <summary>
    /// 服务器信息对象
    /// </summary>
    public class ServerInfo
    {
        public string ServerText { get { return Label + "|"+MainIP; } }
        public int Num { get; set; }

        [Sign("SUBID")]
        public string Subid { set; get; }
        [Sign("os")]
        public string Os { set; get; }
        [Sign("ram")]
        public string Ram { set; get; }
        [Sign("disk")]
        public string Disk { set; get; }
        [Sign("main_ip")]
        public string MainIP { set; get; }
        [Sign("vcpu_count")]
        public string NCPU { set; get; }
        [Sign("location")]
        public string Location { set; get; }
        [Sign("default_password")]
        public string Password { set; get; }
        [Sign("date_created")]
        public string DateCreate { set; get; }
        [Sign("pending_charges")]
        public string Charge { set; get; }
        [Sign("status")]
        public string Status { set; get; }
        [Sign("cost_per_month")]
        public string MonthCharge { set; get; }
        //[Sign("current_bandwidth_gb")]
        //public float Bandwidth { set; get; }
        //[Sign("allowed_bandwidth_gb")]
        //public string BandMax { set; get; }
        [Sign("power_status")]
        public string Power { set; get; }
        [Sign("server_state")]
        public string ServerState { set; get; }
        [Sign("label")]
        public string Label { set; get; }
        [Sign("tag")]
        public string Tag { set; get; }

        public ServerInfo()
        {

        }
    }
}
