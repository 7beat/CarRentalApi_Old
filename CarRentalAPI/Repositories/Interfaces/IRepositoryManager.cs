namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IVehicle2Repository Vehicle2 { get; }
        //IUserAuthenticationRepository UserAuthentication { get; }
        IVehicleRepository Vehicles { get; }
        Task SaveAsync();
    }
}
