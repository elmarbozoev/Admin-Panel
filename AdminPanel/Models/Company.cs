namespace AdminPanel.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Employee>? Employees { get; set; }

        public Company()
        {
            Employees = new List<Employee>();
        }
    }
}
