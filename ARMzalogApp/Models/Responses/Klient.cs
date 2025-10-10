using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models.Responses;


public partial class Klient
{
    public int Id { get; set; }

    public int KlKod { get; set; }

    public string? KlNam { get; set; }

    public string? KlAdr { get; set; }

    public string? KlFio1 { get; set; }

    public string? KlFio2 { get; set; }

    public string? KlFio3 { get; set; }

    public string? KlTel1 { get; set; }

    public string? KlTel2 { get; set; }

    public short? KlTipkl { get; set; }

    public short? KlOtr { get; set; }

    public DateTime? KlDatareg { get; set; }

    public DateTime? KlData { get; set; }

    public string? KlInn { get; set; }

    public short? KlGni { get; set; }

    public byte? KlStat { get; set; }

    public byte? KlKl { get; set; }

    public byte? KlVids { get; set; }

    public string? KlFio { get; set; }

    public string? KlDok { get; set; }

    public string? KlSrdok { get; set; }

    public string? KlNdok { get; set; }

    public DateTime? KlDatevp { get; set; }

    public string? KlMvd { get; set; }

    public string? KlKodter { get; set; }

    public short? KlKodstr { get; set; }

    public string? KlOkpo { get; set; }

    public string? KlRnsf { get; set; }

    public string? KlKeyw { get; set; }

    public byte? KlRelig { get; set; }

    public int? KlNom { get; set; }

    public string Org { get; set; } = null!;

    public string KlReg { get; set; } = null!;

    public string Mreg { get; set; } = null!;

    public string Opf { get; set; } = null!;

    public string KlGrajd { get; set; } = null!;

    public string Uind { get; set; } = null!;

    public string Uobl { get; set; } = null!;

    public string Uraion { get; set; } = null!;

    public string Uul { get; set; } = null!;

    public string Udom { get; set; } = null!;

    public string Ukorpuc { get; set; } = null!;

    public string Uctroen { get; set; } = null!;

    public string Ukvart { get; set; } = null!;

    public string Find { get; set; } = null!;

    public string Fobl { get; set; } = null!;

    public string Fraion { get; set; } = null!;

    public string Ful { get; set; } = null!;

    public string Fdom { get; set; } = null!;

    public string Fkorpuc { get; set; } = null!;

    public string Fctroen { get; set; } = null!;

    public string Fkvart { get; set; } = null!;

    public string Fnp { get; set; } = null!;

    public string Fmr { get; set; } = null!;

    public DateTime Fdr { get; set; }

    public string KlFam { get; set; } = null!;

    public string KlName { get; set; } = null!;

    public string KlOtch { get; set; } = null!;

    public string Uknp { get; set; } = null!;

    public string Unp { get; set; } = null!;

    public string Fknp { get; set; } = null!;

    public string KlVid { get; set; } = null!;

    public string Rchp { get; set; } = null!;

    public int KlAffin { get; set; }

    public string Grajd { get; set; } = null!;

    public string Filial { get; set; } = null!;

    public string P482 { get; set; } = null!;

    public string KlDolgn { get; set; } = null!;

    public string? KlDokend { get; set; }

    public short? KlLevel { get; set; }

    public short? KlOtv { get; set; }

    public string? KlPassw { get; set; }

    public short? KlGr { get; set; }

    public byte? KlSms { get; set; }

    public string? KlIsys { get; set; }

    public int KlCfr { get; set; }

    public int Kodb { get; set; }

    public string? KlOtrn { get; set; }

    public int? KlNomuch { get; set; }

    public string? KlMector { get; set; }

    public int? KlGrup { get; set; }

    public DateTime? Kldatlic { get; set; }

    public int? Predct { get; set; }

    public int? Klnomlic { get; set; }

    public int? Problem { get; set; }

    public int? CibOpf { get; set; }

    public string? CibCountry { get; set; }

    public int? CibTdok { get; set; }

    public int? Cycle { get; set; }

    public int? KlNational { get; set; }

    public int? KlSempolog { get; set; }

    public DateTime? KlIndpdater { get; set; }

    public string? KlIndpnom { get; set; }

    public string? KlIndporgan { get; set; }

    public string? KlIndpmestreg { get; set; }

    public byte[]? Photo { get; set; }

    public string? KatKl { get; set; }

    public int? KatKd { get; set; }

    public int? SentToCrif { get; set; }

    public int? IsInternational { get; set; }

    public DateTime? LkregDate { get; set; }
}