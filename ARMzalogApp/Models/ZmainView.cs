using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models;

public partial class ZmainView
{
    public int ZvPozn { get; set; }

    public DateTime? ZvDate { get; set; }

    public string? ZvNom { get; set; }

    public int? ZvKodkl { get; set; }

    public string? ZvNamkl { get; set; }

    public string? ZvAdr { get; set; }

    public string? ZvTel { get; set; }

    public byte? ZvStat { get; set; }

    public byte? ZvStatus { get; set; }

    public int? Interest { get; set; }

    public byte? ZvKl { get; set; }

    public string ZvKodv { get; set; } = null!;

    public decimal? ZvSum { get; set; }

    public byte? ZvVidkr { get; set; }

    public short? ZvSrok { get; set; }

    public DateTime? ZvDate1 { get; set; }

    public short? ZvShkr { get; set; }

    public byte? ZvVidob { get; set; }

    public string? ZvOpisob { get; set; }

    public byte? ZvFl { get; set; }

    public string? Otv { get; set; }

    public byte? ZvVibor { get; set; }

    public string? ZvKom2 { get; set; }

    public string? ZvKom { get; set; }

    public int? ZvPozndg { get; set; }

    public DateTime? ZvDate2 { get; set; }

    public string? AmountReq { get; set; }

    public string? AmountRecom { get; set; }

    public int? TypBis { get; set; }

    public int? Procent { get; set; }

    public string? Owner { get; set; }

    public int? CelKr { get; set; }

    public string? PriceNank { get; set; }

    public string? PriceMarket { get; set; }

    public string? Telev { get; set; }

    public string? IFam { get; set; }

    public string? KlName { get; set; }

    public string? KlOtch { get; set; }

    public int? RGod { get; set; }

    public int? ZvNational { get; set; }

    public string? ZvFmr { get; set; }

    public string? ZvIndpsrok { get; set; }

    public string? ZvDokend { get; set; }

    public DateTime? ZvIndpdater { get; set; }

    public string? ZvIndpnom { get; set; }

    public string? FioCupr { get; set; }

    public decimal? IpDol { get; set; }

    public string? BisProch { get; set; }

    public decimal? BisCom { get; set; }

    public int? BisCrok { get; set; }

    public decimal? PtrAvtoCom { get; set; }

    public string? PtrModel { get; set; }

    public decimal? PtrMavtoCom { get; set; }

    public int? PtrMavtoGod { get; set; }

    public int? PtrMavtoKpm { get; set; }

    public string? PtrMmodel { get; set; }

    public string? PtrMopic { get; set; }

    public string? Z1Jildom { get; set; }

    public string? Z1Kvart { get; set; }

    public string? Z1Nejilpom { get; set; }

    public string? Z1Mavto { get; set; }

    public string? Z1Kont { get; set; }

    public string? Z1Prochs { get; set; }

    public string? Z1Ser { get; set; }

    public string? Z1Vid { get; set; }

    public string? Z1Fio { get; set; }

    public string? Z1Adress { get; set; }

    public string? DbTipbis { get; set; }

    public string? DbReal { get; set; }

    public string? DbPeriod { get; set; }

    public string? DbGeogr { get; set; }

    public string? DbPoctavka { get; set; }

    public string? Doljnoct { get; set; }

    public string? DoljCup { get; set; }

    public string? RabCupr { get; set; }

    public string? RabNach { get; set; }

    public int? PorChislo { get; set; }

    public int? PorMec { get; set; }

    public string? PorProp { get; set; }

    public string? PorProj { get; set; }

    public string? PorObr { get; set; }

    public string? PorAkt9 { get; set; }

    public string? TelefDom { get; set; }

    public string? PasProp { get; set; }

    public string? TelefCot { get; set; }

    public string? CrokResh { get; set; }

    public int? ZvOtv { get; set; }

    public string Reshenie { get; set; } = null!;

    public string? Path5 { get; set; }

    public string? Path4 { get; set; }

    public string? Path3 { get; set; }

    public string? Path2 { get; set; }

    public int? ProcentResh { get; set; }

    public string? Path1 { get; set; }

    public int Kodb { get; set; }

    public string? ZvMvd { get; set; }

    public DateTime? ZvDatevp { get; set; }

    public string? ZvNdok { get; set; }

    public string? ZvSrdok { get; set; }

    public string? ZvDok { get; set; }

    public DateTime? ZvFdr { get; set; }

    public int? ZvGr { get; set; }

    public string? Revenue { get; set; }

    public string? ZvGrajd { get; set; }

    public decimal? AmountResh { get; set; }

    public DateTime? ZvDatareg { get; set; }

