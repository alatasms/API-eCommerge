using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class EmailConfirmationFailedException : Exception
    {

        public EmailConfirmationFailedException() : base("Email doğrulaması esnasında bir hata ile karşılaşıldı.")
        {
        }

        public EmailConfirmationFailedException(string? message) : base(message)
        {
        }

        public EmailConfirmationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
