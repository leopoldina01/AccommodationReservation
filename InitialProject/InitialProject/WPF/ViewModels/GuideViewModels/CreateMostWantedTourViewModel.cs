using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels
{
    public class CreateMostWantedTourViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region PROPERTIES
		private bool _isCountryCBEnabled;
		public bool IsCountryCBEnabled
		{
			get
			{
				return _isCountryCBEnabled;
			}
			set
			{
				if (_isCountryCBEnabled != value)
				{
					_isCountryCBEnabled = value;
					OnPropertyChanged(nameof(IsCountryCBEnabled));
				}
			}
		}

		private bool _isCityCBEnabled;
		public bool IsCityCBEnabled
		{
			get
			{
				return _isCityCBEnabled;
			}
			set
			{
				if (_isCityCBEnabled != value)
				{
					_isCityCBEnabled = value;
					OnPropertyChanged(nameof(IsCityCBEnabled));
				}
			}
		}

		private bool _isLanguageCBEnabled;
		public bool IsLanguageCBEnabled
		{
			get
			{
				return _isLanguageCBEnabled;
			}
			set
			{
				if (_isLanguageCBEnabled != value)
				{
					_isLanguageCBEnabled = value;
					OnPropertyChanged(nameof(IsLanguageCBEnabled));
				}
			}
		}

        private bool _isMostWantedLocationChecked;
        public bool IsMostWantedLocationChecked
        {
            get
            {
                return _isMostWantedLocationChecked;
            }
            set
            {
                if (_isMostWantedLocationChecked != value)
                {
                    _isMostWantedLocationChecked = value;
                    IsCountryCBEnabled = !value;
                    IsCityCBEnabled = !value;
                    OnPropertyChanged(nameof(IsMostWantedLocationChecked));
                }
            }
        }

        private bool _isMostWantedLanguageChecked;
        public bool IsMostWantedLanguageChecked
        {
            get
            {
                return _isMostWantedLanguageChecked;
            }
            set
            {
                if (_isMostWantedLanguageChecked != value)
                {
                    _isMostWantedLanguageChecked = value;
                    IsLanguageCBEnabled = !value;
                    OnPropertyChanged(nameof(IsMostWantedLanguageChecked));
                }
            }
        }

        private string _tourName;
        public string TourName
        {
            get => _tourName;

            set
            {
                if (_tourName != value)
                {
                    _tourName = value;

                    _errorsViewModel.ClearErrors(nameof(TourName));
                    if (string.IsNullOrEmpty(_tourName))
                    {
                        _errorsViewModel.AddError(nameof(TourName), "Name is required");
                    }

                    OnPropertyChanged(nameof(TourName));
                }
            }
        }

        private string _selectedCountry;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;

                    _errorsViewModel.ClearErrors(nameof(SelectedCountry));
                    if (string.IsNullOrEmpty(_selectedCountry))
                    {
                        _errorsViewModel.AddError(nameof(SelectedCountry), "Country is required");
                    }

                    OnPropertyChanged(nameof(SelectedCountry));
                    LoadCities();
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;

                    _errorsViewModel.ClearErrors(nameof(SelectedCity));
                    if (string.IsNullOrEmpty(_selectedCity))
                    {
                        _errorsViewModel.AddError(nameof(SelectedCity), "City is required");
                    }

                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;

                    _errorsViewModel.ClearErrors(nameof(SelectedLanguage));
                    if (string.IsNullOrEmpty(_selectedLanguage))
                    {
                        _errorsViewModel.AddError(nameof(SelectedLanguage), "Language is required");
                    }

                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;

                    _errorsViewModel.ClearErrors(nameof(Description));
                    if (string.IsNullOrEmpty(_description))
                    {
                        _errorsViewModel.AddError(nameof(Description), "Description is required");
                    }

                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private int _maxGuests;
        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (_maxGuests != value)
                {
                    _maxGuests = value;

                    _errorsViewModel.ClearErrors(nameof(MaxGuests));
                    if (_maxGuests < 1)
                    {
                        _errorsViewModel.AddError(nameof(MaxGuests), "Must be more than 0");
                    }
                    else if (_maxGuests > 1000)
                    {
                        _errorsViewModel.AddError(nameof(MaxGuests), "Must be less than 1001");
                    }

                    OnPropertyChanged(nameof(MaxGuests));
                }
            }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;

                    _errorsViewModel.ClearErrors(nameof(Duration));
                    if (_duration < 1)
                    {
                        _errorsViewModel.AddError(nameof(Duration), "Must be more than 0");
                    }
                    else if (_duration > 1000)
                    {
                        _errorsViewModel.AddError(nameof(Duration), "Must be less than 1001");
                    }

                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        private DateTime _datePickerSelectedDate;
        public DateTime DatePickerSelectedDate
        {
            get => _datePickerSelectedDate;
            set
            {
                if (_datePickerSelectedDate != value)
                {
                    _datePickerSelectedDate = value;
                    OnPropertyChanged(nameof(DatePickerSelectedDate));
                }
            }
        }

        private string _enteredCheckpointName;
        public string EnteredCheckpointName
        {
            get => _enteredCheckpointName;
            set
            {
                if (_enteredCheckpointName != value)
                {
                    _enteredCheckpointName = value;
                    OnPropertyChanged(nameof(EnteredCheckpointName));
                }
            }
        }

        private BitmapImage _selectedImage;
        public BitmapImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }

        private BitmapImage _coverImage;
        public BitmapImage CoverImage
        {
            get
            {
                return _coverImage;
            }
            set
            {
                if (_coverImage != value)
                {
                    _coverImage = value;
                    OnPropertyChanged(nameof(CoverImage));
                }
            }
        }

        private bool _isCoverImageSelected;
        public bool IsCoverImageSelected
        {
            get
            {
                return _isCoverImageSelected;
            }
            set
            {
                if (_isCoverImageSelected != value)
                {
                    _isCoverImageSelected = value;
                    OnPropertyChanged(nameof(IsCoverImageSelected));
                }
            }
        }

        private bool _isNextButtonVisible;
        public bool IsNextButtonVisible
        {
            get
            {
                return _isNextButtonVisible;
            }
            set
            {
                if (_isNextButtonVisible != value)
                {
                    _isNextButtonVisible = value;
                    OnPropertyChanged(nameof(IsNextButtonVisible));
                }
            }
        }

        private bool _isConfirmButtonVisible;
        public bool IsConfirmButtonVisible
        {
            get
            {
                return _isConfirmButtonVisible;
            }
            set
            {
                if (_isConfirmButtonVisible != value)
                {
                    _isConfirmButtonVisible = value;
                    OnPropertyChanged(nameof(IsConfirmButtonVisible));
                }
            }
        }

        private bool _isGeneralTabSelected;
        public bool IsGeneralTabSelected
        {
            get
            {
                return _isGeneralTabSelected;
            }
            set
            {
                if (_isGeneralTabSelected != value)
                {
                    _isGeneralTabSelected = value;
                    OnPropertyChanged(nameof(IsGeneralTabSelected));
                }
            }
        }

        private bool _isDateTimeTabSelected;
        public bool IsDateTimeTabSelected
        {
            get
            {
                return _isDateTimeTabSelected;
            }
            set
            {
                if (_isDateTimeTabSelected != value)
                {
                    _isDateTimeTabSelected = value;
                    OnPropertyChanged(nameof(IsDateTimeTabSelected));
                }
            }
        }

        private bool _isCheckpointsTabSelected;
        public bool IsCheckpointsTabSelected
        {
            get
            {
                return _isCheckpointsTabSelected;
            }
            set
            {
                if (_isCheckpointsTabSelected != value)
                {
                    _isCheckpointsTabSelected = value;
                    OnPropertyChanged(nameof(IsCheckpointsTabSelected));
                }
            }
        }

        private bool _isImagesTabSelected;
        public bool IsImagesTabSelected
        {
            get
            {
                return _isImagesTabSelected;
            }
            set
            {
                if (_isImagesTabSelected != value)
                {
                    _isImagesTabSelected = value;
                    OnPropertyChanged(nameof(IsImagesTabSelected));
                }
            }
        }


        public ObservableCollection<DateTime> Dates { get; set; }
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public ObservableCollection<string> CheckpointNames { get; set; }
        public List<BitmapImage> Images { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly CheckpointService _checkpointService;
        private readonly TourImageService _tourImageService;
        private readonly TourRequestService _tourRequestService;
        private readonly RequestedTourNotificationService _requestedTourNotificationService;

        private readonly User _guide;
        private readonly Stack<Tuple<ReversibleCommand, object?>> _commandStack;
        #endregion

        #region VALIDATION
        private readonly ErrorsViewModel _errorsViewModel;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public bool IsValid => !HasErrors;

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(IsValid));
        }

        private bool Validate()
        {
            _errorsViewModel.ClearErrors(nameof(TourName));
            if (string.IsNullOrEmpty(TourName))
            {
                _errorsViewModel.AddError(nameof(TourName), "Name is required");
            }


            _errorsViewModel.ClearErrors(nameof(SelectedCountry));
            if (string.IsNullOrEmpty(SelectedCountry))
            {
                _errorsViewModel.AddError(nameof(SelectedCountry), "Country is required");
            }

            _errorsViewModel.ClearErrors(nameof(SelectedCity));
            if (string.IsNullOrEmpty(SelectedCity))
            {
                _errorsViewModel.AddError(nameof(SelectedCity), "City is required");
            }

            _errorsViewModel.ClearErrors(nameof(SelectedLanguage));
            if (string.IsNullOrEmpty(SelectedLanguage))
            {
                _errorsViewModel.AddError(nameof(SelectedLanguage), "Language is required");
            }

            _errorsViewModel.ClearErrors(nameof(Description));
            if (string.IsNullOrEmpty(Description))
            {
                _errorsViewModel.AddError(nameof(Description), "Description is required");
            }

            _errorsViewModel.ClearErrors(nameof(MaxGuests));
            if (MaxGuests < 1)
            {
                _errorsViewModel.AddError(nameof(MaxGuests), "Must be more than 0");
            }
            else if (MaxGuests > 1000)
            {
                _errorsViewModel.AddError(nameof(MaxGuests), "Must be less than 1001");
            }

            _errorsViewModel.ClearErrors(nameof(Duration));
            if (Duration < 1)
            {
                _errorsViewModel.AddError(nameof(Duration), "Must be more than 0");
            }
            else if (Duration > 1000)
            {
                _errorsViewModel.AddError(nameof(Duration), "Must be less than 1001");
            }

            _errorsViewModel.ClearErrors(nameof(Dates));
            if (Dates.Count == 0)
            {
                _errorsViewModel.AddError(nameof(Dates), "Must set at least 1 start date & time");
            }

            _errorsViewModel.ClearErrors(nameof(CheckpointNames));
            if (CheckpointNames.Count < 2)
            {
                _errorsViewModel.AddError(nameof(CheckpointNames), "Must set at least 2 checkpoints");
            }

            _errorsViewModel.ClearErrors(nameof(Images));
            if (Images.Count == 0)
            {
                _errorsViewModel.AddError(nameof(Images), "Must add at least 1 image");
            }

            return !HasErrors;
        }
        #endregion

        public CreateMostWantedTourViewModel(User guide, Stack<Tuple<ReversibleCommand, object?>> commandStack)
        {
            _guide = guide;
            _commandStack = commandStack;

            _tourService = new TourService();
            _locationService = new LocationService();
            _checkpointService = new CheckpointService();
            _tourImageService = new TourImageService();
            _tourRequestService = new TourRequestService();
            _requestedTourNotificationService = new RequestedTourNotificationService();

            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

            Dates = new ObservableCollection<DateTime>();
            Countries = new List<string>();
            Cities = new ObservableCollection<string>();
            Languages = new ObservableCollection<string>() { "Afrikaans", "Albanian", "Amharic", "Arabic", "Armenian", "Assamese", "Azerbaijani", "Basque", "Belarusian", "Bengali", "Bosnian", "Bulgarian", "Burmese", "Catalan", "Cebuano", "Chichewa", "Chinese (Mandarin)", "Corsican", "Croatian", "Czech", "Danish", "Dutch", "English", "Esperanto", "Estonian", "Finnish", "French", "Frisian", "Galician", "Georgian", "German", "Greek", "Gujarati", "Haitian Creole", "Hausa", "Hawaiian", "Hebrew", "Hindi", "Hmong", "Hungarian", "Icelandic", "Igbo", "Indonesian", "Irish", "Italian", "Japanese", "Javanese", "Kannada", "Kazakh", "Khmer", "Kinyarwanda", "Korean", "Kurdish (Kurmanji)", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian", "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Malayalam", "Maltese", "Maori", "Marathi", "Mongolian", "Myanmar (Burmese)", "Nepali", "Norwegian", "Odia (Oriya)", "Pashto", "Persian", "Polish", "Portuguese", "Punjabi", "Romanian", "Russian", "Samoan", "Scots Gaelic", "Serbian", "Sesotho", "Shona", "Sindhi", "Sinhala", "Slovak", "Slovenian", "Somali", "Spanish", "Sundanese", "Swahili", "Swedish", "Tagalog (Filipino)", "Tajik", "Tamil", "Tatar", "Telugu", "Thai", "Turkish", "Turkmen", "Ukrainian", "Urdu", "Uyghur", "Uzbek", "Vietnamese", "Welsh", "Xhosa", "Yiddish", "Yoruba", "Zulu" };
            CheckpointNames = new ObservableCollection<string>();
            Images = new List<BitmapImage>();

            IsCountryCBEnabled = true;
            IsCityCBEnabled = true;
            IsLanguageCBEnabled = true;

            DatePickerSelectedDate = DateTime.Now;
            IsGeneralTabSelected = true;
            IsDateTimeTabSelected = false;
            IsCheckpointsTabSelected = false;
            IsImagesTabSelected = false;
            IsNextButtonVisible = true;
            IsConfirmButtonVisible = false;

            LoadCountries();

            AddDateCommand = new ReversibleCommand(AddDateCommand_Execute, AddDateCommand_CanExecute, AddDateCommand_Reverse);
            RemoveDateCommand = new ReversibleCommand(RemoveDateCommand_Execute, RemoveDateCommand_CanExecute, RemoveDateCommand_Reverse);
            AddCheckpointCommand = new ReversibleCommand(AddCheckpointCommand_Execute, AddCheckpointCommand_CanExecute, AddCheckpointCommand_Reverse);
            RemoveCheckpointCommand = new ReversibleCommand(RemoveCheckpointCommand_Execute, RemoveCheckpointCommand_CanExecute, RemoveCheckpointCommand_Reverse);
            AddImageCommand = new RelayCommand(AddImageCommand_Execute);
            RemoveImageCommand = new RelayCommand(RemoveImageCommand_Execute, RemoveImageCommand_CanExecute);
            SetAsCoverCommand = new RelayCommand(SetAsCoverCommand_Execute, SetAsCoverCommand_CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImageCommand_Execute, PreviousImageCommand_CanExecute);
            NextImageCommand = new RelayCommand(NextImageCommand_Execute, NextImageCommand_CanExecute);
            ConfirmCommand = new RelayCommand(ConfirmCommand_Execute, ConfirmCommand_CanExecute);
            NextCommand = new RelayCommand(NextCommand_Execute, NextCommand_CanExecute);
            PreviousCommand = new RelayCommand(PreviousCommand_Execute, PreviousCommand_CanExecute);
            LoadCitiesCommand = new RelayCommand(LoadCitiesCommand_Execute);
            TabControlSelectionChangedCommand = new RelayCommand(TabControlSelectionChangedCommand_Execute);
            MostWantedLocationCheckedCommand = new RelayCommand(MostWantedLocationCheckedCommand_Execute);
            MostWantedLanguageCheckedCommand = new RelayCommand(MostWantedLanguageCheckedCommand_Execute);
            MostWantedLocationUncheckedCommand = new RelayCommand(MostWantedLocationUncheckedCommand_Execute);
            MostWantedLanguageUncheckedCommand = new RelayCommand(MostWantedLanguageUncheckedCommand_Execute);
        }

        private void LoadCountries()
        {
            Countries = _locationService.GetAllCountries().ToList();
            Countries.Sort();
        }

        private void AddImage(string fileName)
        {
            Uri uri = new Uri(fileName);
            BitmapImage newImage = new BitmapImage(uri);

            if (SelectedImage == null)
            {
                Images.Add(newImage);
            }
            else
            {
                Images.Insert(Images.IndexOf(SelectedImage) + 1, newImage);
            }
            SelectedImage = newImage;
            IsCoverImageSelected = false;
        }

        private void LoadCities()
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != SelectedCountry) continue;
                Cities.Add(location.City);
            }
        }

        private void ResetValues()
        {
            TourName = "";
            SelectedCountry = null;
            SelectedCity = null;
            SelectedLanguage = null;
            Description = "";
            MaxGuests = 0;
            Duration = 0;
            Dates.Clear();
            CheckpointNames.Clear();
            Images.Clear();
            SelectedImage = null;
            CoverImage = null;
            IsMostWantedLocationChecked = false;
            IsMostWantedLanguageChecked = false;
        }

        #region COMMANDS
        public ReversibleCommand AddDateCommand { get; }
        public ReversibleCommand RemoveDateCommand { get; }
        public ReversibleCommand AddCheckpointCommand { get; }
        public ReversibleCommand RemoveCheckpointCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand RemoveImageCommand { get; }
        public RelayCommand SetAsCoverCommand { get; }
        public RelayCommand PreviousImageCommand { get; }
        public RelayCommand NextImageCommand { get; }
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand PreviousCommand { get; }
        public RelayCommand LoadCitiesCommand { get; }
        public RelayCommand TabControlSelectionChangedCommand { get; }
        public RelayCommand MostWantedLocationCheckedCommand { get; }
        public RelayCommand MostWantedLanguageCheckedCommand { get; }
        public RelayCommand MostWantedLocationUncheckedCommand { get; }
        public RelayCommand MostWantedLanguageUncheckedCommand { get; }

        public void AddDateCommand_Execute(object? parameter)
        {
            Dates.Add(DatePickerSelectedDate);
            Validate();
            _commandStack.Push(new Tuple<ReversibleCommand, object?>(AddDateCommand, parameter));
        }

        public bool AddDateCommand_CanExecute(object? parameter)
        {
            return !Dates.Contains(DatePickerSelectedDate) && DatePickerSelectedDate.CompareTo(DateTime.Now) > 0;
        }

        public void AddDateCommand_Reverse(object? parameter)
        {
            Dates.RemoveAt(Dates.Count - 1);
            Validate();
        }

        public void RemoveDateCommand_Execute(object? parameter)
        {
            DateTime date = (DateTime)parameter;
            Dates.Remove(date);
            Validate();
            _commandStack.Push(new Tuple<ReversibleCommand, object?>(RemoveDateCommand, parameter));
        }

        public bool RemoveDateCommand_CanExecute(object? parameter)
        {
            return parameter is not null;
        }

        public void RemoveDateCommand_Reverse(object? parameter)
        {
            DateTime date = (DateTime)parameter;
            Dates.Add(date);
            Validate();
        }

        public void AddCheckpointCommand_Execute(object? parameter)
        {
            CheckpointNames.Add(EnteredCheckpointName);
            EnteredCheckpointName = "";
            Validate();
            _commandStack.Push(new Tuple<ReversibleCommand, object?>(AddCheckpointCommand, parameter));
        }

        public bool AddCheckpointCommand_CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(EnteredCheckpointName) && !CheckpointNames.Contains(EnteredCheckpointName);
        }

        public void AddCheckpointCommand_Reverse(object? parameter)
        {
            CheckpointNames.RemoveAt(CheckpointNames.Count - 1);
            Validate();
        }

        public void RemoveCheckpointCommand_Execute(object? parameter)
        {
            string checkpointName = parameter as string;
            CheckpointNames.Remove(checkpointName);
            Validate();
            _commandStack.Push(new Tuple<ReversibleCommand, object?>(RemoveCheckpointCommand, parameter));
        }

        public bool RemoveCheckpointCommand_CanExecute(object? parameter)
        {
            return parameter is not null;
        }

        public void RemoveCheckpointCommand_Reverse(object? parameter)
        {
            string checkpointName = parameter as string;
            CheckpointNames.Add(checkpointName);
            Validate();
        }

        public void AddImageCommand_Execute(object? parameter)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";

            if ((bool)dlg.ShowDialog()) AddImage(dlg.FileName);

            Validate();
        }

        public void RemoveImageCommand_Execute(object? parameter)
        {
            var index = Images.IndexOf(SelectedImage);
            if (SelectedImage == Images.Last()) index--;
            Images.Remove(SelectedImage);
            if (Images.Count == 0)
            {
                SelectedImage = null;
            }
            else
            {
                SelectedImage = Images[index];
            }
            IsCoverImageSelected = SelectedImage == CoverImage;

            Validate();
        }

        public bool RemoveImageCommand_CanExecute(object? parameter)
        {
            return SelectedImage is not null;
        }

        public void SetAsCoverCommand_Execute(object? parameter)
        {
            CoverImage = SelectedImage;
            IsCoverImageSelected = true;
        }

        public bool SetAsCoverCommand_CanExecute(object? parameter)
        {
            return SelectedImage is not null && SelectedImage != CoverImage;
        }

        public void PreviousImageCommand_Execute(object? prameter)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                if (SelectedImage == Images[i])
                {
                    SelectedImage = Images[i - 1];
                    IsCoverImageSelected = SelectedImage == CoverImage;
                    return;
                }
            }
        }

        public bool PreviousImageCommand_CanExecute(object? parameter)
        {
            return Images.Count != 0 && SelectedImage != Images.First();
        }

        public void NextImageCommand_Execute(object? parameter)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                if (SelectedImage == Images[i])
                {
                    SelectedImage = Images[i + 1];
                    IsCoverImageSelected = SelectedImage == CoverImage;
                    return;
                }
            }
        }

        public bool NextImageCommand_CanExecute(object? parameter)
        {
            return Images.Count != 0 && SelectedImage != Images.Last();
        }

        public void ConfirmCommand_Execute(object? parameter)
        {
            if (MessageBox.Show("Are you sure you want to create this tour?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.No) return;

            CoverImage ??= Images.First();
            foreach (var startTime in Dates)
            {
                Tour tour = _tourService.Create(TourName, SelectedCountry, SelectedCity, Description, SelectedLanguage, MaxGuests, startTime, Duration, CoverImage.ToString(), _guide.Id);
                foreach (var checkpointName in CheckpointNames)
                {
                    _checkpointService.Create(checkpointName, tour);
                }

                foreach (var image in Images)
                {
                    _tourImageService.Create(image.ToString(), tour);
                }
            _requestedTourNotificationService.Create(tour);
            }

            ResetValues();

            MessageBox.Show("Tour successfully created", "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

            _commandStack.Clear();
        }

        public bool ConfirmCommand_CanExecute(object? parameter)
        {
            return !HasErrors;
        }

        public void NextCommand_Execute(object? parameter)
        {
            if (IsGeneralTabSelected)
            {
                IsGeneralTabSelected = false;
                IsDateTimeTabSelected = true;
            }
            else if (IsDateTimeTabSelected)
            {
                IsDateTimeTabSelected = false;
                IsCheckpointsTabSelected = true;
            }
            else if (IsCheckpointsTabSelected)
            {
                IsCheckpointsTabSelected = false;
                IsImagesTabSelected = true;
                IsConfirmButtonVisible = true;
                IsNextButtonVisible = false;
            }
        }

        public bool NextCommand_CanExecute(object? parameter)
        {
            return !IsImagesTabSelected;
        }

        public void PreviousCommand_Execute(object? parameter)
        {
            if (IsDateTimeTabSelected)
            {
                IsDateTimeTabSelected = false;
                IsGeneralTabSelected = true;
            }
            else if (IsCheckpointsTabSelected)
            {
                IsCheckpointsTabSelected = false;
                IsDateTimeTabSelected = true;
            }
            else if (IsImagesTabSelected)
            {
                IsImagesTabSelected = false;
                IsCheckpointsTabSelected = true;
                IsConfirmButtonVisible = false;
                IsNextButtonVisible = true;
            }
        }

        public bool PreviousCommand_CanExecute(object? parameter)
        {
            return !IsGeneralTabSelected;
        }

        public void LoadCitiesCommand_Execute(object? parameter)
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != SelectedCountry) continue;
                Cities.Add(location.City);
            }
        }

        public void TabControlSelectionChangedCommand_Execute(object? parameter)
        {
            if (IsImagesTabSelected)
            {
                IsNextButtonVisible = false;
                IsConfirmButtonVisible = true;
            }
            else
            {
                IsNextButtonVisible = true;
                IsConfirmButtonVisible = false;
            }
            Validate();
        }

        public void MostWantedLocationCheckedCommand_Execute(object? parameter)
        {
            var location = _tourRequestService.GetMostWantedLocation();
            SelectedCountry = location.Country;
            SelectedCity = location.City;
        }

        public void MostWantedLanguageCheckedCommand_Execute(object? parameter)
        {
            SelectedLanguage = _tourRequestService.GetMostWantedLanguage();
        }

        public void MostWantedLocationUncheckedCommand_Execute(object? parameter)
        {
            SelectedCountry = null;
            SelectedCity = null;
        }

        public void MostWantedLanguageUncheckedCommand_Execute(object? parameter)
        {
            SelectedLanguage = null;
        }
        #endregion
    }
}
