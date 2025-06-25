using Bear.Core.Common.Enums;
using Bear.Core.Entity;
namespace Bear.Core.Models
{

    //public class TableFormParam : PageParam
    //{
    //    public string KeyWord { get; set; }

    //}


    public class TableFormParam
    {

        public string Tableof { get; set; }
        public string Router { get; set; }
        public string ConfigId { get; set; }
    }
    public class UpdateTableFormParam : TableForm { 
       
    }




}
