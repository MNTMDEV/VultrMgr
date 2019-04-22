using Imagine.Uwp.Json.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VultrMgr
{
    public class StorageInfo
    {
        [Sign("SUBID")]
        public int SUBID { get; set; }
        [Sign("date_created")]
        public string CreateDate { get; set; }
        [Sign("cost_per_month")]
        public int Cost { get; set; }
        [Sign("status")]
        public string Status { get; set; }
        [Sign("size_gb")]
        public int Size { get; set; }
        [Sign("DCID")]
        public int DCID { get; set; }
        [Sign("attached_to_SUBID")]
        public int ATSUBID { get; set; }
        [Sign("label")]
        public string Label { get; set; }

        public int Count { get; set; }

        public StorageInfo()
        {
        }
    }
}
