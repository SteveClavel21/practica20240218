create database prueba20240218DB
go


use prueba20240218DB
go

create table Categorias(
CategoriaId int primary key identity(1,1),
Nombre nvarchar(100) not null
);

create table Productos(
ProductoId int primary key identity(1,1),
Nombre nvarchar(100) not null,
Precio decimal(10,2) not null,
Descripcion nvarchar(max),
Imagen image,
CategoriaId int foreign key references Categorias(CategoriaId) not null
);