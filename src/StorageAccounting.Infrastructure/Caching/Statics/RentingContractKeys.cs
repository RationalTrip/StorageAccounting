namespace StorageAccounting.Infrastructure.Caching.Statics
{
    internal class RentingContractKeys
    {
        private const string ENTITY_NAME = "RentingContract";

        public static string GetAllKey() =>
            $"_Get_All_{ENTITY_NAME}";

        public static string GetCountKey() =>
            GetAllKey() + "_Count";

        public static string GetByIdKey(int id) =>
            $"_Get_{ENTITY_NAME}_By_{id}";
    }
}
