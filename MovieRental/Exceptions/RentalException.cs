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

    public class InvalidRentalRequestException : RentalException
    {
        public InvalidRentalRequestException(string message) : base(message) { }
    }

    public class InvalidDaysRentedException : RentalException
    {
        public InvalidDaysRentedException(int daysRented)
            : base($"Days rented must be greater than 0. Received: {daysRented}")
        { }
    }

    public class InvalidPaymentMethodException : RentalException
    {
        public InvalidPaymentMethodException(string paymentMethod)
            : base($"Payment method is required and cannot be empty. Received: {paymentMethod}")
        { }
    }

    public class CustomerNameRequiredException : RentalException
    {
        public CustomerNameRequiredException()
            : base("Customer name is required and cannot be empty")
        { }
    }
}