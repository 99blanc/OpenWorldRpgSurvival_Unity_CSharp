use ksh;

create table player
(
	id int identity primary key not null,
	position varchar(20),
	rotation varchar(20),
	euler varchar(20),
	dead bit,
	spawn bit,
	health int,
	stamina int,
	thirst int,
	eat int,
	rest int,
	db numeric(3, 0)
);

create table inventoryArray
(
	id int identity primary key not null,
	array int
);

create table inventoryItemName
(
	id int identity primary key not null,
	name varchar(40)
);

create table inventoryItem
(
	id int identity primary key not null,
	item int
);

create table inventory
(
	id int identity primary key not null,
	inventoryArray int references inventoryArray(id),
	inventoryItemName int references inventoryItemName(id),
	inventoryItem int references inventoryItem(id)
);

create table gearArray
(
	id int identity primary key not null,
	array int
);

create table gearItemName
(
	id int identity primary key not null,
	name varchar(40)
);

create table gearItem
(
	id int identity primary key not null,
	item int
);

create table gear
(
	id int identity primary key not null,
	gearArray int references gearArray(id),
	gearItemName int references gearItemName(id),
	gearItem int references gearItem(id)
);

create table colHealth
(
	id int identity primary key not null,
	health int
);

create table colPosition
(
	id int identity primary key not null,
	position varchar(20)
);

create table colRand
(
	id int identity primary key not null,
	random int
);

create table colRotRand
(
	id int identity primary key not null,
	rotRandom float
);

create table constName
(
	id int identity primary key not null,
	name varchar(40)
);

create table constPosition
(
	id int identity primary key not null,
	position varchar(20)
);

create table constRotation
(
	id int identity primary key not null,
	rotation varchar(20)
);

create table destroyObject
(
	id int identity primary key not null,
	position varchar(20)
);

create table gatherObject
(
	id int identity primary key not null,
	position varchar(20)
);

create table world
(
	id int identity primary key not null,
	fogDensity float,
	farClip float,
	colHealth int references colHealth(id),
	colPosition int references colPosition(id),
	colRand int references colRand(id),
	colRotRand int references colRotRand(id),
	constName int references constName(id),
	constPosition int references constPosition(id),
	constRotation int references constRotation(id),
	destroyObject int references destroyObject(id),
	gatherObject int references gatherObject(id),
);

create table animalName
(
	id int identity primary key not null,
	name varchar(40)
);

create table animalHealth
(
	id int identity primary key not null,
	health int
);

create table animalPosition
(
	id int identity primary key not null,
	position varchar(20)
);

create table animal
(
	id int identity primary key not null,
	animalName int references animalName(id),
	animalHealth int references animalHealth(id),
	animalPosition int references animalPosition(id),
);

create table monsterName
(
	id int identity primary key not null,
	name varchar(40)
);

create table monsterHealth
(
	id int identity primary key not null,
	health int
);

create table monsterPosition
(
	id int identity primary key not null,
	position varchar(20)
);

create table monster
(
	id int identity primary key not null,
	monsterName int references monsterName(id),
	monsterHealth int references monsterHealth(id),
	monsterPosition int references monsterPosition(id),
);