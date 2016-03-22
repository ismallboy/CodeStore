1.增加字段：
use sda_main;
drop procedure if exists schema_change;
delimiter ';;';
create procedure schema_change() begin
if NOT exists (select * from information_schema.columns where table_name = 'product' and column_name = 'IfFinalist') then
		alter table product add column IfFinalist bit COMMENT '是否入围，true入围，null或false不入围';
end if;
end;;
delimiter ';';
call schema_change();
drop procedure if exists schema_change;
2.修改数据库字段类型
--alter table upload change column <old name> <new name> <new datatype>
alter table filetable change column fdata fdata MediumBlob
3.连表更新字段
update admin_follow_customer as a, admin_user_extension as b
set a.UserId = b.UserId, a.Remark = NULL
WHERE a.Remark = b.Email;

update a left join b on a.id=b.a_id set a.title='aaaaa',b.body='bbbb' where a.id=1;
4.
