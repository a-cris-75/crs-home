-- SQL Manager 2010 Lite for SQL Server 3.6.0.1
-- ---------------------------------------
-- Host      : LOCALHOST\SQLEXPRESS
-- Database  : DB_CRISVAIO
-- Version   : Microsoft SQL Server  9.00.2047.00


--
-- Definition for user Admin : 
--

--
-- Definition for user LGUserDB : 
--


--
-- Definition for user SuperLogin : 
--

--
-- Definition for schema Admin : 
--

--
-- Definition for user-defined data type autoincrement : 
--

--
-- Definition for table Accoppiate : 
--

CREATE TABLE Accoppiate (
  Accoppiate int NOT NULL,
  Descrizione varchar(32) NULL
)


--
-- Definition for table Ambi : 
--

CREATE TABLE Ambi (
  Ambi int NOT NULL,
  Data datetime,
  Ruota INT,
  N1 INT,
  N2 int,
  Decine int ,
  ColpoN1 int ,
  ColpoN2 int ,
  Tipo_FreqN1 int ,
  Tipo_FreqN2 int ,
  Dist_Ambo_Prec int 
)


--
-- Definition for table AmbiInDecina : 
--

CREATE TABLE AmbiInDecina (
  Data smalldatetime NOT NULL,
  Ruota int NOT NULL,
  Estratto int NOT NULL,
  Decina int,
  Volte int,
  Posizione int
)


--
-- Definition for table ARCHIVIO_PREVISIONI : 
--

CREATE TABLE ARCHIVIO_PREVISIONI (
  Data_Previsione smalldatetime NULL,
  Ruota int,
  Previsione int,
  Num_Estrazione_Succ int,
  Tipo_Frequenza int,
  Segnale int,
  Previsioni int,
  PrevAmbo int,
  Attendibilita int,
  Distanza_Segnale int,
  Pendenza int
)
GO

--
-- Definition for table Archivio_Segnali : 
--

CREATE TABLE Archivio_Segnali (
  Segnali int NOT NULL,
  Data_Segnale smalldatetime NULL,
  Parametri_Segnale int NULL,
  Parametri_Calcolo int NULL,
  Pendenza int NULL,
  DistSegnaleNegativo int NULL,
  Heigth_Segnale int NULL,
  Ruota int NULL,
  Accoppiata int NULL
)
GO

--
-- Definition for table CalcUnion : NOT USED
--

CREATE TABLE CalcUnion (
  NumCalcolo int NOT NULL,
  Tipo int NULL,
  Descrizione varchar(24) COLLATE Latin1_General_CI_AS NULL,
  DataInizio smalldatetime NULL,
  DataFine smalldatetime NULL
)
GO

--
-- Definition for table Combinazioni : 
--

CREATE TABLE Combinazioni (
  NumeriPerBolletta int NOT NULL,
  TipoSorte int NOT NULL,
  Combinazioni int NULL,
  Vincita int NULL,
  IndiceDiEq int NULL
)
GO

--
-- Definition for table Decine_Frequenti : NOT USED
--

CREATE TABLE Decine_Frequenti (
  Decina int NOT NULL,
  Ruota int NOT NULL,
  NumEstrDiSortita int NOT NULL,
  QtaNumInDecina int NULL,
  Data_Evento smalldatetime NULL,
  Scompensazione int NULL,
  Frequenza int NULL
)
GO

--
-- Definition for table ESTRAZIONI : 
--

CREATE TABLE ESTRAZIONI (
  Estrazioni int NOT NULL,
  Numero int NULL,
  Data smalldatetime NULL,
  Ruota int NULL,
  N1 int NULL,
  N2 int NULL,
  N3 int NULL,
  N4 int NULL,
  N5 int NULL,
  IdEstrazione int NULL
)
GO

--
-- Definition for table Gemelli : 
--

CREATE TABLE Gemelli (
  Gemelli int NOT NULL,
  Ruota int NULL,
  Gemello int NULL,
  Data smalldatetime NULL
)
GO

--
-- Definition for table Giocate : 
--

CREATE TABLE Giocate (
  Giocate int NOT NULL,
  Descrizione varchar(128) COLLATE Latin1_General_CI_AS NULL,
  Data_Inizio smalldatetime NULL,
  Data_Fine smalldatetime NULL,
  Tot_Investito float NULL,
  Tot_Guadagnato float NULL,
  Utile_Min float NULL,
  Utile_Min_Perc int NULL,
  Capitale_Iniziale float NULL,
  Capitale_Attuale float NULL,
  Quota_Iniziale float NULL
)
GO

--
-- Definition for table Grafici : 
--

CREATE TABLE Grafici (
  Ruote varchar(20) COLLATE Latin1_General_CI_AS NOT NULL,
  Decine varchar(3) COLLATE Latin1_General_CI_AS NULL,
  TipoLinea int NOT NULL,
  DataInizio smalldatetime NULL,
  ID int NOT NULL
)
GO

--
-- Definition for table Impostazioni_Giocate : 
--

CREATE TABLE Impostazioni_Giocate (
  Impostazioni_Giocate int NOT NULL,
  Giocata int NULL,
  Data smalldatetime NULL,
  Tipo_Giocata varchar(16) COLLATE Latin1_General_CI_AS NULL,
  N1 int NULL,
  N2 int NULL,
  N3 int NULL,
  N4 int NULL,
  N5 int NULL,
  N6 int NULL,
  Ruota int NULL,
  Quota_Investita float NULL,
  Guadagnato float NULL
)
GO

--
-- Definition for table Incroci : 
--

CREATE TABLE Incroci (
  Incroci int NOT NULL,
  Previsione int NULL,
  Occorrenze int NULL,
  Ruota int NULL,
  Data smalldatetime NULL
)
GO

--
-- Definition for table Lotto : 
--

