using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.Equipments;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Services;
using StorageAccounting.WebAPI.Extensions;

namespace StorageAccounting.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        private const string CreateEquipmentRouteName = nameof(EquipmentController) + "." + nameof(GetByIdAsync);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<EquipmentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EquipmentReadDto>>> GetAllAsync(CancellationToken token,
            [FromQuery] int? start = null,
            [FromQuery] int? size = null) =>
            (await _equipmentService.GetAllAsync(start, size, token))
                .Match<ActionResult<IEnumerable<EquipmentReadDto>>>(
                    equipments => Ok(equipments),
                    exc => exc.Handle());

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CountReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CountReadDto>> GetCountAsync(CancellationToken token) =>
            (await _equipmentService.GetCountAsync(token))
                .Match<ActionResult<CountReadDto>>(
                    count => Ok(count),
                    exc => exc.Handle());

        [HttpGet("{id}", Name = CreateEquipmentRouteName)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(EquipmentReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<EquipmentReadDto>> GetByIdAsync(int id, CancellationToken token) =>
            (await _equipmentService.GetByIdAsync(id, token))
                .Match<ActionResult<EquipmentReadDto>>(
                    equipment => Ok(equipment),
                    exc => exc.Handle());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EquipmentReadDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<EquipmentReadDto>> CreateAsync(EquipmentCreateDto createModel,
            CancellationToken token) =>
            (await _equipmentService.CreateAsync(createModel, token))
                .Match<ActionResult<EquipmentReadDto>>(
                    equipment => CreatedAtRoute(CreateEquipmentRouteName, new { id = equipment.Id }, equipment),
                    exc => exc.Handle());

        [HttpGet("{id}/contracts")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<RentingContractReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int id,
            CancellationToken token,
            [FromQuery] int? start = null,
            [FromQuery] int? size = null) =>
            (await _equipmentService.GetRentingContractsAsync(id, start, size, token))
                .Match<ActionResult<IEnumerable<RentingContractReadDto>>>(
                    contract => Ok(contract),
                    exc => exc.Handle());


        [HttpGet("{id}/contracts/count")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CountReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CountReadDto>> GetRentingContractsCountAsync(int id,
            CancellationToken token) =>
            (await _equipmentService.GetRentingContractsCountAsync(id, token))
                .Match<ActionResult<CountReadDto>>(
                    count => Ok(count),
                    exc => exc.Handle());
    }
}
