﻿namespace CarRentalAPI.Models.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
