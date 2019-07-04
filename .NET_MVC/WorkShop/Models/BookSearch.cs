using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkShop.Models
{
    public class BookSearch
    {
        [DisplayName("圖書類別")]
        public string BookClassID { get; set; }
        [DisplayName("書名")]
        public string BookName { get; set; }
        [DisplayName("購書日期")]
        public string BookBuyDate { get; set; }
        [DisplayName("借閱狀況代號")]
        public string BookStatusID { get; set; }
        [DisplayName("借閱人")]
        public string KeeperID { get; set; }
    }

}