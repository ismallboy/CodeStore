USE [AmwayFrameworkWorkFlow]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_MSAD_SyncAttendeeToCtrip]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_MSAD_SyncAttendeeToCtrip]
GO

USE [AmwayFrameworkWorkFlow]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		����
-- Create date: 2015/1/19
-- Description:	���������ڵ�����������ӵ�Я�̿���ӳ���
-- =============================================
CREATE PROCEDURE [dbo].[SP_MSAD_SyncAttendeeToCtrip]
AS
    BEGIN
        --���ҵ�ǰ����Я�̿���
        DECLARE @MaxSN INT       
        SELECT  @MaxSN = ISNULL(MAX([MAPPING_SN]), 100000)
        FROM    [AmwayFrameworkWorkflow].[dbo].[TSTB_MSAD_FLIGHT_MAPPING];    
            
        WITH    CET
                  AS ( SELECT   ROW_NUMBER() OVER ( PARTITION BY [DSTNUM],
                                                    [DSTNAM] ORDER BY [ID] ) AS RN ,
                                [ID] ,
                                ROW_NUMBER() OVER ( ORDER BY [ID] DESC )
                                + @MaxSN AS [MAPPING_SN] ,
                                [DSTNUM] ,
                                [DSTNAM] ,
                                [TYPE] ,
                                [SEND_RESULT] ,
                                [STATUS] ,
                                [CREATOR] ,
                                [CREATE_DATE] ,
                                [UPDATOR] ,
                                [UPDATE_DATE]
                       FROM     ( SELECT    NEWID() AS ID ,
                                            [DSTNUM] ,
                                            [DSTNAM] ,
                                            0 AS [TYPE] ,
                                            0 AS [SEND_RESULT] ,
                                            1 AS [STATUS] ,
                                            'CNMeetingApp' AS [CREATOR] ,
                                            GETDATE() AS [CREATE_DATE] ,
                                            'CNMeetingApp' AS [UPDATOR] ,
                                            GETDATE() AS [UPDATE_DATE]
                                  FROM      [AmwayFrameworkWorkflow].[dbo].[TSTB_MSAD_ATTENDEES_INFO]
                                  UNION ALL
                                  SELECT    NEWID() AS ID ,
                                            CASE WHEN ISNULL([ADA_CARD], '') = ''
                                                 THEN [GUEST_ID]
                                                 ELSE [ADA_CARD]
                                            END AS [DSTNUM] , --����а��������ð������Ű�Я�̿���
                                            [GUEST_NAME] AS [DSTNAM] ,
                                            1 AS [TYPE] ,
                                            0 AS [SEND_RESULT] ,
                                            1 AS [STATUS] ,
                                            'CNMeetingApp' AS [CREATOR] ,
                                            GETDATE() AS [CREATE_DATE] ,
                                            'CNMeetingApp' AS [UPDATOR] ,
                                            GETDATE() AS [UPDATE_DATE]
                                  FROM      AmwayFrameworkWorkFlow.dbo.TSTB_MSGM_GUEST_MEETING_REAL_GUEST_LIST
                                  WHERE     STATUS = 1
                                ) AS A
                       WHERE    A.DSTNAM IS NOT NULL
                                AND A.DSTNAM <> ''
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   [AmwayFrameworkWorkflow].[dbo].[TSTB_MSAD_FLIGHT_MAPPING]
                                                 WHERE  DSTNUM = A.DSTNUM
                                                        AND [DSTNAM] = A.[DSTNAM] )
                     )
            -- ���������ڵ�������α�������ӵ�Я�̿���ӳ���
        INSERT  INTO [AmwayFrameworkWorkflow].[dbo].[TSTB_MSAD_FLIGHT_MAPPING]
                ( [ID] ,
                  [MAPPING_SN] ,
                  [DSTNUM] ,
                  [DSTNAM] ,
                  [TYPE] ,
                  [SEND_RESULT] ,
                  [STATUS] ,
                  [CREATOR] ,
                  [CREATE_DATE] ,
                  [UPDATOR] ,
                  [UPDATE_DATE]
                )
                SELECT  [ID] ,
                        [MAPPING_SN] ,
                        [DSTNUM] ,
                        [DSTNAM] ,
                        [TYPE] ,
                        [SEND_RESULT] ,
                        [STATUS] ,
                        [CREATOR] ,
                        [CREATE_DATE] ,
                        [UPDATOR] ,
                        [UPDATE_DATE]
                FROM    CET
                WHERE   CET.RN = 1
    END



