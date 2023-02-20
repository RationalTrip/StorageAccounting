using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Entities;
using StorageAccounting.WebAPI.Extensions;

namespace StorageAccounting.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentingContractController : ControllerBase
    {
        private readonly IRentingContractService _contractService;

        public RentingContractController(IRentingContractService contractService)
        {
            _contractService = contractService;
        }

        private const string CreateRentingContractRouteName = nameof(RentingContract) + "." + nameof(GetByIdAsync);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<RentingContractReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RentingContractReadDto>>> GetAllAsync(CancellationToken token,
            [FromQuery] int? start = null,
            [FromQuery] int? size = null) =>
            (await _contractService.GetAllAsync(start, size, token))
                .Match<ActionResult<IEnumerable<RentingContractReadDto>>>(
                    contracts => Ok(contracts),
                    exc => exc.Handle());

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CountReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CountReadDto>> GetCountAsync(CancellationToken token) =>
            (await _contractService.GetCountAsync(token))
                .Match<ActionResult<CountReadDto>>(
                    count => Ok(count),
                    exc => exc.Handle());

        [HttpGet("{id}", Name = CreateRentingContractRouteName)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RentingContractReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<RentingContractReadDto>> GetByIdAsync(int id, CancellationToken token) =>
            (await _contractService.GetByIdAsync(id, token))
                .Match<ActionResult<RentingContractReadDto>>(
                    contract => Ok(contract),
                    exc => exc.Handle());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RentingContractReadDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<RentingContractReadDto>> CreateAsync(RentingContractCreateDto createModel,
            CancellationToken token) =>
            (await _contractService.CreateAsync(createModel, token))
                .Match<ActionResult<RentingContractReadDto>>(
                    contract => CreatedAtRoute(CreateRentingContractRouteName, new { id = contract.Id }, contract),
                    exc => exc.Handle());

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RemoveAsync(int id, CancellationToken token) =>
            (await _contractService.RemoveAsync(id, token))
                .Match<ActionResult>(
                    () => Ok(),
                    exc => exc.Handle());
    }
}
