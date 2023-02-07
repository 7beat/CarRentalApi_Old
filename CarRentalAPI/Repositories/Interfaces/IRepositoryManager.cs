namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IVehicleRepository Vehicles { get; }
        IRentalRepository Rentals { get; }
        IUserRepository Users { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