CREATE TABLE Lotto (
  DATA smalldatetime NOT NULL,
  Numero int NULL,
  BA1 int NULL,
  BA2 int NULL,
  BA3 int NULL,
  BA4 int NULL,
  BA5 int NULL,
  CA1 int NULL,
  CA2 int NULL,
  CA3 int NULL,
  CA4 int NULL,
  CA5 int NULL,
  FI1 int NULL,
  FI2 int NULL,
  FI3 int NULL,
  FI4 int NULL,
  FI5 int NULL,
  GE1 int NULL,
  GE2 int NULL,
  GE3 int NULL,
  GE4 int NULL,
  GE5 int NULL,
  MI1 int NULL,
  MI2 int NULL,
  MI3 int NULL,
  MI4 int NULL,
  MI5 int NULL,
  NA1 int NULL,
  NA2 int NULL,
  NA3 int NULL,
  NA4 int NULL,
  NA5 int NULL,
  PA1 int NULL,
  PA2 int NULL,
  PA3 int NULL,
  PA4 int NULL,
  PA5 int NULL,
  RM1 int NULL,
  RM2 int NULL,
  RM3 int NULL,
  RM4 int NULL,
  RM5 int NULL,
  TO1 int NULL,
  TO2 int NULL,
  TO3 int NULL,
  TO4 int NULL,
  TO5 int NULL,
  VE1 int NULL,
  VE2 int NULL,
  VE3 int NULL,
  VE4 int NULL,
  VE5 int NULL,
  IdEstrazione int NULL
)
GO

--
-- Definition for table MagicNumbers : 
--

