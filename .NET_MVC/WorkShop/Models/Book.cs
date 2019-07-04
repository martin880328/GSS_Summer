using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkShop.Models
{
    public class Book 
    {
        [DisplayName("圖書類別代號")]
        public string BookClassID { get; set; }
        [DisplayName("圖書類別")]
        public string BookClassName { get; set; }
        [DisplayName("書名")]
        public string BookName { get; set; }
        [DisplayName("購書日期")]
        public string BookBuyDate { get; set; }
        [DisplayName("借閱狀況")]
        public string BookStatus { get; set; }
        [DisplayName("借閱狀況代號")]
        public string BookStatusID { get; set; }
        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }
        [DisplayName("書籍編號")]
        public int BookID { get; set; }
        [DisplayName("出版社")]
        public string BookPublisher { get; set; }
        [DisplayName("作者")]
        public string BookAuthor { get; set; }
        [DisplayName("借閱人")]
        public string KeeperID { get; set; }
        [DisplayName("書籍簡介")]
        public string BookIntro { get; set; }
    }

}