USE CandyShop;

CREATE TABLE users(userId varchar(5) primary key , userName char(18), phoneNumber int);
create table candies(candieId varchar(5) primary key, candieName char(18),price decimal);
CREATE TABLE orders(orderId varchar(5) primary key, userId varchar(5) FOREIGN KEY REFERENCES users(userId) ,
candieId varchar(5) FOREIGN KEY REFERENCES candies (candieId), quantity int);
ALTER table orders add orderStatus char(15);
alter table users ALTER COLUMN phoneNumber varchar(10);
