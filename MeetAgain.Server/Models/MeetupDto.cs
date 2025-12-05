using MeetAgain.Server.Models;

namespace MeetAgain.Server.Models
{
    public class MeetupDto
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime EventDateTime { get; set; }
    }
}