﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyntraClone.API.DTOs;
using MyntraClone.API.Services;
using System.Security.Claims;

namespace MyntraClone.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/addresses")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // Helper method to get the current user ID from claims
        private int GetCurrentUserId()
        {
            var userIdStr = User.Identity?.Name;
            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out var id))
            {
                return id;
            }

            var nameIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(nameIdClaim) && int.TryParse(nameIdClaim, out id))
            {
                return id;
            }

            throw new UnauthorizedAccessException("User ID not found in claims");
        }

        // GET: api/users/{userId}/addresses
        [HttpGet]
        public async Task<IActionResult> GetAddresses(int userId)
        {
            try
            {
                // Security check: Users can only view their own addresses unless they are admins
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
                return Ok(addresses);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // GET: api/users/{userId}/addresses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int userId, int id)
        {
            try
            {
                // Security check: Users can only view their own addresses unless they are admins
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var address = await _addressService.GetAddressByIdAsync(userId, id);
                if (address == null)
                {
                    return NotFound($"Address with ID {id} not found for user {userId}");
                }

                return Ok(address);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // POST: api/users/{userId}/addresses
        [HttpPost]
        public async Task<IActionResult> CreateAddress(int userId, [FromBody] CreateAddressDto dto)
        {
            try
            {
                // Security check: Users can only create addresses for themselves unless they are admins
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                // Validate the model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var address = await _addressService.CreateAddressAsync(userId, dto);
                return CreatedAtAction(nameof(GetAddress), new { userId, id = address.Id }, address);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // PUT: api/users/{userId}/addresses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int userId, int id, [FromBody] UpdateAddressDto dto)
        {
            try
            {
                // Security check: Users can only update their own addresses unless they are admins
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                // Validate the model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var address = await _addressService.UpdateAddressAsync(userId, id, dto);
                if (address == null)
                {
                    return NotFound($"Address with ID {id} not found for user {userId}");
                }

                return Ok(address);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // DELETE: api/users/{userId}/addresses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int userId, int id)
        {
            try
            {
                // Security check: Users can only delete their own addresses unless they are admins
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var result = await _addressService.DeleteAddressAsync(userId, id);
                if (!result)
                {
                    return NotFound($"Address with ID {id} not found for user {userId}");
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // PATCH: api/users/{userId}/addresses/{id}/default
        [HttpPatch("{id}/default")]
        public async Task<IActionResult> SetDefaultAddress(int userId, int id)
        {
            try
            {
                // Security check: Users can only update their own addresses unless they are admins
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var address = await _addressService.SetDefaultAddressAsync(userId, id);
                if (address == null)
                {
                    return NotFound($"Address with ID {id} not found for user {userId}");
                }

                return Ok(address);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
