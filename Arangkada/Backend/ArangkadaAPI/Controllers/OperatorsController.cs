using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ArangkadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorsController : ControllerBase
    {
        private readonly IOperatorService _service;
        private readonly ILogger<OperatorsController> _logger;

        public OperatorsController(IOperatorService service,
                              ILogger<OperatorsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new operator.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/operators/create
        ///     {
        ///         "fullName": "John Doe",
        ///         "username": "johndoe",
        ///         "password": "password123",
        ///         "email": "johndoe@example.com"
        ///     }
        /// 
        /// </remarks>
        /// <param name="operatorToCreate">The operator details to create.</param>
        /// <returns>The newly created operator.</returns>
        /// <response code="201">Returns the newly created operator.</response>
        /// <response code="400">If the operator is invalid or required properties are not fulfilled correctly.</response>
        /// <response code="409">If the operator already exists.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPost("register", Name = "RegisterOperator")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OperatorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterOperator([FromBody] OperatorCreationDto operatorToCreate)
        {
            try
            {
                if (await _service.GetByFullName(operatorToCreate.FullName) != null)
                {
                    return Conflict("Operator already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newOperator = await _service.CreateOperator(operatorToCreate);

                return CreatedAtRoute("GetOperatorById", new { newOperator!.Id }, newOperator);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(RegisterOperator)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves an operator by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/operators/by/id/{1}
        ///     {
        ///         "id":       "1",
        ///         "fullName": "John Doe",
        ///         "username": "johndoe",
        ///         "email": "johndoe@example.com"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the operator to retrieve.</param>
        /// <returns>The operator object, or null if the operator does not exist.</returns>
        /// <response code="200">Returns the operator object.</response>
        /// <response code="404">If the operator with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}", Name = "GetOperatorById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OperatorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOperatorById(int id)
        {
            try
            {
                var Operator = await _service.GetById(id);
                if (Operator == null)
                {
                    return NotFound($"Operator with id: {id} does not exist.");
                }
                return Ok(Operator);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetOperatorById)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves an operator by username.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/operators/by/un/{johndoe}
        ///     {
        ///         "id":       "1",
        ///         "fullName": "John Doe",
        ///         "username": "johndoe",
        ///         "email": "johndoe@example.com"
        ///     }
        /// </remarks>
        /// <param name="username">The username of the operator to retrieve.</param>
        /// <returns>The operator object, or null if the operator does not exist.</returns>
        /// <response code="200">Returns the operator object.</response>
        /// <response code="404">If the operator with the specified username does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("un/{username}", Name = "GetOperatorByUsername")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OperatorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOperatorByUsername(string username)
        {
            try
            {
                var Operator = await _service.GetByUsername(username);
                if (Operator == null)
                {
                    return NotFound($"Operator with name: {username} does not exist.");
                }
                return Ok(Operator);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetOperatorByUsername)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a password by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/operators/1/p
        ///     {
        ///         "password": "***********"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the password to retrieve.</param>
        /// <returns>The password with the specified ID.</returns>
        /// <response code="200">Returns the password with the specified ID.</response>
        /// <response code="404">If the password with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}/p", Name = "GetPasswordById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPasswordById(int id)
        {
            try
            {
                var pass = await _service.GetPasswordById(id);
                if (pass == null)
                {
                    return NotFound($"Operator with id: {id} does not exist.");
                }
                return Ok(pass);
            }
            catch(Exception ex) {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetPasswordById)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves an operator's verification status by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET/api/operators/1/v
        ///     {
        ///         "isVerified": "true"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>The operator's verification status with its ID.</returns>
        /// <response code="200">Returns the operator's verification status with its ID.</response>
        /// <response code="404">If the  operator's verification status with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}/v", Name = "GetIsVerifiedById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIsVerifiedById(int id)
        {
            try
            {
                var isVerified = await _service.GetIsVerifiedById(id);
                if (isVerified == null)
                {
                    return NotFound($"Operator with id: {id} does not exist.");
                }
                return Ok(isVerified);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetIsVerifiedById)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Updates an operator.
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///
        ///     PUT /api/operators/{1}/edit
        ///     {
        ///         "fullName": "Jack Doe",
        ///         "username": "jackjdoe",
        ///         "password": "janedoe123",
        ///         "email": "jackdoe@example.com"
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">The ID of the operator to update.</param>
        /// <param name="OperatorToUpdate">The operator details to update.</param>
        /// <returns>The updated operator.</returns>
        /// <response code="200">Returns the updated operator.</response>
        /// <response code="400">If the operator request body is invalid.</response>
        /// <response code="404">If the operator with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}/edit", Name = "EditOperator")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OperatorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditOperator(int id, [FromBody] OperatorUpdateDto OperatorToUpdate)
        {
            try
            {
                var Operator = await _service.GetById(id);
                if (Operator == null)
                {
                    return NotFound($"Operator with id: {id} does not exist.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedOperator = await _service.UpdateOperator(id, OperatorToUpdate);
                return Ok(updatedOperator!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EditOperator)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Updates the verification status of an operator.
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///
        ///     PUT /api/operators/{2}/verify
        ///     {
        ///         "isVerified": "true"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the operator to update.</param>
        /// <param name="valid">The bool value to update on the operator's verification status.</param>
        /// <returns>The updated operator.</returns>
        /// <response code="200">Returns the updated operator.</response>
        /// <response code="404">If the operator with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}/email/verify", Name = "UpdateVerification")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVerification(int id, bool valid)
        {
            try
            {

                var Operator = await _service.GetById(id);
                if (Operator == null)
                {
                    return NotFound($"Operator with id: {id} does not exist.");
                }

                var updatedVerification = await _service.UpdateIsVerified(id, valid);
                return Ok(updatedVerification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateVerification)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes an operator.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/Operators/{id}/delete
        ///     
        ///     String Output No Id found
        ///     Operator with id: {id} does not exist.
        ///     
        ///     String Output Delete Success
        ///     Operator with id: {id} is successfully deleted.
        /// </remarks>
        /// <param name="id">The ID of the operator to delete.</param>
        /// <returns>A message indicating whether the operator was successfully deleted.</returns>
        /// <response code="200">The operator was successfully deleted.</response>
        /// <response code="404">If the operator with the specified ID does not exist.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}/remove", Name = "DeleteOperator")]
        [ProducesResponseType(typeof(OperatorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOperator(int id)
        {
            try
            {
                var Operator = await _service.DeleteOperator(id);
                if (Operator == false)
                {
                    return NotFound($"Operator with id: {id} does not exist.");
                }
                return Ok($"Operator with id: {id} is successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteOperator)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
