using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.DTOs
{
    public class LanguageStatistics
    {
        public string Language { get; set; }
        public double NumberOfRequests { get; set; }

        public LanguageStatistics()
        {
            
        }

        public LanguageStatistics(string language, double numberOfRequests)
        {
            Language = language;
            NumberOfRequests = numberOfRequests;
        }
    }
}
