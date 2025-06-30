using BearPlatform.Common.Enums;
using BearPlatform.Entity;
namespace BearPlatform.Models
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
