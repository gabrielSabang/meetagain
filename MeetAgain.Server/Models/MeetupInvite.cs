using MeetAgain.Server.Models;
using Google.Cloud.Firestore;

namespace MeetAgain.Server.Models
{
    [FirestoreData]
    public class MeetupInvite
    {
        [FirestoreProperty] public string UserId { get; set; } = "";
        [FirestoreProperty] public string Status { get; set; } = "pending";
        [FirestoreProperty] public string SentAt { get; set; } = "";
    }
}
