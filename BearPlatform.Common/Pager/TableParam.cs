using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Enums;

namespace BearPlatform.Common.Pager
{
    public class TableParam
    {
        public Dictionary<string, Dictionary<string, string>> Search { get; set; }
        public IDictionary<string, OrderTypeEnum> SortList
        {
            get;
            set;
        }
    }
    
}
