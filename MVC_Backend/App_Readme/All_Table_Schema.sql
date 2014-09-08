USE [Workshop]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemUser](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Account] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateUser] [uniqueidentifier] NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_SystemUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO


CREATE TABLE [dbo].[Category](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateUser] [uniqueidentifier] NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO


CREATE TABLE [dbo].[Article](
	[ID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[Summary] [nvarchar](1024) NOT NULL,
	[ContentText] [nvarchar](max) NOT NULL,
	[IsPublish] [bit] NOT NULL,
	[PublishDate] [datetime] NOT NULL,
	[ViewCount] [int] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateUser] [uniqueidentifier] NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])
GO

ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_Category]
GO

ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_IsPublish]  DEFAULT ((0)) FOR [IsPublish]
GO

ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_ViewCount]  DEFAULT ((0)) FOR [ViewCount]
GO

ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO


CREATE TABLE [dbo].[Photo](
	[ID] [uniqueidentifier] NOT NULL,
	[ArticleID] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateUser] [uniqueidentifier] NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_Article] FOREIGN KEY([ArticleID])
REFERENCES [dbo].[Article] ([ID])
GO

ALTER TABLE [dbo].[Photo] CHECK CONSTRAINT [FK_Photo_Article]
GO

ALTER TABLE [dbo].[Photo] ADD  CONSTRAINT [DF_Photo_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Photo] ADD  CONSTRAINT [DF_Photo_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO


