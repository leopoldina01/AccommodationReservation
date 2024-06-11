using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class SuperGuideViewModel : ViewModelBase
    {
        #region PROPERTIES
        public List<string> Languages { get; set; }
        public ObservableCollection<GuideStatistics> GuideStatistics { get; set; }

        private readonly User _guide;
        private readonly TourService _tourService;
        private readonly TourReviewService _tourReviewService;
        private readonly SuperGuideService _superGuideService;
        private readonly VoucherService _voucherService;
        private readonly TourReservationService _tourReservationService;
        #endregion
        public SuperGuideViewModel(User guide)
        {
            _guide = guide;
            _tourService = new TourService();
            _tourReviewService = new TourReviewService();
            _superGuideService = new SuperGuideService();
            _voucherService = new VoucherService();
            _tourReservationService = new TourReservationService();
            Languages = new List<string>() { "Afrikaans", "Albanian", "Amharic", "Arabic", "Armenian", "Assamese", "Azerbaijani", "Basque", "Belarusian", "Bengali", "Bosnian", "Bulgarian", "Burmese", "Catalan", "Cebuano", "Chichewa", "Chinese (Mandarin)", "Corsican", "Croatian", "Czech", "Danish", "Dutch", "English", "Esperanto", "Estonian", "Finnish", "French", "Frisian", "Galician", "Georgian", "German", "Greek", "Gujarati", "Haitian Creole", "Hausa", "Hawaiian", "Hebrew", "Hindi", "Hmong", "Hungarian", "Icelandic", "Igbo", "Indonesian", "Irish", "Italian", "Japanese", "Javanese", "Kannada", "Kazakh", "Khmer", "Kinyarwanda", "Korean", "Kurdish (Kurmanji)", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian", "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Malayalam", "Maltese", "Maori", "Marathi", "Mongolian", "Myanmar (Burmese)", "Nepali", "Norwegian", "Odia (Oriya)", "Pashto", "Persian", "Polish", "Portuguese", "Punjabi", "Romanian", "Russian", "Samoan", "Scots Gaelic", "Serbian", "Sesotho", "Shona", "Sindhi", "Sinhala", "Slovak", "Slovenian", "Somali", "Spanish", "Sundanese", "Swahili", "Swedish", "Tagalog (Filipino)", "Tajik", "Tamil", "Tatar", "Telugu", "Thai", "Turkish", "Turkmen", "Ukrainian", "Urdu", "Uyghur", "Uzbek", "Vietnamese", "Welsh", "Xhosa", "Yiddish", "Yoruba", "Zulu" };
            GuideStatistics = new ObservableCollection<GuideStatistics>();

            LoadGuideInfo();

            BecomeSuperGuideCommand = new RelayCommand(BecomeSuperGuideCommand_Execute, BecomeSuperGuideCommand_CanExecute);
            ResignCommand = new RelayCommand(ResignCommand_Execute);
        }

        private void LoadGuideInfo()
        {
            foreach (var language in Languages)
            {
                var tours = _tourService.GetAllForGuideAndLanguageInLastYear(_guide, language);

                if (tours.Count() != 0)
                {
                    var averageGrade = _tourReviewService.GetAverageGrade(tours);
                    GuideStatistics.Add(new GuideStatistics(language, tours.Count(), (double)averageGrade));
                }
            }
        }

        #region COMMANDS
        public RelayCommand BecomeSuperGuideCommand { get; }
        public RelayCommand ResignCommand { get; }

        public void BecomeSuperGuideCommand_Execute(object? parameter)
        {
            var statistics = parameter as GuideStatistics;
            if (MessageBox.Show("Are you sure you want to become super guide for " + statistics.Language + "? This action is not reversible", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.No) return;
            _superGuideService.Create(_guide.Id, statistics.Language);
        }

        public bool BecomeSuperGuideCommand_CanExecute(object? parameter)
        {
            var statistics = parameter as GuideStatistics;
            return statistics is not null && statistics.TourNumber >= 20 && statistics.AverageGrade > 9.0 && _superGuideService.GetByGuideAndLanguage(_guide, statistics.Language) is null;
        }

        public void ResignCommand_Execute(object? parameter)
        {
            if (MessageBox.Show("Are you sure you want to resign? This action is not reversible", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No) return;
            foreach (var tour in _tourService.GetFutureToursByGuide(_guide))
            {
                _tourService.CancelTour(tour);
                _tourReservationService.DeleteAllReservationsForCancelledTour(tour, null);
            }
            _voucherService.MakeVouchersForSpecificGuideAvailibleForAllGuides(_guide);
        }
        #endregion
    }
}
