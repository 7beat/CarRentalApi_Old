namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IVehicle2Repository Vehicle2 { get; }
        //IUserAuthenticationRepository UserAuthentication { get; }
        IVehicleRepository Vehicles { get; }
        IRentalRepository Rentals { get; }
        IUserRepository Users { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
