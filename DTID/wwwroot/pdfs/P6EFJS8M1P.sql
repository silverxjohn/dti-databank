USE [dti_databank]
GO

INSERT INTO [dbo].[Years]
           ([DateCreated]
           ,[DateUpdated]
           ,[Name])
     VALUES
           (getdate() ,getdate() ,'2000'),
		   (getdate() ,getdate() ,'2001'),
		   (getdate() ,getdate() ,'2002'),
		   (getdate() ,getdate() ,'2003'),
		   (getdate() ,getdate() ,'2004'),
		   (getdate() ,getdate() ,'2005'),
		   (getdate() ,getdate() ,'2006'),
		   (getdate() ,getdate() ,'2007'),
		   (getdate() ,getdate() ,'2008'),
		   (getdate() ,getdate() ,'2009'),
		   (getdate() ,getdate() ,'2010'),
		   (getdate() ,getdate() ,'2011'),
		   (getdate() ,getdate() ,'2012'),
		   (getdate() ,getdate() ,'2013'),
		   (getdate() ,getdate() ,'2014'),
		   (getdate() ,getdate() ,'2015'),
		   (getdate() ,getdate() ,'2016'),
		   (getdate() ,getdate() ,'2017'),
		   (getdate() ,getdate() ,'2018')

		   INSERT INTO [dbo].[Months]
           ([DateCreated]
           ,[DateUpdated]
           ,[Name])
     VALUES
           (getdate() , getdate() , 'January'),
		   (getdate() , getdate() , 'February'),
		   (getdate() , getdate() , 'March'),
		   (getdate() , getdate() , 'April'),
		   (getdate() , getdate() , 'May'),
		   (getdate() , getdate() , 'June'),
		   (getdate() , getdate() , 'July'),
		   (getdate() , getdate() , 'August'),
		   (getdate() , getdate() , 'September'),
		   (getdate() , getdate() , 'October'),
		   (getdate() , getdate() , 'November'),
		   (getdate() , getdate() , 'December')

INSERT INTO [dbo].[Quarters]
           ([DateCreated]
           ,[DateUpdated]
           ,[Name])
     VALUES
            (getdate() , getdate() , 'Q1'),
			(getdate() , getdate() , 'Q2'),
			(getdate() , getdate() , 'Q3'),
			(getdate() , getdate() , 'Q4')
GO



GO


