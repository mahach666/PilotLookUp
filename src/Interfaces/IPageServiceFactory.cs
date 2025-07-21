using PilotLookUp.Contracts;

namespace PilotLookUp.Interfaces
{
    public interface IPageServiceFactory
    {
        IPageService CreatePageService(StartViewInfo startViewInfo);
    }
} 