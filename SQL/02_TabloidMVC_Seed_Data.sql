USE [TabloidMVC]
GO

SET IDENTITY_INSERT [UserType] ON
INSERT INTO [UserType] ([ID], [Name]) VALUES (1, 'Admin'), (2, 'Author'), (3, 'Admin_Deactivated'), (4, 'Author_Deactivated');
SET IDENTITY_INSERT [UserType] OFF


SET IDENTITY_INSERT [Category] ON
INSERT INTO [Category] ([Id], [Name], [IsDeleted]) 
VALUES (1, 'Technology', 0), (2, 'Close Magic', 0), (3, 'Politics', 0), (4, 'Science', 0), (5, 'Improv', 0), 
	   (6, 'Cthulhu Sightings', 0), (7, 'History', 0), (8, 'Home and Garden', 0), (9, 'Entertainment', 0), 
	   (10, 'Cooking', 0), (11, 'Music', 0), (12, 'Movies', 0), (13, 'Regrets', 0);
SET IDENTITY_INSERT [Category] OFF


SET IDENTITY_INSERT [Tag] ON
INSERT INTO [Tag] ([Id], [Name])
VALUES (1, 'C#'), (2, 'JavaScript'), (3, 'Cyclopean Terrors'), (4, 'Family');
SET IDENTITY_INSERT [Tag] OFF

SET IDENTITY_INSERT [UserProfile] ON
INSERT INTO [UserProfile] (
	[Id], [FirstName], [LastName], [DisplayName], [Email], [CreateDateTime], [ImageLocation], [UserTypeId])
VALUES (1, 'Admina', 'Strator', 'admin', 'admin@example.com', SYSDATETIME(), NULL, 1);
SET IDENTITY_INSERT [UserProfile] OFF

SET IDENTITY_INSERT [Post] ON
INSERT INTO [Post] (
	[Id], [Title], [Content], [ImageLocation], [CreateDateTime], [PublishDateTime], [IsApproved], [CategoryId], [UserProfileId])
VALUES (
	1, 'C# is the Best Language', 
'There are those' + char(10) + 'who do not believe' + char(10) + 'C# is the best.' + char(10) + 'They are wrong.',
    'https://gizmodiva.com/wp-content/uploads/2017/10/SCOTT-A-WOODWARD_1SW1943-1170x689.jpg',SYSDATETIME(), SYSDATETIME(), 1, 1, 1);
SET IDENTITY_INSERT [Post] OFF

SET IDENTITY_INSERT [Reaction] ON
INSERT INTO [Reaction] ([Id],[Name],[ImageLocation])
Values (1,'EggDance', 'https://emoji.slack-edge.com/T03F2SDTJ/egg-dance/bf79c1ac7be25851.gif'), 
( 2 , 'Theater', 'https://slack-imgs.com/?c=1&o1=gu&url=https%3A%2F%2Fa.slack-edge.com%2Fproduction-standard-emoji-assets%2F13.0%2Fgoogle-small%2F1f3ad%402x.png'), 
( 3 , 'Joy', 'https://a.slack-edge.com/production-standard-emoji-assets/13.0/google-small/1f602@2x.png'),
( 4 , 'PizzaSpin', 'https://emoji.slack-edge.com/T03F2SDTJ/pizzaspin/f536c60be15e719c.gif'),
( 5 , 'BabyYoda', 'https://emoji.slack-edge.com/T03F2SDTJ/baby_yoda_soup/730ab14e55635afd.gif');
SET IDENTITY_INSERT [Reaction] OFF
