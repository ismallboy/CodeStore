USE [AmwayFrameworkWorkflow]
GO

/****** Object:  StoredProcedure [dbo].[SP_MSAT_WritePublishInterfaceTable]    Script Date: 07/10/2015 15:03:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_MSAT_WritePublishInterfaceTable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_MSAT_WritePublishInterfaceTable]
GO

USE [AmwayFrameworkWorkflow]
GO

/****** Object:  StoredProcedure [dbo].[SP_MSAT_WritePublishInterfaceTable]    Script Date: 07/10/2015 15:03:39 ******/
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
    @RECORD_ID VARCHAR(50)
AS
	BEGIN
		SET NOCOUNT ON;
		
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

		delete TSTB_MSAT_EX_ACTIVITY_GUEST
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_ACTIVITY_GUEST(
			ID,
			RECORD_ID,
			GUEST_ID,
			GUEST_NAME,
			PHOTO_URL,
			JOB_GRADE_CODE,
			JOB_GRADE_NAME,
			COURSE_ID,
			COURSE_NAME,
			GUEST_DESC,
			UPDATE_DATE
		)

		--declare @RECORD_ID varchar(50)
		--set @RECORD_ID = 'fd592a6e-2997-4438-a359-f541aafb9b15'
		select  ID,
			RECORD_ID,
			GUEST_ID,
			GUEST_NAME,
			PHOTO_URL,
			JOB_GRADE_CODE,
			JOB_GRADE_NAME,
			COURSE_ID,
			COURSE_NAME,
			GUEST_DESC,
			GETDATE()
		from TSTB_MSAT_ACTIVITY_GUEST
		where RECORD_ID = @RECORD_ID and STATUS = 1

		delete TSTB_MSAT_EX_ACTIVITY_SCHEDULE
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_ACTIVITY_SCHEDULE(
			ID,
			RECORD_ID,
			ACTIVITY_DATE,
			BEGIN_TIME,
			END_TIME,
			SPEAKER,
			CONTENT,
			UPDATE_DATE
		)
		--declare @RECORD_ID varchar(50)
		--set @RECORD_ID = 'fd592a6e-2997-4438-a359-f541aafb9b15'
		select  ID,
			RECORD_ID,
			ACTIVITY_DATE,
			BEGIN_TIME,
			END_TIME,
			SPEAKER,
			CONTENT,
			GETDATE()
		from TSTB_MSAT_ACTIVITY_SCHEDULE
		where  RECORD_ID = @RECORD_ID and STATUS = 1

		delete TSTB_MSAT_EX_TICKET_CATEGORY
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_TICKET_CATEGORY(
			ID,
			RECORD_ID,
			TICKET_CATEGORY,
			PRICE,
			QUANTITY,
			REMARK,
			SORT,
			UPDATE_DATE
		)
		--declare @RECORD_ID varchar(50)
		--set @RECORD_ID = '494f66ef-b611-4c50-b1c4-53e87d0d9d2c'
		select  ID,
			RECORD_ID,
			TICKET_CATEGORY,
			PRICE,
			QUANTITY,
			REMARK,
			SORT,
			GETDATE()
		from TSTB_MSET_TICKET_CATEGORY
		where  RECORD_ID = @RECORD_ID

		delete TSTB_MSAT_EX_ATTACHMENT
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_ATTACHMENT(
			ID,
			RECORD_ID,
			CATEGORY,
			URL,
			DISPLAY_NAME,
			EXTEND_NAME,
			FILE_SIZE,
			REMARK,
			UPDATE_DATE
		)
		--declare @RECORD_ID varchar(50)
		--set @RECORD_ID = '494f66ef-b611-4c50-b1c4-53e87d0d9d2c'
		select  ID,
			RECORD_ID,
			CATEGORY,
			URL,
			DISPLAY_NAME,
			EXTEND_NAME,
			FILE_SIZE,
			REMARK,
			GETDATE()
		from TSTB_MSAT_ATTACHMENT
		where  RECORD_ID = @RECORD_ID and STATUS = 1

		delete TSTB_MSAT_EX_CONTACT_WAY
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_CONTACT_WAY(
			ID,
			RECORD_ID,
			NAME,
			PHONE,
			UPDATE_DATE
		)
		--declare @RECORD_ID varchar(50)
		--set @RECORD_ID = '494f66ef-b611-4c50-b1c4-53e87d0d9d2c'
		select  ID,
			RECORD_ID,
			NAME,
			PHONE,
			GETDATE()
		from TSTB_MSAT_CONTACT_WAY
		where  RECORD_ID = @RECORD_ID and STATUS = 1

		delete TSTB_MSAT_EX_KINDLY_REMINDER
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_KINDLY_REMINDER(
			ID,
			RECORD_ID,
			TITLE,
			CONTENT,
			UPDATE_DATE
		)
		--declare @RECORD_ID varchar(50)
		--set @RECORD_ID = '494f66ef-b611-4c50-b1c4-53e87d0d9d2c'
		select  ID,
			RECORD_ID,
			TITLE,
			CONTENT,
			GETDATE()
		from TSTB_MSAT_KINDLY_REMINDER
		where  RECORD_ID = @RECORD_ID and STATUS = 1

		delete TSTB_MSAT_EX_REGISTER_SETTING_ITEM
		where RECORD_ID = @RECORD_ID

		insert into TSTB_MSAT_EX_REGISTER_SETTING_ITEM(
			ID,
			RECORD_ID,
			CODE,
			NAME,
			DISPLAY_ORDER,
			UPDATE_DATE,
			STATUS
		)
		--declare @RECORD_ID varchar(50)
		--set @RECORD_ID = '494f66ef-b611-4c50-b1c4-53e87d0d9d2c'
		select  ID,
			RECORD_ID,
			CODE,
			NAME,
			DISPLAY_ORDER,
			GETDATE(),
			STATUS
		from TSTB_MSAT_REGISTER_SETTING_ITEM
		where  RECORD_ID = @RECORD_ID
	if @@ERROR <> 0--如果出错
		begin
			rollback tran--全部回滚
			return 0
		end
	else
		begin
			COMMIT TRAN--则全部提交
			return 1
		end
	END



GO


