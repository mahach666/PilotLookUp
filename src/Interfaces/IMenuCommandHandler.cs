using Ascon.Pilot.SDK.Menu;

namespace PilotLookUp.Interfaces
{
    // Логика меню плагина: построение пунктов и обработка кликов.
    // App только делегирует сюда вызовы из MEF-интерфейсов
    public interface IMenuCommandHandler
    {
        void BuildMainMenu(IMenuBuilder builder);
        void BuildLookSelectedItem(IMenuBuilder builder);
        void ItemClick(string name);
    }
}