    public string? ZvOpf { get; set; }

    public string? ZvInn { get; set; }

    public decimal? RekProcent { get; set; }

    public int? RekVidpog { get; set; }

    public string? FaktAdres { get; set; }

    public int? ZvGrup { get; set; }

    public string? RegAdres { get; set; }

    public string? ZvTelfax { get; set; }

    public string? ZvGrupFio { get; set; }

    public decimal? UtvSumma { get; set; }

    public string? UtvVal { get; set; }

    public int? UtvSrok { get; set; }

    public decimal? UtvProcent { get; set; }

    public int? UtvVidpog { get; set; }

    public decimal? UtvOcCt { get; set; }

    public string? CibStat { get; set; }

    public string? CibType { get; set; }

    public short? DbPoruchRods1 { get; set; }

    public short? DbPoruchNedvij { get; set; }

    public string? PtrDopoln { get; set; }

    public short? DbPoruchAvto1 { get; set; }

    public string? ZvEmail { get; set; }

    public byte? ZvKredur { get; set; }

    public string? ZvEmailSupr { get; set; }

    public string? Refinancing { get; set; }

    public decimal? Volume { get; set; }

    public int? UrFiz { get; set; }

    public int? KlKod { get; set; }

    public string? KlAdr { get; set; }

    public int? Cycle { get; set; }

    public string? OtFio { get; set; }

    public int? OtPodrazd { get; set; }

    public string? DpNam { get; set; }

    public int? BvPozn { get; set; }

    public int? BvKodkl { get; set; }

    public string? BvFamSt { get; set; }

    public string? BvDopDoh { get; set; }

    public string? BvOsnDoh { get; set; }

    public string? BvBisAdres { get; set; }

    public string? BvRabStart { get; set; }

    public string? BvRabStaj { get; set; }

    public string? BvRabAdres { get; set; }

    public string? BvTelefSl { get; set; }

    public string? BvOstatok2 { get; set; }

    public string? BvSumma2 { get; set; }

    public string? BvProcSt2 { get; set; }

    public string? BvCrok2 { get; set; }

    public string? BvBank2 { get; set; }

    public string? BvOstatok1 { get; set; }

    public string? BvSumma1 { get; set; }

    public string? BvProcSt1 { get; set; }

    public DateTime? BvDate { get; set; }

    public string? BvKodv { get; set; }

    public string? BvObr { get; set; }

    public string? BvTelefSupr { get; set; }

    public string? BvSummaSupr { get; set; }

    public string? BvChild { get; set; }

    public string? BvNedv1 { get; set; }

    public string? BvNedvAdr1 { get; set; }

    public string? BvNedvS1 { get; set; }

    public string? BvNedv2 { get; set; }

    public string? BvNedvS2 { get; set; }

    public string? BvNedvAdr2 { get; set; }

    public string? BvBank1 { get; set; }

    public string? BvCrok1 { get; set; }

    public string? BvAgro1 { get; set; }

    public string? BvAgro12 { get; set; }

    public string? BvObesp { get; set; }

    public string? BvDobro { get; set; }

    public string? BvAdrAvto1 { get; set; }

    public string? BvVladAvto1 { get; set; }

    public string? BvVladNedv1 { get; set; }

    public string? BvInfKl { get; set; }

    public string? BvBdat3 { get; set; }

    public string? BvBdat2 { get; set; }

    public string? BvBdat1 { get; set; }

    public string? BvSumma3 { get; set; }

    public string? BvProcSt3 { get; set; }

    public string? BvCrok3 { get; set; }

    public string? BvBank3 { get; set; }

    public string? BvRisc { get; set; }

    public string? BvPgs3 { get; set; }

    public string? BvDoc { get; set; }

    public string? BvPgs2 { get; set; }

    public string? BvPgs1 { get; set; }

    public string? BvCopy { get; set; }

    public string? BvTypAnk { get; set; }

    public string? BvTypRez { get; set; }

    public string? BvPos { get; set; }

    public string? BvSpec { get; set; }

    public decimal? BvNedvCen1 { get; set; }

    public short? BvOpros { get; set; }

    public short? BvProsr3 { get; set; }

    public short? BvProsr2 { get; set; }

    public short? BvProsr1 { get; set; }

    public DateTime? BvDateOb { get; set; }

    public string? BvVidpog { get; set; }

    public string? BvKredProd { get; set; }

    public decimal? BvNedvCen2 { get; set; }

    public string? BvValdohKyr { get; set; }

    public short? BvOtkaz { get; set; }

    public string? BvStrdohInoe { get; set; }

    public string? BvStrdohName { get; set; }

    public string? BvStrdohRus { get; set; }

