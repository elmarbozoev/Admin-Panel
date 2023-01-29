using System.Security.Cryptography.X509Certificates;

namespace AdminPanel.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public MediaFile? ProfilePicture { get; set; }
    }
}
