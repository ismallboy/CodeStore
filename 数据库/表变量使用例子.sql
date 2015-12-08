USE [AmwayFrameworkWorkFlow]
GO

/****** Object:  StoredProcedure [dbo].[SP_MS_ActivityInfoTotalReport]    Script Date: 12/03/2015 10:27:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Allen>
-- Create date: <2015��11��19��>
-- Description:	<���Ϣͳ�Ʊ���>
-- =============================================
create PROCEDURE [dbo].[SP_MS_ActivityInfoTotalReport] 
	@beginTime datetime,
	@endTime datetime,
	@activityRegion varchar(50),
	@activityPrv varchar(50),
	@activityCity varchar(50),
	@activityKind varchar(50),
	@meetingCategory varchar(50),
	@meetingDetailCode varchar(50)
AS
BEGIN

--��ʵ�ʲ�������
declare @REAL_JOIN_NUMBER table (RECORD_ID varchar(50), realJoinNumber int)
INSERT INTO @REAL_JOIN_NUMBER 
select actualJoinNumber.RECORD_ID, actualJoinNumber.realJoinNumber
	from  
	(
		--ʵ�����飺1 �ѵ������ճ�ϯ������ 2 δ�������ճ�ϯ����0
		SELECT attendeeInfo.MEETINFO_RECORD_ID AS RECORD_ID, ISNULL(SUM(CASE WHEN IS_ATTEND = '��' THEN 1 ELSE 0 END),0) AS realJoinNumber
		FROM TSTB_MSAD_MEETINFO AS msadMeetInfo
		left join TSTB_MS_APPLY_INFO as msApplyInfo on msApplyInfo.RECORD_ID = msadMeetInfo.RECORD_ID and msApplyInfo.STATUS = 1
		LEFT JOIN TSTB_MSAD_ATTENDEES_INFO AS attendeeInfo ON msadMeetInfo.RECORD_ID = attendeeInfo.MEETINFO_RECORD_ID AND msadMeetInfo.STATUS = 1
		WHERE msadMeetInfo.IS_REAL_NAME = 1 AND attendeeInfo.STATUS = 1 AND msApplyInfo.FIRST_END_DATE IS NOT NULL
		GROUP BY attendeeInfo.MEETINFO_RECORD_ID	
		UNION ALL
		--��ʵ�����飺1 δ����0��2�ѽ���ȡ�ʵ�ʱ��������First_End_Date��Ϊ�գ�
		SELECT a.RECORD_ID,CASE WHEN b.FIRST_END_DATE IS NULL THEN 0 ELSE c.ACTIVITY_JOIN_NUMBER END AS realJoinNumber
		FROM TSTB_MSAD_MEETINFO a
		INNER JOIN TSTB_MS_APPLY_INFO b  ON a.RECORD_ID = b.RECORD_ID
		INNER JOIN TSTB_MS_REAL_ACTIVITY_INFO c ON a.RECORD_ID = c.RECORD_ID
		WHERE (a.IS_REAL_NAME != 1 OR a.IS_REAL_NAME IS NULL) AND a.STATUS=1 AND b.STATUS=1 AND c.STATUS = 1
		
	) AS actualJoinNumber


--ȡ������ʱ�䷶Χ�ڵ�ÿ���ѽ����Ļ�Ļ��ϸ���˾�����
declare @DETAIL_CODE_AVG_BUDEGE table (MEETING_DETAIL_CODE varchar(50), FEE_AVG decimal(12,2))
INSERT INTO @DETAIL_CODE_AVG_BUDEGE 
SELECT realActivityInfo.MEETING_DETAIL_CODE,
		(CASE WHEN SUM(actualJoinNumber.realJoinNumber)=0 OR SUM(actualJoinNumber.realJoinNumber) IS NULL
		THEN 0.00
		ELSE ISNULL(CAST(SUM(feeInfo.EXPENSE_USED_AMOUNT)/SUM(actualJoinNumber.realJoinNumber) AS DECIMAL(12,2)),0.00)
		END) AS FEE_AVG
	FROM TSTB_MS_REAL_ACTIVITY_INFO AS realActivityInfo
	LEFT JOIN TSTB_MSBG_ACTUAL_FEE_INFO AS feeInfo ON feeInfo.ACTIVITY_RECORD_ID = realActivityInfo.RECORD_ID
		AND feeInfo.STATUS = 1
	LEFT JOIN dbo.TSTB_MS_APPLY_INFO AS msApplyInfo ON msApplyInfo.RECORD_ID = realActivityInfo.RECORD_ID AND msApplyInfo.STATUS = 1
	LEFT JOIN @REAL_JOIN_NUMBER AS actualJoinNumber ON actualJoinNumber.RECORD_ID = realActivityInfo.RECORD_ID
	WHERE (ISNULL(@beginTime,'') = '' OR realActivityInfo.START_TIME >= @beginTime) AND (ISNULL(@endTime,'') = '' OR realActivityInfo.START_TIME <= @endTime)
		AND msApplyInfo.FIRST_END_DATE IS NOT NULL AND realActivityInfo.STATUS = 1--ȡ�Ѿ������Ļ�Ľ�������
	GROUP BY  realActivityInfo.MEETING_DETAIL_CODE--, realActivityInfo.MEETING_CATEGORY_CODE,realActivityInfo.MEETING_KIND_CODE
	
	SELECT realActivity.RECORD_ID,
		realActivity.ACTIVITY_REGION_CODE,
		realActivity.ACTIVITY_REGION_NAME,
		realActivity.ACTIVITY_PRV_CODE,
		realActivity.ACTIVITY_PRV_NAME,
		realActivity.ACTIVITY_CITY_CODE,
		realActivity.ACTIVITY_CITY_NAME,
		realActivity.ACTIVITY_NAME,
		realactivity.ACTIVITY_SN,
		realactivity.MEETING_CATEGORY_CODE,
		realactivity.MEETING_CATEGORY_NAME,
		realactivity.MEETING_DETAIL_CODE,
		realactivity.MEETING_DETAIL_NAME,
		realActivity.MEETING_DETAIL_CHILD_CODE,
		realActivity.MEETING_DETAIL_CHILD_NAME,
		realActivity.BRAND_TYPE,
		BR.BRAND_CATEGORY_NAME AS BRAND_CATEGORY_NAME,
		--�Ŀ��
			 STUFF(( SELECT ',' + T2.PURPOSE_NAME
							  FROM dbo.TSTB_MS_PLAN_ACTIVITY_PURPOSE_REL t2
							  WHERE t2.RECORD_ID = realActivity.RECORD_ID
							  ORDER BY T2.CREATE_DATE ASC
							  FOR XML PATH('') ), 1, 1, '')
		AS ACTIVYTI_PURPOSE_NAME ,
		realactivity.START_TIME,
		realactivity.FINISH_TIME,
		realactivity.ACTIVITY_JOIN_NUMBER,
		feeinfo.EXPENSE_TOTAL_AMOUNT,
		feeinfo.EXPENSE_USED_AMOUNT,
		--�������������������Ϊnull
			CASE WHEN actualJoinNumber.realJoinNumber = 0 OR actualJoinNumber.realJoinNumber IS NULL
			THEN 0
			ELSE
				feeInfo.EXPENSE_USED_AMOUNT/actualJoinNumber.realJoinNumber
			END
	    AS ACTIVITY_PER_AVG, 
		itemRecord.APPLY_EMP_DEPT_CODE,
		itemRecord.APPLY_EMP_DEPT_NAME_CN,
		detailAVG.FEE_AVG
	FROM TSTB_MS_REAL_ACTIVITY_INFO AS realActivity
	LEFT JOIN TSTB_COM_MS_ITEM_RECORD AS itemRecord ON itemRecord.RECORD_ID = realActivity.RECORD_ID AND itemRecord.STATUS = 1
	LEFT JOIN TSTB_MSBG_ACTUAL_FEE_INFO AS feeInfo ON feeInfo.ACTIVITY_RECORD_ID = realActivity.RECORD_ID AND feeInfo.STATUS = 1
	LEFT JOIN @DETAIL_CODE_AVG_BUDEGE AS detailAVG ON detailAVG.MEETING_DETAIL_CODE = realActivity.MEETING_DETAIL_CODE
	LEFT JOIN dbo.TSTB_MS_APPLY_INFO AS applyInfo ON applyInfo.RECORD_ID = realActivity.RECORD_ID
	LEFT JOIN dbo.TSTB_MS_REAL_ACTIVITY_BRAND_REL BR ON BR.RECORD_ID = realActivity.RECORD_ID AND BR.STATUS = 1
	LEFT JOIN @REAL_JOIN_NUMBER AS actualJoinNumber ON actualJoinNumber.RECORD_ID = realActivity.RECORD_ID
	WHERE (ISNULL(@beginTime,'') = '' OR realActivity.START_TIME >= @beginTime) 
		AND (ISNULL(@endTime,'') = '' OR realActivity.START_TIME <= @endTime)
		AND (ISNULL(@activityRegion,'') = '' OR realActivity.ACTIVITY_REGION_CODE = @activityRegion)
		AND (ISNULL(@activityPrv,'') = '' OR realActivity.ACTIVITY_PRV_NAME LIKE '%' + @activityPrv + '%')
		AND (ISNULL(@activityCity,'') = '' OR realActivity.ACTIVITY_CITY_NAME LIKE '%' + @activityCity + '%')
		AND (ISNULL(@activityKind,'') = '' OR realActivity.MEETING_KIND_CODE = @activityKind)
		AND (ISNULL(@meetingCategory,'') = '' OR realActivity.MEETING_CATEGORY_CODE = @meetingCategory)
		AND (ISNULL(@meetingDetailCode,'') = '' OR realActivity.MEETING_DETAIL_CODE = @meetingDetailCode)

END





GO


