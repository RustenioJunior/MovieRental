using System;

namespace MovieRental.Exceptions
{
    public abstract class CustomerException : Exception
    {
        public CustomerException(string message) : base(message) { }
    }

    public class CustomerNotFoundException : CustomerException
    {
        public CustomerNotFoundException(int customerId)
            : base($"Customer with ID {customerId} was not found")
        { }
    }

    public class CustomerEmailAlreadyExistsException : CustomerException
    {
        public CustomerEmailAlreadyExistsException(string email)
            : base($"Customer with email '{email}' already exists")
        { }
    }

    public class CustomerInactiveException : CustomerException
    {
        public CustomerInactiveException(int customerId)
            : base($"Customer with ID {customerId} is inactive")
        { }
    }
}