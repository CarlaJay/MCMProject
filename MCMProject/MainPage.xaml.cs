using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            Console.WriteLine("Add Movie button clicked."); // Debugging statement

            var newMovie = new Movie
            {
                Title = "New Movie",
                Genre = "Action",
                ReleaseYear = 2023,
                Director = "John Doe",
                Rating = 7.5m,
                Watched = false
            };

            _movieService.AddMovie(newMovie);
            LoadMovies();
        }

        private void OnDeleteMovieClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var movieId = (int)button.CommandParameter;

            _movieService.DeleteMovie(movieId);
            LoadMovies();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}