GO

--MSAD
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fun_MSAD_GetTripArrange]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fun_MSAD_GetTripArrange]
GO

USE [AmwayFrameworkWorkFlow]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,> ��ѯ�α���ƱԤ����ͨ����
-- =============================================
CREATE FUNCTION [dbo].[fun_MSAD_GetTripArrange]
    (
      @RECORD_ID VARCHAR(100) ,
      @DSTNUM VARCHAR(100) ,
      @DSTNAM VARCHAR(100) ,
      @TRIP_TYPE INT
    )
RETURNS VARCHAR(100)
AS
    BEGIN
        DECLARE @RESULT VARCHAR(50);
        DECLARE @RESULT_DEFAULT VARCHAR(50)= '��˾����';--Ĭ�Ϸ���ֵ
        
        --ע���α���������������У����α��������Ա����ͬʱ���ڣ���������ȡ��Ʊ��Ϣ��
        
        --���̰���
        IF @TRIP_TYPE = 1
            BEGIN
        --��ѯ��Ʊ��ͨ��Ϣ
                SELECT  @RESULT = F.RETURN_TRIP_ARRANGE
                FROM    dbo.TSTB_MSAD_FLIGHT_INFO F
                WHERE   F.DSTNUM = @DSTNUM
                        AND F.DSTNAM = @DSTNAM
                        AND F.MEETINFO_RECORD_ID = @RECORD_ID
                        AND F.ATTENDEES_TYPE = 1
                        AND F.STATUS = 1
                
        --�����Ʊ��ͨ��ϢΪ�գ���ȡ������Ľ�ͨ��Ϣ
                IF LEN(ISNULL(@RESULT, '')) = 0
                    SELECT  @RESULT = F.RETURN_TRIP_ARRANGE
                    FROM    dbo.TSTB_MSAD_ATTENDEES_INFO F
                    WHERE   F.DSTNUM = @DSTNUM
                            AND F.DSTNAM = @DSTNAM
                            AND F.MEETINFO_RECORD_ID = @RECORD_ID
                            AND F.STATUS = 1
                IF LEN(ISNULL(@RESULT, '')) = 0
                    SET @RESULT = @RESULT_DEFAULT     
                RETURN @RESULT
            END
        --ȥ�̰���            
        IF @TRIP_TYPE = 2
            BEGIN 
        --��ѯ��Ʊ��ͨ��Ϣ
                SELECT  @RESULT = F.UPCOMING_TRIP_ARRANGE
                FROM    dbo.TSTB_MSAD_FLIGHT_INFO F
                WHERE   F.DSTNUM = @DSTNUM
                        AND F.DSTNAM = @DSTNAM
                        AND F.MEETINFO_RECORD_ID = @RECORD_ID
                        AND F.ATTENDEES_TYPE = 1
                        AND F.STATUS = 1
                
        --�����Ʊ��ͨ��ϢΪ�գ���ȡ������Ľ�ͨ��Ϣ
                IF LEN(ISNULL(@RESULT, '')) = 0
                    SELECT  @RESULT = F.UPCOMING_TRIP_ARRANGE
                    FROM    dbo.TSTB_MSAD_ATTENDEES_INFO F
                    WHERE   F.DSTNUM = @DSTNUM
                            AND F.DSTNAM = @DSTNAM
                            AND F.MEETINFO_RECORD_ID = @RECORD_ID
                            AND F.STATUS = 1
                IF LEN(ISNULL(@RESULT, '')) = 0
                    SET @RESULT = @RESULT_DEFAULT
                RETURN @RESULT
            END
        RETURN @RESULT_DEFAULT
    END






GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FUN_MSAD_GetGuestTicketReservation]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[FUN_MSAD_GetGuestTicketReservation]
GO

USE [AmwayFrameworkWorkFlow]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,> ��ȡ�α���Ʊ��Ϣ
-- =============================================
CREATE FUNCTION [dbo].[FUN_MSAD_GetGuestTicketReservation]
    (
      @RECORD_ID NVARCHAR(100) --�ID
    )
