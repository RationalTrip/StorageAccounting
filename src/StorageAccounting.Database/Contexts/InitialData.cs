using StorageAccounting.Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace StorageAccounting.Database.Contexts
{
    internal static class InitialData
    {
        private static Equipment Router { get; } = new Equipment
        {
            Id = 1,
            Name = "Router",
            RequiredArea = 1
        };

        private static Equipment Switch { get; } = new Equipment
        {
            Id = 2,
            Name = "Switch",
            RequiredArea = 2
        };

        private static Equipment Server { get; } = new Equipment
        {
            Id = 3,
            Name = "Server",
            RequiredArea = 5
        };

        private static Equipment Cluster { get; } = new Equipment
        {
            Id = 4,
            Name = "Cluster",
            RequiredArea = 50
        };

        private static Equipment Miner { get; } = new Equipment
        {
            Id = 5,
            Name = "Miner",
            RequiredArea = 4
        };

        private static StorageRoom Dorm { get; } = new StorageRoom
        {
            Id = 1,
            Name = "Dorm: Lomonosova 35",
            TotalArea = 10
        };

        private static StorageRoom Office { get; } = new StorageRoom
        {
            Id = 2,
            Name = "Office: Kyiv, Polyva 21",
            TotalArea = 250
        };

        private static StorageRoom Rada { get; } = new StorageRoom
        {
            Id = 3,
            Name = "Verkhovna Rada of Ukraine: Kyiv, Bankova 6-8",
            TotalArea = 1000
        };
        private static StorageRoom Frex { get; } = new StorageRoom
        {
            Id = 4,
            Name = "Frex: Faculty",
            TotalArea = 800
        };
        private static StorageRoom Storage { get; } = new StorageRoom
        {
            Id = 5,
            Name = "Storage: general",
            TotalArea = 50
        };
        private static StorageRoom Shelter { get; } = new StorageRoom
        {
            Id = 6,
            Name = "Shelter: it must be empty, because you should be there when air alarm",
            TotalArea = 100
        };

        public static IEnumerable<Equipment> GetInitialEquipment =>
            new Equipment[] { Router, Switch, Server, Cluster, Miner };

        public static IEnumerable<StorageRoom> GetInitialStorageRooms =>
            new StorageRoom[] { Dorm, Office, Rada, Frex, Storage, Shelter };

        public static IEnumerable<RentingContract> GetInitialRentingContract =>
            new RentingContract[]
        {
            new RentingContract
            {
                Id = 1,
                EquipmentId = Switch.Id,
                //Equipment = Switch,
                RoomId = Dorm.Id,
                //Room = Dorm,
                EquipmentCount = 3
            },
            new RentingContract
            {
                Id = 2,
                EquipmentId = Router.Id,
                //Equipment = Router,
                RoomId = Dorm.Id,
                //Room = Dorm,
                EquipmentCount = 2
            },
            new RentingContract
            {
                Id = 3,
                EquipmentId = Router.Id,
                //Equipment = Router,
                RoomId = Office.Id,
                //Room = Office,
                EquipmentCount = 20
            },
            new RentingContract
            {
                Id = 4,
                EquipmentId = Switch.Id,
                //Equipment = Switch,
                RoomId = Office.Id,
                //Room = Office,
                EquipmentCount = 15
            },
            new RentingContract
            {
                Id = 5,
                EquipmentId = Server.Id,
                //Equipment = Server,
                RoomId = Office.Id,
                //Room = Office,
                EquipmentCount = 10
            },
            new RentingContract
            {
                Id = 6,
                EquipmentId = Switch.Id,
                //Equipment = Switch,
                RoomId = Office.Id,
                //Room = Office,
                EquipmentCount = 10
            },
            new RentingContract
            {
                Id = 7,
                EquipmentId = Switch.Id,
                //Equipment = Switch,
                RoomId = Office.Id,
                //Room = Office,
                EquipmentCount = 15
            },
            new RentingContract
            {
                Id = 8,
                EquipmentId = Router.Id,
                //Equipment = Router,
                RoomId = Rada.Id,
                //Room = Rada,
                EquipmentCount = 150
            },
            new RentingContract
            {
                Id = 9,
                EquipmentId = Switch.Id,
                //Equipment = Switch,
                RoomId = Rada.Id,
                //Room = Rada,
                EquipmentCount = 50
            },
            new RentingContract
            {
                Id = 10,
                EquipmentId = Server.Id,
                //Equipment = Server,
                RoomId = Rada.Id,
                //Room = Rada,
                EquipmentCount = 10
            },
            new RentingContract
            {
                Id = 11,
                EquipmentId = Cluster.Id,
                //Equipment = Cluster,
                RoomId = Frex.Id,
                //Room = Frex,
                EquipmentCount = 6
            },
            new RentingContract
            {
                Id = 12,
                EquipmentId = Router.Id,
                //Equipment = Router,
                RoomId = Frex.Id,
                //Room = Frex,
                EquipmentCount = 60
            },
            new RentingContract
            {
                Id = 13,
                EquipmentId = Router.Id,
                //Equipment = Router,
                RoomId = Frex.Id,
                //Room = Frex,
                EquipmentCount = 40
            },
            new RentingContract
            {
                Id = 14,
                EquipmentId = Switch.Id,
                //Equipment = Switch,
                RoomId = Frex.Id,
                //Room = Frex,
                EquipmentCount = 50
            },
            new RentingContract
            {
                Id = 15,
                EquipmentId = Router.Id,
                //Equipment = Router,
                RoomId = Storage.Id,
                //Room = Storage,
                EquipmentCount = 35
            }
        };
    }
}
