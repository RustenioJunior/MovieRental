using System;

namespace MovieRental.Exceptions
{
    public abstract class MovieException : Exception
    {
        public MovieException(string message) : base(message) { }
    }

    public class MovieNotFoundException : MovieException
    {
        public MovieNotFoundException(int movieId)
            : base($"Movie with ID {movieId} was not found")
        { }
    }

    public class MovieNotAvailableException : MovieException
    {
        public MovieNotAvailableException(string movieTitle)
            : base($"Movie '{movieTitle}' is not available for rental")
        { }
    }

    public class MovieAlreadyRentedException : MovieException
    {
        public MovieAlreadyRentedException(string movieTitle)
            : base($"Movie '{movieTitle}' is already rented")
        { }
    }
}