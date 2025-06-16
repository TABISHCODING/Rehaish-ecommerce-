﻿using Microsoft.EntityFrameworkCore;
using MyntraClone.API.Data;
using MyntraClone.API.DTOs;
using MyntraClone.API.Models;

namespace MyntraClone.API.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _context;

        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AddressDto>> GetAddressesByUserIdAsync(int userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return addresses.Select(MapToDto).ToList();
        }

        public async Task<AddressDto?> GetAddressByIdAsync(int userId, int addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == addressId);

            return address == null ? null : MapToDto(address);
        }

        public async Task<AddressDto> CreateAddressAsync(int userId, CreateAddressDto dto)
        {
            // Check if user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            // Create new address
            var address = new Address
            {
                UserId = userId,
                Name = dto.Name,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                IsDefault = dto.IsDefault
            };

            // If this is the default address or the first address, handle default status
            if (dto.IsDefault || !await _context.Addresses.AnyAsync(a => a.UserId == userId))
            {
                // Reset any existing default addresses
                var defaultAddresses = await _context.Addresses
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .ToListAsync();

                foreach (var defaultAddress in defaultAddresses)
                {
                    defaultAddress.IsDefault = false;
                }

                // Set this address as default
                address.IsDefault = true;
            }

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return MapToDto(address);
        }

        public async Task<AddressDto?> UpdateAddressAsync(int userId, int addressId, UpdateAddressDto dto)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == addressId);

            if (address == null)
            {
                return null;
            }

            // Update address properties
            address.Name = dto.Name;
            address.AddressLine1 = dto.AddressLine1;
            address.AddressLine2 = dto.AddressLine2;
            address.City = dto.City;
            address.State = dto.State;
            address.PostalCode = dto.PostalCode;
            address.Country = dto.Country;
            address.Phone = dto.Phone;

            // Handle default address status
            if (dto.IsDefault && !address.IsDefault)
            {
                // Reset any existing default addresses
                var defaultAddresses = await _context.Addresses
                    .Where(a => a.UserId == userId && a.IsDefault && a.Id != addressId)
                    .ToListAsync();

                foreach (var defaultAddress in defaultAddresses)
                {
                    defaultAddress.IsDefault = false;
                }

                address.IsDefault = true;
            }
            else
            {
                address.IsDefault = dto.IsDefault;
            }

            await _context.SaveChangesAsync();
            return MapToDto(address);
        }

        public async Task<bool> DeleteAddressAsync(int userId, int addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == addressId);

            if (address == null)
            {
                return false;
            }

            _context.Addresses.Remove(address);

            // If the deleted address was the default, set another address as default if available
            if (address.IsDefault)
            {
                var nextAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.Id != addressId);

                if (nextAddress != null)
                {
                    nextAddress.IsDefault = true;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AddressDto?> SetDefaultAddressAsync(int userId, int addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == addressId);

            if (address == null)
            {
                return null;
            }

            // Reset any existing default addresses
            var defaultAddresses = await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault && a.Id != addressId)
                .ToListAsync();

            foreach (var defaultAddress in defaultAddresses)
            {
                defaultAddress.IsDefault = false;
            }

            // Set this address as default
            address.IsDefault = true;

            await _context.SaveChangesAsync();
            return MapToDto(address);
        }

        // Helper method to map Address entity to AddressDto
        private static AddressDto MapToDto(Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                UserId = address.UserId,
                Name = address.Name,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                Phone = address.Phone,
                IsDefault = address.IsDefault
            };
        }
    }
}
