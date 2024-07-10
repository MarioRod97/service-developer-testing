-- Adminer 4.8.1 PostgreSQL 16.2 (Debian 16.2-1.pgdg110+2) dump
create database "vendors";
\connect "vendors";


CREATE SEQUENCE vendorinfo_id_seq INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1;

CREATE TABLE "public"."vendorinfo" (
    "id" integer DEFAULT nextval('vendorinfo_id_seq') NOT NULL,
    "name" character varying(30) NOT NULL,
    "address" character varying(300) NOT NULL,
    CONSTRAINT "vendorinfo_pkey" PRIMARY KEY ("id")
) WITH (oids = false);

INSERT INTO "vendorinfo" ("id", "name", "address") VALUES
(1,	'Microsoft',	'1212 Bluebird Court, Belleview WA'),
(2,	'Jetbrains',	'9898 Maple Court, Czech Republic'),
(3,	'Bungie',	'993 Zavala Place, Bellview WA');

-- 2024-07-10 11:37:16.46025+00