IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('TSTB_MSAT_ACTIVITY_INFO') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE TSTB_MSAT_ACTIVITY_INFO
;

CREATE TABLE TSTB_MSAT_ACTIVITY_INFO ( 
	RECORD_ID varchar(50) NOT NULL,    -- �ID 
	ACTIVITY_SN varchar(50),    -- ���� 
	PUBLISH_STATUS int,    -- ����״̬��1=�ݸ壬2=�޸�δ������3=�����У�4=�ѷ�����9=����ʧ�� 
	PUBLISH_TIME datetime,    -- ����ʱ�� 
	PUSH_STATUS int,    -- ����״̬��0=δ����,1=������,2=������ 
	PUSH_TIME datetime,    -- ����ʱ�� 
	IS_CANCEL bit DEFAULT 0,    -- �Ƿ���:1=�ѳ�����0=δ���� 
	AD_IMPROT_TYPE varchar(50),    -- ����ͣ���ȡ�����ķ�ʽ:ACTI��ѵ,Ӫ��ϵ����ѵ��ԭ����,�������飩 
	ACTIVITY_CONTENT nvarchar(4000),    -- ����� 
	ACTIVITY_PHOTO_URL nvarchar(400),    -- ��ƬͼƬURL 
	HALL_NAME nvarchar(100),    -- �᳡���� 
	HALL_ADDRESS nvarchar(1000),    -- �᳡��ַ 
	HALL_PHOTO_URL nvarchar(400),    -- �᳡ͼƬURL 
	ACTIVITY_PREVIEW_URL nvarchar(400),    -- Ԥ��·�� 
	CREATE_DATE datetime,
	CREATOR varchar(50),
	UPDATE_DATE datetime,
	UPDATOR varchar(50),
	STATUS int,
	NOTIFICATION_CNT int DEFAULT 0 NOT NULL,    -- ���Ϣ���ʹ��� 
	FIRST_PREVIEW_TIME datetime    -- ��һ��Ԥ��ʱ�� 
)
;

ALTER TABLE TSTB_MSAT_ACTIVITY_INFO ADD CONSTRAINT PK_TSTB_MSAT_ACTIVITY_INFO 
	PRIMARY KEY CLUSTERED (RECORD_ID)
;

EXEC sp_addextendedproperty 'MS_Description', '�ֻ����ֻ��Ϣ', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO
;
EXEC sp_addextendedproperty 'MS_Description', '�ID', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', RECORD_ID
;

EXEC sp_addextendedproperty 'MS_Description', '����', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_SN
;

EXEC sp_addextendedproperty 'MS_Description', '����״̬��1=�ݸ壬2=�޸�δ������3=�����У�4=�ѷ�����9=����ʧ��', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUBLISH_STATUS
;

EXEC sp_addextendedproperty 'MS_Description', '����ʱ��', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUBLISH_TIME
;

EXEC sp_addextendedproperty 'MS_Description', '����״̬��0=δ����,1=������,2=������', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUSH_STATUS
;

EXEC sp_addextendedproperty 'MS_Description', '����ʱ��', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', PUSH_TIME
;

EXEC sp_addextendedproperty 'MS_Description', '�Ƿ���:1=�ѳ�����0=δ����', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', IS_CANCEL
;

EXEC sp_addextendedproperty 'MS_Description', '����ͣ���ȡ�����ķ�ʽ:ACTI��ѵ,Ӫ��ϵ����ѵ��ԭ����,�������飩', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', AD_IMPROT_TYPE
;

EXEC sp_addextendedproperty 'MS_Description', '�����', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_CONTENT
;

EXEC sp_addextendedproperty 'MS_Description', '��ƬͼƬURL', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_PHOTO_URL
;

EXEC sp_addextendedproperty 'MS_Description', '�᳡����', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', HALL_NAME
;

EXEC sp_addextendedproperty 'MS_Description', '�᳡��ַ', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', HALL_ADDRESS
;

EXEC sp_addextendedproperty 'MS_Description', '�᳡ͼƬURL', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', HALL_PHOTO_URL
;

EXEC sp_addextendedproperty 'MS_Description', 'Ԥ��·��', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', ACTIVITY_PREVIEW_URL
;






EXEC sp_addextendedproperty 'MS_Description', '���Ϣ���ʹ���', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', NOTIFICATION_CNT
;

EXEC sp_addextendedproperty 'MS_Description', '��һ��Ԥ��ʱ��', 'Schema', dbo, 'table', TSTB_MSAT_ACTIVITY_INFO, 'column', FIRST_PREVIEW_TIME
;

