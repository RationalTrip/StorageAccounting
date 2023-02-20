using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.Equipments;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Application.Services
{
    public interface IEquipmentService
    {
        Task<Result<IEnumerable<EquipmentReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token);

        Task<Result<CountReadDto>> GetCountAsync(CancellationToken token);

        Task<Result<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int equipmentId,
            int? start,
            int? size,
            CancellationToken token);

        Task<Result<CountReadDto>> GetRentingContractsCountAsync(int equipmentId,
            CancellationToken token);

        Task<Result<EquipmentReadDto>> GetByIdAsync(int id, CancellationToken token);

        Task<Result<EquipmentReadDto>> CreateAsync(EquipmentCreateDto model, CancellationToken token);
    }
}
