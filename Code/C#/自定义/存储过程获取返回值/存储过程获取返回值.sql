USE [AmwayFrameworkWorkflow]
GO

/****** Object:  StoredProcedure [dbo].[SP_MSAT_WritePublishInterfaceTable]    Script Date: 07/13/2015 11:52:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









-- =============================================
-- Author:		Allen
-- Create date: 2015年7月10日
-- Description:	活动手机助手发布接口操作表，把数据写入为微信端提供数据的表
-- =============================================
CREATE PROCEDURE [dbo].[SP_MSAT_WritePublishInterfaceTable]
(
    @RECORD_ID VARCHAR(50),
    @outputValue int output
 )
AS
	BEGIN
		SET NOCOUNT on;
		
		SET XACT_ABORT ON;--出错则全部回滚操作
		BEGIN TRAN--设置事务
		
		delete TSTB_MSAT_EX_ACTIVITY_INFO
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_ACTIVITY_INFO(
			RECORD_ID,
			ACTIVITY_SN,
			ACTIVITY_NAME,
			START_TIME,
			FINISH_TIME,
			ACTIVITY_CITY_CODE,
			ACTIVITY_CITY_NAME,
			ACTIVITY_PHOTO_URL,
			ACTIVITY_CONTENT,
			HALL_NAME,
			HALL_ADDRESS,
			IS_OPEN_REGISTER,
			REGISTER_BIGIN_TIME,
			REGISTER_END_TIME,
			REGISTER_NOTICE,
			FIRST_PREVIEW_TIME,
			ACTIVITY_TYPE,
			ACTIVITY_SUBJECT,
			THEME_COLOR,
			PREVIEW_TIME,
			SETTING_START_TIME,
			SETTING_END_TIME,
			PUBLISH_START_TIME,
			PUBLISH_END_TIME,
			TICKET_DESCRIPTION,
			IS_REAL_NAME,
			IS_CHARGE,
			UPDATE_DATE
		)

		select realMeetInfo.RECORD_ID,
			realMeetInfo.ACTIVITY_SN,
			realMeetInfo.ACTIVITY_NAME,
			realMeetInfo.START_TIME,
			realMeetInfo.FINISH_TIME,
			realMeetInfo.ACTIVITY_CITY_CODE,
			realMeetInfo.ACTIVITY_CITY_NAME,
			msatMeetInfo.ACTIVITY_PHOTO_URL,
			msatMeetInfo.ACTIVITY_CONTENT,
			msatMeetInfo.HALL_NAME,
			msatMeetInfo.HALL_ADDRESS,
			msatRegSet.IS_OPEN_REGISTER,
			msatRegSet.REGISTER_BEGIN_TIME,
			msatRegSet.REGISTER_END_TIME,
			msatRegSet.REGISTER_NOTICE,
			msatMeetInfo.FIRST_PREVIEW_TIME,
			msatMeetInfo.ACTIVITY_TYPE,
			msatMeetInfo.ACTIVITY_SUBJECT,
			msatMeetInfo.THEME_COLOR,
			msatMeetInfo.PREVIEW_TIME,
			msadMeetInfo.SETTING_START_TIME,
			msadMeetInfo.SETTING_END_TIME,
			msadMeetInfo.PUBLISH_START_TIME,
			msadMeetInfo.PUBLISH_END_TIME,
			msatMeetInfo.TICKET_DESCRIPTION,
			ISNULL(msadMeetInfo.IS_REAL_NAME,0) AS IS_REAL_NAME,
			ISNULL(msadMeetInfo.IS_CHARGE,0) AS IS_CHARGE,
			GETDATE()
		from TSTB_MS_REAL_ACTIVITY_INFO as realMeetInfo
			join TSTB_MSAT_ACTIVITY_INFO as msatMeetInfo on msatMeetInfo.RECORD_ID = realMeetInfo.RECORD_ID
			left join TSTB_MSAT_REGISTER_SETTING msatRegSet on msatRegSet.RECORD_ID = realMeetInfo.RECORD_ID
			left join TSTB_MSAD_MEETINFO as msadMeetInfo on msadMeetInfo.RECORD_ID = realMeetInfo.RECORD_ID
		where  realMeetInfo.RECORD_ID = @RECORD_ID and realMeetInfo.STATUS = 1

	if @@ERROR <> 0--如果出错
		begin
			rollback tran--全部回滚
			set @outputValue = 0
		end
	else
		begin
			COMMIT TRAN--则全部提交
			set @outputValue = 1
		end
	END









GO


