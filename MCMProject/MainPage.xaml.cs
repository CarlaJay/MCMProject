using System;
using System.ComponentModel;
using System.Reflection.PortableExecutable;

namespace MCMProject
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private readonly MovieService _movieService;
        private List<Movie> _movies;

        public List<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();

            var dbContext = new MovieDbContext();
            _movieService = new MovieService(dbContext);

            LoadMovies();
            BindingContext = this;
        }

        private void LoadMovies()
        {
            Movies = _movieService.GetAllMovies();
            Console.WriteLine($"Loaded {Movies.Count} movies."); // Debugging statement
        }

        private void OnAddMovieClicked(object sender, EventArgs e)
        {
            // Retrieve input values
            var title = TitleEntry.Text?.Trim();
            var genre = GenreEntry.Text?.Trim();
            var releaseYearText = ReleaseYearEntry.Text?.Trim();
            var director = DirectorEntry.Text?.Trim();
            var ratingText = RatingEntry.Text?.Trim();
            var watched = WatchedCheckbox.IsChecked;

            // Validate input
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(genre) ||
                string.IsNullOrWhiteSpace(releaseYearText) || string.IsNullOrWhiteSpace(director) ||
                string.IsNullOrWhiteSpace(ratingText))
            {
                DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            if (!int.TryParse(releaseYearText, out int releaseYear) || !decimal.TryParse(ratingText, out decimal rating))
            {
                DisplayAlert("Error", "Invalid Release Year or Rating. Please enter valid numbers.", "OK");
                return;
            }

            // Create a new movie
            var newMovie = new Movie
            {
                Title = title,
                Genre = genre,
                ReleaseYear = releaseYear,
                Director = director,
                Rating = rating,
                Watched = watched
            };

            // Add the movie and refresh the list
            _movieService.AddMovie(newMovie);
            LoadMovies();

            // Clear input fields
            TitleEntry.Text = string.Empty;
            GenreEntry.Text = string.Empty;
            ReleaseYearEntry.Text = string.Empty;
            DirectorEntry.Text = string.Empty;
            RatingEntry.Text = string.Empty;
            WatchedCheckbox.IsChecked = false;
        }

        private void OnDeleteMovieClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var movieId = (int)button.CommandParameter;

            _movieService.DeleteMovie(movieId);
            LoadMovies();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}