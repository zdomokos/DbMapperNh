-- snake case table
drop table color2;

CREATE TABLE color2 (
    color_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    color_name VARCHAR NOT NULL
);

INSERT INTO color2 (color_id, color_name) OVERRIDING SYSTEM VALUE VALUES (2, 'White');
INSERT INTO color2 (color_id, color_name) VALUES (NULL, 'White');
INSERT INTO color2 (color_name) VALUES ('White3');
INSERT INTO color2 (color_id, color_name) VALUES (3, 'White');
INSERT INTO color2 (color_name) VALUES ('White3');
INSERT INTO color2 (color_name) VALUES ('White3');


SELECT * from color2_color_id_seq;

update color2_color_id_seq   set last_value=10;

SELECT setval('color2_color_id_seq', 21, true);





-- quoted table
drop table color3;

CREATE TABLE "Color3"
(
    "Id"          integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY ,
    "Name"        varchar(30) NOT NULL,
    "Description" text
);

INSERT INTO "Color3" ("Id", "Name") OVERRIDING SYSTEM VALUE VALUES (2, 'White');
INSERT INTO "Color3" ("Id", "Name") VALUES (NULL, 'White');
INSERT INTO "Color3" ("Name") VALUES ('White3');
INSERT INTO "Color3" ("Id", "Name") VALUES (3, 'White');
INSERT INTO "Color3" ("Name") VALUES ('White3');
INSERT INTO "Color3" ("Name") VALUES ('White3');

select * from "Color3_Id_seq";

nextval("Color3_Id_seq");


