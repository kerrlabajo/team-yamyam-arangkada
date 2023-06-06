using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace ArangkadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IDriverService _driverService;
        private readonly IVehicleService _vehicleService;

        public TransactionsController(ITransactionService transactionService,
            ILogger<TransactionsController> logger,
            IDriverService driverService,
            IVehicleService vehicleService)
        {
            _transactionService = transactionService;
            _logger = logger;
            _driverService = driverService;
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Records a new transaction to the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/transactions/record
        ///     {
        ///         "id": 11,
        ///         "operatorName": "Operator 1",
        ///         "driverName": "Daniel Williams",
        ///         "amount": 1500,
        ///         "date": "2023-05-10"
        ///     }
        ///
        /// </remarks>
        /// <param name="transactionToCreate">The transaction details to add.</param>
        /// <returns>The newly created transaction.</returns>
        /// <response code="201">Returns the newly created transaction.</response>
        /// <response code="400">If the transaction is invalid or required properties are not fulfilled correctly.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPost("record", Name = "RecordTransaction")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecordTransaction([FromBody] TransactionCreationDto transactionToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
               
                //Get driver using transactionToCreate's driver name variable
                var driver = await _driverService.GetByFullName(transactionToCreate.DriverName);

                if(driver == null)
                {
                    return BadRequest("No driver name in transactionToCreate.");
                }

                //Get a vehicle using the plate number of a driver's assigned vehicle
                var vehicle = await _vehicleService.GetByPlateNumber(driver.VehicleAssigned);

                if (vehicle == null)
                {
                    return NotFound($"Vehicle with plate number {driver.VehicleAssigned} could not be found.");
                }

                if (transactionToCreate.Amount < vehicle.RentFee)
                {
                    return BadRequest("Amount is less than or equal to rent fee.");
                }

                //Update the previously retrieved vehicle's rent status to false
                var updateVehicleRentStatus = await _vehicleService.UpdateRentStatus(vehicle!.Id, false);

                if (updateVehicleRentStatus == null)
                {
                    return NotFound("Failed to update rent status.");
                }

                var newTransaction = await _transactionService.CreateTransaction(transactionToCreate);

                return CreatedAtRoute("GetTransactionById", new { newTransaction!.Id }, newTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(RecordTransaction)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves all transactions belonging to a specific operator.
        /// </summary>
        /// <remarks>
        /// 
        ///     GET /api/Transactions/by/op/{2}
        ///      {
        ///         "id": 6,
        ///         "operatorName": "Operator 2",
        ///         "driverName": "Michael Johnson",
        ///         "amount": 1200,
        ///         "date": "2023-05-06"
        ///     },
        ///     {
        ///         "id": 7,
        ///         "operatorName": "Operator 2",
        ///         "driverName": "Emily Brown",
        ///         "amount": 1300,
        ///         "date": "2023-05-07"
        ///     },
        /// </remarks>
        /// <param name="operatorId">The ID of the operator.</param>
        /// <returns>List of all transactions belonging to the given operator Id in the system.</returns>
        /// <response code="404">If no transactions were found for the specified operator ID.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("operator/{operatorId}", Name = "GetTransactionsByOperatorId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionsByOperatorId(int operatorId)
        {
            try
            {
                var transactions = await _transactionService.GetAllByOperatorId(operatorId);

                if(transactions == null)
                {
                    return NotFound($"Transaction with operator id: {operatorId} does not exist.");
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTransactionsByOperatorId)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves transaction by ID.
        /// </summary>
        /// <remarks>
        /// 
        ///     GET /api/Transactions/by/op/{6}
        ///      {
        ///         "id": 6,
        ///         "operatorName": "Operator 2",
        ///         "driverName": "Michael Johnson",
        ///         "amount": 1200,
        ///         "date": "2023-05-06"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the transaction.</param>
        /// <returns>Transaction of the given Id in the system.</returns>
        /// <response code="404">If no transaction were found for the specified ID.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}", Name = "GetTransactionById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetById(id);
                if (transaction == null)
                {
                    return NotFound($"Transaction with id: {id} does not exist.");
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTransactionById)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Updates the details of a transaction.
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///
        ///     PUT /api/Transactions/{6}/edit
        ///     {
        ///         "id": 6,
        ///         "operatorName": "Operator 2",
        ///         "driverName": "Michael Johnson",
        ///         "amount": 2000,
        ///         "date": "2023-05-10"
        ///     }
        ///
        /// 
        /// </remarks>
        /// <param name="id">The ID of the transaction.</param>
        /// <param name="transactionToUpdate">Transaction details to update.</param>
        /// <returns> The newly updated details of the transaction.</returns>
        /// <response code="404">If no transaction were found for the specified ID.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}/edit", Name = "EditTransaction")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditTransaction(int id, [FromBody] TransactionUpdateDto transactionToUpdate)
        {
            try
            {

                var transaction = await _transactionService.GetById(id);
                if (transaction == null)
                {
                    return NotFound($"Transaction with id: {id} does not exist.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Get driver using transactionToCreate's driver name variable
                var driver = await _driverService.GetByFullName(transaction.DriverName);

                if (driver == null)
                {
                    return NotFound("No driver found from transactionToUpdate.");
                }

                //Get a vehicle using the plate number of a driver's assigned vehicle
                var vehicle = await _vehicleService.GetByPlateNumber(driver.VehicleAssigned);

                if (vehicle == null)
                {
                    return NotFound($"Vehicle with plate number {driver.VehicleAssigned} could not be found.");
                }

                if (transactionToUpdate.Amount < vehicle.RentFee)
                {
                    return BadRequest("Amount is less than the rent fee.");
                }

                var updatedTransaction = await _transactionService.UpdateTransaction(id, transactionToUpdate);
                
                return Ok(updatedTransaction!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EditTransaction)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes a transaction from the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/Transactions/{id}/delete
        ///     
        ///     String Output No Id found
        ///     Transaction with id: {id} does not exist.
        ///     
        ///     String Output Delete Success
        ///     Transaction with id: {id} is successfully deleted.
        ///     
        /// </remarks>
        /// <param name="id">The ID of the transaction to be deleted.</param>
        /// <returns> A system message indicating the success of the operation. </returns>
        /// <response code="404">If no transaction were found for the specified ID.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}/delete", Name = "DeleteTransaction")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var currentTransaction = await _transactionService.GetById(id);
                if (currentTransaction == null)
                {
                    return NotFound($"Transaction with id: {id} does not exist.");
                }

                //Get driver using transactionToCreate's driver name variable
                var driver = await _driverService.GetByFullName(currentTransaction.DriverName);

                if (driver == null)
                {
                    return BadRequest("No driver name in transactionToCreate.");
                }

                //Update an assigned vehicle to a driver using previously retrieved driver id
                var updateVehicleAssigned = await _driverService.UpdateVehicleAssigned(driver.Id, null);

                if (updateVehicleAssigned == null)
                {
                    return NotFound("An error occured in updating vehicle assigned.");
                }

                var isDeleted = await _transactionService.DeleteTransactionById(id);
                if (isDeleted == false)
                {
                    return NotFound($"Transaction with id: {id} does not exist.");
                }
                return Ok($"Transaction with id: {id} is successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteTransaction)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

    }
}
