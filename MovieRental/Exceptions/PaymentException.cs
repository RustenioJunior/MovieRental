using System;

namespace MovieRental.Exceptions
{
    public abstract class PaymentException : Exception
    {
        public PaymentException(string message) : base(message) { }
    }

    public class PaymentFailedException : PaymentException
    {
        public PaymentFailedException(string message) : base(message) { }
    }

    public class PaymentProviderNotAvailableException : PaymentException
    {
        public PaymentProviderNotAvailableException(string provider)
            : base($"Payment provider '{provider}' is not available")
        { }
    }

    public class InsufficientFundsException : PaymentException
    {
        public InsufficientFundsException(decimal amount)
            : base($"Insufficient funds for payment of {amount:C}")
        { }
    }
}