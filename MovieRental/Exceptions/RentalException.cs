using System;

namespace MovieRental.Exceptions
{
    public abstract class RentalException : Exception
    {
        public RentalException(string message) : base(message) { }
    }

    public class RentalNotFoundException : RentalException
    {
        public RentalNotFoundException(int rentalId)
            : base($"Rental with ID {rentalId} was not found")
        { }
    }

    public class RentalAlreadyReturnedException : RentalException
    {
        public RentalAlreadyReturnedException(int rentalId)
            : base($"Rental with ID {rentalId} has already been returned")
        { }
    }
}