using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ArangkadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<DriversController> _logger;
        public DriversController(IDriverService driverService, IVehicleService vehicleService, ILogger<DriversController> logger)
        {
            _driverService = driverService;
            _vehicleService = vehicleService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new driver to the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/drivers/add
        ///     {
        ///         "fullName": "John Doe",
        ///         "contactNumber": "1234567890",
        ///         "licenseNumber": "12345678",
        ///         "address": "123 Main Street",
        ///         "expirationDate": "2022-05-31",
        ///         "dlCodes": "A,B"
        ///     }
        ///
        /// </remarks>
        /// <param name="driverToCreate">The driver details to add.</param>
        /// <returns>The newly created driver.</returns>
        /// <response code="201">Returns the newly created driver.</response>
        /// <response code="400">If the driver is invalid.</response>
        /// <response code="404">If the driver failed to be created.</response>
        /// <response code="409">If the driver already exists.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPost("add", Name = "AddDriver")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDriver([FromBody] DriverCreationDto driverToCreate)
        {
            try
            {
                if (await _driverService.GetByFullName(driverToCreate.FullName) != null)
                {
                    return Conflict("Driver already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newDriver = await _driverService.CreateDriver(driverToCreate);

                return CreatedAtRoute("GetDriverById", new { id = newDriver!.Id }, newDriver);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddDriver)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves all drivers belonging to a specific operator.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/drivers/by/op/{operatorId}
        ///
        /// </remarks>
        /// <param name="operatorId">The ID of the operator.</param>
        /// <returns>A list of drivers belonging to the specified operator.</returns>
        /// <response code="200">Returns the list of drivers.</response>
        /// <response code="404">If no drivers were found for the specified operator ID.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("operator/{operatorId}", Name = "GetDriversByOperatorId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DriverDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDriversByOperatorId(int operatorId)
        {
            try
            {
                var drivers = await _driverService.GetAllByOperatorId(operatorId);

                if (drivers == null)
                {
                    return NotFound($"No drivers found for the specified operator ID: {operatorId}.");
                }

                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDriversByOperatorId)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a driver by their ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/drivers/{id}
        ///
        /// </remarks>
        /// <param name="id">The ID of the driver.</param>
        /// <returns>The driver with the specified ID.</returns>
        /// <response code="200">Returns the driver.</response>
        /// <response code="404">If the driver with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}", Name = "GetDriverById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDriverById(int id)
        {
            try
            {
                var driver = await _driverService.GetById(id);
                if(driver == null)
                {
                    return NotFound($"Driver with id: {id} does not exist.");
                }
                return Ok(driver);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDriverById)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Updates the details of a driver.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/drivers/{id}/edit
        ///     {
        ///         "fullName": "John Doe",
        ///         "contactNumber": "1234567890",
        ///         "licenseNumber": "12345678",
        ///         "address": "123 Main Street",
        ///         "expirationDate": "2022-05-31",
        ///         "dlCodes": "A,B"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The ID of the driver.</param>
        /// <param name="driverToUpdate">The updated driver details.</param>
        /// <returns>The updated driver.</returns>
        /// <response code="200">Returns the updated driver.</response>
        /// <response code="404">If the driver with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}/edit", Name = "EditDriver")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditDriver(int id, [FromBody] DriverUpdateDto driverToUpdate)
        {
            try
            {
                if(await _driverService.GetById(id) == null)
                {
                    return NotFound($"Driver with id: {id} does not exist.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedDriver = await _driverService.UpdateDriver(id, driverToUpdate);

                return Ok(updatedDriver!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EditDriver)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Assigns a driver to a vehicle.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/drivers/{id}/assign
        ///     {
        ///         "plateNumberToAssign": "ABC123"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The ID of the driver.</param>
        /// <param name="vehicle">The plate number of the vehicle to assign.</param>
        /// <returns>The updated driver.</returns>
        /// <response code="200">Returns the updated driver.</response>
        /// <response code="404">If the driver or the vehicle does not exist.</response>
        /// <response code="409">If the vehicle is already rented.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}/assign", Name = "AssignDriver")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignDriver(int id, string vehicle)
        {
            try
            {
                var existingVehicle = await _vehicleService.GetByPlateNumber(vehicle);
                    if (existingVehicle == null)
                    {
                        return NotFound($"Vehicle with plate number: {vehicle} does not exist.");
                    }

                    if (existingVehicle.RentStatus)
                    {
                        return StatusCode(409, $"Vehicle with plate number: {vehicle} is already rented.");
                    }
               
                var updatedDriver = await _driverService.UpdateVehicleAssigned(id, vehicle);
                    
                await _vehicleService.UpdateRentStatus(existingVehicle.Id, true);

                return Ok(updatedDriver!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AssignDriver)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Removes a driver from the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/drivers/{id}/remove
        ///
        /// </remarks>
        /// <param name="id">The ID of the driver to remove.</param>
        /// <returns>A message indicating the success of the operation.</returns>
        /// <response code="200">Returns a message indicating the successful deletion.</response>
        /// <response code="404">If the driver does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}/remove", Name = "RemoveDriver")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveDriver(int id)
        {
            try
            {
                var driverToDelete = await _driverService.DeleteDriver(id);
                if (driverToDelete == false)
                {
                    return NotFound($"Driver with id: {id} does not exist.");
                }
                return Ok($"Driver with id: {id} is successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(RemoveDriver)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