    public string? BvStrdohKyr { get; set; }

    public string? BvValdohUsa { get; set; }

    public string? BvValdohRus { get; set; }

    public string? BvOstatok3 { get; set; }

    public string? BvInf2Kl { get; set; }

    public int? BvSubProd { get; set; }

    public string? BvTypRez1 { get; set; }

    public string? BvKomKred { get; set; }

    public string? Adress { get; set; }

    public int? Refund { get; set; }

    public int? BvChild1 { get; set; }

    public int? Family3Income { get; set; }

    public int? Family2Income { get; set; }

    public int? Family1Income { get; set; }

    public short? Family3WorkExperience { get; set; }

    public short? Family2WorkExperience { get; set; }

    public short? Family1WorkExperience { get; set; }

    public string? Family3Work { get; set; }

    public string? Family2Work { get; set; }

    public string? Family1Work { get; set; }

    public short? Family3Age { get; set; }

    public short? Family2Age { get; set; }

    public short? Family1Age { get; set; }

    public string? Family3 { get; set; }

    public string? Family2 { get; set; }

    public string? Family1 { get; set; }

    public string? LoanHistoryInfo { get; set; }

    public short? MaxDelays { get; set; }

    public string? ShareHolder1 { get; set; }

    public string? ShareHolder2 { get; set; }

    public string? ShareHolder3 { get; set; }

    public string? ShareHolder4 { get; set; }

    public string? SharePart2 { get; set; }

    public string? SharePart1 { get; set; }

    public string? SharePart3 { get; set; }

    public string? SharePart4 { get; set; }

    public string? Position1InLe { get; set; }

    public string? Position2InLe { get; set; }

    public string? Position3InLe { get; set; }

    public string? Position4InLe { get; set; }

    public string? PlaceOfActivityLe { get; set; }

    public short? CountOfStaff { get; set; }

    public string? TimeOfActivityLe { get; set; }

    public short? WorkingDays { get; set; }

    public string? Contract1Name { get; set; }

    public string? Contract1Amount { get; set; }

    public DateTime? Contract1Date { get; set; }

    public string? Contract1Executed { get; set; }

    public string? Contract2Name { get; set; }

    public string? Contract2Amount { get; set; }

    public DateTime? Contract2Date { get; set; }

    public string? Contract2Executed { get; set; }

    public string? Contract3Name { get; set; }

    public string? Contract3Amount { get; set; }

    public DateTime? Contract3Date { get; set; }

    public string? Contract3Executed { get; set; }

    public string? Contract4Name { get; set; }

    public string? Contract4Amount { get; set; }

    public DateTime? Contract4Date { get; set; }

    public string? Contract4Executed { get; set; }

    public string? Contract5Name { get; set; }

    public string? Contract5Amount { get; set; }

    public DateTime? Contract5Date { get; set; }

    public string? Contract5Executed { get; set; }

    public string? FioGlbuh { get; set; }

    public int? IdOnline { get; set; }

    public string? Currency1 { get; set; }

    public string? Currency2 { get; set; }

    public string? Currency3 { get; set; }

    public string? SProch { get; set; }

    public string? PorDox { get; set; }

    public string? CelIsp2 { get; set; }

    public string? CelIsp { get; set; }

    public string? KatKl { get; set; }

    public int? Cycle2 { get; set; }

    public int? DbKol1 { get; set; }

    public string? Atyjoni { get; set; }

    public string? Darek { get; set; }

    public string? IdProch { get; set; }

    public int? DbKol2 { get; set; }

    public string? SNam { get; set; }

    public int? FamStat { get; set; }

    public int? IpCrok { get; set; }

    public string? IpKogo { get; set; }

    public decimal? PorKrCom { get; set; }

    public decimal? PorKrDol { get; set; }

    public DateTime? Rezdt1 { get; set; }

    public DateTime? Rezdt2 { get; set; }

    public DateTime? Rezdt3 { get; set; }

    public string? ZvOpisob1 { get; set; }

    public string? DbProch { get; set; }

    public int? DetKolm18 { get; set; }

    public decimal? PorDoxCom2 { get; set; }

    public string? DzAdress { get; set; }

    public decimal? PorDoxDol2 { get; set; }

    public string? DzFio { get; set; }

    public int? DetKolb18 { get; set; }

    public string? Gaz { get; set; }

    public int? DetKol { get; set; }

    public int? PorKol1 { get; set; }

    public int? PorKol2 { get; set; }

    public decimal? RemCom { get; set; }

    public int? Kvartk { get; set; }

    public int? BisProc1 { get; set; }

    public decimal? PorOstcom { get; set; }

    public decimal? PorOstdol { get; set; }
}

