using InitialProject.Serializer;
using System;

namespace InitialProject.Domain.Models
{
    public class Comment : ISerializable
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string UserBeenThere { get; set; }
        public int NumberOfReports { get; set; }

        public Comment() { }

        public Comment(int id, int forumId, string text, int userId)
        {
            Id = id;
            ForumId = forumId;
            Text = text;
            UserId = userId;
            NumberOfReports = 0;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ForumId.ToString(), Text, UserId.ToString(), NumberOfReports.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ForumId = Convert.ToInt32(values[1]);
            Text = values[2];
            UserId = Convert.ToInt32(values[3]);
            NumberOfReports = Convert.ToInt32(values[4]);
        }
    }
}
