
USE GSSWEB
GO
SELECT ans.result AS Seq ,
			  ans.BookClass,
			  ans.BookID,
			  ans.BookName,
			  ans.Cnt
FROM (SELECT ROW_NUMBER() OVER (PARTITION BY bc.BOOK_CLASS_NAME ORDER BY COUNT(bd.BOOK_ID)DESC) AS result, 
			  bc.BOOK_CLASS_NAME AS BookClass ,
		      bd.BOOK_ID AS BookID, 
		      bd.BOOK_NAME AS BookName ,
		      COUNT(bd.BOOK_ID) AS Cnt
		FROM BOOK_DATA AS bd
		LEFT JOIN BOOK_CLASS AS bc
				    	ON bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID
		LEFT JOIN BOOK_LEND_RECORD AS blr
					    ON bd.BOOK_ID = blr.BOOK_ID 
		GROUP BY bc.BOOK_CLASS_NAME,
							 bd.BOOK_ID,
							 bd.BOOK_NAME
	) AS ans
WHERE ans.result<=3
ORDER BY BookClass, Cnt DESC,ans.BookID