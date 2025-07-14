using PilotLookUp.Interfaces;
using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PilotLookUp.Resources;

namespace PilotLookUp.Model.Services
{
    public class DataInitializationService : IDataInitializationService
    {
        private readonly IRepoService _repoService;
        private readonly IValidationService _validationService;
        private readonly IUserNotificationService _notificationService;

        public DataInitializationService(
            IRepoService repoService, 
            IValidationService validationService,
            IUserNotificationService notificationService)
        {
            _repoService = repoService;
            _validationService = validationService;
            _notificationService = notificationService;
        }

        public async Task<List<ListItemVM>> InitializeDataFromRepositoryAsync()
        {
            try
            {
                var repoData = _repoService.GetWrapedRepo();
                if (repoData == null || !repoData.Any())
                {
                    _notificationService.ShowInfo(Strings.RepositoryEmpty, Strings.InfoTitle);
                    return new List<ListItemVM>();
                }

                var listItems = repoData
                    .Where(x => x != null)
                    .Select(x => new ListItemVM(x, _validationService))
                    .Where(x => x != null)
                    .ToList();

                return await Task.FromResult(listItems);
            }
            catch (System.Exception ex)
            {
                _notificationService.ShowError(string.Format(Strings.ErrorTitle, ex.Message), Strings.ErrorTitle);
                return new List<ListItemVM>();
            }
        }

        public List<ListItemVM> InitializeDataFromRepository()
        {
            try
            {
                var repoData = _repoService.GetWrapedRepo();
                if (repoData == null || !repoData.Any())
                {
                    _notificationService.ShowInfo(PilotLookUp.Resources.Strings.RepositoryEmpty, PilotLookUp.Resources.Strings.InfoTitle);
                    return new List<ListItemVM>();
                }

                return repoData
                    .Where(x => x != null)
                    .Select(x => new ListItemVM(x, _validationService))
                    .Where(x => x != null)
                    .ToList();
            }
            catch (System.Exception ex)
            {
                _notificationService.ShowError(string.Format(PilotLookUp.Resources.Strings.ErrorLoadingRepository, ex.Message), PilotLookUp.Resources.Strings.ErrorTitle);
                return new List<ListItemVM>();
            }
        }

        public bool ValidateData(List<ListItemVM> data)
        {
            if (data == null)
                return false;

            var validItems = data.Where(x => x != null).ToList();
            if (validItems.Count != data.Count)
            {
                System.Diagnostics.Debug.WriteLine($"[DataInitializationService] Обнаружены null в данных: {data.Count - validItems.Count}");
            }

            return validItems.Any();
        }
    }
} 