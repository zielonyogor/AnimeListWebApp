CREATE TABLE odznaki(
    nazwa VARCHAR2(20) NOT NULL CONSTRAINT pk_odznaki PRIMARY KEY,
    kolor_tla VARCHAR2(20) DEFAULT 'ffffff' NOT NULL,
    kolor_nazwy VARCHAR2(20) DEFAULT '000000' NOT NULL,
    opis VARCHAR(100)
);

CREATE TABLE konta(
    adres_email VARCHAR2(50) NOT NULL CONSTRAINT pk_konto PRIMARY KEY,
    nazwa_uzytkownika VARCHAR2(20) NOT NULL UNIQUE,
    haslo VARCHAR2(20) NOT NULL,
    data_zalozenia DATE DEFAULT CURRENT_DATE,
    zdjecie VARCHAR(300) NULL,
    opis VARCHAR2(500) NULL
);

CREATE TABLE admini(
    adres_email VARCHAR2(50) NOT NULL CONSTRAINT pk_admini PRIMARY KEY,
    CONSTRAINT fk_adres_email FOREIGN KEY(adres_email) REFERENCES konta(adres_email)
);

CREATE TABLE uzytkownicy(
    adres_email VARCHAR2(50) NOT NULL,
    uprawnienia VARCHAR2(1) DEFAULT 'n' NOT NULL,
    
    CONSTRAINT pk_uzytkownicy PRIMARY KEY (adres_email),
    CONSTRAINT fk_adres_email_uzytkownik FOREIGN KEY(adres_email) REFERENCES konta(adres_email),
    CONSTRAINT chk_uprawnienia CHECK(uprawnienia in ('n', 'm')) --none or moderator
);

CREATE TABLE znajomi(
    adres_email_uzytkownika_1 CONSTRAINT fk_uzytkownik_1 REFERENCES uzytkownicy(adres_email),
    adres_email_uzytkownika_2 CONSTRAINT fk_uzytkownik_2 REFERENCES uzytkownicy(adres_email),

    CONSTRAINT pk_znajomi PRIMARY KEY(adres_email_uzytkownika_1, adres_email_uzytkownika_2)
);

CREATE TABLE autorzy(
    id_autora NUMBER(5) GENERATED ALWAYS AS IDENTITY CONSTRAINT pk_autorzy PRIMARY KEY,
    nazwa_autora VARCHAR2(30) NOT NULL,
    zdjecie VARCHAR2(300) NULL,
    link_wikipedia VARCHAR2(100) NULL
);

CREATE TABLE studia(
    nazwa_studia VARCHAR2(20) NOT NULL CONSTRAINT pk_studia PRIMARY KEY,
    link_wikipedia VARCHAR2(100) NULL
);

CREATE TABLE gatunki(
    nazwa VARCHAR(24) NOT NULL CONSTRAINT pk_nazwa_gatunku PRIMARY KEY
);

CREATE TABLE media(
    id_medium NUMBER(10) GENERATED ALWAYS AS IDENTITY CONSTRAINT pk_medium PRIMARY KEY,
    nazwa VARCHAR(30) NOT NULL,
    status_wydania VARCHAR2(14) DEFAULT 'nieskonczone'
    CONSTRAINT chk_status_medium CHECK(status_wydania IN ('nieskonczone', 'skonczone', 'zapowiedziane')),
    plakat VARCHAR(300) NULL,
    data_wydania DATE DEFAULT CURRENT_DATE,
    opis VARCHAR(1000) NULL,
    rodzaj_medium VARCHAR2(1) NOT NULL CONSTRAINT chk_rodzaj_medium CHECK(rodzaj_medium IN ('m', 'a')) -- m - manga, a - anime
);

CREATE TABLE media_powiazania(
    id_medium_1 NOT NULL,
    id_medium_2 NOT NULL,
    
    CONSTRAINT fk_media_powiazania_1 FOREIGN KEY(id_medium_1) REFERENCES media(id_medium),
    CONSTRAINT fk_media_powiazania_2 FOREIGN KEY(id_medium_2) REFERENCES media(id_medium),
    CONSTRAINT pk_media_powiazania PRIMARY KEY (id_medium_1, id_medium_2)
);

CREATE TABLE media_gatunki(
    id_medium CONSTRAINT fk_media_gatunki REFERENCES media(id_medium),
    nazwa_gatunku CONSTRAINT fk_gatunki_media REFERENCES gatunki(nazwa),

    CONSTRAINT pk_media_gatunki PRIMARY KEY (id_medium, nazwa_gatunku)
);

CREATE TABLE manga(
    id_medium NUMBER(10) 
    CONSTRAINT fk_id_manga REFERENCES media(id_medium)
    CONSTRAINT pk_id_manga PRIMARY KEY,

    typ VARCHAR2(10) DEFAULT 'manga' CONSTRAINT chk_typ_mangi CHECK (typ IN ('manga', 'light novel', 'oneshot')),
    id_autora NUMBER(5) NOT NULL CONSTRAINT fk_id_autora REFERENCES autorzy(id_autora)
);

