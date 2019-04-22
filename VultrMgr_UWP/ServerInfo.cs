using Imagine.Uwp.Json.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VultrMgr
{
    /// <summary>
    /// 服务器信息对象
    /// </summary>
    public class ServerInfo
    {
        public string ServerText { get { return Label + "|" + MainIP; } }
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
