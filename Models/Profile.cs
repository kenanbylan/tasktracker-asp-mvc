using System.ComponentModel.DataAnnotations;

namespace tasktracker.Models;

public class Profile
{
    [Key]
    [Display(Name = "user_id")]
    public string UserId { get; set; }

    [Display(Name = "user_name")]
    public string UserName { get; set; }

    [Display(Name = "user_surname")]
    public string UserSurname { get; set; }

    [Display(Name = "user_image")]
    public string UserImage { get; set; }

    [Display(Name = "user_email")]
    public string UserEmail { get; set; }

    [Display(Name = "user_phone")]
    public string UserPhone { get; set; }

    [Display(Name = "user_bio")]
    public string UserBio { get; set; }

    [Display(Name = "joined_events")]
    public string JoinedEvents { get; set; }

    [Display(Name = "created_events")]
    public string CreatedEvents { get; set; }
    
    
}