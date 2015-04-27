--1.转换成两位小数
select cast(13.1252 as decimal(10,2))
--2.转换日期格式
select CONVERT(VARCHAR(19), GETDATE() ,120)
