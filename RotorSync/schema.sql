-- Script Date: 1/30/2017 2:58 PM  - ErikEJ.SqlCeScripting version 3.5.2.64
-- Database information:
-- Database: C:\Code\RotorSync\RotorSync\RSDatabase.db
-- ServerVersion: 3.14.2
-- DatabaseSize: 24 KB
-- Created: 12/22/2016 9:18 PM

-- User Table information:
-- Number of tables: 4
-- channels: -1 row(s)
-- config: -1 row(s)
-- frequencymap: -1 row(s)
-- HDHR: -1 row(s)

SELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [HDHR] (
  [Id] INTEGER NOT NULL
, [deviceID] text NOT NULL
, [ip] text NOT NULL
, CONSTRAINT [sqlite_master_PK_HDHR] PRIMARY KEY ([Id])
);
CREATE TABLE [frequencymap] (
  [channel] INTEGER NOT NULL
, [frequency] bigint NOT NULL
, CONSTRAINT [sqlite_master_PK_frequencymap] PRIMARY KEY ([channel])
);
CREATE TABLE [config] (
  [Id] INTEGER NOT NULL
, [sensitivity] bigint NOT NULL
, [HDHRdeviceID] text NULL
, [rotorTimeout] bigint NULL
, [rotorAzimuth] bigint NULL
, [rotorCOMPort] text NULL
, CONSTRAINT [sqlite_master_PK_config] PRIMARY KEY ([Id])
);
CREATE TABLE [channels] (
  [Id] INTEGER NOT NULL
, [callsign] text NULL
, [realchannel] bigint NOT NULL
, [virtualchannel] bigint NULL
, [network] text NULL
, [distance] real NULL
, [azimuth] bigint NOT NULL
, CONSTRAINT [sqlite_master_PK_channels] PRIMARY KEY ([Id])
);
COMMIT;

