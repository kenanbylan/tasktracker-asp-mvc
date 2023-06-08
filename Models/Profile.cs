using System.ComponentModel.DataAnnotations;

namespace tasktracker.Models
{
    public class Profile
    {
        [Key]
        public string UserId { get; set; }
        
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserImage { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserBio { get; set; }
        public string JoinedEvents { get; set; }
        public string CreatedEvents { get; set; }
    }
}