CREATE TABLE MagicNumbers (
  ID int NOT NULL,
  Numero float NOT NULL,
  NumeroAssociato float NULL,
  Moltiplicatore1 float NULL,
  Moltiplicatore2 float NULL,
  Moltiplicatore3 float NULL,
  Spread float NULL,
  Descrizione varchar(64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

EXEC sp_addextendedproperty 'MS_Description', N'Primary key', 'schema', 'Admin', 'table', 'MagicNumbers', 'column', 'ID'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Numero magico', 'schema', 'Admin', 'table', 'MagicNumbers', 'column', 'Numero'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Numero associato al numero magico', 'schema', 'Admin', 'table', 'MagicNumbers', 'column', 'NumeroAssociato'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Valore da moltiplicare col numero magico per ottenere un numero normalente di scala diversa(es 25950 * 0.1 = 2595 che è un numero da considerare)', 'schema', 'Admin', 'table', 'MagicNumbers', 'column', 'Moltiplicatore1'
GO

EXEC sp_addextendedproperty 'MS_Description', N'E'' un valore di soglia entro il quale il numero confrontato col numero magico è considerato simile o uguale allo stesso numero magico, quindi da prendere in considerazione', 'schema', 'Admin', 'table', 'MagicNumbers', 'column', 'Spread'
GO

--
-- Definition for table messages : 
--

CREATE TABLE messages (
  MsgKey int NULL,
  ITA nvarchar(128) COLLATE Latin1_General_CI_AS NULL,
  ENG nvarchar(128) COLLATE Latin1_General_CI_AS NULL,
  FRA nvarchar(128) COLLATE Latin1_General_CI_AS NULL,
  TED nvarchar(128) COLLATE Latin1_General_CI_AS NULL,
  SPA nvarchar(128) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table Numeri_Frequenti : 
--

CREATE TABLE Numeri_Frequenti (
  Numero int NOT NULL,
  Ruota int NOT NULL,
  NumEstrDiSortita int NOT NULL,
  Data_Evento smalldatetime NULL,
  Scompensazione int NULL,
  Frequenza int NULL,
  Decina int NULL
)
GO

--
-- Definition for table Numeri_Pazzaglia : 
--

CREATE TABLE Numeri_Pazzaglia (
  NumSpia int NOT NULL,
  N2 int NOT NULL,
  Decina int NULL,
  Ruota int NOT NULL
)
GO

--
-- Definition for table Numeri_Sfaldamento : 
--

CREATE TABLE Numeri_Sfaldamento (
  Ruota int NOT NULL,
  Data smalldatetime NOT NULL,
  Numero int NOT NULL,
  Sfaldamento int NULL
)
GO

--
-- Definition for table Numeri_Vicini : 
--

CREATE TABLE Numeri_Vicini (
  N1 int NOT NULL,
  N2 int NOT NULL,
  Data_Evento smalldatetime NOT NULL,
  DistN1N2 int NOT NULL,
  Ruota int NOT NULL
)
GO

--
-- Definition for table NumeriInFrequenza : 
--

CREATE TABLE NumeriInFrequenza (
  Data_Fine smalldatetime NOT NULL,
  Data_Inizio smalldatetime NOT NULL,
  AmpiezzaEstr int NOT NULL,
  Numero int NOT NULL,
  Ruota int NOT NULL,
  Data_Evento smalldatetime NOT NULL,
  Frequenza int NULL,
  Colpo int NULL,
  Decina int NULL
)
GO

--
-- Definition for table Parametri_Calcolo : 
--

CREATE TABLE Parametri_Calcolo (
  Parametri_Calcolo int NOT NULL,
  Colpo1 bit NULL,
  Colpo2 bit NULL,
  Colpo3 bit NULL,
  F_Dir_1R bit NULL,
  F_Dir_2R bit NULL,
  F_Ind_2R bit NULL,
  F_Ind_1R bit NULL,
  Decine varchar(3) COLLATE Latin1_General_CI_AS NULL,
  EstrDecGem varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Data_Inizio smalldatetime NULL,
  Data_Fine smalldatetime NULL
)
GO

--
-- Definition for table Parametri_Pendenze : 
--

CREATE TABLE Parametri_Pendenze (
  Parametri_Pendenze int NOT NULL,
  Ang_Linea_Apertura_Da int NULL,
  Ang_Linea_Chiusura_Da int NULL,
  Ang_Linea_Segnale_Da int NULL,
  Ang_Linea_Apertura_A int NULL,
  Ang_Linea_Chiusura_A int NULL,
  Ang_Linea_Segnale_A int NULL,
  Diff_Ang_LSegnAper_Da int NULL,
  Diff_Ang_LSegnAper_A int NULL
)
GO

--
-- Definition for table Parametri_Segnale : 
--

CREATE TABLE Parametri_Segnale (
  Parametri_Segnale int NOT NULL,
  Ampiezza_Freq1 int NULL,
  Ampiezza_Freq2 int NULL,
  Ampiezza_CanaleA float NULL,
  Ampiezza_CanaleB float NULL,
  Tipo_Media1 varchar(16) COLLATE Latin1_General_CI_AS NULL,
  Tipo_Media2 varchar(16) COLLATE Latin1_General_CI_AS NULL,
  Stop_Giocate int NULL,
  Go_Giocate int NULL,
  Heigth_Segnale_Da float NULL,
  Heigth_Segnale_A float NULL,
  Linea_Principale int NULL,
  Linea_Apertura int NULL,
  Linea_Chiusura int NULL,
  Parametri_Calcolo int NULL,
  Parametri_Pendenze int NULL,
  SegnAperturaPos varchar(1) COLLATE Latin1_General_CI_AS NULL,
  SegnAperturaNeg varchar(1) COLLATE Latin1_General_CI_AS NULL,
  SegnChiusuraPos varchar(1) COLLATE Latin1_General_CI_AS NULL,
  SegnChiusuraNeg varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Descrizione varchar(80) COLLATE Latin1_General_CI_AS NULL,
  MaxPrevGiuste int NULL,
  LimitiSupInf varchar(1) COLLATE Latin1_General_CI_AS NULL,
  CanaleAVariabile varchar(1) COLLATE Latin1_General_CI_AS NULL,
  CanaleBVariabile varchar(1) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table Parametri_SegnDecine : 
--

CREATE TABLE Parametri_SegnDecine (
  Id int NOT NULL,
  Ampiezza int NULL,
  EsisteGruppoFormazione varchar(1) COLLATE Latin1_General_CI_AS NULL,
  EsisteConferma varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Conf2NumSegn varchar(1) COLLATE Latin1_General_CI_AS NULL,
  EsisteAmboInDecPrec varchar(1) COLLATE Latin1_General_CI_AS NULL,
  EsisteAmboInDecSucc varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Saturazione int NULL,
  EsisteVuoto varchar(1) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table Pendenze : 
--

CREATE TABLE Pendenze (
  Pendenze int NOT NULL,
  Data smalldatetime NULL,
  Pendenza_LApertura int NULL,
  Pendenza_LSegnale int NULL,
  Pendenza_LChiusura int NULL,
  RuotaOAcc varchar(1) COLLATE Latin1_General_CI_AS NULL,
  NumRuotaOAcc int NULL,
  DifferenzaLineeIncrocio int NULL,
  AperturaChiusuraSegn int DEFAULT 0 NULL
)
GO

EXEC sp_addextendedproperty 'MS_Description', N'0: pendenza riferita al segnale di apertura
1: pendenza su chiusura segnale', 'schema', 'Admin', 'table', 'Pendenze', 'column', 'AperturaChiusuraSegn'
GO

--
-- Definition for table Picchi : 
--

CREATE TABLE Picchi (
  IDGrafico int NOT NULL,
  X int NOT NULL,
  Data smalldatetime NULL,
  Valore float NULL,
  Tipo int NULL
)
GO

--
-- Definition for table PREVISIONI : 
--

CREATE TABLE PREVISIONI (
  Data_Previsione smalldatetime NOT NULL,
  Ruota int NOT NULL,
  Previsione int NOT NULL,
  Num_Estrazione_Succ int NULL,
  Tipo_Frequenza int NULL,
  PrevAmbo int NULL,
  Previsioni int NOT NULL,
  PrevTerno int NULL,
  Segnale int NOT NULL,
  Attendibilita int NULL,
  Distanza_Segnale int NULL,
  Pendenza int NULL,
  Heigth_Prev_Prec int NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS DEFAULT 'F' NULL
)
GO

EXEC sp_addextendedproperty 'MS_Description', N'Non è usato dato che la pendenza viene inserita solo per il segnale (26-01-2011)', 'schema', 'Admin', 'table', 'PREVISIONI', 'column', 'Pendenza'
GO

--
-- Definition for table PrevisioniCadenze : 
--

CREATE TABLE PrevisioniCadenze (
  PrevisioniCadenze int NOT NULL,
  Data_Previsione smalldatetime NULL,
  Ruota int NULL,
  Previsione int NULL,
  PrevAmbo int NULL,
  PrevTerno int NULL,
  Attendibilita int NULL
)
GO

--
-- Definition for table PrevisioniDecine : 
--

CREATE TABLE PrevisioniDecine (
  Data_Previsione smalldatetime NULL,
  Ruota int NULL,
  Previsione int NULL,
  Segnale int NULL,
  PrevAmbo int NULL,
  PrevisioniDecine int NOT NULL,
  PrevTerno int NULL,
  Attendibilita int NULL,
  Distanza_Segnale int NULL
)
GO

--
-- Definition for table PrevisioniDecineTMP : 
--

CREATE TABLE PrevisioniDecineTMP (
  Data_Previsione smalldatetime NULL,
  Ruota int NULL,
  Previsione int NULL,
  Segnale int NULL,
  PrevAmbo int NULL,
  PrevisioniDecine int NOT NULL,
  PrevTerno int NULL,
  Attendibilita int NULL,
  Distanza_Segnale int NULL
)
GO

--
-- Definition for table PrevisioniNumFrequenti : 
--

CREATE TABLE PrevisioniNumFrequenti (
  Data_Previsione smalldatetime NULL,
  Ruota int NULL,
  Previsione int NULL,
  PrevAmbo int NULL,
  PrevisioniNumFrequenti int NOT NULL,
  PrevTerno int NULL,
  Segnale int NULL,
  Attendibilita int NULL,
  Distanza_Segnale int NULL,
  Pendenza int NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL,
  NumDataPrev int NULL
)
GO

--
-- Definition for table PrevisioniNumSpia : 
--

CREATE TABLE PrevisioniNumSpia (
  PrevisioniNumSpia int NOT NULL,
  Data_Previsione smalldatetime NULL,
  Ruota int NULL,
  Previsione int NULL,
  PrevAmbo int NULL,
  PrevTerno int NULL,
  Attendibilita int NULL,
  Segnale int NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table PrevisioniPosizione : 
--

CREATE TABLE PrevisioniPosizione (
  PrevisioniPosizione int NOT NULL,
  Data_Previsione smalldatetime NULL,
  Segnale int NULL,
  Ruota int NULL,
  Previsione int NULL,
  Distanza_Segnale int NULL,
  PrevAmbo int NULL,
  Posizione int NULL
)
GO

--
-- Definition for table PREVISIONIRITARDI : 
--

CREATE TABLE PREVISIONIRITARDI (
  Data_Previsione smalldatetime NULL,
  Ruota int NULL,
  Previsione int NULL,
  Num_Estrazione_Succ int NULL,
  Tipo_Frequenza int NULL,
  PrevAmbo int NULL,
  Previsioni int NOT NULL,
  PrevTerno int NULL,
  Segnale int NULL,
  Attendibilita int NULL,
  Distanza_Segnale int NULL
)
GO

--
-- Definition for table Proprieta_Decine : 
--

CREATE TABLE Proprieta_Decine (
  Decina int NOT NULL,
  Ruota int NOT NULL,
  NumEstrDiSortita int NOT NULL,
  QtaNumInDecina int NULL,
  Data_Evento smalldatetime NOT NULL,
  Scompensazione int NULL,
  Saturazione int NULL,
  Intervallo_Saturazione int NULL,
  Frequenza int NULL
)
GO

--
-- Definition for table Proprieta_Estratti : 
--

CREATE TABLE Proprieta_Estratti (
  Numero int NOT NULL,
  Ruota int NOT NULL,
  NumEstrDiSortita int NOT NULL,
  Data_Evento smalldatetime NOT NULL,
  Scompensazione int NULL,
  Saturazione int NULL,
  Intervallo_Saturazione int NULL,
  Decina int NULL,
  Frequenza int NULL,
  DistUltimaUscita int NULL
)
GO

--
-- Definition for table Regola1 : 
--

CREATE TABLE Regola1 (
  Data_Evento smalldatetime NOT NULL,
  Ruota int NOT NULL,
  NumeroA int NOT NULL,
  Tipo_Frequenza int NOT NULL,
  Num_Estrazione_Succ int NOT NULL,
  Regola1 int NULL,
  Data_Numero smalldatetime NULL,
  Accoppiata int NULL,
  NumeroB int NULL,
  Frequenza int NULL,
  Distanza_Ruota int NULL,
  Distanza_Accoppiata int NULL,
  Posizione int NULL
)
GO

--
-- Definition for table REGOLA1_DECINE : 
--

CREATE TABLE REGOLA1_DECINE (
  Data_Evento smalldatetime NOT NULL,
  Ruota int NOT NULL,
  NumeroA int NOT NULL,
  Tipo_Frequenza int NOT NULL,
  Num_Estrazione_Succ int NOT NULL,
  Regola1_Decine int NULL,
  Data_Numero smalldatetime NULL,
  Accoppiata int NULL,
  NumeroB int NULL,
  DecinaA int NULL,
  DecinaB int NULL,
  Frequenza int NULL,
  Volte int NULL,
  Distanza_Accoppiata int NULL,
  Distanza_Ruota int NULL
)
GO

--
-- Definition for table Regola1_DecineB : 
--

CREATE TABLE Regola1_DecineB (
  Regola1_DecineB int NOT NULL,
  Data_Evento smalldatetime NULL,
  Ruota int NULL,
  Accoppiata int NULL,
  NumeroA int NULL,
  DecinaA int NULL,
  Volte int NULL
)
GO

--
-- Definition for table Regola1ALL : 
--

CREATE TABLE Regola1ALL (
  Data_Evento smalldatetime NULL,
  Ruota int NULL,
  NumC1D int NULL,
  NumC1I int NULL,
  NumC2D int NULL,
  NumC2I int NULL,
  NumC3D int NULL,
  NumC3I int NULL,
  Accopppiata int NULL,
  Decina1 int NULL,
  Decina2 int NULL,
  Regola1ALL int NOT NULL
)
GO

--
-- Definition for table Regola1ALLDec : 
--

CREATE TABLE Regola1ALLDec (
  Data_Evento smalldatetime NOT NULL,
  Ruota int NOT NULL,
  NumeroA int NOT NULL,
  Tipo_Frequenza int DEFAULT 1 NOT NULL,
  Num_Estrazione_Succ int DEFAULT 1 NOT NULL,
  Accoppiata int NULL,
  Decina1 int NULL,
  Decina2 int NULL,
  Decine12 int NOT NULL
)
GO

--
-- Definition for table Regola1ALLDec_Params : 
--

CREATE TABLE Regola1ALLDec_Params (
  Data_Inizio smalldatetime NOT NULL,
  Decina1 int NULL,
  Decina2 int NULL,
  Decine12 int NOT NULL,
  Data_Fine smalldatetime NOT NULL
)
GO

--
-- Definition for table Regola1B : 
--

CREATE TABLE Regola1B (
  Regola1B int NOT NULL,
  Data_Evento smalldatetime NULL,
  Accoppiata int NULL,
  Ruota int NULL,
  NumeroA int NULL
)
GO

--
-- Definition for table Regola1DecALLDec : 
--

CREATE TABLE Regola1DecALLDec (
  Data_Evento smalldatetime NOT NULL,
  Ruota int NOT NULL,
  NumeroA int NOT NULL,
  Tipo_Frequenza int NOT NULL,
  Num_Estrazione_Succ int NOT NULL,
  Accoppiata int NULL,
  Decina int NULL,
  PosNumInDec int NULL,
  Volte int NULL,
  Decine12 int DEFAULT 41 NOT NULL
)
GO

--
-- Definition for table Regola1Filtri : 
--

CREATE TABLE Regola1Filtri (
  Regola1Filtri int NOT NULL,
  Tipo_Filtro int NULL,
  Accoppiata int NULL,
  Ruota int NULL,
  Data_Evento smalldatetime NULL,
  Data_Segnale smalldatetime NULL,
  NumeroA int NULL,
  NumeroB int NULL,
  Frequenza int NULL,
  Tipo_Frequenza int NULL,
  Num_Estrazione_Succ int NULL,
  Distanza_Ruota int NULL,
  Distanza_Segnale int NULL,
  Attendibilita int NULL,
  Giocate int NULL,
  Giocate_Totali int NULL
)
GO

--
-- Definition for table Regola1GemALLDec : 
--

CREATE TABLE Regola1GemALLDec (
  Data_Evento smalldatetime NOT NULL,
  Ruota int NOT NULL,
  NumeroA int NOT NULL,
  Tipo_Frequenza int NOT NULL,
  Num_Estrazione_Succ int NOT NULL,
  Accoppiata int NULL,
  Decina1 int NULL,
  Decina2 int NULL,
  Volte int NULL,
  Decine12 int NOT NULL,
  Gemello int NOT NULL
)
GO

--
-- Definition for table Ripetizioni : 
--

CREATE TABLE Ripetizioni (
  ID int NOT NULL,
  Data_Evento smalldatetime NULL,
  IDEstrRipetizioneParziale int NULL,
  IDEstrRipetizioneTotale int DEFAULT -1 NULL,
  RipetizioneTotale int DEFAULT 0 NULL,
  IDGruppo int NULL,
  DistRipetizionePrec int NULL,
  RigaGriglia int NULL,
  Array varchar(180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

EXEC sp_addextendedproperty 'MS_Description', N'Id estrazione in cui si verifica la somiglianza parziale', 'schema', 'Admin', 'table', 'Ripetizioni', 'column', 'IDEstrRipetizioneParziale'
GO

EXEC sp_addextendedproperty 'MS_Description', N'1: totale; 0: parziale secondo parametro', 'schema', 'Admin', 'table', 'Ripetizioni', 'column', 'RipetizioneTotale'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Identificativo del gruppo, identificato a sua volta dall''array dei valori cercati', 'schema', 'Admin', 'table', 'Ripetizioni', 'column', 'IDGruppo'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Distanza in estrazioni dalla ripetizione precedente', 'schema', 'Admin', 'table', 'Ripetizioni', 'column', 'DistRipetizionePrec'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Numero di roiga della griglia, utile quando non si dispone dell''informazione relativa alla data o all''id dell''estrazione', 'schema', 'Admin', 'table', 'Ripetizioni', 'column', 'RigaGriglia'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Array di valori simile a quello confrontato presente nella tabella RipetizioniGruppo', 'schema', 'Admin', 'table', 'Ripetizioni', 'column', 'Array'
GO

--
-- Definition for table RipetizioniGruppo : 
--

CREATE TABLE RipetizioniGruppo (
  ID int NOT NULL,
  Array varchar(180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  Ruota int NULL,
  MinIndRipetizioneParziale int NULL,
  Spread float NULL,
  TipoBaseNumerica int NULL,
  Col int NULL,
  IDEstrPrimaManifestazione int NULL,
  LastArrayVal float NULL,
  SecondLastArrayVal float NULL
)
GO

EXEC sp_addextendedproperty 'MS_Description', N'insieme di valori separati da -', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'Array'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Identifrica la ruota, non è smepre usato perchè le serie possono prescindere dalla ruota', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'Ruota'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Numerpo di elementi minimo per stabilire che la ripetizione è parziale', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'MinIndRipetizioneParziale'
GO

EXEC sp_addextendedproperty 'MS_Description', N'valore di canale in cui considerare valida la somiglianza in altezza', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'Spread'
GO

EXEC sp_addextendedproperty 'MS_Description', N'parametro che indica il tipo di base numerica usata: 1: regola1,.. 4: numeri scelti
campo non sempre usato: non è detto che la sequenza numerica sia riferita a una griglia generato dal tipo doi base numerica', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'TipoBaseNumerica'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Colonna della griglia in cui è stat effettuata la ricerca. Serve per discriminare una ricerca da un''altra', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'Col'
GO

EXEC sp_addextendedproperty 'MS_Description', N'ID dell''estrazione della prima volta che si manifesta la ripetizione; è utile nmel caso di calcolo ricorsivo in cui dato un array comincio a controllare la sua esistenza dall''estrazione successiva', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'IDEstrPrimaManifestazione'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Ultimo valore dell''array cercato; unita con l''info simile sulla penultima pos dell''array è utile per filtrare solo i gruppi con determinate caratteristiche nelle ultime posizioni dell''array', 'schema', 'Admin', 'table', 'RipetizioniGruppo', 'column', 'LastArrayVal'
GO

--
-- Definition for table Risultati : 
--

CREATE TABLE dbo.Risultati (
  BA1 int NULL,
  BA2 int NULL,
  BA3 int NULL,
  BA4 int NULL,
  BA5 int NULL,
  CA1 int NULL,
  CA2 int NULL,
  CA3 int NULL,
  CA4 int NULL,
  CA5 int NULL,
  DATA smalldatetime NULL,
  FI1 int NULL,
  FI2 int NULL,
  FI3 int NULL,
  FI4 int NULL,
  FI5 int NULL,
  GE1 int NULL,
  GE2 int NULL,
  GE3 int NULL,
  GE4 int NULL,
  GE5 int NULL,
  IdEstrazione int NULL,
  MI1 int NULL,
  MI2 int NULL,
  MI3 int NULL,
  MI4 int NULL,
  MI5 int NULL,
  NA1 int NULL,
  NA2 int NULL,
  NA3 int NULL,
  NA4 int NULL,
  NA5 int NULL,
  Numero int NULL,
  PA1 int NULL,
  PA2 int NULL,
  PA3 int NULL,
  PA4 int NULL,
  PA5 int NULL,
  RM1 int NULL,
  RM2 int NULL,
  RM3 int NULL,
  RM4 int NULL,
  RM5 int NULL,
  TO1 int NULL,
  TO2 int NULL,
  TO3 int NULL,
  TO4 int NULL,
  TO5 int NULL,
  VE1 int NULL,
  VE2 int NULL,
  VE3 int NULL,
  VE4 int NULL,
  VE5 int NULL
)
GO

--
-- Definition for table Ruote : 
--

CREATE TABLE Ruote (
  Ruote int NOT NULL,
  Descrizione varchar(50) COLLATE Latin1_General_CI_AS NULL,
  Accoppiata int NULL,
  Posizione_Accoppiata int NULL,
  Sigla varchar(2) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table Segnali : 
--

CREATE TABLE Segnali (
  Segnali int NOT NULL,
  Data_Segnale smalldatetime NULL,
  Parametri_Segnale int NULL,
  Parametri_Calcolo int NULL,
  Pendenza int NULL,
  DistSegnaleNegativo int NULL,
  Heigth_Segnale float NULL,
  Ruota int NULL,
  Accoppiata int NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Decine varchar(3) COLLATE Latin1_General_CI_AS NULL,
  Decine12 int NULL,
  ChiusuraAutomatica int DEFAULT 0 NULL
)
GO

EXEC sp_addextendedproperty 'MS_Description', N'Vale per i segnali da regola 1 e contiene la prima decina * 10 + la decina della seconda ruota', 'schema', 'Admin', 'table', 'Segnali', 'column', 'Decine12'
GO

EXEC sp_addextendedproperty 'MS_Description', N'0: se chiuso da segnale negativo
1: se chiuso automaticamente alla comparsa di un nuoivo segnale di apertura', 'schema', 'Admin', 'table', 'Segnali', 'column', 'ChiusuraAutomatica'
GO

--
-- Definition for table SegnaliDecine : 
--

CREATE TABLE SegnaliDecine (
  Segnali int NOT NULL,
  Data_Segnale smalldatetime NULL,
  Ruota int NULL,
  IdParams int NULL,
  TypeSegnal varchar(10) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table SegnaliNumFrequenti : 
--

CREATE TABLE SegnaliNumFrequenti (
  Segnali int NOT NULL,
  Data_Segnale smalldatetime NULL,
  Pendenza int NULL,
  DistSegnaleNegativo int NULL,
  Heigth_Segnale float NULL,
  Ruota int NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Data_Inizio_Prev smalldatetime NULL
)
GO

--
-- Definition for table SegnaliNumSpia : 
--

CREATE TABLE SegnaliNumSpia (
  Segnali int NOT NULL,
  Data_Segnale smalldatetime NULL,
  Ruota int NULL,
  Parametri_Segnale int NULL,
  NumSpia int NULL,
  N2 int NULL,
  Occorrenze int NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Tipo int NULL
)
GO

--
-- Definition for table SegnaliPosizione : 
--

CREATE TABLE SegnaliPosizione (
  Segnali int NOT NULL,
  Data_Segnale smalldatetime NULL,
  Ruota int NULL,
  Posizione int NULL,
  Decine varchar(3) COLLATE Latin1_General_CI_AS NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL,
  Tipo_Frequenza int NULL,
  Colpo int NULL
)
GO

--
-- Definition for table SegnaliRitardi : 
--

CREATE TABLE SegnaliRitardi (
  Segnali int NOT NULL,
  Data_Segnale smalldatetime NULL,
  DistSegnaleNegativo int NULL,
  Ruota int NULL,
  Accoppiata int NULL,
  Decine varchar(3) COLLATE Latin1_General_CI_AS NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table Tipo_Frequenza : 
--

CREATE TABLE Tipo_Frequenza (
  Tipo_Frequenza int NOT NULL,
  Descrizione varchar(128) COLLATE Latin1_General_CI_AS NULL,
  Sigla varchar(50) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table Tipo_Linee : 
--

CREATE TABLE Tipo_Linee (
  Tipo_Linee int NOT NULL,
  Descrizione varchar(50) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table UNIONPREVISIONI : 
--

CREATE TABLE UNIONPREVISIONI (
  UnionPrev int NULL,
  Data_Previsione smalldatetime NULL,
  Tipo_Previsione int NULL,
  Ruota int NULL,
  Previsione int NULL,
  PrevAmbo int NULL,
  PrevTerno int NULL,
  Attendibilita int NULL,
  Investito float NULL,
  Guadagnato float NULL,
  NumCalcolo int NULL,
  Filtrato varchar(1) COLLATE Latin1_General_CI_AS NULL
)
GO

--
-- Definition for table UNIONPREVISIONI2 : 
--

CREATE TABLE UNIONPREVISIONI2 (
  UnionPrev int NULL,
  Data_Previsione smalldatetime NULL,
  Tipo_Previsione int NULL,
  Ruota int NULL,
  Previsione2 int NULL,
  Attendibilita int NULL,
  Investito float NULL,
  Guadagnato float NULL,
  NumCalcolo int NULL
)
GO

--
-- Definition for table ValoriGrafico : 
--

CREATE TABLE ValoriGrafico (
  IDGrafico int NOT NULL,
  X int NOT NULL,
  Y float NULL,
  Data smalldatetime NULL
)
GO

--
-- Definition for indices : 
--

ALTER TABLE Accoppiate
ADD CONSTRAINT PK_Accoppiate 
PRIMARY KEY CLUSTERED (Accoppiate)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Ambi
ADD CONSTRAINT PK_Ambi 
PRIMARY KEY CLUSTERED (Ambi)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE AmbiInDecina
ADD CONSTRAINT PK_AmbiInDecina 
PRIMARY KEY CLUSTERED (Data, Ruota, Estratto)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Archivio_Segnali
ADD CONSTRAINT PK_Archivio_Segnali 
PRIMARY KEY CLUSTERED (Segnali)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE CalcUnion
ADD CONSTRAINT PK_CalcUnion 
PRIMARY KEY CLUSTERED (NumCalcolo)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)

GO

ALTER TABLE Combinazioni
ADD CONSTRAINT PK_Combinazioni 
PRIMARY KEY CLUSTERED (NumeriPerBolletta, TipoSorte)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Decine_Frequenti
ADD CONSTRAINT PK_DecFreq 
PRIMARY KEY CLUSTERED (NumEstrDiSortita, Ruota, Decina)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

CREATE UNIQUE NONCLUSTERED INDEX AK_IdEstr ON ESTRAZIONI
  (IdEstrazione, Ruota)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

CREATE UNIQUE NONCLUSTERED INDEX Data_Ruota ON ESTRAZIONI
  (Data, Ruota)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE ESTRAZIONI
ADD CONSTRAINT PK_ESTRAZIONI 
PRIMARY KEY CLUSTERED (Estrazioni)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Gemelli
ADD CONSTRAINT PK_Gemelli 
PRIMARY KEY CLUSTERED (Gemelli)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Giocate
ADD CONSTRAINT PK_Giocate 
PRIMARY KEY CLUSTERED (Giocate)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Grafici
ADD CONSTRAINT PK_Grafici 
PRIMARY KEY CLUSTERED (ID)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Impostazioni_Giocate
ADD CONSTRAINT PK_Impostazioni_Giocate 
PRIMARY KEY CLUSTERED (Impostazioni_Giocate)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Incroci
ADD CONSTRAINT PK_Incroci 
PRIMARY KEY CLUSTERED (Incroci)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE UNIQUE NONCLUSTERED INDEX AK_IdEstr ON Lotto
  (IdEstrazione)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Lotto
ADD CONSTRAINT PK_Lotto 
PRIMARY KEY CLUSTERED (DATA)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE UNIQUE NONCLUSTERED INDEX MagicNumbers_uq ON MagicNumbers
  (Numero)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE MagicNumbers
ADD PRIMARY KEY CLUSTERED (ID)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON Numeri_Frequenti
  (Ruota, Data_Evento)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Numeri_Frequenti
ADD CONSTRAINT PK_NumFreq 
PRIMARY KEY CLUSTERED (NumEstrDiSortita, Ruota, Numero)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Numeri_Pazzaglia
ADD CONSTRAINT PK_NUMPAZZ 
PRIMARY KEY CLUSTERED (NumSpia, N2, Ruota)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Numeri_Sfaldamento
ADD CONSTRAINT PK_Sfaldammento 
PRIMARY KEY CLUSTERED (Ruota, Data, Numero)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_Data ON Numeri_Vicini
  (Ruota, Data_Evento)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Numeri_Vicini
ADD CONSTRAINT PK_NumVicini 
PRIMARY KEY CLUSTERED (N1, N2, Data_Evento, DistN1N2, Ruota)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON NumeriInFrequenza
  (Ruota, Data_Evento)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE NumeriInFrequenza
ADD CONSTRAINT PK_NumInFreq 
PRIMARY KEY CLUSTERED (Data_Fine, Data_Inizio, AmpiezzaEstr, Numero, Ruota, Data_Evento)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Parametri_Calcolo
ADD CONSTRAINT PK_Parametri_Calcolo 
PRIMARY KEY CLUSTERED (Parametri_Calcolo)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Parametri_Pendenze
ADD CONSTRAINT PK_Parametri_Pendenze 
PRIMARY KEY CLUSTERED (Parametri_Pendenze)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Parametri_Segnale
ADD CONSTRAINT PK_Parametri_Segnale 
PRIMARY KEY CLUSTERED (Parametri_Segnale)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Parametri_SegnDecine
ADD CONSTRAINT IX_ParamSegnDec 
PRIMARY KEY NONCLUSTERED (Id)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaAccData ON Pendenze
  (RuotaOAcc, Data)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Pendenze
ADD CONSTRAINT PK_Pendenze 
PRIMARY KEY CLUSTERED (Pendenze)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Picchi
ADD CONSTRAINT PK_Picchi 
PRIMARY KEY CLUSTERED (IDGrafico, X)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON PREVISIONI
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PREVISIONI
ADD CONSTRAINT PREVISIONI_pk 
PRIMARY KEY CLUSTERED (Previsioni)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX IX_KEY ON PrevisioniCadenze
  (Data_Previsione, Ruota, Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PrevisioniCadenze
ADD CONSTRAINT pk_PrevCadenze 
PRIMARY KEY CLUSTERED (PrevisioniCadenze)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON PrevisioniDecine
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PrevisioniDecine
ADD CONSTRAINT IX_PrevDec 
PRIMARY KEY NONCLUSTERED (PrevisioniDecine)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PrevisioniDecineTMP
ADD CONSTRAINT IX_PrevDecTmp 
PRIMARY KEY NONCLUSTERED (PrevisioniDecine)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON PrevisioniNumFrequenti
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PrevisioniNumFrequenti
ADD CONSTRAINT PK_PrevNumFreq 
PRIMARY KEY CLUSTERED (PrevisioniNumFrequenti)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON PrevisioniNumSpia
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PrevisioniNumSpia
ADD CONSTRAINT PK_PrevNumSpia 
PRIMARY KEY CLUSTERED (PrevisioniNumSpia)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON PrevisioniPosizione
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PrevisioniPosizione
ADD CONSTRAINT PK_PrevPosizione 
PRIMARY KEY CLUSTERED (PrevisioniPosizione)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON PREVISIONIRITARDI
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX IX_DRP ON PREVISIONIRITARDI
  (Data_Previsione, Ruota, Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE PREVISIONIRITARDI
ADD CONSTRAINT IX_PrevRit 
PRIMARY KEY NONCLUSTERED (Previsioni)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Proprieta_Decine
ADD CONSTRAINT PK_PrDec 
PRIMARY KEY CLUSTERED (NumEstrDiSortita, Ruota, Decina, Data_Evento)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Proprieta_Estratti
ADD CONSTRAINT PK_ProprEstr 
PRIMARY KEY CLUSTERED (Data_Evento, NumEstrDiSortita, Ruota, Numero)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON Regola1
  (Ruota, Data_Evento)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Regola1
ADD CONSTRAINT PK_Regola1 
PRIMARY KEY CLUSTERED (Data_Evento, Ruota, NumeroA, Tipo_Frequenza, Num_Estrazione_Succ)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE REGOLA1_DECINE
ADD CONSTRAINT PK_REGOLA1_DECINE 
PRIMARY KEY CLUSTERED (Data_Evento, Ruota, NumeroA, Tipo_Frequenza, Num_Estrazione_Succ)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Regola1_DecineB
ADD CONSTRAINT PK_Regola1_DecineB 
PRIMARY KEY CLUSTERED (Regola1_DecineB)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Regola1ALL
ADD CONSTRAINT Regola1ALL_pk 
PRIMARY KEY CLUSTERED (Regola1ALL)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Regola1ALLDec
ADD CONSTRAINT PK_RuotaData_Regola1ALLDec 
PRIMARY KEY CLUSTERED (Decine12, Ruota, Data_Evento, Tipo_Frequenza, Num_Estrazione_Succ, NumeroA)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX Regola1ALLDec_idx ON Regola1ALLDec
  (Data_Evento, Decine12)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Regola1ALLDec_Params
ADD CONSTRAINT PK_RuotaData_Regola1ALLDec_Params 
PRIMARY KEY CLUSTERED (Decine12, Data_Inizio, Data_Fine)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Regola1B
ADD CONSTRAINT PK_Regola1B 
PRIMARY KEY CLUSTERED (Regola1B)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Regola1DecALLDec
ADD CONSTRAINT PK_REGOLA1ALLDecD 
PRIMARY KEY CLUSTERED (Data_Evento, Ruota, NumeroA, Tipo_Frequenza, Num_Estrazione_Succ, Decine12)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Regola1Filtri
ADD CONSTRAINT PK_Regola1Filtri 
PRIMARY KEY CLUSTERED (Regola1Filtri)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Regola1GemALLDec
ADD CONSTRAINT PK_REGOLA1ALLDecD_Regola1GemALLDec 
PRIMARY KEY CLUSTERED (Data_Evento, Ruota, NumeroA, Tipo_Frequenza, Num_Estrazione_Succ, Decine12)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Ripetizioni
ADD PRIMARY KEY CLUSTERED (ID)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX Ripetizioni_idx ON Ripetizioni
  (Data_Evento, IDGruppo)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE RipetizioniGruppo
ADD PRIMARY KEY CLUSTERED (ID)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX RipetizioniGruppo_idx ON RipetizioniGruppo
  (Array, Col)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Ruote
ADD CONSTRAINT PK_Ruote 
PRIMARY KEY CLUSTERED (Ruote)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_Data ON Segnali
  (Data_Segnale, Ruota)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON Segnali
  (Ruota, Data_Segnale)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Segnali
ADD CONSTRAINT PK_Segnali 
PRIMARY KEY CLUSTERED (Segnali)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON SegnaliDecine
  (Ruota, Data_Segnale)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE SegnaliDecine
ADD CONSTRAINT PK_SegnaliDecine 
PRIMARY KEY CLUSTERED (Segnali)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON SegnaliNumFrequenti
  (Ruota, Data_Segnale)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE SegnaliNumFrequenti
ADD CONSTRAINT PK_SegnNumFreq 
PRIMARY KEY CLUSTERED (Segnali)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON SegnaliNumSpia
  (Ruota, Data_Segnale)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE SegnaliNumSpia
ADD CONSTRAINT PK_SegnNumSpia 
PRIMARY KEY CLUSTERED (Segnali)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON SegnaliPosizione
  (Ruota, Data_Segnale)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE SegnaliPosizione
ADD CONSTRAINT PK_Posizione 
PRIMARY KEY CLUSTERED (Segnali)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON SegnaliRitardi
  (Ruota, Data_Segnale)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE SegnaliRitardi
ADD CONSTRAINT PK_SegnaliRitardi 
PRIMARY KEY CLUSTERED (Segnali)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Tipo_Frequenza
ADD CONSTRAINT PK_Tipo_Frequenza 
PRIMARY KEY CLUSTERED (Tipo_Frequenza)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE Tipo_Linee
ADD CONSTRAINT PK_Tipo_Linee 
PRIMARY KEY CLUSTERED (Tipo_Linee)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON UNIONPREVISIONI
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX IX_KEY ON UNIONPREVISIONI
  (Data_Previsione, Ruota, Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

CREATE NONCLUSTERED INDEX AK_RuotaData ON UNIONPREVISIONI2
  (Ruota, Data_Previsione)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

ALTER TABLE ValoriGrafico
ADD CONSTRAINT PK_ValGraf 
PRIMARY KEY CLUSTERED (IDGrafico, X)
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
 
GO

--
-- Role Membership
--

EXEC sp_addrolemember 'db_owner', 'Admin'
GO

EXEC sp_addrolemember 'db_ddladmin', 'Admin'
GO

EXEC sp_addrolemember 'db_owner', 'LGUserDB'
GO

EXEC sp_addrolemember 'db_ddladmin', 'LGUserDB'
GO

EXEC sp_addrolemember 'db_ddladmin', 'SuperLogin'
GO

EXEC sp_addrolemember 'db_owner', 'SuperLogin'
GO

