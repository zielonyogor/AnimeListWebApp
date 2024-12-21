CREATE TABLE Badge(
    Name VARCHAR2(20) NOT NULL CONSTRAINT pkBadge PRIMARY KEY,
    BackgroundColor VARCHAR2(20) DEFAULT 'ffffff' NOT NULL,
    NameColor VARCHAR2(20) DEFAULT '000000' NOT NULL,
    Description VARCHAR(100)
);

CREATE TABLE Account(
    AccountId NUMBER(10)  GENERATED ALWAYS AS IDENTITY CONSTRAINT pkAccount PRIMARY KEY,
    EmailAddress VARCHAR2(50) NOT NULL UNIQUE,
    Username VARCHAR2(20) NOT NULL UNIQUE,
    Password VARCHAR2(20) NOT NULL,
    CreateDate DATE DEFAULT CURRENT_DATE,
    ImageLink VARCHAR(300) NULL,
    Description VARCHAR2(500) NULL,
    AccountPrivilege VARCHAR2(1) DEFAULT 'n' NOT NULL,
    CONSTRAINT chkAccountPrivilege CHECK(AccountPrivilege in ('n', 'm', 'a')) --none or moderator or admin
);

CREATE TABLE UserBadge(
    AccountId CONSTRAINT fkUserBadge REFERENCES Account(AccountId),
    BadgeName VARCHAR2(20) NOT NULL CONSTRAINT fkBadgeUser REFERENCES Badge(Name),

    CONSTRAINT pkUserBadge PRIMARY KEY(AccountId, BadgeName)
);

CREATE TABLE Friend(
    AccountId1 CONSTRAINT fkUser1 REFERENCES Account(AccountId),
    AccountId2 CONSTRAINT fkUser2 REFERENCES Account(AccountId),

    CONSTRAINT pkFriend PRIMARY KEY(AccountId1, AccountId2),
    CONSTRAINT chkFriendPair CHECK (AccountId1 != AccountId2)
);

CREATE TABLE Author(
    Id NUMBER(5) GENERATED ALWAYS AS IDENTITY CONSTRAINT pkAuthor PRIMARY KEY,
    Name VARCHAR2(30) NOT NULL,
    Image VARCHAR2(300) NULL,
    WikipediaLink VARCHAR2(100) NULL
);

CREATE TABLE Studio(
    Name VARCHAR2(20) NOT NULL CONSTRAINT pkStudio PRIMARY KEY,
    WikipediaLink VARCHAR2(100) NULL
);

CREATE TABLE Genre(
    Name VARCHAR(24) NOT NULL CONSTRAINT pkGenre PRIMARY KEY
);

CREATE TABLE Medium(
    Id NUMBER(10) GENERATED ALWAYS AS IDENTITY CONSTRAINT pkMedium PRIMARY KEY,
    Name VARCHAR(30) NOT NULL,
    Status VARCHAR2(14) DEFAULT 'Not finished'
    CONSTRAINT chkMediumStatus CHECK(Status IN ('Not finished', 'Finished', 'To be released')),
    Count NUMBER(4) DEFAULT 0 NOT NULL,
    Poster VARCHAR(300) NULL,
    PublishDate DATE DEFAULT CURRENT_DATE,
    Description VARCHAR(1000) NULL,
    Type VARCHAR2(1) NOT NULL CONSTRAINT chkMediumType CHECK(Type IN ('M', 'A')) -- m - manga, a - anime
);

CREATE TABLE MediumConnection(
    IdMedium1 NUMBER(10) NOT NULL,
    IdMedium2 NUMBER(10) NOT NULL,
    
    CONSTRAINT fkMediumConnection1 FOREIGN KEY(IdMedium1) REFERENCES Medium(Id),
    CONSTRAINT fkMediumConnection2 FOREIGN KEY(IdMedium2) REFERENCES Medium(Id),
    CONSTRAINT pkMediumConnection PRIMARY KEY (IdMedium1, IdMedium2),
    
    CONSTRAINT chkMediumConnection CHECK (IdMedium1 != IdMedium2)
);

CREATE TABLE MediumGenre(
    IdMedium CONSTRAINT fkMediumGenre REFERENCES Medium(Id),
    GenreName CONSTRAINT fkGenreMedium REFERENCES Genre(Name),

    CONSTRAINT pkMediumGenre PRIMARY KEY (IdMedium, GenreName)
);

CREATE TABLE Manga(
    MediumId NUMBER(10) 
    CONSTRAINT fkMangaId REFERENCES Medium(Id)
    CONSTRAINT pkMangaId PRIMARY KEY,

    Type VARCHAR2(10) DEFAULT 'Manga' CONSTRAINT chkMangaType CHECK (Type IN ('Manga', 'Light novel', 'Oneshot')),
    AuthorId NUMBER(5) NOT NULL CONSTRAINT fkAuthorId REFERENCES Author(Id)
);

