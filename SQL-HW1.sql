
USE GSSWEB
GO
SELECT KEEPER_ID AS KeeperId,
			   mm.USER_CNAME AS CName, 
			   mm.USER_ENAME AS EName,
			   YEAR(LEND_DATE) AS BorrowYear,
			   COUNT(*) AS BorrowCnt
FROM BOOK_LEND_RECORD AS blr
LEFT JOIN MEMBER_M AS mm
			   ON blr.KEEPER_ID=mm.[USER_ID]
GROUP BY YEAR(LEND_DATE),
					KEEPER_ID,
				    mm.USER_CNAME,
					mm.USER_ENAME
ORDER BY blr.KEEPER_ID



