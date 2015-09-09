IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('TSTB_MSAT_ACTIVITY_INFO') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE TSTB_MSAT_ACTIVITY_INFO
;

CREATE TABLE TSTB_MSAT_ACTIVITY_INFO ( 
	RECORD_ID varchar(50) NOT NULL,    -- 活动ID 
	ACTIVITY_SN varchar(50),    -- 活动编号 
	PUBLISH_STATUS int,    -- 发布状态：1=草稿，2=修改未发布，3=发布中，4=已发布，9=发布失败 
	PUBLISH_TIME datetime,    -- 发布时间 
	PUSH_STATUS int,    -- 推送状态：0=未推送,1=推送中,2=已推送 
	PUSH_TIME datetime,    -- 推送时间 
	IS_CANCEL bit DEFAULT 0,    -- 是否撤消:1=已撤销，0=未撤销 
	AD_IMPROT_TYPE varchar(50),    -- 活动类型（获取名单的方式:ACTI培训,营销系列培训（原三大）,公开会议） 
	ACTIVITY_CONTENT nvarchar(4000),    -- 活动内容 
	ACTIVITY_PHOTO_URL nvarchar(400),    -- 卡片图片URL 
	HALL_NAME nvarchar(100),    -- 会场名称 
	HALL_ADDRESS nvarchar(1000),    -- 会场地址 
	HALL_PHOTO_URL nvarchar(400),    -- 会场图片URL 
	ACTIVITY_PREVIEW_URL nvarchar(400),    -- 预览路径 
	CREATE_DATE datetime,
	CREATOR varchar(50),
	UPDATE_DATE datetime,
	UPDATOR varchar(50),
	STATUS int,
	NOTIFICATION_CNT int DEFAULT 0 NOT NULL,    -- 活动信息推送次数 
	FIRST_PREVIEW_TIME datetime    -- 第一次预览时间 
)
;

ALTER TABLE TSTB_MSAT_ACTIVITY_INFO ADD CONSTRAINT PK_TSTB_MSAT_ACTIVITY_INFO 
	PRIMARY KEY CLUSTERED (RECORD_ID)
;

EXEC sp_addextendedproperty 'MS_Description', '手机助手活动信息', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO
;
EXEC sp_addextendedproperty 'MS_Description', '活动ID', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', RECORD_ID
;

EXEC sp_addextendedproperty 'MS_Description', '活动编号', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_SN
;

EXEC sp_addextendedproperty 'MS_Description', '发布状态：1=草稿，2=修改未发布，3=发布中，4=已发布，9=发布失败', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUBLISH_STATUS
;

EXEC sp_addextendedproperty 'MS_Description', '发布时间', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUBLISH_TIME
;

EXEC sp_addextendedproperty 'MS_Description', '推送状态：0=未推送,1=推送中,2=已推送', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUSH_STATUS
;

EXEC sp_addextendedproperty 'MS_Description', '推送时间', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUSH_TIME
;

EXEC sp_addextendedproperty 'MS_Description', '是否撤消:1=已撤销，0=未撤销', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', IS_CANCEL
;

EXEC sp_addextendedproperty 'MS_Description', '活动类型（获取名单的方式:ACTI培训,营销系列培训（原三大）,公开会议）', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', AD_IMPROT_TYPE
;

EXEC sp_addextendedproperty 'MS_Description', '活动内容', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_CONTENT
;

EXEC sp_addextendedproperty 'MS_Description', '卡片图片URL', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_PHOTO_URL
;

EXEC sp_addextendedproperty 'MS_Description', '会场名称', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', HALL_NAME
;

EXEC sp_addextendedproperty 'MS_Description', '会场地址', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', HALL_ADDRESS
;

EXEC sp_addextendedproperty 'MS_Description', '会场图片URL', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', HALL_PHOTO_URL
;

EXEC sp_addextendedproperty 'MS_Description', '预览路径', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_PREVIEW_URL
;






EXEC sp_addextendedproperty 'MS_Description', '活动信息推送次数', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', NOTIFICATION_CNT
;

EXEC sp_addextendedproperty 'MS_Description', '第一次预览时间', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', FIRST_PREVIEW_TIME
;

