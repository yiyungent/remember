CREATE TABLE Plugin_ArticleContent(
       ID int primary key auto_increment,
       Title varchar(100),       
       Content varchar(2000),       
       LastUpdateTime datetime,       
       AuthorId int
)