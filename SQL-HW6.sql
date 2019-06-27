
USE GSSWEB
GO
SELECT pt.classid AS ClassID,
			  pt.classname AS ClassName,
			  CASE WHEN pt.[2016] IS NULL THEN '0' ELSE pt.[2016] END AS 'CNT2016',
			  CASE WHEN pt.[2017] IS NULL THEN '0' ELSE pt.[2017] END AS 'CNT2017',
			  CASE WHEN pt.[2018] IS NULL THEN '0' ELSE pt.[2018] END AS 'CNT2018',
			  CASE WHEN pt.[2019] IS NULL THEN '0' ELSE pt.[2019] END AS 'CNT2019'
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
				GROUP BY bc.BOOK_CLASS_ID, bc.BOOK_CLASS_NAME, YEAR(blr.LEND_DATE)
			) AS ans
PIVOT(
				SUM(ans.[year])
				FOR ans.yyyy IN ([2016],[2017],[2018],[2019])
     ) AS pt
ORDER BY pt.classid