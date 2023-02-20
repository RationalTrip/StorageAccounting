using System;

namespace StorageAccounting.Domain.Exceptions.Results
{
    public class StorageAccountingException : Exception
    {
        public StorageAccountingException(string title, string message) : base(message)
        {
            Title = title;
        }
        public string Title { get; set; }
    }
}
