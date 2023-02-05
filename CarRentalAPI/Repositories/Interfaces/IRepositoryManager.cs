namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IVehicle2Repository Vehicle { get; }
        //IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
