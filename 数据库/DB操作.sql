--�޸��ֶ�����
Alter table MSTB_MR_LOG alter column OPTION_STATUS_AFTER int null

--�����ֶ�
USE [AmwayFrameworkWorkflow]
GO

/****** Object:  Table [dbo].[MSTB_MR_ROOM_INFO]    Script Date: 11/06/2014 20:19:43 ******/
IF NOT EXISTS ( SELECT  d.name ,
                    a.*
            FROM    sys.syscolumns a
                    INNER JOIN sysobjects d ON a.id = d.id
                                               AND d.xtype = 'U'
                                               AND d.name <> 'dtproperties'
            WHERE   a.name = 'REMARK'
                    AND d.name = 'MSTB_MR_ROOM_INFO' ) 
    ALTER TABLE MSTB_MR_ROOM_INFO ADD REMARK NVARCHAR(500) NULL
GO

--