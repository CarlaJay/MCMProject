using System.ComponentModel;

namespace MCMProject
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private readonly MovieService _movieService;
        private List<Movie> _movies;
        private int? _selectedMovieId; // Tracks the ID of the selected movie for editing

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

            SaveButton.Text = "Add Movie"; // Default text for the button
        }

        private void LoadMovies()
        {
            Movies = _movieService.GetAllMovies();
        }

        private void OnSaveMovieClicked(object sender, EventArgs e)
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

            if (_selectedMovieId.HasValue)
            {
                // Update existing movie
                var movieToUpdate = _movieService.GetAllMovies().FirstOrDefault(m => m.MovieID == _selectedMovieId.Value);
                if (movieToUpdate != null)
                {
                    movieToUpdate.Title = title;
                    movieToUpdate.Genre = genre;
                    movieToUpdate.ReleaseYear = releaseYear;
                    movieToUpdate.Director = director;
                    movieToUpdate.Rating = rating;
                    movieToUpdate.Watched = watched;

                    _movieService.UpdateMovie(movieToUpdate);
                    _selectedMovieId = null; // Reset the selected movie ID
                    SaveButton.Text = "Add Movie"; // Reset button text
                }
            }
            else
            {
                // Create new movie
                var newMovie = new Movie
                {
                    Title = title,
                    Genre = genre,
                    ReleaseYear = releaseYear,
                    Director = director,
                    Rating = rating,
                    Watched = watched
                };

                _movieService.AddMovie(newMovie);
            }

            // Clear input fields and reload movies
            ClearInputFields();
            LoadMovies();
        }

        private void OnEditMovieClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var movieId = (int)button.CommandParameter;

            // Find the selected movie
            var selectedMovie = _movieService.GetAllMovies().FirstOrDefault(m => m.MovieID == movieId);
            if (selectedMovie != null)
            {
                // Populate input fields with the selected movie's details
                TitleEntry.Text = selectedMovie.Title;
                GenreEntry.Text = selectedMovie.Genre;
                ReleaseYearEntry.Text = selectedMovie.ReleaseYear?.ToString();
                DirectorEntry.Text = selectedMovie.Director;
                RatingEntry.Text = selectedMovie.Rating?.ToString();
                WatchedCheckbox.IsChecked = selectedMovie.Watched;

                // Set the selected movie ID and update the button text
                _selectedMovieId = selectedMovie.MovieID;
                SaveButton.Text = "Update Movie";
            }
        }

        private void OnDeleteMovieClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var movieId = (int)button.CommandParameter;

            _movieService.DeleteMovie(movieId);
            LoadMovies();
        }

        private void ClearInputFields()
        {
            TitleEntry.Text = string.Empty;
            GenreEntry.Text = string.Empty;
            ReleaseYearEntry.Text = string.Empty;
            DirectorEntry.Text = string.Empty;
            RatingEntry.Text = string.Empty;
            WatchedCheckbox.IsChecked = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}