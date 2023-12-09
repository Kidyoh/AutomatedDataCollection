namespace AutomatedDataCollectionApi.Services
{
    public interface IDataProcessService
    {
        List<string> GetConfigFileEndPoints();
        Task<string> AddConfigFileEndPoints(List<string> endpoints);
        Task<string> EditConfigFileEndPoints(List<string> endpoints);
        Task<string> DeleteConfigFileEndPoints(List<string> endpointsToDelete);
        Task<string> RunPythonScript();
    }
}

