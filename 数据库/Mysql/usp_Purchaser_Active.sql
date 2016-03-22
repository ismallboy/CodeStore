CREATE PROCEDURE usp_Purchaser_Active(in beginDate datetime, in endDate datetime)
BEGIN
		set beginDate = DATE(beginDate);
		set endDate = DATE(endDate);
		while beginDate <= endDate DO
			-- 如果当天一下三个条件其中一条成立，则该采购商属于当天活跃，往表里插入该条数据
			IF EXISTS (SELECT 1 FROM stat_purchaser_active WHERE date(ActiveTime) = beginDate LIMIT 1) THEN
				DELETE 
				from stat_purchaser_active
				where date(ActiveTime) = beginDate;
			END IF;
			INSERT into stat_purchaser_active(Id,PurchaserId,ActiveTime)
			SELECT REPLACE(UUID(),'-','') as Id,T.CompanyId,beginDate
			FROM
			(
				SELECT DISTINCT pi.CompanyId
				FROM log_product lp 
				JOIN user_info u ON lp.AccountName = u.UserName
				JOIN  purchaser_info as pi on pi.CompanyId = u.CompanyId and pi.CompanyState = 0
				WHERE  lp.UserType=1 and DATE(lp.CreateDate)=beginDate-- '2015-08-26';

				UNION

				SELECT DISTINCT pi.CompanyId-- DATE(ls.CreateDate) 
				FROM log_supplier ls 
				JOIN user_info u ON ls.AccountName=u.UserName 
				JOIN  purchaser_info as pi on pi.CompanyId = u.CompanyId and pi.CompanyState = 0
				WHERE ls.UserType=1 and DATE(ls.CreateDate)=beginDate-- > '2015-08-23'

				UNION

				SELECT DISTINCT pi.CompanyId-- DATE(lpu.CreateDate) 
				FROM log_purchaser lpu
				JOIN  purchaser_info as pi on pi.CompanyId = lpu.CompanyId and pi.CompanyState = 0
				WHERE lpu.LogType =6 AND lpu.CompanyId = pi.CompanyId and DATE(lpu.CreateDate)=beginDate-- > '2015-08-23'
			) as T;
		set beginDate = DATE_ADD(beginDate,INTERVAL 1 DAY);
		end while;	
END;

