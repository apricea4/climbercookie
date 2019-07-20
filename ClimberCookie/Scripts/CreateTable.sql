
create table customer (
id serial primary key
, custName varchar(50) not null
, address varchar(50) not null
, phone varchar(15) not null
);

create table inventory(
id serial primary key
, ItemDescr varchar(50) not null
, ItemPrice money not null
, OnHand int not null
);

create table objectId(
id serial primary key
, TableName varchar(40)
);

create table orders (
id serial primary key
, custID int not null
, shipAddressID int not null
, sent bool
, received bool
, datePlaced date
, dateReceived date
);

create table ShippingAddress(
id serial primary key
, custID int not null
, address varchar(50) not null
, phone varchar(15) not null
);