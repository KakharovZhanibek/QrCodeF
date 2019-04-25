use test
create table Purchases
(
Id int identity,
PurchaseGuid uniqueidentifier not null,
PurchaseDate date not null,
ProductId int not null foreign key references Products(Id),
CustomerName varchar(256) not null,
ExchangeRate float not null,
PriceKZT float not null,
PurchaseQr varbinary(max) not null,
ShippingQr varbinary(max) not null
)

create table Products
(
Id int identity primary key,
[Name] varchar(256) not null,
PriceUSD float not null
)

create table qrCodes
(
Id int identity,
Content varbinary(max) not null,
QrCodeType int not null
)
insert into Products values('Тостер',2000.15)