CREATE TABLE Anime(
    MediumId NUMBER(10) 
    CONSTRAINT fkAnimeId REFERENCES Medium(Id)
    CONSTRAINT pkAnimeId PRIMARY KEY,

    Type VARCHAR2(5) DEFAULT 'TV', CONSTRAINT chkAnimeType CHECK (Type IN ('TV', 'Movie', 'OVA')),
    StudioName VARCHAR2(20) NOT NULL CONSTRAINT fkNameStudio REFERENCES Studio(Name)
);

CREATE TABLE ListElement (
    AccountId NUMBER(10) NOT NULL CONSTRAINT fkElementAccountId REFERENCES Account(AccountId) ON DELETE CASCADE,
    MediumId NUMBER(10) NOT NULL CONSTRAINT fkElementMedium REFERENCES Medium(Id),
    FinishedNumber NUMBER(4) DEFAULT 0 CONSTRAINT chkFinished CHECK (FinishedNumber >= 0),
    Status VARCHAR2(13) DEFAULT 'Watching' NOT NULL CONSTRAINT chkElementStatus CHECK (Status IN ('Completed', 'Plan to watch', 'On-hold', 'Dropped', 'Watching')),
    Rating NUMBER(2) DEFAULT NULL CONSTRAINT chkRating CHECK (Rating BETWEEN 0 AND 10),
    MediumComment VARCHAR2(200),
    StartDate DATE DEFAULT CURRENT_DATE,
    FinishDate DATE,
    
    CONSTRAINT pkListElement PRIMARY KEY (AccountId, MediumId),
    CONSTRAINT chkPlanToWatch CHECK (
        Status != 'Plan to watch' OR (Rating IS NULL AND FinishedNumber = 0)
    )
);

CREATE TABLE Review(
    AccountId NUMBER(10) NOT NULL,
    MediumId NUMBER(10) CONSTRAINT fkReviewMedium REFERENCES Medium(Id),
    
    Description VARCHAR2(500) NOT NULL,
    Feeling VARCHAR2(17) DEFAULT 'Recommended' NOT NULL 
        CONSTRAINT chkFeeling CHECK (Feeling IN ('Recommended', 'Not recommended', 'Mixed feelings')),
    PostDate DATE DEFAULT CURRENT_DATE NOT NULL,
    
    CONSTRAINT fkUserReview FOREIGN KEY (AccountId) REFERENCES Account(AccountId),
    CONSTRAINT pkReview PRIMARY KEY (AccountId, MediumId)
);

CREATE TABLE Character(
    Id NUMBER(12) GENERATED ALWAYS AS IDENTITY CONSTRAINT pkCharacter PRIMARY KEY,
    Name VARCHAR2(30) NOT NULL,
    Image VARCHAR(300) NULL,
    Description VARCHAR2(500) NULL
);

CREATE TABLE MediumCharacter(s
    MediumId CONSTRAINT fkMediumCharacter REFERENCES Medium(Id),
    CharacterId CONSTRAINT fkCharacterMedium REFERENCES Character(Id),

    CONSTRAINT pkMediumCharacter PRIMARY KEY (MediumId, CharacterId)
);

CREATE TABLE UserCharacter(
    AccountId CONSTRAINT fkUserCharacter REFERENCES Account(AccountId),
    CharacterId CONSTRAINT fkCharacterUser REFERENCES Character(Id),

    CONSTRAINT pkUserCharacter PRIMARY KEY (AccountId, CharacterId)
);

COMMIT;

-------------------------------------------------------------------------------------------------------------
--USUWANIE BAZY:

-- DROP TABLE UserCharacter CASCADE CONSTRAINTS;
-- DROP TABLE MediumCharacter CASCADE CONSTRAINTS;
-- DROP TABLE Character CASCADE CONSTRAINTS;
-- DROP TABLE Review CASCADE CONSTRAINTS;
-- DROP TABLE ListElement CASCADE CONSTRAINTS;
-- DROP TABLE Anime CASCADE CONSTRAINTS;
-- DROP TABLE Manga CASCADE CONSTRAINTS;
-- DROP TABLE MediumGenre CASCADE CONSTRAINTS;
-- DROP TABLE MediumConnection CASCADE CONSTRAINTS;
-- DROP TABLE Medium CASCADE CONSTRAINTS;
-- DROP TABLE Genre CASCADE CONSTRAINTS;
-- DROP TABLE Studio CASCADE CONSTRAINTS;
-- DROP TABLE Author CASCADE CONSTRAINTS;
-- DROP TABLE Friend CASCADE CONSTRAINTS;
-- DROP TABLE UserBadge CASCADE CONSTRAINTS;
-- DROP TABLE Account CASCADE CONSTRAINTS;
-- DROP TABLE Badge CASCADE CONSTRAINTS;

INSERT INTO Medium(Name, Type) VALUES('Chainsaw Man', 'A'); 
INSERT INTO Studio(Name) VALUES('Mappa');
INSERT INTO Anime(MediumId, StudioName) VALUES((SELECT Id FROM Medium WHERE Name='Chainsaw Man' AND Type='A'), 'Mappa'); 
COMMIT;