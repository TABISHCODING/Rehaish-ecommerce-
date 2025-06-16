﻿using MyntraClone.API.DTOs;

namespace MyntraClone.API.Services
{
    public interface IAddressService
    {
        Task<List<AddressDto>> GetAddressesByUserIdAsync(int userId);
        Task<AddressDto?> GetAddressByIdAsync(int userId, int addressId);
        Task<AddressDto> CreateAddressAsync(int userId, CreateAddressDto dto);
        Task<AddressDto?> UpdateAddressAsync(int userId, int addressId, UpdateAddressDto dto);
        Task<bool> DeleteAddressAsync(int userId, int addressId);
        Task<AddressDto?> SetDefaultAddressAsync(int userId, int addressId);
    }
}
