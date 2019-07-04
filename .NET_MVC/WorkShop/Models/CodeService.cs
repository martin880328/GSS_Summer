using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WorkShop.Models
{
    public class CodeService
    {
        // 取得DB連線字串
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        //設定依書籍分類尋找的功能
        public List<SelectListItem> GetClassTable(string arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT BOOK_CLASS_ID AS BookClassID,
                                 BOOK_CLASS_NAME AS BookClassName
                          FROM BOOK_CLASS
                          ORDER BY BookClassName";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter SqlAdapter = new SqlDataAdapter(cmd);
                SqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapClassData(dt, arg);
        }
        //設定依書籍分類尋找的功能
        private List<SelectListItem> MapClassData(DataTable dt, string arg)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                if (row["BookClassID"].ToString() == arg)
                {
                    result.Add(new SelectListItem()
                    {
                        Text = row["BookClassName"].ToString(),
                        Value = row["BookClassID"].ToString(),
                        Selected = true
                    });
                }
                else
                {
                    result.Add(new SelectListItem()
                    {
                        Text = row["BookClassName"].ToString(),
                        Value = row["BookClassID"].ToString(),
                    });
                }
            }
            return result;
        }
        //設定依借閱者搜尋的功能
        public List<SelectListItem> GetKeeperSearch(string arg, int mode)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT [USER_ID] AS KeeperID,
                                 [USER_ENAME] as Keeper,
                                 [USER_CNAME] as KeeperC
                           FROM MEMBER_M";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter SqlAdapter = new SqlDataAdapter(cmd);
                SqlAdapter.Fill(dt);
                conn.Close();
            }
            if (mode == 0) return this.MapKeeperData(dt);
            else return this.MapKeeperData2(dt, arg);
        }
        //將拿到資料做排序
        private List<SelectListItem> MapKeeperData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["Keeper"].ToString(),
                    Value = row["KeeperID"].ToString()
                });
            }
            return result;
        }
        private List<SelectListItem> MapKeeperData2(DataTable dt, string arg)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                if (row["KeeperID"].ToString() == arg)
                {
                    result.Add(new SelectListItem()
                    {
                        Text = row["Keeper"].ToString() + '-' + row["KeeperC"].ToString(),
                        Value = row["KeeperID"].ToString(),
                        Selected = true
                    });
                }
                else
                {
                    result.Add(new SelectListItem()
                    {
                        Text = row["Keeper"].ToString() + '-' + row["KeeperC"].ToString(),
                        Value = row["KeeperID"].ToString(),
                    });
                }
            }
            return result;
        }
        //設定依借閱狀態搜尋的功能
        public List<SelectListItem> GetStatusTable(string arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT CODE_ID AS [BookStatusID],
                                  CODE_NAME AS [BookStatus]
                           FROM BOOK_CODE
                           WHERE CODE_TYPE = 'BOOK_STATUS'";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter SqlAdapter = new SqlDataAdapter(cmd);
                SqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapStatusData(dt, arg);
        }
        //將拿到的資料做排序
        private List<SelectListItem> MapStatusData(DataTable dt, string arg)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                if (row["BookStatusID"].ToString() == arg)
                {
                    result.Add(new SelectListItem()
                    {
                        Text = row["BookStatus"].ToString(),
                        Value = row["BookStatusID"].ToString(),
                        Selected = true
                    });
                }
                else
                {
                    result.Add(new SelectListItem()
                    {
                        Text = row["BookStatus"].ToString(),
                        Value = row["BookStatusID"].ToString(),
                    });
                }
            }
            return result;
        }
        // 取得書籍資料
        public List<Models.Book> GetBookData(Models.BookSearch arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  bcl.BOOK_CLASS_NAME AS [書籍類別],
			                       bd.BOOK_NAME AS [書名],
			                       FORMAT( bd.BOOK_BOUGHT_DATE, 'd', 'zh-TW') AS [購書日期],
			                       IsNull (mm.USER_ENAME ,'') AS [借閱人],
			                       bco.CODE_NAME AS [狀態] 
                           FROM BOOK_DATA AS bd 
                           LEFT JOIN  BOOK_LEND_RECORD AS  blr
		                            ON blr.BOOK_ID = bd.BOOK_ID
                           LEFT JOIN BOOK_CLASS AS bcl
		                            ON bd.BOOK_CLASS_ID = bcl.BOOK_CLASS_ID
                           LEFT JOIN BOOK_CODE AS bco
		                            ON  bd.BOOK_STATUS= bco.CODE_ID
                           LEFT JOIN MEMBER_M AS mm
		                            ON mm.USER_ID=blr.KEEPER_ID
                           WHERE NOT bco.CODE_TYPE_DESC='血型'+'%' AND
                                 bd.BOOK_NAME LIKE '%' +@BookName+'%' AND
                                 bd.BOOK_CLASS_ID LIKE'%'+ @BookClassID+'%' AND
                                 ISNULL(bd.BOOK_KEEPER,'') LIKE'%'+@Keeper_ID+'%' AND    
                                 bd.BOOK_STATUS LIKE'%' @BookStatusID+'%'
                           ORDER BY bd.BOOK_ID ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("BookName", arg.BookName == null ? string.Empty : arg.BookName));
                cmd.Parameters.Add(new SqlParameter("BookClassID", arg.BookClassID == null ? string.Empty : arg.BookClassID));
                cmd.Parameters.Add(new SqlParameter("KeeperID", arg.KeeperID == null ? string.Empty : arg.KeeperID));
                cmd.Parameters.Add(new SqlParameter("BookStatusID", arg.BookStatusID == null ? string.Empty : arg.BookStatusID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookData(dt);
        }
        //將拿到的書籍資料做排序
        private List<Models.Book> MapBookData(DataTable dt)
        {
            List<Models.Book> result = new List<Models.Book>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new Models.Book()
                {
                    BookClassID = row["BookClassID"].ToString(),
                    BookClassName = row["BookClassName"].ToString(),
                    BookID = (int)row["BookID"],
                    BookName = row["BookName"].ToString(),
                    KeeperID = row["KeeperID"].ToString(),
                    BookStatus = row["BookStatus"].ToString(),
                    BookStatusID = row["BookStatusID"].ToString(),
                    BookBuyDate = row["BookButDate"].ToString(),

                });
            }
            return result;
        }
        // 設定新增書籍的功能
        public bool InsertBookInfo(Models.Book arg)
        {
            if (arg.BookName == null || arg.BookAuthor == null || arg.BookPublisher == null || arg.BookIntro == null || arg.BookBuyDate == null || arg.BookClassID == null)
            {
                return false;
            }

            DataTable dt = new DataTable();
            string sql = @"INSERT INTO BOOK_DATA 
                                (
                                        BOOK_NAME, 
                                        BOOK_AUTHOR, 
                                        BOOK_PUBLISHER, 
                                        BOOK_NOTE, 
                                        BOOK_BOUGHT_DATE, 
                                        BOOK_CLASS_ID, 
                                        BOOK_STATUS
                             )
		                     VALUES (
                                        @BookName, 
                                        @BookAuthor, 
                                        @BookPublisher, 
                                        @BookIntro, 
                                        CONVERT(DATETIME, @BookBuyDate), 
                                        @BookClassID, 
                                        'A'
                             )";
            int BookID;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", arg.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", arg.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BookIntro", arg.BookIntro));
                cmd.Parameters.Add(new SqlParameter("@BookBuyDate", arg.BookBuyDate));
                cmd.Parameters.Add(new SqlParameter("@BookClassID", arg.BookClassID));
                BookID = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return true;
        }

        /// 設定刪除書籍的功能
        public void DeleteBook(int arg)
        {
            DataTable dt = new DataTable();
            string sql = @"DELETE BOOK_DATA WHERE BOOK_ID = @BookID";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookID", arg));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
        }
        public List<Models.Book> GetBookByCondtioin(Models.BookSearch arg)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT BD.BOOK_CLASS_ID AS BookClassID,
		                            BCL.BOOK_CLASS_NAME AS BookClassName,
		                            BD.BOOK_ID AS BookID,
		                            BD.BOOK_NAME AS BookName,
		                            ISNULL(BD.BOOK_KEEPER, '') AS KeeperID,
		                            ISNULL(MM.USER_ENAME, '') AS BookKeeper,
		                            BD.BOOK_STATUS AS BookStatusID,
		                            BCO.CODE_NAME AS BookStatus,
		                            FORMAT(BD.BOOK_BOUGHT_DATE, 'yyyy/MM/dd') AS BookBuyDate
                            FROM BOOK_DATA AS BD
                            INNER JOIN BOOK_CLASS BCL ON BD.BOOK_CLASS_ID = BCL.BOOK_CLASS_ID
                            INNER JOIN BOOK_CODE AS BCO ON BD.BOOK_STATUS = BCO.CODE_ID AND BCO.CODE_TYPE = 'BOOK_STATUS'
                            LEFT JOIN MEMBER_M AS MM ON BD.BOOK_KEEPER = MM.[USER_ID]
                            WHERE BD.BOOK_NAME LIKE '%'+@BookName+'%' AND
		                            BD.BOOK_CLASS_ID LIKE @BookClassId+'%' AND
		                            ISNULL(BD.BOOK_KEEPER, '') LIKE @KeeperID+'%' AND
		                            BD.BOOK_STATUS LIKE '%'+@BookStatusID+'%'
                            ORDER BY BookId";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName == null ? string.Empty : arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookClassID", arg.BookClassID == null ? string.Empty : arg.BookClassID));
                cmd.Parameters.Add(new SqlParameter("@KeeperID", arg.KeeperID == null ? string.Empty : arg.KeeperID));
                cmd.Parameters.Add(new SqlParameter("@BookStatusID", arg.BookStatusID == null ? string.Empty : arg.BookStatusID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }

        private List<Models.Book> MapBookDataToList(DataTable BookData)
        {
            List<Models.Book> result = new List<Book>();
            foreach (DataRow row in BookData.Rows)
            {
                result.Add(new Book()
                {
                    BookClassID = row["BookClassID"].ToString(),
                    BookClassName = row["BookClassName"].ToString(),
                    BookID = (int)row["BookID"],
                    BookName = row["BookName"].ToString(),
                    KeeperID = row["KeeperID"].ToString(),
                    BookKeeper = row["BookKeeper"].ToString(),
                    BookStatusID = row["BookStatusID"].ToString(),
                    BookStatus = row["BookStatus"].ToString(),
                    BookBuyDate = row["BookBuyDate"].ToString()
                });
            }
            return result;
        }


    }
}