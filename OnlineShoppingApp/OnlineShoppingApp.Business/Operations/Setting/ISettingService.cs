namespace OnlineShoppingApp.Business.Operations.Setting;

public interface ISettingService
{
    Task ToggleMaintenance();
    bool GetMaintenanceState();
}