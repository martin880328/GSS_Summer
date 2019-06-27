
USE GSSWEB
GO
SELECT ans.classid AS ClassID, 
	   ans.classname AS ClassName,
	   SUM(CASE ans.yyyy WHEN 2016 THEN ans.[year] ELSE 0 END) AS 'CNT2016',
	   SUM(CASE ans.yyyy WHEN 2017 THEN ans.[year] ELSE 0 END) AS 'CNT2017',
       SUM(CASE ans.yyyy WHEN 2018 THEN ans.[year] ELSE 0 END) AS 'CNT2018',
	   SUM(CASE ans.yyyy WHEN 2019 THEN ans.[year] ELSE 0 END) AS 'CNT2019'
FROM(
		SELECT bc.BOOK_CLASS_ID AS classid, 
			   bc.BOOK_CLASS_NAME AS classname, 
			   YEAR(blr.LEND_DATE) AS yyyy,
			   COUNT( YEAR(blr.LEND_DATE)) AS [year]
		FROM BOOK_LEND_RECORD AS blr
		LEFT JOIN BOOK_DATA AS bd 
				ON bd.BOOK_ID = blr.BOOK_ID 
		LEFT JOIN BOOK_CLASS AS bc
				ON bc.BOOK_CLASS_ID=bd.BOOK_CLASS_ID
		GROUP BY bc.BOOK_CLASS_ID, 
							 bc.BOOK_CLASS_NAME, 
							 YEAR(blr.LEND_DATE)
     )ans
GROUP BY ans.classid,ans.classname
ORDER BY ans.classid