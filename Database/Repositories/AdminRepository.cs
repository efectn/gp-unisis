using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class AdminRepository
{
    private readonly ApplicationDbContext _context;

    public AdminRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddAdmin(Admin admin)
    {
        if (admin == null)
        {
            throw new ArgumentNullException(nameof(admin));
        }

        var existAdmin = _context.Admins.FirstOrDefault(a => a.Email == admin.Email);
        if (existAdmin != null)
        {
            throw new InvalidOperationException($"Admin with email {admin.Email} already exist");
        }

        if (string.IsNullOrEmpty(admin.Name) || string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password))
        {
            throw new ArgumentException("All required properties should be set");
        }

        _context.Admins.Add(admin);
        _context.SaveChanges();
    }

    public void UpdateAdmin(Admin admin)
    {
        if (admin == null)
        {
            throw new ArgumentException(nameof(admin));
        }

        var existAdmin = _context.Admins.FirstOrDefault(a => a.Id == admin.Id);
        if (existAdmin == null)
        {
            throw new InvalidOperationException($"Admin with ID {admin.Id} does not exist.");
        }

        if (string.IsNullOrEmpty(admin.Name) || string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password))
        {
            throw new ArgumentException("All required properties should be set");
        }

        existAdmin.Name = admin.Name;
        existAdmin.Password = admin.Password;
        existAdmin.Email = admin.Email;

        _context.SaveChanges();
    }

    public void DeleteAdmin(Admin admin)
    {
        if (admin == null)
        {
            throw new ArgumentNullException(nameof(admin));
        }

        var existing = _context.Admins.FirstOrDefault(a => a.Id == admin.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Admin with ID {admin.Id} does not exist.");
        }

        _context.Admins.Remove(existing);
        _context.SaveChanges();
    }

    public List<Admin> GetAllAdminsName()
    {
        return _context.Admins.ToList();
    }
}
