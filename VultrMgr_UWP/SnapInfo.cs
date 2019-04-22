using Imagine.Uwp.Json.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VultrMgr
{
    public class SnapInfo
    {
        [Sign("SNAPSHOTID")]
        public string SnapID { get; set; }
        [Sign("date_created")]
        public string CreateDate { get; set; }
        [Sign("description")]
        public string Description { get; set; }
        [Sign("size")]
        public string Size { get; set; }
        [Sign("status")]
        public string Status { get; set; }
        [Sign("OSID")]
        public string OSID { get; set; }
        [Sign("APPID")]
        public string APPID { get; set; }

        public int Count { get; set; }

        public SnapInfo()
        {

        }
    }
}
