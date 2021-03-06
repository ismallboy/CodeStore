USE [AmwayFrameworkWorkflow]
GO

DECLARE @adaCard VARCHAR(50), @jobGradeCode VARCHAR(50)
SET @adaCard='344703'
SET @jobGradeCode='G'
    DECLARE @BaseCount INT ,
        @UsedCount INT ,
        @BelongBeginYear DATETIME ,
        @BelongEndYear DATETIME 
    BEGIN
		SET @BaseCount = 0
		-- 年度的计算，每年的9-1到次年的8-31为个年度
        IF DATEPART(MONTH, GETDATE()) > 8 
            BEGIN
				SET @BelongBeginYear = CAST(CAST(DATEPART(Year, GETDATE()) AS VARCHAR) + '-09-01' AS DATETIME)
                SET @BelongEndYear = CAST(CAST(DATEPART(Year, GETDATE()) + 1 AS VARCHAR) + '-08-31' AS DATETIME)
            END 
        ELSE 
            BEGIN
                SET @BelongBeginYear = CAST(CAST(DATEPART(Year, GETDATE()) - 1 AS VARCHAR) + '-09-01' AS DATETIME)
                SET @BelongEndYear = CAST(CAST(DATEPART(Year, GETDATE()) AS VARCHAR) + '-08-31' AS DATETIME)
            END
            
PRINT @BelongBeginYear
PRINT @BelongEndYear


		-- 计划嘉宾的剩余场次（营销人员才有场次）
		-- 基本场次（或奖励场次）- 已使用- 已申请场次，不计算特殊及豁免场次
		-- 场次来源（1，基础场次，2：豁免场次，3：特殊场次, 4:非营销人员场次）
        SELECT  @BaseCount = TOTAL_COUNT
        FROM    MSTB_MSGM_SHOWS_REWARD_INFO
        WHERE   ADA_CARD = @adaCard AND STATUS = 1 
		
		-- 如果没有奖励场次则拿基本场次
        IF ISNULL(@BaseCount, 0) = 0 
            BEGIN		
                SELECT  @BaseCount = BASE_COUNT
                FROM    MSTB_MSGM_SHOWS_BASE_INFO
                WHERE   CHARINDEX(@jobGradeCode, JOB_GRADE_CODE) > 0 AND STATUS = 1
            END
		PRINT @BaseCount
		-- 只有基本场次>0才会计算消耗场次，否则剩余场次为0
        IF ISNULL(@BaseCount, 0) > 0 
            BEGIN
                SELECT  @UsedCount = SUM(SHOW_COUNT)
                FROM    View_MSGM_Meeting_Guest_List
                WHERE   ADA_CARD = @adaCard AND SHOW_FROM = 1 AND USE_STATUS != 4 
                AND DATEDIFF(DAY, MEETING_BEGIN_TIME, @BelongBeginYear) <= 0 AND DATEDIFF(DAY, MEETING_BEGIN_TIME, @BelongEndYear) >= 0
            END 
    END 
    SELECT @BaseCount - ISNULL(@UsedCount,0) 

