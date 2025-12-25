CREATE TABLE [dbo].[Table]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [ClientType] NVARCHAR(MAX) NOT NULL, 
    [BaseCost] FLOAT NOT NULL, 
    [PricingStrategyType] NVARCHAR(MAX) NOT NULL, 
    [Discount Value] FLOAT NULL, 
    [AdditionalInfo] NVARCHAR(MAX) NULL
)
