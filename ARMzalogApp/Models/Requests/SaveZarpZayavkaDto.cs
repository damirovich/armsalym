using System;

namespace ARMzalogApp.Models.Requests;

public class SaveZarpZayavkaDto
{
    // === Системные и Основные ===
    public int? ZvPozn { get; set; }
    public string? OtNom { get; set; }
    public DateTime? ZvDate { get; set; } // Дата заявки
    public byte? ZvStat { get; set; }
    public byte? ZvStatus { get; set; }
    public int? UrFiz { get; set; } // 1 - Физлицо

    // === Кредит ===
    public decimal? ZvSum { get; set; }
    public short? ZvSrok { get; set; }
    public int? CelKr { get; set; }
    public byte? ZvVidkr { get; set; }
    public string? ZvKom { get; set; } // Вид кредита (строка/ID)
    public string? ZvKodv { get; set; } // Валюта (KGS, USD)
    public string? CelIsp { get; set; } // Группа/Категория/Субпродукт
    public byte? ZvKredur { get; set; } // Уровень Кредитного Комитета
    public int? Interest { get; set; }
    public short? ZvShkr { get; set; }

    // === Паспорт ===
    public string? ZvDok { get; set; }
    public string? ZvSrdok { get; set; }
    public string? ZvNdok { get; set; }
    public DateTime? ZvDatevp { get; set; }
    public DateTime? ZvDokend { get; set; } // Может быть строкой или датой
    public string? ZvMvd { get; set; }
    public string? ZvGrajd { get; set; } // Гражданство
    public int? ZvNational { get; set; } // Национальность
    public DateTime? ZvDatareg { get; set; }

    // === Клиент ===
    public int? ZvKodkl { get; set; }
    public string? KlFam { get; set; }
    public string? KlName { get; set; }
    public string? KlOtch { get; set; }
    public string? ZvNamkl { get; set; } // Полное ФИО одной строкой
    public string? ZvInn { get; set; }
    public DateTime? ZvFdr { get; set; } // Дата рождения
    public int? RGod { get; set; } // Год рождения
    public string? ZvFmr { get; set; } // Место рождения
    public int? ZvGr { get; set; } // Пол (1/2)
    public int? ZvKl { get; set; } // Резидентство
    public string? Atyjoni { get; set; } // Аты-Жону (Отчество/Род)

    // === Адреса и Контакты ===
    public string? RegAdres { get; set; } // Адрес прописки
    public string? FaktAdres { get; set; } // Фактический адрес
    public string? ZvAdr { get; set; } // Адрес (основной)
    public string? Adress { get; set; } // Доп. поле адреса
    public string? IdProch { get; set; } // Катталган дареги
    public string? Darek { get; set; } // Жашаган жери
    public string? ZvTel { get; set; } // Телефон основной
    public string? ZvTelfax { get; set; } // Телефон доп.
    public string? ZvEmail { get; set; }

    // === Работа и Доходы ===
    public string? Doljnoct { get; set; } // Место работы и должность
    public string? BvRabStaj { get; set; } // Стаж (маппится в rab_nach)
    public string? RabNach { get; set; } // Прямое поле, если нужно

    // Финансы (Поля из ViewModel)
    public decimal? SalaryAmount { get; set; }
    public decimal? MonthlyExpenses { get; set; }
    public decimal? SfLoansService { get; set; }
    public int? ClientType { get; set; }
    public string? IncomeType { get; set; } // ZvIndpnom
    public string? IncomeDescription { get; set; } // ZvOpisob

    // Доп. финансовые поля
    public string? AmountReq { get; set; }
    public DateTime? ZvIndpdater { get; set; }
    public string? ZvIndpsrok { get; set; }

    // === Семья ===
    public FamilyStatusType? FamStat { get; set; } // Enum или int
    public string? FioCupr { get; set; }
    public string? RabCupr { get; set; }
    public string? DoljCup { get; set; }
    public string? BvTelefSupr { get; set; } // Телефон супруга
    public string? ZvEmailSupr { get; set; } // Часто дублирует тел. супруга
    public int? DetKol { get; set; } // Количество детей

    // === Родственники / Поручители (Блок VII) ===
    public string? DbTipbis { get; set; } // ФИО Близкого
    public string? DbPeriod { get; set; } // Место работы близкого
    public string? DbGeogr { get; set; } // Кем приходится
    public string? DbPoctavka { get; set; } // Телефон близкого
    public short? DbPoruchRods1 { get; set; } // Является поручителем (1-да, 2-нет)
    public short? DbPoruchNedvij { get; set; }
    public short? DbPoruchAvto1 { get; set; }

    // === Прочие технические поля (Заглушки и служебные) ===
    public int? IpCrok { get; set; } // ID Руководителя (otnom_r)
    public int? TypBis { get; set; }
    public string? PtrMmodel { get; set; }
    public int? PtrMavtoGod { get; set; }
    public decimal? PtrAvtoCom { get; set; }
    public string? BisProch { get; set; }
    public int? BisCrok { get; set; }
    public string? Z1Prochs { get; set; }
    public string? Z1Adress { get; set; }
    public string? Z1Fio { get; set; }
    public string? Z1Ser { get; set; }
    public string? Z1Vid { get; set; }
    public string? PorAkt9 { get; set; }
    public string? PorObr { get; set; }
    public string? ZvGrupFio { get; set; }
    public string? Reshenie { get; set; }
    public int? Refund { get; set; } // Используется как Регион
    public string? FioGlbuh { get; set; }
    public string? SProch { get; set; }
    public int? DbKol2 { get; set; }
    public int? Cycle2 { get; set; }

    // Пути к файлам (если понадобятся)
    public string? Path1 { get; set; }
    public string? Path2 { get; set; }
    public string? Path3 { get; set; }
    public string? Path4 { get; set; }
    public string? Path5 { get; set; }


    public int BvTypRez { get; set; }  
    public int BvTypRez1 { get; set; } 
}