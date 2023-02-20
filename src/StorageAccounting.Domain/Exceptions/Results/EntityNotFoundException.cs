namespace StorageAccounting.Domain.Exceptions.Results
{
    public class EntityNotFoundException : StorageAccountingException
    {
        public EntityNotFoundException(string id) :
            base("Entity Not Found",
                $"Entity with id '{id}' not found")
        { }

        public EntityNotFoundException(string id, string entityName) :
            base("Entity Not Found",
                $"Entity '{entityName}' with id '{id}' not found")
        { }

    }
}
