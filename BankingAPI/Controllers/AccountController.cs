using BankingBL.Models;
using BankingBL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAccountServices _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountServices accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount(int userId, [FromBody] CreateAccountDto newAccount)
        {
            try
            {
                var accountId = await _accountService.CreateAccountAsync(userId, newAccount);

                return Created(nameof(CreateAccount),  accountId );
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Validation failed for CreateAccount request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing CreateUser request.");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(int userId, Deposit deposit)
        {
            try
            {
                var updatedBalance = await _accountService.DepositAsync(userId, deposit);
                return Ok(new { Balance = updatedBalance });
            }
            
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Validation failed for Deposit request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing Deposit request.");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(int userId, Withdraw withdraw)
        {
            try
            {
                var updatedBalance = await _accountService.WithdrawAsync(userId, withdraw);
                return Ok(new { Balance = updatedBalance });
            }
            
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Validation failed for withdraw request.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Validation failed for withdraw request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing withdraw request.");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount(int userId, Guid accountId)
        {
            try
            {
                var result = await _accountService.DeleteAccountAsync(userId, accountId);
                if (result)
                {
                    return Ok("Account deleted successfully.");
                }
                else
                {
                    return BadRequest("Unable to delete account.");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Validation failed for delete request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing delete account request.");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
