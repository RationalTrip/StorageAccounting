namespace StorageAccounting.Domain.Exceptions.Results
{
    public class NotEnoughAreaException : StorageAccountingException
    {
        public NotEnoughAreaException(int requiredArea, int availableArea) :
            base("Not enough area",
                $"Not enough area: available area '{availableArea}', required area '{requiredArea}'")
        {

        }
    }
}
