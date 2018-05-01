--Создаём Базу
create database GroupDB
go

--Настройка БД
alter database GroupDB
collate Cyrillic_General_CI_AS
go

use GroupDB
go

create table Position
(
	id int not null identity(1,1) constraint PK_Position_id primary key,
	name varchar(30) not null
)
go

insert into Position values ('инженер-технолог'), ('инженер-конструктор')
go

CREATE TABLE Department
(
    id int not null identity(1,1) constraint PK_Department_id primary key,
    name varchar(30) not null
)
go

insert into Department values ('КБ'), ('Технологический отдел')
go

CREATE TABLE Stavka
(
    id int not null identity(1,1) constraint PK_Stavka_id primary key,
    id_position int not null constraint FK_Stavka_Position_id foreign key references Position(id),
	n_data date not null,
	salary int not null
)
go

--запрещаем добавлять на одну должность с той же датой, разные ставки
alter table Stavka
add constraint UniqStavka_Ndata_id_position UNIQUE(id_position, n_data)
go

CREATE TABLE CountPosition
(
    id int not null identity(1,1) constraint PK_CountPosition_id primary key,
    id_department int not null constraint FK_CountPosition_Department_id foreign key references Department(id),
	id_position int not null constraint FK_CountPosition_Position_id foreign key references Position(id),
	data date,
	pcount int
)
go

--процедура отслеживает одно ограничение и добавляет данные в таблицу
create proc AddCountPosition
	@dep int, 
	@pos int,
	@data date,
	@counts int
as
	set nocount on
	declare @datstavka date = (select min(n_data) from Stavka where id_position = @pos)
	if(@data < @datstavka)
		throw 51000, 'Дата начала штатного расписания менише чем минимальная дата начала действия данной должности', 11
	else
		insert into CountPosition values (@dep, @pos, @data, @counts)
go

--функция ворачивает следующую ближайшую дату
create function NextDate(@department int, @data date)
returns date
as
	begin
		return (Select top(1) data from CountPosition
			where id_department = @department AND data > @data
			order by data)
	end
go

--функция ворачивает сумму ставки на опрределённую дату
create function FC_Salary(@id_position int, @data date)
returns int
as
	begin
		return (select top(1) salary From Stavka where id_position = @id_position and n_data <= @data order by n_data DESC)
	end
go

--процедура показывает отчёт
create proc ProcReport
	@dataS date,
	@dataPO date
as
	set nocount on
	Select D.name, data as 'S', isnull(dbo.NextDate(id_department, data), @dataPO) as 'PO', (pcount * dbo.FC_Salary(id_position, data)) as 'SUMM'
	from CountPosition CP
	join Department D on D.id = CP.id_department
		where data >= @dataS AND data <= @dataPO
go