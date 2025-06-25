using System.Collections.Generic;
using Bear.Core.Common.Pager;

namespace Bear.Core.SqlSugar
{
    public class PageParam: TableParam
    {
        public int StartIndex => (PageIndex - 1) * PageSize;

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }







        public PageParam()
        {
            PageSize = 10;
            PageIndex = 1;
        }

    }
}
