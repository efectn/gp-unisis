namespace gp_unisis.Database.Entities;

public class Faculty
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string Dean { get; set; }
    public string ViceDean { get; set; }

    public ICollection<Department> Departments { get; set; }
}