RETURNS TABLE
AS
RETURN
    ( SELECT    BA.ID ,
                @RECORD_ID MeetinfoRecordID ,
                '' GroupDisplay ,
                GU.ADA_CARD Dstnum ,
                GU.GUEST_NAME Dstnam ,
                CASE WHEN BA.REPRESENT_ATIVE = GU.GUEST_NAME THEN 1
                     ELSE 0
                END LegalPerson ,
                '' LevelPosition ,
                BA.REGISTE_REGION_NAME Dsarea ,
                BA.REGISTE_REGION_CODE DsareaCode ,
                BA.REGISTE_PRV_NAME Dstprv ,
                BA.REGISTE_PRV_CODE DstprvCode ,
                REPLACE(BA.REGISTE_CITY_NAME, '��', '') Dstcty ,
                BA.REGISTE_CITY_CODE DstctyCode ,
                BA.SHOP_NAME Dstpsc ,
                BA.SHOP_CODE DstpscCode ,
                CASE WHEN BA.SEX = 0 THEN 'Ů'
                     ELSE '��'
                END Dstsex ,
                BA.IDENTITY_CARD Dstidn ,
                BA.TELPHONE PhoneNumber ,
                BA.EDUCATIONAL_BACKGROUND EducationBackground ,
                0 InvStatus ,
                DBO.fun_MSAD_GetTripArrange(gu.RECORD_ID,
                                            CASE WHEN LEN(ISNULL(GU.ADA_CARD,
                                                              '')) = 0
                                                 THEN GU.GUEST_ID
                                                 ELSE GU.ADA_CARD
                                            END, GU.GUEST_NAME, 2) UpcomingTripArrange ,
                DBO.fun_MSAD_GetTripArrange(gu.RECORD_ID,
                                            CASE WHEN LEN(ISNULL(GU.ADA_CARD,
                                                              '')) = 0
                                                 THEN GU.GUEST_ID
                                                 ELSE GU.ADA_CARD
                                            END, GU.GUEST_NAME, 1) ReturnTripArrange ,
                '1' IsGuest ,
                ( SELECT    F.CTRIP_ORDER_NUMBER
                  FROM      dbo.TSTB_MSAD_FLIGHT_INFO F
                  WHERE     F.ATTENDEES_INFO_ID = GU.GUEST_ID
                            AND F.MEETINFO_RECORD_ID = GU.RECORD_ID
                            AND F.STATUS = 1
                ) CtripOrderNumber ,
                ( SELECT    F.GFLIGHT_INFORMATION
                  FROM      dbo.TSTB_MSAD_FLIGHT_INFO F
                  WHERE     F.ATTENDEES_INFO_ID = GU.GUEST_ID
                            AND F.MEETINFO_RECORD_ID = GU.RECORD_ID
                            AND F.STATUS = 1
                ) GflightInformation ,
                ( SELECT    F.BFLIGHT_INFORMATION
                  FROM      dbo.TSTB_MSAD_FLIGHT_INFO F
                  WHERE     F.ATTENDEES_INFO_ID = GU.GUEST_ID
                            AND F.MEETINFO_RECORD_ID = GU.RECORD_ID
                            AND F.STATUS = 1
                ) BflightInformation ,
                ISNULL(( SELECT F.TICKET_STATUS
                         FROM   dbo.TSTB_MSAD_FLIGHT_INFO F
                         WHERE  F.ATTENDEES_INFO_ID = GU.GUEST_ID
                                AND F.MEETINFO_RECORD_ID = GU.RECORD_ID
                                AND F.STATUS = 1
                       ), '') TicketStatus ,
                CASE WHEN LEN(ISNULL(GU.ADA_CARD, '')) = 0 THEN GU.GUEST_ID
                     ELSE GU.ADA_CARD
                END TicketDstNum
      FROM      dbo.TSTB_MSGM_GUEST_MEETING_REAL_GUEST_LIST GU ,
                dbo.MSTB_MSGM_GUEST_BASE_INFO BA
      WHERE     GU.GUEST_ID = BA.ID
                AND BA.STATUS = 1
                AND GU.STATUS = 1
                AND GU.RECORD_ID = @RECORD_ID
    )

GO