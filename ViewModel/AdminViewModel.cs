using System.Security.Cryptography;
using System.Text;
using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class AdminViewModel
{
    private readonly AdminRepository _adminRepository;

    public AdminViewModel(AdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public void ListAdmins()
    {
        var admins = _adminRepository.GetAllAdminsName();
        Console.WriteLine("Adminler:");
        foreach (var admin in admins)
        {
            Console.WriteLine($"- ID: {admin.Id} | İsim: {admin.Name} | Email: {admin.Email}");
        }
    }

    public void AddAdmin()
    {
        Console.WriteLine("Yeni admin ekleme:");
        Console.Write("İsim: ");
        string name = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        Console.Write("Şifre: ");
        string password = Console.ReadLine();
        
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Tüm alanlar doldurulmalıdır.");
            return;
        }
        
        // Check if email is already in use
        if (_adminRepository.GetAllAdminsName().Any(a => a.Email == email))
        {
            Console.WriteLine("Bu email zaten kullanılıyor.");
            return;
        }

        string hashedPassword = HashPasswordMD5(password);

        var admin = new Admin
        {
            Name = name,
            Email = email,
            Password = hashedPassword
        };

        try
        {
            _adminRepository.AddAdmin(admin);
            Console.WriteLine("Admin başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateAdmin()
    {
        Console.WriteLine("Admin güncelleme:");
        Console.Write("Admin ID: ");
        int id = int.Parse(Console.ReadLine());

        // Fetch current admin info
        var existing = _adminRepository.GetAllAdminsName().FirstOrDefault(a => a.Id == id);
        if (existing == null)
        {
            Console.WriteLine("Admin bulunamadı.");
            return;
        }

        Console.Write("Yeni isim (boş bırakılırsa mevcut kalır): ");
        string name = Console.ReadLine();

        Console.Write("Yeni email (boş bırakılırsa mevcut kalır): ");
        string email = Console.ReadLine();

        Console.Write("Yeni şifre (boş bırakılırsa mevcut kalır): ");
        string password = Console.ReadLine();

        existing.Name = string.IsNullOrEmpty(name) ? existing.Name : name;
        existing.Email = string.IsNullOrEmpty(email) ? existing.Email : email;

        if (!string.IsNullOrEmpty(password))
        {
            existing.Password = HashPasswordMD5(password);
        }

        try
        {
            _adminRepository.UpdateAdmin(existing);
            Console.WriteLine("Admin başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteAdmin()
    {
        Console.WriteLine("Admin silme:");
        Console.Write("Admin ID: ");
        int id = int.Parse(Console.ReadLine());

        var admin = _adminRepository.GetAllAdminsName().FirstOrDefault(a => a.Id == id);
        if (admin == null)
        {
            Console.WriteLine("Admin bulunamadı.");
            return;
        }

        try
        {
            _adminRepository.DeleteAdmin(admin);
            Console.WriteLine("Admin başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    private string HashPasswordMD5(string password)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
