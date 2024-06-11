using InitialProject.Serializer;

namespace InitialProject.Domain.Models
{
    public class TourReview : ISerializable
    {
        public int Id { get; set; }
        public int GuideKnowledgeGrade { get; set; }
        public int GuideLanguageGrade { get; set; }
        public int InterestingnessGrade { get; set; }
        public string Comment { get; set; }
        public CheckpointArrival Arrival { get; set; }
        public int ArrivalId { get; set; }

        public TourReview()
        {

        }

        public TourReview(int guideKnowledgeGrade, int guideLanguageGrade, int interestingnessGrade, string comment, int arrivalId)
        {
            GuideKnowledgeGrade = guideKnowledgeGrade;
            GuideLanguageGrade = guideLanguageGrade;
            InterestingnessGrade = interestingnessGrade;
            Comment = comment;
            ArrivalId = arrivalId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideKnowledgeGrade.ToString(), GuideLanguageGrade.ToString(), InterestingnessGrade.ToString(), Comment, ArrivalId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuideKnowledgeGrade = int.Parse(values[1]);
            GuideLanguageGrade = int.Parse(values[2]);
            InterestingnessGrade = int.Parse(values[3]);
            Comment = values[4];
            ArrivalId = int.Parse(values[5]);
        }
    }
}
