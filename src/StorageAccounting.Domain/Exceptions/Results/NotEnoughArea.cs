namespace StorageAccounting.Domain.Exceptions.Results
{
    public class NotEnoughArea : StorageAccountingException
    {
        public NotEnoughArea(int requiredArea, int availableArea) :
            base("Not enough area",
                $"Not enough area: available area '{availableArea}', required area '{requiredArea}'")
        {

        }
    }
}
