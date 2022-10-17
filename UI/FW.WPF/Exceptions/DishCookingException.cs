using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FW.WPF.Domain.Exceptions;

public class DishCookingException : Exception
{
    public string DishError { get; set; }
    public DishCookingException(string dishError)
    {
        DishError = dishError; 
    }

    public DishCookingException(string dishError, string message) : base(message)
    {
        DishError = dishError;
    }

    public DishCookingException(string dishError, string message, Exception innerException) : base(message, innerException)
    {
        DishError = dishError;
    }
}
