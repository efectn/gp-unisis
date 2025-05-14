// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, Efe!");


public class Admin
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Sifre { get; set; }
    public string Mail { get; set; }
}


public class Fakulte
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Adres { get; set; }
    public string IletisimNo { get; set; }
    public string Dekan { get; set; }
    public string DekanYardimcisi { get; set; }

    public ICollection<Bolum> Bolumler { get; set; }
    public ICollection<AkademisyenFakulte> AkademisyenFakulteler { get; set; }
}


public class Bolum
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Adres { get; set; }
    public string IletisimNo { get; set; }
    public string Baskani { get; set; }
    public string BaskanYardimcisi { get; set; }

    public int FakulteId { get; set; }
    public Fakulte Fakulte { get; set; }

    public ICollection<Ogrenci> Ogrenciler { get; set; }
    public ICollection<DersGrubuBolum> DersGrubuBolumler { get; set; }
}


public class Semester
{
    public int Id { get; set; }
    public DateTime Baslangic { get; set; }
    public DateTime Bitis { get; set; }
    public DateTime YilSonuSinavTarihi { get; set; }

    public ICollection<DersSemester> Dersler { get; set; }
    public ICollection<Not> Notlar { get; set; }
    public ICollection<Transkript> Transkriptler { get; set; }
}


public class Akademisyen
{
    public int Id { get; set; }
    public string AdSoyad { get; set; }
    public string Sifre { get; set; }
    public string Mail { get; set; }

    public ICollection<AkademisyenFakulte> AkademisyenFakulteler { get; set; }
    public ICollection<Ders> Dersler { get; set; }
    public ICollection<Duyuru> Duyurular { get; set; }
}


public class AkademisyenFakulte
{
    public int AkademisyenId { get; set; }
    public Akademisyen Akademisyen { get; set; }

    public int FakulteId { get; set; }
    public Fakulte Fakulte { get; set; }
}


public class Ders
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Kod { get; set; }
    public int Kredi { get; set; }
    public int Yariyil { get; set; }
    public int Kontejan { get; set; }
    public bool SecmeliMi { get; set; }
    public string Aciklama { get; set; }

    public int AkademisyenId { get; set; }
    public Akademisyen Akademisyen { get; set; }

    public ICollection<DersSemester> DersSemesters { get; set; }
    public ICollection<CourseScheduleEntry> Programlar { get; set; }
    public ICollection<Not> Notlar { get; set; }
    public ICollection<Duyuru> Duyurular { get; set; }
    public ICollection<DersDersGrubu> DersGrubuIliskileri { get; set; }
}

public class DersSemester
{
    public int DersId { get; set; }
    public Ders Ders { get; set; }

    public int SemesterId { get; set; }
    public Semester Semester { get; set; }
}


public class CourseScheduleEntry
{
    public int Id { get; set; }
    public int DersId { get; set; }
    public Ders Ders { get; set; }

    public string Gun { get; set; }
    public TimeSpan Baslangic { get; set; }
    public TimeSpan Bitis { get; set; }
}


public class Duyuru
{
    public int Id { get; set; }

    public int? AdminId { get; set; }
    public Admin Admin { get; set; }

    public int? AkademisyenId { get; set; }
    public Akademisyen Akademisyen { get; set; }

    public int? DersId { get; set; }
    public Ders Ders { get; set; }

    public string Baslik { get; set; }
    public string Aciklama { get; set; }
    public DateTime BitisTarihi { get; set; }
}


public class Ogrenci
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Numara { get; set; }
    public string Sifre { get; set; }
    public string Mail { get; set; }
    public string Tc { get; set; }
    public DateTime DogumTarihi { get; set; }
    public bool MezunMu { get; set; }

    public int BolumId { get; set; }
    public Bolum Bolum { get; set; }

    public ICollection<Not> Notlar { get; set; }
    public ICollection<Transkript> Transkriptler { get; set; }
}


public class Not
{
    public int Id { get; set; }

    public int DersId { get; set; }
    public Ders Ders { get; set; }

    public int SemesterId { get; set; }
    public Semester Semester { get; set; }

    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; }

    public string SinavAdi { get; set; }
    public double Yuzdelik { get; set; }
    public double NotDegeri { get; set; }
}


public class Transkript
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; }

    public int SemesterId { get; set; }
    public Semester Semester { get; set; }

    public double HarfPuani { get; set; }
    public bool DerstenKaldiMi { get; set; }
}


public class DersGrubu
{
    public int Id { get; set; }
    public string Ad { get; set; }

    public ICollection<DersDersGrubu> Dersler { get; set; }
    public ICollection<DersGrubuBolum> Bolumler { get; set; }
}


public class DersDersGrubu
{
    public int DersId { get; set; }
    public Ders Ders { get; set; }

    public int DersGrubuId { get; set; }
    public DersGrubu DersGrubu { get; set; }
}


public class DersGrubuBolum
{
    public int DersGrubuId { get; set; }
    public DersGrubu DersGrubu { get; set; }

    public int BolumId { get; set; }
    public Bolum Bolum { get; set; }
}


public class UniversityContext : DbContext
{
    public DbSet<Admin> Adminler { get; set; }
    public DbSet<Fakulte> Fakulteler { get; set; }
    public DbSet<Bolum> Bolumler { get; set; }
    public DbSet<Semester> Donemler { get; set; }
    public DbSet<Akademisyen> Akademisyenler { get; set; }
    public DbSet<AkademisyenFakulte> AkademisyenFakulteler { get; set; }
    public DbSet<Ders> Dersler { get; set; }
    public DbSet<DersSemester> DersSemesters { get; set; }
    public DbSet<CourseScheduleEntry> DersProgramlari { get; set; }
    public DbSet<Duyuru> Duyurular { get; set; }
    public DbSet<Ogrenci> Ogrenciler { get; set; }
    public DbSet<Not> Notlar { get; set; }
    public DbSet<Transkript> Transkriptler { get; set; }
    public DbSet<DersGrubu> DersGruplari { get; set; }
    public DbSet<DersDersGrubu> DersDersGruplari { get; set; }
    public DbSet<DersGrubuBolum> DersGrubuBolumler { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("");
    }
}