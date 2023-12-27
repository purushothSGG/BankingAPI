using BankingBL.Models;
using BankingBL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = await _userService.CreateUserAsync(userDto);

                
                return Created(nameof(CreateUser),  userId );
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation failed for CreateUser request.");
                return BadRequest("Validation failed for CreateUser request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing CreateUser request.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        


    }
}
