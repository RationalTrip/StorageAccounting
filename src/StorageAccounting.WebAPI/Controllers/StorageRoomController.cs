using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Models.Dtos.StorageRooms;
using StorageAccounting.Application.Services;
using StorageAccounting.WebAPI.Extensions;

namespace StorageAccounting.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StorageRoomController : ControllerBase
    {
        private readonly IStorageRoomService _roomService;

        public StorageRoomController(IStorageRoomService roomService)
        {
            _roomService = roomService;
        }

        private const string CreateStorageRoomRouteName = nameof(StorageRoomController) + "." + nameof(GetByIdAsync);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<StorageRoomReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StorageRoomReadDto>>> GetAllAsync(CancellationToken token,
            [FromQuery] int? start = null,
            [FromQuery] int? size = null) =>
            (await _roomService.GetAllAsync(start, size, token))
                .Match<ActionResult<IEnumerable<StorageRoomReadDto>>>(
                    room => Ok(room),
                    exc => exc.Handle());

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CountReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CountReadDto>> GetCountAsync(CancellationToken token) =>
            (await _roomService.GetCountAsync(token))
                .Match<ActionResult<CountReadDto>>(
                    count => Ok(count),
                    exc => exc.Handle());

        [HttpGet("{id}", Name = CreateStorageRoomRouteName)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StorageRoomReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<StorageRoomReadDto>> GetByIdAsync(int id, CancellationToken token) =>
            (await _roomService.GetByIdAsync(id, token))
                .Match<ActionResult<StorageRoomReadDto>>(
                    room => Ok(room),
                    exc => exc.Handle());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(StorageRoomReadDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<StorageRoomReadDto>> CreateAsync(StorageRoomCreateDto createModel,
            CancellationToken token) =>
            (await _roomService.CreateAsync(createModel, token))
                .Match<ActionResult<StorageRoomReadDto>>(
                    room => CreatedAtRoute(CreateStorageRoomRouteName, new { id = room.Id }, room),
                    exc => exc.Handle());

        [HttpGet("{id}/contracts")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<RentingContractReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int id,
            CancellationToken token,
            [FromQuery] int? start = null,
            [FromQuery] int? size = null) =>
            (await _roomService.GetRentingContractsAsync(id, start, size, token))
                .Match<ActionResult<IEnumerable<RentingContractReadDto>>>(
                    contracts => Ok(contracts),
                    exc => exc.Handle());


        [HttpGet("{id}/contracts/count")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CountReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CountReadDto>> GetRentingContractsCountAsync(int id,
            CancellationToken token) =>
            (await _roomService.GetRentingContractsCountAsync(id, token))
                .Match<ActionResult<CountReadDto>>(
                    count => Ok(count),
                    exc => exc.Handle());

        [HttpGet("{id}/area")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StorageRoomRentedAreaReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<StorageRoomRentedAreaReadDto>> GetStorageRoomRentedAreaAsync(int id,
            CancellationToken token) =>
            (await _roomService.GetRentedAreaAsync(id, token))
                .Match<ActionResult<StorageRoomRentedAreaReadDto>>(
                    roomRentedArea => Ok(roomRentedArea),
                    exc => exc.Handle());
    }
}
