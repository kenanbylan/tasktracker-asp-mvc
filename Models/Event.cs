using System.ComponentModel.DataAnnotations;

namespace tasktracker.Models;

public class Event
{
    [Key]
    public int event_id { get; set; }
    [Required]
    
    public string event_name { get; set; }
    public string event_description { get; set; }
    public string event_image { get; set; }
    public string event_date { get; set; }
    public string event_location { get; set; }
    public string event_ownerID { get; set; }
    public string event_requirement { get; set; }
    public int event_fee { get; set; }
    public bool event_active { get; set; }
    public int number_members { get; set; }
    public string event_membersID { get; set; }
}