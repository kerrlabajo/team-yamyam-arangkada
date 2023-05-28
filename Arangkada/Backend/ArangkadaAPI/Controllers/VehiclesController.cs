using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Models;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ArangkadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehiclesController> _logger;       

        public VehiclesController(IVehicleService vehicleService, ILogger<VehiclesController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;          
            
        }

        /// <summary>
        /// Adds a new vehicle to the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/vehicles/add
        ///     {
        ///         "operatorName": "John Doe",
        ///         "crNumber": "1234567890",
        ///         "plateNumber": "ABC123",
        ///         "bodyType": "sedan",
        ///         "make": "Toyota",
        ///         "rentFee": 500,
        ///         "rentStatus": true
        ///     }
        ///
        /// </remarks>
        /// <param name="vehicleToAdd">The vehicle details to add.</param>
        /// <returns>The newly created vehicle.</returns>
        /// <response code="201">Returns the newly created vehicle.</response>
        /// <response code="400">If the vehicle details is invalid.</response>
        /// <response code="404">If the operator for the vehicle is not found.</response>
        /// <response code="409">If the vehicle with the same plate number already exists.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPost("add", Name = "AddVehicle")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleCreationDto vehicleToAdd)
        {
            try
            {                                  
                if(await _vehicleService.GetByPlateNumber(vehicleToAdd.PlateNumber) != null)
                {
                    return Conflict("Vehicle with the same plate number already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newVehicle = await _vehicleService.AddVehicle(vehicleToAdd);

                return CreatedAtRoute("GetVehicleById", new { id = newVehicle!.Id }, newVehicle);

            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddVehicle)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves all vehicles currently existing in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Vehicles
        ///     {
        ///          "id": 1,
        ///          "operatorName": "Operator 1",
        ///          "crNumber": "CR0001",
        ///          "plateNumber": "AAA1234",
        ///          "bodyType": "Sedan",
        ///          "make": "Toyota",
        ///          "rentFee": 500,
        ///          "rentStatus": true
        ///     },
        ///     {
        ///          "id": 2,
        ///          "operatorName": "Operator 2",
        ///          "crNumber": "CR0002",
        ///          "plateNumber": "BBB5678",
        ///          "bodyType": "SUV",
        ///          "make": "Honda",
        ///          "rentFee": 1500,
        ///          "rentStatus": false
        ///     }
        /// </remarks>
        /// <returns>List of all vehicles.</returns>
        /// <response code="200">Returns the list of all vehicles.</response>
        /// <response code="204">If no vehicle was found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet(Name = "GetVehicles")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAll();

                if (vehicles.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(vehicles);
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetVehicles)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves all vehicles with matching operator id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Vehicles/by/op/1
        ///     {
        ///          "id": 1,
        ///          "operatorName": "Operator 1",
        ///          "crNumber": "CR0001",
        ///          "plateNumber": "AAA1234",
        ///          "bodyType": "Sedan",
        ///          "make": "Toyota",
        ///          "rentFee": 500,
        ///          "rentStatus": true
        ///     },
        ///     {
        ///          "id": 3,
        ///          "operatorName": "Operator 1",
        ///          "crNumber": "CR0003",
        ///          "plateNumber": "CCC9012",
        ///          "bodyType": "Truck",
        ///          "make": "Ford",
        ///          "rentFee": 2000,
        ///          "rentStatus": false
        ///     }
        /// </remarks>
        /// <returns>List of all vehicles with matching operator id.</returns>
        /// <response code="200">Returns the list of all vehicle by operator id</response>
        /// <response code="404">If no vehicles with matching operator id was found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("by/op/{operatorId}", Name = "GetVehiclesByOperatorId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVehiclesByOperatorId(int operatorId)
        {
            try
            {
                var vehicles = await _vehicleService.GetAllByOperatorId(operatorId);

                if (vehicles.IsNullOrEmpty())
                {
                    return NotFound($"Operator with id: {operatorId} does not exist.");
                }
               
                return Ok(vehicles);
                
              
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetVehiclesByOperatorId)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a vehicle with matcing id in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Vehicles/1
        ///     {
        ///          "id": 1,
        ///          "operatorName": "Operator 1",
        ///          "crNumber": "CR0001",
        ///          "plateNumber": "AAA1234",
        ///          "bodyType": "Sedan",
        ///          "make": "Toyota",
        ///          "rentFee": 500,
        ///          "rentStatus": true
        ///     }
        /// </remarks>
        /// <returns>A vehicle with matching id.</returns>
        /// <response code="200">Returns a vehicle with matching operator id</response>
        /// <response code="404">If no vehicle with matching id was found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}", Name = "GetVehicleById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            try
            {
                var Vehicle = await _vehicleService.GetById(id);

                if(Vehicle == null)
                {
                    return NotFound($"Vehicle with id: {id} does not exist.");
                }
                return Ok(Vehicle);
            }catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetVehicleById)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves all vehicles with matching plate number.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Vehicles/pn/AAA1234
        ///     {
        ///          "id": 1,
        ///          "operatorName": "Operator 1",
        ///          "crNumber": "CR0001",
        ///          "plateNumber": "AAA1234",
        ///          "bodyType": "Sedan",
        ///          "make": "Toyota",
        ///          "rentFee": 500,
        ///          "rentStatus": true
        ///     }
        /// </remarks>
        /// <returns>A vehicle with matching plate number.</returns>
        /// <response code="200">Returns a vehicle with matching plate number</response>
        /// <response code="404">If no vehicles with matching plate number was found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("pn/{plateNumber}", Name = "GetVehicleByPlateNumber")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVehicleByPlateNumber(string plateNumber)
        {
            try
            {
                var Vehicle = await _vehicleService.GetByPlateNumber(plateNumber);
                if (Vehicle == null)
                {
                    return NotFound($"Vehicle with plate number: {plateNumber} does not exist.");
                }
                return Ok(Vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetVehicleByPlateNumber)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Edits a vehicle's rent status.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Vehicles/1/status/edit
        ///     {
        ///         "id": 1,
        ///         "operatorName": "Operator 1"
        ///         "crNumber": "CR0001",
        ///         "plateNumber: "AAA1234",
        ///         "bodyType": "Sedan",
        ///         "make": "Toyota"
        ///         "rentFee": 500,
        ///         "rentStatus": true
        ///     }
        ///     
        /// 
        /// </remarks>
        /// <returns>A vehicle with the updated rentStatus.</returns>
        /// <response code="200">Returns a vehicle with updated rent status</response>
        /// <response code="404">If no vehicles with matching id was found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}/status/edit", Name = "EditRentStatus")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditRentStatus(int id, bool rentStatus)
        {
            try
            {
                if (await _vehicleService.GetById(id) == null)
                {
                    return NotFound($"Vehicle with id: {id} does not exist.");
                }

                var vehicle = await _vehicleService.UpdateRentStatus(id, rentStatus);

                return Ok(vehicle!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EditRentStatus)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Edits a vehicle's rent fee.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Vehicles/1/fee/edit
        ///     {
        ///         "id": 1,
        ///         "operatorName": "Operator 1"
        ///         "crNumber": "CR0001",
        ///         "plateNumber: "AAA1234",
        ///         "bodyType": "Sedan",
        ///         "make": "Toyota"
        ///         "rentFee": 1500,
        ///         "rentStatus": true
        ///     }
        ///     
        /// 
        /// </remarks>
        /// <returns>A vehicle with the updated rentFee.</returns>
        /// <response code="200">Returns a vehicle with updated rent fee</response>
        /// <response code="404">If no vehicles with matching id was found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}/fee/edit", Name = "EditRentFee")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditRentFee(int id, double rentFee)
        {
            try
            {
                if(await _vehicleService.GetById(id) == null)
                {
                    return NotFound($"Vehicle with id: {id} does not exist.");
                }

                var vehicle = await _vehicleService.UpdateRentFee(id, rentFee);

                return Ok(vehicle!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EditRentFee)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Delete a vehicle
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/Vehicles/{id}/remove
        ///     
        ///     String Output No Id found
        ///     Vehicle with id: {id} does not exist
        /// 
        ///     String Output Delete Success
        ///     Vehicle with id: {id} is successfully deleted.
        ///     
        /// </remarks>
        /// <returns>A vehicle with the updated rentStatus.</returns>
        /// <response code="200">Returns a message on whether or not the vehicle was deleted</response>
        /// <response code="404">If no vehicles with matching id was found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}/remove", Name = "RemoveVehicle")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveVehicle(int id)
        {
            try
            {
                var vehicle = await _vehicleService.DeleteVehicle(id);

                if (vehicle == false)
                {
                    return NotFound($"Vehicle with id: {id} does not exist.");
                }
                return Ok($"Vehicle with id: {id} is successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(RemoveVehicle)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
