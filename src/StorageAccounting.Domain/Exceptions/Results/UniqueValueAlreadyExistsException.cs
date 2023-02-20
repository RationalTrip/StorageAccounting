namespace StorageAccounting.Domain.Exceptions.Results
{
    public class UniqueValueAlreadyExistsException : StorageAccountingException
    {
        public UniqueValueAlreadyExistsException(string value,
            string propertyName,
            string entityName,
            string existedEntityId) :
            base("Entity with unique value already exists",
                GetMessage(value, propertyName, entityName, existedEntityId))
        { }

        private static string GetMessage(string value, string propertyName, string entityName, string existedEntityId) =>
            $"Unique value already exists: in entity '{entityName}' for the " +
                $"property '{propertyName}' already exists value '{value}'. " +
                $"Entity with such value has id '{existedEntityId}'";
    }
}