CREATE TABLE anime(
    id_medium NUMBER(10) 
    CONSTRAINT fk_id_anime REFERENCES media(id_medium)
    CONSTRAINT pk_id_anime PRIMARY KEY,

    typ VARCHAR2(10) DEFAULT 'tv' CONSTRAINT chk_typ_anime CHECK (typ IN ('tv', 'film', 'ova')),
    nazwa_studia VARCHAR2(20) NOT NULL CONSTRAINT fk_nazwa_studia REFERENCES studia(nazwa_studia)
);

CREATE TABLE elementy_listy(
    adres_email_uzytkownika CONSTRAINT fk_lista_uzytkownik REFERENCES konta(adres_email) ON DELETE CASCADE,
    id_medium NUMBER(10) CONSTRAINT fk_lista_medium REFERENCES media(id_medium),

    liczba_ukonczonych_czesci NUMBER(4) DEFAULT 0 CONSTRAINT chk_ukonczone CHECK(liczba_ukonczonych_czesci >= 0),
    status VARCHAR(14) DEFAULT 'w trakcie' NOT NULL CONSTRAINT chk_status_elementu
    CHECK(status IN ('skonczone', 'w planach', 'wstrzymane', 'porzucone', 'w trakcie')),
    ocena NUMBER(2) DEFAULT NULL CONSTRAINT chk_ocena CHECK(ocena BETWEEN 0 AND 10),
    uwagi VARCHAR(200) NULL,
    data_rozpoczecia DATE DEFAULT CURRENT_DATE,
    data_ukonczenia DATE NULL,

    CONSTRAINT pk_element_listy PRIMARY KEY (adres_email_uzytkownika, id_medium),
    --jeśli 'w planach' to nie można dać oceny ani mieć postępu
    CONSTRAINT chk_w_planach CHECK(
        status != 'w planach' OR (ocena IS NULL and liczba_ukonczonych_czesci = 0)
    )
);

CREATE TABLE recenzja (
    adres_email_uzytkownika VARCHAR2(50),
    id_medium NUMBER(10) CONSTRAINT fk_recenzja_medium REFERENCES media(id_medium),
    
    opis VARCHAR2(500) NOT NULL,
    odczucia VARCHAR2(17) DEFAULT 'polecam' NOT NULL 
        CONSTRAINT chk_odczucia CHECK (odczucia IN ('polecam', 'nie polecam', 'mieszane odczucia')),
    data_wystawienia DATE DEFAULT CURRENT_DATE NOT NULL,
    
    CONSTRAINT fk_recenzja_uzytkownik FOREIGN KEY (adres_email_uzytkownika) REFERENCES konta(adres_email),
    CONSTRAINT pk_recenzja PRIMARY KEY (adres_email_uzytkownika, id_medium)
);

CREATE TABLE postacie(
    id_postaci NUMBER(12) GENERATED ALWAYS AS IDENTITY CONSTRAINT pk_postacie PRIMARY KEY,
    nazwa_postaci VARCHAR2(30) NOT NULL,
    zdjecie VARCHAR(300) NULL,
    opis VARCHAR2(500) NULL
);

CREATE TABLE media_postacie(
    id_medium CONSTRAINT fk_media_postacie REFERENCES media(id_medium),
    id_postaci CONSTRAINT fk_postacie_media REFERENCES postacie(id_postaci),

    CONSTRAINT pk_media_postacie PRIMARY KEY (id_medium, id_postaci)
);

CREATE TABLE postacie_uzytkownikow(
    adres_email_uzytkownika CONSTRAINT fk_uzytkownicy_postacie REFERENCES konta(adres_email),
    id_postaci CONSTRAINT fk_postacie_uzytkownicy REFERENCES postacie(id_postaci),

    CONSTRAINT pk_postacie_uzytkownikow PRIMARY KEY (adres_email_uzytkownika, id_postaci)
);


-------------------------------------------------------------------------------------------------------------
--USUWANIE BAZY:

DROP TABLE postacie_uzytkownikow CASCADE CONSTRAINTS;
DROP TABLE media_postacie CASCADE CONSTRAINTS;
DROP TABLE postacie CASCADE CONSTRAINTS;
DROP TABLE recenzja CASCADE CONSTRAINTS;
DROP TABLE elementy_listy CASCADE CONSTRAINTS;
DROP TABLE anime CASCADE CONSTRAINTS;
DROP TABLE manga CASCADE CONSTRAINTS;
DROP TABLE media_gatunki CASCADE CONSTRAINTS;
DROP TABLE media_powiazania CASCADE CONSTRAINTS;
DROP TABLE media CASCADE CONSTRAINTS;
DROP TABLE gatunki CASCADE CONSTRAINTS;
DROP TABLE studia CASCADE CONSTRAINTS;
DROP TABLE autorzy CASCADE CONSTRAINTS;
DROP TABLE znajomi CASCADE CONSTRAINTS;
DROP TABLE uzytkownicy CASCADE CONSTRAINTS;
DROP TABLE admini CASCADE CONSTRAINTS;
DROP TABLE konta CASCADE CONSTRAINTS;
DROP TABLE odznaki CASCADE CONSTRAINTS;