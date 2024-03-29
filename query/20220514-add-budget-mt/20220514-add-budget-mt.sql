USE [BudgetControl]
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_UpdateRemainAmount]    Script Date: 14/5/2565 11:20:57 ******/
DROP PROCEDURE [dbo].[sp_BudgetMT_UpdateRemainAmount]
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Update]    Script Date: 14/5/2565 11:20:57 ******/
DROP PROCEDURE [dbo].[sp_BudgetMT_Transaction_Update]
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Insert]    Script Date: 14/5/2565 11:20:57 ******/
DROP PROCEDURE [dbo].[sp_BudgetMT_Transaction_Insert]
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_GetById]    Script Date: 14/5/2565 11:20:57 ******/
DROP PROCEDURE [dbo].[sp_BudgetMT_Transaction_GetById]
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Get]    Script Date: 14/5/2565 11:20:57 ******/
DROP PROCEDURE [dbo].[sp_BudgetMT_Transaction_Get]
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Delete]    Script Date: 14/5/2565 11:20:57 ******/
DROP PROCEDURE [dbo].[sp_BudgetMT_Transaction_Delete]
GO
ALTER TABLE [dbo].[BudgetMTTransaction] DROP CONSTRAINT [DF_BudgetMTTransaction_Id]
GO
/****** Object:  Table [dbo].[BudgetMTTransaction]    Script Date: 14/5/2565 11:20:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BudgetMTTransaction]') AND type in (N'U'))
DROP TABLE [dbo].[BudgetMTTransaction]
GO
/****** Object:  Table [dbo].[BudgetMT]    Script Date: 14/5/2565 11:20:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BudgetMT]') AND type in (N'U'))
DROP TABLE [dbo].[BudgetMT]
GO
/****** Object:  Table [dbo].[BudgetMT]    Script Date: 14/5/2565 11:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BudgetMT](
	[BudgetID] [uniqueidentifier] NOT NULL,
	[AccountID] [nvarchar](10) NULL,
	[CostCenterID] [nvarchar](10) NULL,
	[Sequence] [int] NOT NULL,
	[Year] [nvarchar](4) NULL,
	[BudgetAmount] [decimal](18, 2) NOT NULL,
	[WithdrawAmount] [decimal](18, 2) NOT NULL,
	[RemainAmount] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedBy] [nvarchar](128) NULL,
	[CreatedAt] [datetime] NULL,
	[ModifiedBy] [nvarchar](128) NULL,
	[ModifiedAt] [datetime] NULL,
	[DeletedBy] [nvarchar](128) NULL,
	[DeletedAt] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BudgetMTTransaction]    Script Date: 14/5/2565 11:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BudgetMTTransaction](
	[Id] [uniqueidentifier] NOT NULL,
	[BudgetId] [uniqueidentifier] NOT NULL,
	[Year] [varchar](4) NULL,
	[MTNo] [varchar](50) NULL,
	[Title] [varchar](500) NULL,
	[OwnerDepartment] [varchar](500) NULL,
	[OwnerCostCenter] [varchar](20) NULL,
	[Participant] [varchar](500) NULL,
	[SeminarDate] [date] NULL,
	[Location] [varchar](500) NULL,
	[ParticipantCount] [int] NULL,
	[HasMealMorning] [bit] NULL,
	[HasMealAfternoon] [bit] NULL,
	[Remark] [varchar](5000) NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[RemainAmount] [decimal](18, 2) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_BudgetMTTransaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[BudgetMT] ([BudgetID], [AccountID], [CostCenterID], [Sequence], [Year], [BudgetAmount], [WithdrawAmount], [RemainAmount], [Status], [CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], [DeletedBy], [DeletedAt]) VALUES (N'd95ccbd9-c8d5-4615-8a81-4a1f304cdde3', N'52030030', N'H301000040', 0, N'2565', CAST(3290000.00 AS Decimal(18, 2)), CAST(24500.00 AS Decimal(18, 2)), CAST(3265500.00 AS Decimal(18, 2)), 1, N'admin', CAST(N'2022-01-04T09:13:53.897' AS DateTime), N'admin', CAST(N'2022-02-21T11:42:33.107' AS DateTime), NULL, NULL)
GO
ALTER TABLE [dbo].[BudgetMTTransaction] ADD  CONSTRAINT [DF_BudgetMTTransaction_Id]  DEFAULT (newid()) FOR [Id]
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Delete]    Script Date: 14/5/2565 11:20:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BudgetMT_Transaction_Delete]
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier ,
	@DeleteBy varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @BudgetId uniqueidentifier

	SELECT TOp(1) @BudgetId = BudgetId from [BudgetMTTransaction] WHERE @Id = @Id
	
    -- Insert statements for procedure here
	UPDATE [dbo].[BudgetMTTransaction]
		SET DeletedBy = @DeleteBy
			, DeletedDate = GETDATE()
	WHERE Id = @Id

	IF @@ROWCOUNT < 1
		RAISERROR('Record does not exist', 16, 1)

	-- Update Budget Amount
	EXEC [dbo].[sp_BudgetMT_UpdateRemainAmount]
		@BudgetId = @BudgetId

	SELECT TOP(1) * FROM [dbo].[BudgetMTTransaction] WHERE [Id] = @Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Get]    Script Date: 14/5/2565 11:20:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BudgetMT_Transaction_Get]
	-- Add the parameters for the stored procedure here
	@Year varchar(10),
	@CostCenter varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @BudetId uniqueidentifier 
	SELECT @BudetId = BudgetId FROM BudgetMT WHERE Year = @Year

	;WITH cte AS (
	SELECT  d.MTNo
			--, h.BudgetAmount
			, SUM(d2.TotalAMount) - MIN(d.TotalAmount) AS PreviousAmount
		  FROM [dbo].[BudgetMTTransaction] d 
			LEFT OUTER JOIN BudgetMT h ON d.BudgetId = h.BudgetId
			CROSS JOIN [BudgetMTTransaction] d2 
		WHERE  d2.MTNo <= d.MTNO 
			AND d.DeletedDate IS NULL
			AND d2.DeletedDate IS NULL
			AND d.BudgetId = @BudetId
			AND d.BudgetId = d2.BudgetId
		GROUP BY d.MTNO
	)
	SELECT  h.[Id]
      ,h.[BudgetId]
      ,h.[Year]
      ,h.[MTNo]
      ,h.[Title]
      ,h.[OwnerDepartment]
	  ,h.[OwnerCostCenter]
      ,h.[Participant]
      ,h.[SeminarDate]
      ,h.[Location]
      ,h.[ParticipantCount]
      ,h.[HasMealMorning]
      ,h.[HasMealAfternoon]
      ,h.[Remark]
	  , BudgetMT.BudgetAmount
      ,h.[TotalAmount]
      ,BudgetMT.BudgetAmount - cte.PreviousAmount - h.TotalAmount AS RemainAmount --h.[RemainAmount]
	  --,cte.PreviousAmount
      ,h.[CreatedBy]
      ,h.[CreatedDate]
      ,h.[ModifiedBy]
      ,h.[ModifiedDate]
      ,h.[DeletedBy]
      ,h.[DeletedDate]
  FROM [dbo].[BudgetMTTransaction] h 
	LEFT OUTER JOIN [dbo].[BudgetMT] ON h.BudgetId = BudgetMT.BudgetId
	LEFT OUTER JOIN cte ON h.MTNo = cte.MTNo
  WHERE h.[Year] = @Year
	AND ([OwnerCostCenter] = @CostCenter OR @CostCenter = 'H301000040')
	AND DeletedDate IS NULL
ORDER BY MTNo ASC

END

--EXEC [sp_BudgetMT_Transaction_Get] @Year = '2565', @CostCenter = 'H301000040'
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_GetById]    Script Date: 14/5/2565 11:20:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BudgetMT_Transaction_GetById]
	-- Add the parameters for the stored procedure here
	@Id		uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT  [Id]
      ,[BudgetId]
      ,[Year]
      ,[MTNo]
      ,[Title]
      ,[OwnerDepartment]
	  ,[OwnerCostCenter]
      ,[Participant]
      ,[SeminarDate]
      ,[Location]
      ,[ParticipantCount]
      ,[HasMealMorning]
      ,[HasMealAfternoon]
      ,[Remark]
      ,[TotalAmount]
      ,[RemainAmount]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[DeletedBy]
      ,[DeletedDate]
  FROM [dbo].[BudgetMTTransaction]
  WHERE [Id] = @Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Insert]    Script Date: 14/5/2565 11:20:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BudgetMT_Transaction_Insert]
	-- Add the parameters for the stored procedure here
	@Year  varchar(4),
	@Title  varchar(500),
	@OwnerDepartment  varchar(500),
	@OwnerCostCenter  varchar(20),
	@Participant  varchar(500),
	@SeminarDate  date,
	@Location  varchar(500),
	@ParticipantCount  int,
	@HasMealMorning  bit,
	@HasMealAfternoon  bit,
	@Remark  varchar(5000),
	@TotalAmount  decimal(18,2),
	@RemainAmount  decimal(18,2),
	@CreatedBy  varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE 
			@Id				uniqueidentifier	= NEWID(),
			@CreatedDate	datetime			= GETDATE(),
			@ModifiedBy		varchar(50)			= @CreatedBy,
			@ModifiedDate	datetime			= GETDATE(),
			@DeletedBy		varchar(50)			= NULL,
			@DeletedDate	datetime			= NULL,
			@BudgetId		uniqueidentifier	= NULL,
			@MTNo			varchar(50)			= NULL,
			@LastestMTNo	varchar(50)			= NULL

	-- 1. Get BudgetId
	SELECT TOP(1) @BudgetId = BudgetID FROM BudgetMT WHERE Year = @Year

	-- 2. Generate MT NO
	SELECT TOP(1) @LastestMTNo = MTNo
	FROM [dbo].[BudgetMTTransaction]
	WHERE BudgetId = @BudgetId
	ORDER BY MTNo DESC 

	SET @MTNo = 'MT-' + RIGHT('0000' + (CONVERT(varchar, CONVERT(int, REPLACE(ISNULL(@LastestMTNo, 0), 'MT-', '')) + 1) ), 4)

	--SELECT RIGHT('0000' + (CONVERT(varchar, CONVERT(int, REPLACE(ISNULL('MT-00012',0), 'MT-', '')) + 1) ), 4)


	INSERT INTO [dbo].[BudgetMTTransaction]
           ([Id]
           ,[BudgetId]
           ,[Year]
           ,[MTNo]
           ,[Title]
           ,[OwnerDepartment]
		   ,[OwnerCostCenter]
           ,[Participant]
           ,[SeminarDate]
           ,[Location]
           ,[ParticipantCount]
           ,[HasMealMorning]
           ,[HasMealAfternoon]
           ,[Remark]
           ,[TotalAmount]
           ,[RemainAmount]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[DeletedBy]
           ,[DeletedDate])
     VALUES
           (@Id,
			@BudgetId,
			@Year,
			@MTNo,
			@Title,
			@OwnerDepartment,
			@OwnerCostCenter,
			@Participant,
			@SeminarDate,
			@Location,
			@ParticipantCount,
			@HasMealMorning,
			@HasMealAfternoon,
			@Remark,
			@TotalAmount,
			@RemainAmount,
			@CreatedBy,
			@CreatedDate,
			@ModifiedBy,
			@ModifiedDate,
			@DeletedBy,
			@DeletedDate)

	IF @@ROWCOUNT < 1
		RAISERROR('can not create record', 16, 1);

	-- Update Budget Amount
	EXEC [dbo].[sp_BudgetMT_UpdateRemainAmount]
		@BudgetId = @BudgetId

	SELECT * FROM [BudgetMTTransaction] WHERE Id = @Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_Transaction_Update]    Script Date: 14/5/2565 11:20:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BudgetMT_Transaction_Update]
	-- Add the parameters for the stored procedure here
	@Id		uniqueidentifier,
	@Year  varchar(4),
	@Title  varchar(500),
	@OwnerDepartment  varchar(500),
	@OwnerCostCenter  varchar(20),
	@Participant  varchar(500),
	@SeminarDate  date,
	@Location  varchar(500),
	@ParticipantCount  int,
	@HasMealMorning  bit,
	@HasMealAfternoon  bit,
	@Remark  varchar(5000),
	@TotalAmount  decimal(18,2),
	@RemainAmount  decimal(18,2),
	@ModifiedBy  varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE 
			@ModifiedDate	datetime			= GETDATE(),
			@DeletedBy		varchar(50)			= NULL,
			@DeletedDate	datetime			= NULL,
			@BudgetId		uniqueidentifier	= NULL,
			@MTNo			varchar(50)			= NULL,
			@LastestMTNo	varchar(50)			= NULL

	-- 1. Get BudgetId
	SELECT TOP(1) @BudgetId = BudgetID FROM BudgetMT WHERE Year = @Year


	UPDATE [dbo].[BudgetMTTransaction]
	   SET [BudgetId] = @BudgetId
		  ,[Year] = @Year
		  ,[Title] = @Title
		  ,[OwnerDepartment] = @OwnerDepartment
		  ,[Participant] = @Participant
		  ,[SeminarDate] = @SeminarDate
		  ,[Location] = @Location
		  ,[ParticipantCount] = @ParticipantCount
		  ,[HasMealMorning] = @HasMealMorning
		  ,[HasMealAfternoon] = @HasMealAfternoon
		  ,[Remark] = @Remark
		  ,[TotalAmount] = @TotalAmount
		  ,[RemainAmount] = @RemainAmount
		  ,[ModifiedBy] = @ModifiedBy
		  ,[ModifiedDate] = @ModifiedDate
		  ,[DeletedBy] = @DeletedBy
		  ,[DeletedDate] = @DeletedDate
	 WHERE [Id] = @Id


	IF @@ROWCOUNT < 1
		RAISERROR('can not update record', 16, 1);

	-- Update Budget Amount
	EXEC [dbo].[sp_BudgetMT_UpdateRemainAmount]
		@BudgetId = @BudgetId

	SELECT * FROM [BudgetMTTransaction] WHERE Id = @Id




END
GO
/****** Object:  StoredProcedure [dbo].[sp_BudgetMT_UpdateRemainAmount]    Script Date: 14/5/2565 11:20:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BudgetMT_UpdateRemainAmount]
	-- Add the parameters for the stored procedure here
	@BudgetId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @TotalAmount decimal(18,2)
	SELECT @TotalAmount = SUM([TotalAmount])
	FROM [dbo].[BudgetMTTransaction]
	where BudgetId = @BudgetId --'D95CCBD9-C8D5-4615-8A81-4A1F304CDDE3'
		and deletedDate IS NULL

	UPDATE [dbo].[BudgetMT] 
		SET WithdrawAmount = @TotalAmount
			,RemainAmount = BudgetAmount - @TotalAmount
	WHERE BudgetId = @BudgetId

END
GO
