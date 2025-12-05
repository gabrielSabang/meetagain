using MeetAgain.Server.Models;

using Google.Cloud.Firestore;

namespace MeetAgain.Server.Models
{
    [FirestoreData]
    public class Meetup
    {
        [FirestoreProperty] public string Id { get; set; } = "";
        [FirestoreProperty] public string Title { get; set; } = "";
        [FirestoreProperty] public string Description { get; set; } = "";
        [FirestoreProperty] public string CreatorUserId { get; set; } = "";
        [FirestoreProperty] public DateTime EventDateTime { get; set; }
        [FirestoreProperty] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
