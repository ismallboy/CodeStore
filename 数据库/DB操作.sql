--修改字段类型
Alter table MSTB_MR_LOG alter column OPTION_STATUS_AFTER int null

--增加字段
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
    
	EXEC sp_addextendedproperty 'MS_Description', '备注', 'Schema', dbo, 'table', MSTB_MR_ROOM_INFO, 'column', REMARK
GO

--更改主键字段类型
alter table Stuinfo alter column Id nchar(36) not null;
alter table Stuinfo add constraint PK_StuInfo primary key clustered(Id);

--