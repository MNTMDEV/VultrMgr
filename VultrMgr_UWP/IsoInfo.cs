using Imagine.Uwp.Json.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VultrMgr
{
    public class IsoInfo
    {
        [Sign("ISOID")]
        public int ISOID { get; set; }
        [Sign("date_created")]
        public string CreateDate { get; set; }
        [Sign("filename")]
        public string FileName { get; set; }
        [Sign("size")]
        public int Size { get; set; }
        [Sign("md5sum")]
        public string MD5 { get; set; }
        [Sign("sha512sum")]
        public string SHA { get; set; }
        [Sign("status")]
        public string Status { get; set; }

        public IsoInfo()
        {

        }
    }
}
