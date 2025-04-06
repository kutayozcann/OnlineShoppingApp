using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;

namespace OnlineShoppingApp.Business.Operations.Setting;

public class SettingManager : ISettingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<SettingEntity> _settingRepository;

    public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
    {
        _unitOfWork = unitOfWork;
        _settingRepository = settingRepository;
    }


    public async Task ToggleMaintenance()
    {
        var setting = _settingRepository.GetById(1);

        setting.MaintenanceMode = !setting.MaintenanceMode;

        _settingRepository.Update(setting);

        try
        {
            await  _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Error while toggling maintenance mode");
        }
    }

    public bool GetMaintenanceState()
    {
        var maintenanceState = _settingRepository.GetById(1).MaintenanceMode;
        
        return maintenanceState;
    }
}