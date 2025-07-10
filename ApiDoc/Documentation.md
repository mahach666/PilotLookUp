# Client Pilot SDK

Описание клиентского SDK для создания модулей расширения к продуктам:
- Pilot-BIM
- Pilot-ICE Enterprise
- Pilot-ICE
- Pilot-ECM
- 3D-Storage


## Рекомендации

Для создания расширений рекомендуется использовать Visual Studio 2019 или выше.

### Создание и настройка проекта расширения

Система расширений **Pilot** основана на стандартном механизме расширений приложений **.NET — Managed Extensibility Framework (MEF)**. Чтобы создать новое расширение для системы Pilot выполните следующие действия:

- Создайте проект типа **ClassLibrary**.
- Вызовите диалог настройки проекта. В поле **AssemblyName** добавьте к названию сборки `.ext2` Например `WPFExtension.ext2`
- Установите целевую платформу (Target Framework) в **.NET Framework 4.7.2** или выше.
- Вызовите диалог редактирования информации о сборке. (Assembly Information...) Для этого нажмите на кнопку `Assembly Information...` и заполните необходимые поля.
    > Обязательно заполните поля **Title**, **Product** и **Assembly version**. Т.к. клиент при загрузке вашего расширения будет искать информацию именно в этих полях.
- Подключите к проекту ссылку на **.NET** сборку **System.ComponentModel**.Composition.

Для удобства отладки проекта расширения выполните следующие шаги:
- настройте сборку проекта в следующую папку `C:\Users\<current_user>\AppData\Local\ASCON\Pilot-ICE Enterprise\Development`
- на вкладке **Debug** выберите пункт **Start external program** и введите путь до установленного клиента. Например, путь для Pilot-ICE Enterprise `C:\Program Files\ASCON\Pilot-ICE Enterprise\Ascon.Pilot.PilotEnterprise.exe`
- сохраните изменения и запустите проект на исполнение.

> Папка **Development** по умолчанию отсутствует. Ее необходимо создать.

> При разработке множества разных расширений для удобства каждое расширение можно поместить в отдельную папку. Например, для расширения `WPFExtension` можно создать папку: `C:\Users\<current_user>\AppData\Local\ASCON\Pilot-ICE Enterprise\Development\WPFExtension\`.

Также вместо устарешего `Packages.config`, рекомендуется использовать `PackageReference` формат управления зависимостями nuget пакетов. Для этого перед созданием проекта, убедитесь, что в настройке Visual Studio `Tools>Options>NuGet Package Manager>General>Default package management format` выбрано значение `PackageReference`. 
Для миграции уже созданных проектов в контестном меню файла `Packages.config` выберете команду `Migrate from packages.config to PackageReference`.


### Подключение SDK к проекту расширения

Для подключения **SDK Pilot** к проекту расширения можно воспользоваться встроенным в **Visual Studio** механизмом распространения пакетов — **NuGet Packages Manager**. Для того, чтобы подключить **SDK** с помощью **Nuget Package Manager** выполните следующие шаги:
- Вызовите [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console);
- Выполните команду:
```cpp
Install-Package Ascon.Pilot.SDK
```

- Щёлкните правой кнопкой мыши на проекте и вызовите команду **Manage NuGet Packages**;
- Установите **NuGet пакет Ascon.Pilot.SDK** для вашего проекта.

Для того, чтобы подключить **SDK** вручную выполните следующие шаги:
- Скачайте примеры использования SDK из [центра загрузок Pilot](https://pilot.ascon.ru/);
- Распакуйте загруженный архив в любую папку.
- Настройте **NuGet Package Manager** на работу с папкой SDK из загруженного архива.
- Установите **NuGet пакет Ascon.Pilot.SDK** для вашего проекта.


## Описание интерфейсов и методов

#### Содержание
[1. Интерфейсы для встраивания в UI](#UIInterfaces)
  - [Новая вкладка INewTabPage](#INewTabPage)
  - [Панель инструментов IToolbar](#IToolbar)
    - [IToolbarBuilder](#IToolbarBuilder)
    - [IToolbarButtonItemBuilder](#IToolbarButtonItemBuilder)
    - [IToolbarToggleButtonItemBuilder](#IToolbarToggleButtonItemBuilder)
    - [IToolbarMenuButtonItemBuilder](#IToolbarMenuButtonItemBuilder)
    - [IToolbarItemSubmenuHandler](#IToolbarItemSubmenuHandler)
  - [Меню IMenu](#IMenu)
    - [IMenuBuilder](#IMenuBuilder)
    - [IMenuItemBuilder](#IMenuItemBuilder)
  - [Горячие клавиши IHotKey](#IHotKey)
    - [IHotKeyCollection](#IHotKeyCollection)
  - [Объекты контекста для построения меню и панели инструментов](#Context)
    - [TasksViewContext2](#TasksViewContext2)
    - [ObjectsViewContext](#ObjectsViewContext)
    - [MainViewContext](#MainViewContext)
    - [SystemTrayContext](#SystemTrayContext)
    - [StorageContext](#StorageContext)
	- [DocumentAnnotationsListContext](#DocumentAnnotationsListContext)
    - [XpsMergerContext](#XpsMergerContext)
    - [XpsRenderContext](#XpsRenderContext)
    - [XpsRenderClickPointContext](#XpsRenderClickPointContext)
    - [GraphicLayerElementContext](#GraphicLayerElementContext)
    - [DocumentFilesContext](#DocumentFilesContext)
    - [LinkedTasksContext2](#LinkedTasksContext2)
    - [LinkedObjectsContext](#LinkedObjectsContext)
	- [SignatureRequestsContext](#SignatureRequestsContext)
	- [AttachmentManagerContext](#AttachmentManagerContext)
  - [Набор вкладок ITabsExtension](#ITabsExtension)
    - [ITabsBuilder](#ITabsBuilder)
  - [Отображение информации для выбранного элемента в Обозревателе проектов](#IDocumentsExplorerDetailsViewProvider)
  - [Добавление новых опций очистки кэша в диалог Управление кэшем](#IClearCache)

[2. Интерфейсы для перехвата событий клиентского приложения](#HandlerInterfaces)
  - [Перехват автоимпорта документов IAutoimportHandler](#IAutoimportHandler)
  - [Перехват печати на принтер IPrintHandler](#IPrintHandler)
  - [Перехват уведомлений из диспетчера уведомлений INotificationsHandler](#INotificationsHandler)
  - [Перехват отображения атрибутов в карточке объекта IObjectCardHandler](#IObjectCardHandler)
    - [Редактирование атрибутов в карточке IAttributeModifier](#IAttributeModifier)
    - [Контекст карточки ObjectCardContext](#ObjectCardContext)
    - [Обработка изменений, сделанных пользователем IObjectChangeHandler](#IObjectChangeHandler)
    - [Редактирование и отмена изменений, сделанных пользователем IObjectChangeProcessor](#IObjectChangeProcessor)
  - [Перехват обработки файлов IFileHandler](#IFileHandler)

[3. Интерфейсы для скриптов автоматизации](#AutomationInterfaces)
  - [Автоматизация. Пользовательские действия IAutomationActivity](#IAutomationActivity)
  - [IAutomationEventContext](#IAutomationEventContext)
  - [IAutomationBackend](#IAutomationBackend)
  - [TriggerType](#TriggerType)
  - [TargetType](#TargetType)
  - [RelationFilterByChangeKind](#RelationFilterByChangeKind)
  - [ActivityExtensions](#ActivityExtensions)

[4. Интерфейсы управления данными](#DataManagementInterfaces)
  - [IObjectsRepository](#IObjectsRepository)
  - [IMessagesRepository](#IMessagesRepository)
  - [IMessagesBuilder](#IMessagesBuilder)
  - [ISignatureModifier](#ISignatureModifier)
  - [ISignatureBuilder](#ISignatureBuilder)
  - [IObjectModifier](#IObjectModifier)
    - [IObjectBuilder](#IObjectBuilder)
  - [IFileProvider](#IFileProvider)
  - [IPilotStorageListener](#IPilotStorageListener)
  - [IPilotStorageCommandController](#IPilotStorageCommandController)
  - [Список доступных команд](#PilotStorageCommand)
  - [IClientInfo](#IClientInfo)

[5. Интерфейсы работы с XPS](#XPSInterfaces)
  - [IXpsRender](#IXpsRender)
  - [IFileSaver](#IFileSaver)
  - [IXpsViewer](#IXpsViewer)
  - [IXpsMerger](#IXpsMerger)
  - [IXpsMergerSaveListener](#IXpsMergerSaveListener)
  - [IGraphicLayerElement](#IGraphicLayerElement)
  - [GraphicLayerElementConstants](#GraphicLayerElementConstants)

[6. Интерфейсы управления](#ProviderInterfaces)
  - [ITabServiceProvider](#ITabServiceProvider)
  - [ITabStateHandler](#ITabStateHandler)
  - [IPilotDialogService](#IPilotDialogService)
  - [IEventAggregator](#IEventAggregator)
  - [IConnectionStateProvider](#IConnectionStateProvider)
  - [IChatControlsProvider](#IChatControlsProvider)
  - [IObjectCardControlProvider ](#IObjectCardControlProvider )
  - [IPilotServiceProvider](#IPilotServiceProvider)
  - [IDocumentExplorer](#IDocumentExplorer)

[7. Интерфейсы для работы с конфигурацией](#AttributeInterfaces)
  - [Конфигурация атрибутов: форматирование, нумераторы, справочники](#AttributeInterfaces)
   - [Интерфейс IAttributeFormatParser](#IAttributeFormatParser)
   - [Интерфейс IReferenceBookConfiguration](#IReferenceBookConfiguration)
     - [Перечисление RefBookKind](#RefBookKind)
     - [Перечисление RefBookEditorType](#RefBookEditorType)
   - [Интерфейс IElementBookConfiguration](#IElementBookConfiguration)
     - [Интерфейс IElementBookDescription](#IElementBookDescription)
     - [Интерфейс IElementBookAutoFill](#IElementBookAutoFill)
     - [Перечисление AutoFillValueSource](#AutoFillValueSource)
   - [Интерфейс ITransitionManager](#ITransitionManager)
   - [Интерфейс IUserStateConfiguration](#IUserStateConfiguration)
   - [Интерфейс INumeratorInfo](#INumeratorInfo)
     - [Пример создания объекта с нумератором](#NumeratorExample)
  - [Конфигурация типов](#TypeConfigurations)
   - [Интерфейс ITypeConfigurationParser](#ITypeConfigurationParser)
   - [Интерфейс IRemarksConfiguration](#IRemarksConfiguration)
     - [Тип RemarkKind](#RemarkKind)
  - [Вспомогательные интерфейсы](#AttributeConfigurationTools)
    - [Интерфейс IElementBookAutoFillWorker](#IElementBookAutoFillWorker)

[8. Управление общими настройками](#CommonSettings)
  - [Интерфейс ISettingsFeature](#ISettingsFeature)
  - [Интерфейс IPersonalSettings](#IPersonalSettings)
  - [Интерфейс ISettingValueProvider](#ISettingValueProvider)
  - [Класс SystemSettingsKeys](#SystemSettingsKeys)

[9. Поиск](#SearchInterfaces)
  - [Интерфейс ISearchService](#ISearchService#ISearchService)
  - [Интерфейс ISmartFolderQueryBuilder](#ISmartFolderQueryBuilder)
  - [Интерфейс IAnnotationQueryBuilder](#IAnnotationQueryBuilder)
  - [Пример создания умной папки](#SmartFolderCreationExample)
  - [Интерфейс IQueryBuilder](#IQueryBuilder)
  - [Интерфейс ISearchResult](#ISearchResult)
  - [Предустановленные условия поиска](#MainSearchTerms)
    - [Условия поиска для объектов ObjectFields](#ObjectFields)
    - [Условия поиска по произвольным атрибутам AttributeFields](#AttributeFields)
    - [Общие условия поиска](#AllTerms)
  - [Пример создания поискового запроса](#ObjectSearchExample)

[10. Команды расширений (устаревший, использовать IPilotServiceProvider)](#Commands)
  - [ICommandHandler](#ICommandHandler)
  - [ICommandInvoker](#ICommandInvoker)
  - [CommandDescription](#CommandDescription)

[11. Типы данных Pilot](#DataTypesInterfaces)
  - [Объекты](#ObjectInterfaces)
    - [IDataObject](#IDataObject)
    - [IType](#IType)
    - [IAttribute](#IAttribute)
    - [IFile](#IFile)
    - [ISignatureRequest](#ISignatureRequest)
    - [IOrganisationUnit](#IOrganisationUnit)
    - [IPerson](#IPerson)
    - [IPosition](#IPosition)
    - [IAccess](#IAccess)
    - [IAccessRecord](#IAccessRecord)
    - [IStorageDataObject](#IStorageDataObject)
    - [IStateInfo](#IStateInfo)
    - [IRelation](#IRelation)
    - [IFilesSnapshot](#IFilesSnapshot)
    - [ILicenseInfo](#ILicenseInfo)
    - [IUserState](#IUserState)
    - [IUserStateMachine](#IUserStateMachine)
    - [IReportItem](#IReportItem)
    - [IMouseLeftClickListener](#IMouseLeftClickListener)
	- [IHistoryItem](#IHistoryItem)
	- [ICertificate](#ICertificate)
- [Перечисления](#Enums)
    - [DataState](#DataState)
    - [SynchronizationState](#SynchronizationState)
    - [TypeKind](#TypeKind)
    - [AccessLevel](#AccessLevel)
    - [StorageObjectState](#StorageObjectState)
    - [ObjectRelationType](#ObjectRelationType)
    - [OrganisationUnitKind](#OrganisationUnitKind)
    - [ThemeNames](#ThemeNames)
    - [PilotBalloonIcon](#PilotBalloonIcon)
	- [MessageType](#MessageType)
	- [ChatKind](#ChatKind)
	- [CadesType](#CadesType)
	- [PdfStamperMode](#PdfStamperMode)
  - [События](#Events)
    - [OfflineEventArgs](#OfflineEventArgs)
    - [OnlineEventArgs](#OnlineEventArgs)
    - [CloseTabEventArgs](#CloseTabEventArgs)
    - [OpenTabEventArgs](#OpenTabEventArgs)
    - [ActiveTabChangedEventArgs](#ActiveTabChangedEventArgs)
    - [TabUndockedEventArgs](#TabUndockedEventArgs)
    - [TabDockedEventArgs](#TabDockedEventArgs)
    - [LoadedEventArgs](#LoadedEventArgs)
    - [UnloadedEventArgs](#UnloadedEventArgs)
  - [Константы](#SystemConstants)
    - [SystemTypeNames](#SystemTypeNames)
    - [SystemAttributeNames](#SystemAttributeNames)
    - [SystemStates](#SystemStates)
    - [SystemObjectIds](#SystemObjectIds)
    - [SystemFileNames](#SystemFileNames)
  - [Сообщения](№Сообщения)
	- [IChatMessage](#IChatMessage)
	- [ITextMessage](#ITextMessage)
	- [IChat](#IChat)
	- [ITextMessageData](#ITextMessageData)
    - [IChatRelation](#IChatRelation)
    - [IChatMember](#IChatMember)

[12. Интерфейсы для работы с шаблонами](#TemplateInterfaces)
  - [Интерфейс ITaskTemplateParser](#ITaskTemplateParser)
	- [Интерфейс ITaskTemplateItem](#ITaskTemplateItem)

[13. Интерфейсы для реализации подписания документов сторонними криптоалгоритмами](#IDocumentCryptoInterfaces)
  - [Интерфейс IDocumentCryptoProvider](#IDocumentCryptoProvider)
  - [Интерфейс ICryptoProvider](#ICryptoProvider)
    - [Интерфейс IReadSignatureListener](#IReadSignatureListener)
	- [Интерфейс IImportedSignatureListener](#IImportedSignatureListener)
  - [ICertificateSelector](#ICertificateSelector)
  - [IDigitalSigner](#IDigitalSigner)

[14. Интерфейсы работы с PDF](#PDFInterfaces)
  - [IPdfStamper](#IPdfStamper)
  - [PdfDocumentContext](#PdfDocumentContext)
  - [StampPositionArgs](#StampPositionArgs)

<a name="UIInterfaces"/>

## 1. Интерфейсы для встраивания в UI 


<a name="INewTabPage"/>

### Интерфейс **INewTabPage**

Для того, чтобы встроить расширение в новую вкладку главного окна клиента Pilot необходимо реализовать интерфейс из пакета **Ascon.Pilot.SDK** `INewTabPage`. Обязательно пометить класс, реализующий интерфейс `INewTabPage`, атрибутом `[Export]`

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;
namespace WpfExtension
{
    [Export(typeof(INewTabPage))]
    public class MainModule : INewTabPage
    {
        public void BuildNewTabPage(INewTabPageHost host)
        {
        }

        public void OnButtonClick(string name)
        {
        }
    }
}
```

#### `INewTabPage.BuildNewTabPage`

Метод вызывается перед тем, как будет построена вкладка **Новая вкладка**.

```cpp
void INewTabPage.BuildNewTabPage(INewTabPageHost host);
```
где:
 - `host` - интерфейс, позволяющий добавлять кнопки.

Интерфейс содержит методы для добавления кнопок и групп на главную страницу Pilot: Метод `SetGroup` создаёт или обновляет группу кнопок с указанным заголовком
```cpp
void INewTabPageHost.SetGroup(string title, Guid groupId)
```
где:
 - `title` локализованный заголовок группы
 - `groupId` идентификатор группы для использования в методе `AddButtonToGroup`

Метод `AddButtonToGroup` служит для добавления кнопки в ранее созданную группу
```cpp
void INewTabPageHost.AddButtonToGroup(string title, string name, string toolTip, byte[] svgIcon, Guid groupId);
```
где:
 - `title` - локализованное название кнопки;
 - `name` - уникальное имя кнопки в пределах расширения;
 - `toolTip` - локализованная подсказка, расположенная под названием кнопки;
 - `svgIcon` - массив байт SVG иконки. Клиент поддерживает иконки только в svg формате.
 - `groupId` - идентификатор группы кнопок.

Метод `AddButton` используется для добавления кнопки в группу по умолчанию, имеющую название "Модули расширения". Обеспечивает обратную совместимость.
```cpp
void INewTabPageHost.AddButton(string title, string name, string toolTip, byte[] svgIcon);
```
где:
 - `title` - локализованное название кнопки;
 - `name` - уникальное имя кнопки в пределах расширения;
 - `toolTip` - локализованная подсказка, расположенная под названием кнопки;
 - `svgIcon` - массив байт SVG иконки. Клиент поддерживает иконки только в svg формате.


#### `NewTabPage.OnButtonClick`

Метод вызывается при нажатии на кнопку, созданную расширением в методе `BuildNewTabPage`.

```cpp
void INewTabPage.OnButtonClick(string name);
```
где:
 - `name` - уникальное имя кнопки, по которой произошло нажатие.




<a name="IToolbar"/>

### Интерфейс `IToolbar<TToolbarContext>`

Интерфейс позволяет встраивать новые команды в панель инструментов. Для этого необходимо реализовать интерфейс из пакета **Ascon.Pilot.SDK** `IToolbar<TToolbarContext>` и обязательно пометить класс, реализующий интерфейс `IToolbar< TToolbarContext>`, атрибутом `[Export]`.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ContextMenuSample
{
    [Export(typeof(IToolbar<TasksViewContext2>))]
    public class ToolbarSample : IToolbar<TasksViewContext2>
    {
        public void Build(IToolbarBuilder builder, TasksViewContext2 context)
        {
         	//...
        }

        public void OnToolbarItemClick(string name, TasksViewContext2 context)
        {
         	//...
        }
    }
}
```

Чтобы встроить новые команды в панель инструментов вкладки **Задания**, необходимо реализовать интерфейc `IToolbar<TasksViewContext2>`.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ContextMenuSample
{
    [Export(typeof(IToolbar<TasksViewContext2>))]
    public class ToolbarSample : IToolbar<TasksViewContext2>
    {
        ...
    }
}
```

Чтобы встроить новые команды в панель инструментов *Обозревателя документов*, необходимо реализовать интерфейc `IToolbar<ObjectsViewContext>`

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ContextMenuSample
{
    [Export(typeof(IToolbar<ObjectsViewContext>))]
    public class ToolbarSample : IToolbar<ObjectsViewContext>
    {
        ...
    }
}
```


#### `IToolbar<TToolbarContext>.Build`

Метод вызывается перед построением панели инструментов.

```cpp
void IToolbar<TToolbarContext>.Build(IToolbarBuilder builder, TToolbarContext context);
```
где:
 - `builder` - интерфейс, позволяющий добавлять различные элементы в панель инструментов;
 - `context` - контекст, позволяющий получить дополнительные сведения о селектированных элементах.


#### `IToolbar<TToolbarContext>.OnToolbarItemClick`

Метод вызывается при нажатии на элемент панели инструментов.

```cpp
void IToolbar<TToolbarContext>.OnToolbarItemClick(string name, TToolbarContext context);
```
где:
 - `name` - уникальное внутреннее имя элемента панели инструментов;
 - `context` - контекст выполнения комманды.


<a name="IToolbarBuilder"/>

### Интерфейс `IToolbarBuilder`

Интерфейс позволяет управлять элементами панели инструментов.

#### `IToolbarBuilder.ItemNames`
Свойство возвращает список уникальных имен элементов панели инструментов.
```cpp
IEnumerable<string> IToolbarBuilder.ItemNames { get; }
```

#### `IToolbarBuilder.Count`

Свойство возвращает количество элементов панели инструментов.
```cpp
int IToolbarBuilder.Count { get; }
```

#### `IToolbarBuilder.AddSeparator`

Метод позволяет добавить разделитель в панель инструментов.
```cpp
void IToolbarBuilder.AddSeparator(int index);
```
где:
 - `index` - индекс, куда вставить разделитель.


#### `IToolbarBuilder.AddButtonItem`

Метод позволяет добавить кнопку в панель инструментов.
```cpp
IToolbarButtonItemBuilder AddButtonItem(string name, int index);
```
где:
 - `name` - уникальное внутреннее имя кнопки;
 - `index` - индекс, куда вставить кнопку.

Возвращает интерфейс добавления свойств кнопки `IToolbarButtonItemBuilder`.


#### `IToolbarBuilder.AddMenuButtonItem` 

Метод позволяет добавить кнопку с выпадающим меню в панель инструментов.
```cpp
IToolbarMenuButtonItemBuilder AddMenuButtonItem(string name, int index);
```
где:
 - `name` - уникальное внутреннее имя кнопки;
 - `index` - индекс, куда вставить кнопку.

Возвращает интерфейс добавления свойств кнопки `IToolbarMenuButtonItemBuilder`.


#### `IToolbarBuilder.AddToggleButtonItem` 

Метод позволяет добавить кнопку-переключатель в панель инструментов.
```cpp
IToolbarToggleButtonItemBuilder AddToggleButtonItem(string name, int index);
```
где:
 - `name` - уникальное внутреннее имя кнопки;
 - `index` - индекс, куда вставить кнопку.

Возвращает интерфейс добавления свойств кнопки `IToolbarToggleButtonItemBuilder`.



#### `IToolbarBuilder.ReplaceButtonItem` 

Метод позволяет заменить любой элемент панели инструментов на кнопку.
```cpp
IToolbarButtonItemBuilder ReplaceButtonItem(string name);
```
где:
 - `name` - уникальное внутреннее имя элемента, который будет заменен.

Возвращает интерфейс добавления свойств кнопки `IToolbarButtonItemBuilder`.



#### `IToolbarBuilder.ReplaceMenuButtonItem` 

Метод позволяет заменить любой элемент панели инструментов на кнопку с выпадающим меню.
```cpp
IToolbarMenuButtonItemBuilder ReplaceMenuButtonItem(string name);
```
где:
 - `name` - уникальное внутреннее имя элемента, который будет заменен.

Возвращает интерфейс добавления свойств кнопки `IToolbarMenuButtonItemBuilder`.



#### `IToolbarBuilder.HandleMenuButtonItemSubmenu` 

Метод позволяет добавить обработчик построения подменю. Прямой доступ к элементам подменю невозможно получить в произвольный момент времени, так как подменю строится "лениво" при его открытии.
```cpp
void HandleMenuButtonItemSubmenu(string name, IToolbarItemSubmenuHandler itemSubmenuHandler);
```
где:
 - `name` - имя элемента меню, в который будет встраиваться обработчик;
 - `itemSubmenuHandler` - обработчик построения подменю (реализуется на стороне модуля расширения).



#### `IToolbarBuilder.ReplaceToggleButtonItem` 

Метод позволяет заменить любой элемент панели инструментов на кнопку-переключатель.
```cpp
IToolbarToggleButtonItemBuilder ReplaceToggleButtonItem(string name);
```
где:
 - `name` - уникальное внутреннее имя элемента, который будет заменен.

Возвращает интерфейс добавления свойств кнопки `IToolbarToggleButtonItemBuilder`.

#### `IToolbarBuilder.RemoveItem`
Метод позволяет удалить элемент панели инструментов.
```cpp
void RemoveItem(string name);
```
где:
 - `name` - уникальное внутреннее имя.

<a name="IToolbarButtonItemBuilder"/>

### Интерфейс `IToolbarButtonItemBuilder` 

Интерфейс позволяет добавить свойства к элементу панели инструменотов.


#### `IToolbarButtonItemBuilder.WithHeader` 

Метод позволяет добавить отображаемое в UI название.
```cpp
IToolbarButtonItemBuilder WithHeader(string header);
```
где:
 - `header` - отображаемое в UI название элемента.


#### `IToolbarButtonItemBuilder.WithIcon` 

Метод позволяет добавить иконку элементу панели инструментов. Обратите внимание, иконка должна быть в формате `SVG`
```cpp
IToolbarButtonItemBuilder WithIcon(byte[] svgIcon);
```
где:
 - `svgIcon` - иконка в формате SVG.


#### `IToolbarButtonItemBuilder.WithIsEnabled` 

Метод позволяет задать значение доступности элемента.
```cpp
IToolbarButtonItemBuilder WithIsEnabled(bool value);
```


#### `IToolbarButtonItemBuilder.WithShowHeader` 

Метод позваляет спрятать или показать заголовок элемента. Чтобы показать элемент только в виде иконки необходимо в метод `WithShowHeader` передать параметр `false`
```cpp
IToolbarButtonItemBuilder WithShowHeader(bool value);
```


#### `IToolbarButtonItemBuilder.WithHint` 

Метод позволяет добавить подсказку к элементу панели инструментов.
```cpp
IToolbarButtonItemBuilder WithHint(string hint);
```
где:
 - `hint` - текст подсказки.


<a name="IToolbarToggleButtonItemBuilder"/>

### Интерфейс `IToolbarToggleButtonItemBuilder` 

Интерфейс позволяет добавить свойства к кнопке-переключателю панели инструменотов. Этот интерфейс является наследником `IToolbarButtonItemBuilder`.

#### `IToolbarToggleButtonItemBuilder.WithIsChecked` 

Метод позволяет задать состояние кнопки-переключателя.
```cpp
IToolbarToggleButtonItemBuilder WithIsChecked(bool value);
```


<a name="IToolbarMenuButtonItemBuilder"/>

### Интерфейс `IToolbarMenuButtonItemBuilder` 

Интерфейс позволяет добавить свойства к кнопке с выпадающим меню панели инструменотов. Этот интерфейс является наследником `IToolbarButtonItemBuilder`.

#### `IToolbarMenuButtonItemBuilder.WithMenu` 

Метод позволяет добавить контекстное меню к кнопке.
```cpp
IToolbarMenuButtonItemBuilder WithMenu(IToolbarItemSubmenuHandler itemSubmenuHandler);
```
где:
 - `itemSubmenuHandler` - класс, описывающий построение выпадающего меню.


<a name="IToolbarItemSubmenuHandler"/>

### Интерфейс `IToolbarItemSubmenuHandler` 

Интерфейс построения меню для кнопки панели инструментов. В пакет `SDK` уже поставляется класс, реализующий данный интерфейс - `ToolbarItemSubmenuHandler`. Для того, чтобы построить меню, необходимо унаследоваться от этого класса.

#### `IToolbarItemSubmenuHandler.OnSubmenuRequested` 

Метод вызывается перед тем как появится меню у кнопки панели инструментов.
```cpp
void OnSubmenuRequested(IToolbarBuilder builder);
```
где:
 - `builder` - интерфейс построения меню.


Пример построения меню для кнопки в панели инструментов:
```cpp
[Export(typeof(IToolbar<TasksViewContext2>))]
public class ToolbarSample : IToolbar<TasksViewContext2>
{
	public void Build(IToolbarBuilder builder, TasksViewContext2 context)
    {
    	builder.AddMenuButtonItem("tbsMenuButton", 0)
                .WithMenu(new MenuHandler())
                .WithHeader("Menu button")
                .WithHint("Menu button");
    }

	public void OnToolbarItemClick(string name, TasksViewContext2 context)
    {
    	if (name == "MenuName")
        {
        	//do somethig
        }
    }
}

public class MenuHandler : ToolbarItemSubmenuHandler
{
    public override void OnSubmenuRequested(IToolbarBuilder builder)
    {
        builder.AddButtonItem("MenuName", 0).WithHeader("item");
    }
}

```

Подробнее можно посмотреть в примере `Ascon.Pilot.SDK.ToolbarSample`, поставляемом вместе с пакетом `SDK`.



<a name="IMenu"/>

### Интерфейс `IMenu<TMenuContext>` 

Интерфейс позволяет встраивать новые команды в различные меню приложения в зависимости от контекста. Для этого необходимо реализовать интерфейс из пакета **Ascon.Pilot.SDK** `IMenu<TMenuContext>` и обязательно пометить класс, реализующий интерфейс **IMenu< TMenuContext >**, атрибутом `[Export]`.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ContextMenuSample
{
    [Export(typeof(IMenu<TasksViewContext2>))]
    public class ToolbarSample : IMenu<TasksViewContext2>
    {
        public void Build(IMenuBuilder builder, TasksViewContext2 context)
        {
         	//...
        }

        public void OnMenuItemClick(string name, TasksViewContext2 context)
        {
         	//...
        }
    }
}
```

Чтобы встроить новые команды в контекстное меню вкладки **Задания**, необходимо реализовать интерфейc `IMenu<TasksViewContext2>`.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ContextMenuSample
{
    [Export(typeof(IMenu<TasksViewContext2>))]
    public class TasksContextMenuSample : IMenu<TasksViewContext2>
    {
        ...
    }
}
```

Чтобы встроить новые команды в контекстное меню **Обозревателя документов**, необходимо реализовать интерфейc `IMenu<ObjectsViewContext>`

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ContextMenuSample
{
    [Export(typeof(IMenu<ObjectsViewContext>))]
    public class ObjectsContextMenuSample : IMenu<ObjectsViewContext>
    {
        ...
    }
}
```


#### `IMenu<TMenuContext>.Build`

Метод вызывается перед построением контекстного меню.

```cpp
void IMenu<TMenuContext>.Build(IMenuBuilder builder, TMenuContext context);
```
где:
 - `builder` - интерфейс, позволяющий добавлять различные элементы в меню;
 - `context` - контекст, позволяющий получить дополнительные сведения о селектированных элементах.


#### `IMenu<TMenuContext>.OnMenuItemClick`

Метод вызывается при нажатии на элемент меню.

```cpp
void IMenu<TMenuContext>.OnMenuItemClick(string name, TMenuContext context);
```
где:
 - `name` - уникальное внутреннее имя элемента меню;
 - `context` - контекст выполнения комманды.


<a name="IMenuBuilder"/>

### Интерфейс `IMenuBuilder` 

Интерфейс позволяет управлять элементами меню.

#### `IMenuBuilder.ItemNames` 

Свойство возвращает список уникальных имен элементов меню.
```cpp
IEnumerable<string> IMenuBuilder.ItemNames { get; }
```

#### `IMenuBuilder.Count` 

Свойство возвращает количество элементов меню.
```cpp
int IMenuBuilder.Count { get; }
```

#### `IMenuBuilder.AddSeparator` 

Метод позволяет добавить разделитель в меню.
```cpp
void IMenuBuilder.AddSeparator(int index);
```
где:
 - `index` - индекс, куда вставить разделитель.


#### `IMenuBuilder.AddItem` 

Метод позволяет добавить пункт меню.
```cpp
IMenuItemBuilder AddItem(string name, int index);
```
где:
 - `name` - уникальное внутреннее имя пункта меню;
 - `index` - индекс, куда вставить пункт меню.

Возвращает интерфейс добавления свойств пункта меню `IMenuItemBuilder`.

#### `ImenuBuilder.AddCheckableItem`
Метод позволяет добавить пункт меню типа кнопка-переключатель.
```cpp
ICheckableMenuItemBuilder AddCheckableItem(string name, int index);
```
где:
 - `name` - уникальное внутреннее имя пункта меню;
 - `index` - индекс, куда вставить пункт меню.

Возвращает интерфейс установки состояния кнопки-переключателя.

#### `IMenuBuilder.ReplaceItem` 

Метод позволяет заменить любой пункт меню.
```cpp
IMenuItemBuilder ReplaceItem(string name);
```
где:
 - `name` - уникальное внутреннее имя;

#### `IMenuBuilder.RemoveItem`
Метод позволяет удалить пункт меню.
```cpp
void RemoveItem(string name);
```
где:
 - `name` - уникальное внутреннее имя.

#### `IMenuBuilder.GetItem` 

Метод позволяет получить построитель пункта меню.
```cpp
IMenuBuilder GetItem(string name);
```
где:
 - `name` - уникальное внутреннее имя;

Возвращает интерфейс добавления пунктов меню `IMenuBuilder`.

<a name="IMenuItemBuilder"/>

### Интерфейс `IMenuItemBuilder` 

Интерфейс позволяет добавлять свойства пункту меню.

#### `IMenuItemBuilder.WithHeader` 

Метод позволяет добавить отображаемое в UI название.
```cpp
IMenuItemBuilder WithHeader(string header);
```
где:
 - `header` - отображаемое в UI название пункта меню.


#### `IMenuItemBuilder.WithIcon` 

Метод позволяет добавить иконку. Обратите внимание, иконка должна быть в формате `SVG` для всех меню кроме `IMenu<StorageContext>`. Иконки для меню Pilot-Storage дожны быть в формате `PNG`.

```cpp
IMenuItemBuilder WithIcon(byte[] icon);
```
где:
 - `icon` - иконка.


#### `IMenuItemBuilder.WithIsEnabled` 

Метод позволяет задать значение доступности элемента.
```cpp
IMenuItemBuilder WithIsEnabled(bool value);
```

#### `IMenuItemBuilder.WithSubmenu` 

Метод позволяет добавить подменю к текущему пункту меню.
```cpp
IMenuBuilder WithSubmenu();
```

<a name="IHotKey"/>

### Интерфейс `IHotKey<THotKeyContext>` 

Интерфейс позволяет назначать горячие клавиши частей приложения в зависимости от контекста. Для этого необходимо реализовать интерфейс из пакета **Ascon.Pilot.SDK** `IHotKey<THotKeyContext>` и обязательно пометить класс, реализующий интерфейс `IHotKey<THotKeyContext>` атрибутом `[Export]`. Чтобы назначить горячие клавиши, которые будут активны в **Обозревателя документов**, необходимо реализовать интерфейc `IHotKey<ObjectsViewContext>`. На данный момент это единственный тип контекста, который поддерживает добавление горячих клавиш.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ExtensionHotKeySample
{
    [Export(typeof(IHotKey<ObjectsViewContext>))]
    public class HotKeySample : IHotKey<ObjectsViewContext>
    {
        public void AssignHotKeys(IHotKeyCollection hotKeyCollection)
        {
         	//...
        }

        public void OnHotKeyPressed(string commandId, ObjectsViewContext context)
        {
         	//...
        }
    }
}
```

#### `IHotKey<THotKeyContext>.AssignHotKeys` 

Метод `AssignHotKeys` вызывается при загрузке интерфейса, который соответсвует указанному контексту. Например, метод расширения для контекста `ObjectsViewContext` будет вызван при открытии вкладки с Обозревателем документов.

```cpp
void IHotKey<THotKeyContext>.AssignHotKeys(IHotKeyCollection hotKeyCollection);
```
где:
 - `hotKeyCollection` - интерфейс коллекции горячих клавиш, который непосредственно добавляет горячие клавиши в интерфейс приложения;

#### `IHotKey<THotKeyContext>.OnHotKeyPressed`

Метод вызывается, когда была нажата одна из комбинаций горячих клавиш, добавленных с помощью расширения.

```cpp
void IHotKey<THotKeyContext>.OnHotKeyPressed(string commandId, ObjectsViewContext context);
```
где:
 - `commandId` - уникальный идентификатор команды, который должен быть задан при добавлении горячей клавиши в методе `AssignHotKeys`;
 - `context` - контекст, в котором горячая клавиша была нажата.
 
 
<a name="IHotKeyCollection"/>

### Интерфейс `IHotKeyCollection` 

Интерфейс коллекции для добавления горячих клавиш.

### `IHotKeyCollection.Add` 

Метод позволяет добавить разделитель новую горячую клавишу. Если для данного контектса уже была задана такая же горячая клавиша, добавление новой перезапишет ранее добавленную команду. Таким образом, можно переопределить поведение стандартных горячих клавиш в используемом контексте.
```cpp
void IMenuBuilder.Add(Key key, ModifierKeys modifierKeys, string commandId;
```
где:
 - `key` - горячая клавиша;
 - `modifierKeys` - модификатор корячей клавиши, позволяющий задать сочетания горячих клавиш с **Ctrl**, **Alt** и **Shift**;
 - `commandId` - уникальный идентификатор команды, который используется в методе `OnHotKeyPressed` для определения того, какую команду нежно выполнить; 

<a name="Context"/>

### Объекты контекста для построения меню и панели инструментов


<a name="TasksViewContext2"/>

#### TasksViewContext2

Чтобы встроить расширение в контекстное меню или панель инструментов вкладки **Задания**, используйте этот тип контекста:
```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<TasksViewContext2>))]
    public class ContextMenuSample : IMenu<TasksViewContext2>
    {
        ...
    }
}
```

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IToolbar<TasksViewContext2>))]
    public class ToolbarSample : IToolbar<TasksViewContext2>
    {
        ...
    }
}
```

<a name="ObjectsViewContext"/>

#### ObjectsViewContext
Чтобы встроить расширение в контекстное меню или панель инструментов **Обозревателя проектов**, используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<ObjectsViewContext>))]
    public class ContextMenuSample : IMenu<ObjectsViewContext>
    {
        ...
    }
}
```

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IToolbar<ObjectsViewContext>))]
    public class ToolbarSample : IToolbar<ObjectsViewContext>
    {
        ...
    }
}
```


<a name="MainViewContext"/>

#### MainViewContext
Чтобы встроить расширение в главное меню приложения используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<MainViewContext>))]
    public class MenuSample : IMenu<MainViewContext>
    {
        ...
    }
}
```

<a name="SystemTrayContext"/>

#### SystemTrayContext
Чтобы встроить расширение в системное меню, используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<SystemTrayContext>))]
    public class MenuSample : IMenu<SystemTrayContext>
    {
        ...
    }
}
```

<a name="StorageContext"/>

#### StorageContext
Чтобы встроить расширение в контекстное меню **Pilot-Storage**, используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<StorageContext>))]
    public class MenuSample : IMenu<StorageContext>
    {
        ...
    }
}
```

<a name="DocumentAnnotationsListContext"/>

#### DocumentAnnotationsListContext
Используйте этот тип контекста, чтобы встроить расширение в панель инструментов и контекстное меню списка замечаний к документу:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IToolbar<DocumentAnnotationsListContext>))]
    public class DocumentAnnotationsListToolbarSample : IToolbar<DocumentAnnotationsListContext>
    {
        ...
    }
	
	[Export(typeof(IMenu<DocumentAnnotationsListContext>))]
    public class DocumentAnnotationsListContextMenuSample : IMenu<DocumentAnnotationsListContext>
    {
        ...
    }
}
```

<a name="XpsMergerContext"/>

#### XpsMergerContext
Чтобы встроить расширение в панель инструментов **компоновщика XPS**, используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IToolbar<XpsMergerContext>))]
    public class XpsMergerToolbarSample : IToolbar<XpsMergerContext>
    {
        ...
    }
}
```

<a name="XpsRenderContext"/>

#### XpsRenderContext
`XpsRenderContext` содержит информацию об открытом в просмотрщике документе. Чтобы встроить расширение в панель инструментов просмотрщика документов, используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IToolbar<XpsRenderContext>))]
    public class XpsRenderToolbarSample : IToolbar<XpsRenderContext>
    {
        ...
    }
}
```

<a name="XpsRenderClickPointContext"/>

#### XpsRenderClickPointContext
`XpsRenderClickPointContext` содержит информацию об открытом в просмотрщике документе и о точке вызова контестного меню, а именно о координатах и номере страницы. Чтобы встроить расширение в контестное меню просмотрщика документов, используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<XpsRenderClickPointContext>))]
    public class XpsRenderContextMenuSample : IMenu<XpsRenderClickPointContext>
    {
        ...
    }
}
```

<a name="GraphicLayerElementContext"/>

#### GraphicLayerElementContext
Графический слой позволяет встраивать через SDK растровые изображения и блоки XAML разметки в XPS документ (см. Ascon.Pilot.SDK.GraphicLayerSample). Добавленные элементы графического слоя можно перемещать по документу и вызывать контекстное меню с командами, добавленными из плагина. Графический слой не является частью XPS документа и доступен для просмотра только в    Pilot. Элементы графического слоя, помеченные как `IsFloating = false`, вшиваются в XPS документ во время печати или при создании запроса на подпись. Чтобы получить XPS с вшитым графическим слоем, необходимо отправить документ на печать в виртуальный принтер **Pilot XPS** или создать запросы на подпись. После этого элементы графического слоя будет невозможно перемещать по документу, вызывать контекстное меню и менять содержимое через плагин. Чтобы встроить расширение в контекстное меню графического элемента используйте этот тип контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<GraphicLayerElementContext>))]
    public class GraphicLayerContextMenuSample : IMenu<GraphicLayerElementContext>
    {
        ...
    }
}
```

<a name="DocumentFilesContext"/>

#### DocumentFilesContext
Чтобы встроить расширение в контекстное меню панели файлов ECM, документа используйте данный вид контекста. Описание свойств контекста:
 - `IEnumerable<IDataObject> EcmDocument { get; }` - ECM документ, для которого была показана панель файлов
 - `IEnumerable<IStorageDataObject> SelectedObjects { get; }` - список выбранных файлов
 - `bool IsContext { get; }` - флаг, показывающий, в каком месте был сделан вызов меню
 - `bool IsMounted { get; }` - флаг, определяющий, был ли смонтирован на **Pilot-Storage** ECM документ в момент вызова меню
 - `DocumentFilesUsage Usage { get; }` - определяет, в каком UI используется панель файлов

  ##### Перечисление `DocumentFilesUsage
Перечисление UI использующих панель файлов

  ##### DocumentFilesUsage.CreateNew
UI создания нового ECM документа

  ##### DocumentFilesUsage.ObjectsExplorer
UI обозревателя документов

  ##### DocumentFilesUsage.CardView
UI карточки документа

  ##### DocumentFilesUsage.TemplatesManager
UI менеджера шаблонов документов

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<DocumentFilesContext>))]
    public class MenuSample : IMenu<DocumentFilesContext>
    {
        ...
    }
}
```


<a name="LinkedTasksContext2"/>

#### LinkedTasksContext2

Чтобы встроить расширение в контекстное меню связанных заданий, используйте данный вид контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<LinkedTasksContext2>))]
    public class MenuSample : IMenu<LinkedTasksContext2>
    {
        ...
    }
}
```

<a name="SignatureRequestsContext"/>

#### SignatureRequestsContext
Чтобы встроить расширение в контекстное меню панели запросов на подпись документа, используйте данный вид контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<SignatureRequestsContext>))]
    public class MenuSample : IMenu<SignatureRequestsContext>
    {
        ...
    }
}
```

<a name="AttachmentManagerContext"/>

#### AttachmentManagerContext
Чтобы встроить расширение в контекстное меню менеджера вложений (например, вложений в задание), используйте данный вид контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<AttachmentManagerContext>))]
    public class MenuSample : IMenu<AttachmentManagerContext>
    {
        ...
    }
}
```

<a name="LinkedObjectsContext"/>

#### LinkedObjectsContext
Чтобы встроить расширение в контекстное меню вкладки **Связи**, используйте данный вид контекста:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IMenu<LinkedObjectsContext>))]
    public class MenuSample : IMenu<LinkedObjectsContext>
    {
        ...
    }
}
```

<a name="ITabsExtension"/>

### Интерфейс `ITabsExtension<TTabsContext>`

Интерфейс для добавления новых вкладок в различные места приложения, использующие коллекции вкладок для отображения определенного контекста. Для создания расширения, добавляющего новые вкладки в коллекцию, необходимо реализовать в расширении интерфейс **Ascon.Pilot.SDK** `ITabsExtension<TTabsContext>` и обязательно пометить класс, реализующий интерфейс `ITabsExtension<TTabsContext>`, атрибутом `[Export]`.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace DocumentExplorerTabsSample
{
    [Export(typeof(ITabsExtension<DocumentExplorerDetailsTabsContext>))]
    public class DocumentExplorerTabsSample : ITabsExtension<DocumentExplorerDetailsTabsContext>
    {
        public void BuildTabs(ITabsBuilder builder, DocumentExplorerDetailsTabsContext context)
        {
         	//Put new tabs building logic here...
        }

        public void OnIsActiveChanged(Guid tabId, bool isActive)
        {
         	//Put tabs activation and deactivation logic here...
        }
        
        public void OnDisposed(Guid tabId, bool isActive)
        {
         	//Put disposing and clearing tabs logic here to free resources when associated tabs collection is no longer displayed...
        }
    }
}
```

Чтобы добавить новые вкладки в коллекцию вкладок связанных элементов **Обозревателя документов**, необходимо реализовать в расширении интерфейс `ITabsExtension<DocumentExplorerDetailsTabsContext>`.

```cpp
namespace DocumentExplorerTabsSample
{
    [Export(typeof(ITabsExtension<DocumentExplorerDetailsTabsContext>))]
    public class DocumentExplorerTabsSample : ITabsExtension<DocumentExplorerDetailsTabsContext>
    {
        ...
    }
}
```

#### `ITabsExtension<TTabsContext>.BuildTabs`

Метод **BuildTabs** вызывается при построении коллекции вкладок и позволяет добавить в коллекцию новые вкладки.

```cpp
void ITabsExtension<TTabsContext>.BuildTabs(ITabsBuilder builder, TTabsContext context);
```
где:
 - `builder` - интерфейс построителя вкладок, который непосредственно позволяет добавить новые вкладки;
 - `context` - контекст коллекции вкладок, который позволяет получить информацию о том, для каких элементов строится коллекция вкладок.

#### `ITabsExtension<TTabsContext>.OnIsActiveChanged`

Метод вызывается при изменении активности вкладки, которая была добавлена с помощью расширения.

```cpp
void ITabsExtension<TTabsContext>.OnIsActiveChanged(Guid tabId, bool isActive);
```
где:
 - `tabId` - уникальный идентификатор вкладки;
 - `isActive` - флаг, указывающий новое состояние вкладки.

#### `ITabsExtension<TTabsContext>.OnDisposed` 

Метод вызывается при очистке коллекции вкладок, в которую были добавлены вкладки расширений. Например, перед перестроением вкладок при смене их контекста или при закрытии пользователем окна, содержащего коллекцию вкладок. Метод позволяет отменить длительные операции или освободить ресурсы, принадлежащие вкладкам расширения, которые больше не будут использоваться.

```cpp
void ITabsExtension<TTabsContext>.OnDisposed(Guid tabId);
```
где:
 - `tabId` - уникальный идентификатор вкладки;

<a name="ITabsBuilder"/>

### Интерфейс `ITabsBuilder`

Интерфейс построителя вкладок расширения, передается в качестве аргумента метода **BuildTabs** и позволяет добавить новые вкладки в коллекцию.

#### `ITabsBuilder.AddTab`

Метод, добавляющий новую вкладку. Обратите внимание, что в коллекцию вкладок может быть добавлено более одной вкладки расширения. Для того, чтобы корректно обрабатывать события изменения активности вкладки и освобождения её ресурсов, при добавлении новой вкладки расширение должно назначить ей уникальный идентификатор, и впоследствии отличать эту вкладку с помощью этого идентификатора. Для примера работы с идентификаторами вкладок обратитесь к примеру **Ascon.Pilot.SDK.TabsExtensionSample**.

```cpp
void ITabsExtension<TTabsContext>.AddTab(Guid id, string title, FrameworkElement view);
```

где:
 - `tabId` - уникальный идентификатор вкладки, который должен быть сгенерирован расширением и впоследствии использован для идентификации вкладок при вызове методов **OnIsActiveChanged** и **OnDisposed**; 
 - `title` - заголовок вкладки;
 - `view` - контент, который будет отображаться в качестве содержимого вкладки.

<a name="DocumentExplorerDetailsTabsContext"/>
#### DocumentExplorerDetailsTabsContext
Класс, описывающий контекст вкладок области связей **Обозревателя документов**. Расширение должно использовать этот тип контекста, чтобы добавить свои вкладки эту область (в дополнение к стандартным вкладкам ЗАДАНИЯ, СВЗЯИ и ЧАТ):
```cpp
public class DocumentExplorerTabsExtension : ITabsExtension<DocumentExplorerDetailsTabsContext>
{
...
}
```

#### `DocumentExplorerDetailsTabsContext.SelectedObject`
Свойство, которое возвращает выбранный в данный момент элемент в **Обозревателе документов**, для которого строится коллекция вкладок.
```cpp
public IDataObject SelectedObject { get; }
```

#### `DocumentExplorerDetailsTabsContext.SnapshotDateTime`
Свойство, которое возвращает время создания выбранной версии документа, выбранного в **Обозревателе документов**. Возвращает null, если выбрана актуальная версия документа.
```cpp
public DateTime? SnapshotDateTime { get; }
```

<a name="IDocumentsExplorerDetailsViewProvider"/>

## Отображения информации для выбранного элемента в Обозревателе проектов

Для того, чтобы заменить стандартную карточку элемента в **Обозревателе проектов** предусмотрен интерфейс `IDocumentsExplorerDetailsViewProvide`

### Интерфейс IDocumentsExplorerDetailsViewProvider

Интерфейс позволяет заменить карточку элемента в **Обозревателе проектов** на свою собственную. Для этого необходимо реализовать интерфейс из пакета Ascon.Pilot.SDK `IDocumentsExplorerDetailsViewProvider` и обязательно пометить класс, реализующий интерфейс **IDocumentsExplorerDetailsViewProvider** атрибутом `[Export]`

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ContextMenuSample
{
    [Export(typeof(IDocumentsExplorerDetailsViewProvider))]
    public class DetailsViewProvider : IDocumentsExplorerDetailsViewProvider
    {
        ...
    }
}
```

### Методы

```cs
FrameworkElement GetDetailsView(ObjectsViewContext context);
```
Метод должен возвращать UI елемент, который будет отображаться у выбранного элемента в **Обозревателе проектов**.

```cs
IType Type { get; }
```
Свойство должно возвращать тип элемента, для которого будет показываться UI елемент.

#### Пример использования:
```cs
[Export(typeof(IDocumentsExplorerDetailsViewProvider))]
public class DetailsViewProvider : IDocumentsExplorerDetailsViewProvider
{
    [ImportingConstructor]
    public DetailsViewProvider(IObjectsRepository repository)
    {
        Type = repository.GetType("section");
    }

    public FrameworkElement GetDetailsView(ObjectsViewContext context)
    {
        var obj = context.SelectedObjects.FirstOrDefault();
        if (obj == null)
            return null;
        var viewModel = new TypeDetailsViewModel(obj.Type);
        var view = new TypeDetailsView
        {
            DataContext = viewModel
        };
        return view;
    }

    public IType Type { get; }
}
```

<a name="IClearCache"/>

## Добавление новых опций очистки кэша в диалог Управление кэшем

Для того, чтобы добавить новую опцию очистки кэша в диалог Управление кэшем предусмотрен интерфейс `IClearCache`

### Интерфейс IClearCache

Интерфейс позволяет добавить новую опцию очистки кэша в диалог **Управление кэшем**. Для этого необходимо реализовать интерфейс из пакета Ascon.Pilot.SDK `IClearCache` и обязательно пометить класс, реализующий интерфейс **IClearCache** атрибутом `[Export]`

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ClearCacheSample
{
    [Export(typeof(IClearCache))]
    public class ClearCacheProcedure : IClearCache
    {
        ...
    }
}
```

### Свойства

```cs
string Title { get; }
```
Свойство должно возвращать название процедуры очистки кэша для модуля расширения, которое будет отображаться в диалоге **Управление кэшем**.

### Методы

```cs
void GetInfo(ClearCacheInfo clearCacheInfo);
```
Метод должен давать информацию о количестве и общем размере файлов кэша, которые могут быть удалены в данный момент. Для этого в данном методе должны задаваться соответсвующие значения свойств объекта класса `ClearCacheInfo`, передаваемого как аргумент метода.

```cs
void ExecuteClearCache();
```
Метод должен выполнять процедуру очистки кэша для модуля расширения. Следует учитывать возможность отмены этой процедуры с помощью метода `StopClearCache`. Данный метод работает на специальном рабочем потоке (worker thread) и не блокирует GUI.

```cs
void StopClearCache();
```
Метод должен отменять выполнение метода `ExecuteClearCache`. Данный метод работает на UI потоке (UI thread).

### События

```cs
event EventHandler<ClearCacheProgressChangedEventArgs> ClearCacheProgressChanged;
```
Событие должно оповещать об изменении прогресса при выполнеии `ExecuteClearCache`.


<a name="HandlerInterfaces"/>

## 2. Интерфейсы для перехвата событий клиентского приложения 


<a name="IAutoimportHandler"/>

### Интерфейс IAutoimportHandler

Интерфейс позволяет реализовать обработку автоимпорта документов (печать на Pilot XPS принтер, загрузка документов в папку автоимпорта). Для этого необходимо реализовать интерфейс из пакета Ascon.Pilot.SDK `IAutoimportHandler` и обязательно пометить класс, реализующий интерфейс **IAutoimportHandler** атрибутом `[Export]`

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace AutoimportHandlingSample
{
    [Export(typeof(IAutoimportHandler))]
    public class AutoimportHandlingSample : IAutoimportHandler
    {
        public bool Handle(string filePath, string sourceFilePath, AutoimportSource autoimportSource)
        {
        }
    }
}
```



<a name="IPrintHandler"/>

### Интерфейс IPrintHandler

Интерфейс позволяет реализовать обработку печати документов с помощью принтера. Для этого необходимо реализовать интерфейс из пакета Ascon.Pilot.SDK `IPrintHandler` и обязательно пометить класс, реализующий интерфейс **IPrintHandler** атрибутом `[Export]`
В слуачае перхвата печати из Компоновщика XPS, **IPrintedDocumentInfo.DocumentId** равно **Guid.Empty**, а в **IPrintedDocumentInfo.PrinterSettings.PrintJobName** вместо имени, для скомпонованного документа, записывается строка формата "Наименование документа#номер страницы|". Например: MultipageXps.xps#2|1 - План на отметке 0,000 - Рабочий чертеж#1

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace PrintHandlingSample
{
    [Export(typeof(IPrintHandler))]
    public class PrintHandlingSample : IPrintHandler
    {
        public bool Handle(IPrintedDocumentInfo printTicket)
        {
        }
    }
}
```




<a name="INotificationsHandler"/>

### Интерфейс INotificationsHandler

Интерфейс позволяет реализовать обработку уведомлений из диспетчера уведомлений. Для этого необходимо реализовать интерфейс из пакета `Ascon.Pilot.SDK.INotificationsHandler` и обязательно пометить класс, реализующий интерфейс **INotificationsHandler**, атрибутом `[Export]`.

Метод `Handle` вызывается перед каждым показом нового уведомления. Если метод вернет `true`, то уведомление не будет показано в стандартном диспетчере уведомлений.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace NotificationsHandlerSample
{
    [Export(typeof(INotificationsHandler))]
    public class NotificationsHandler : INotificationsHandler
    {
        public bool Handle(INotification notification)
        {
        }
    }
}
```

<a name="INotification"/>

### Интерфейс INotification

Интефейс INotification описывает уведомление и содержит следующие члены:

##### INotification.ObjectId

Идентификатор элемента, по которому пришло уведомление.

```cpp
Guid ObjectId { get; }
```

##### INotification.Title

Отображаемое имя элемента, по которому пришло уведомление.

```cpp
string Title { get; }
```

##### INotification.Message

Текст уведомления

```cpp
string Message { get; }
```

##### INotification.UserId

Идентификатор пользователя, изменение которого привело к уведомлению. Значение null означает, что уведомление было сгенерировано серверной логикой без команды пользователя.

```cpp
int? UserId { get; }
```

##### INotification.DateTime

Содержит дату и время создания уведомления в формате UTC.

```cpp
DateTime DateTime { get; }
```

##### INotification.TypeId

Идентификатор типа элемента, по которому пришло уведомление.

```cpp
int TypeId { get; }
```

<a name="INotificationChangesetId"/>

##### INotification.ChangesetId

Метод расширения, возвращающий инкрементальный идентификатор набора изменений, приведшего к созданию уведомления. Получение уведомления по элементу не гарантирует того, что обновленное состояние элемента уже было получено клиентом. Для того, чтобы получить обновленный элемент, подпишитесь на изменения по элементу и дождитесь значения IDataObject.LastChange, большего или равного ChangesetId уведомления. Пример:

```cpp
[Export(typeof(IDataPlugin))]
public class Main : IDataPlugin, IObserver<INotification>
{
	private readonly IObjectsRepository _repository;

	[ImportingConstructor]
	public Main(IObjectsRepository repository)
	{
		_repository = repository;
		repository.SubscribeNotification(NotificationKind.StorageObjectRenamed).Subscribe(this);
	}

	public async void OnNext(INotification value)
	{
		var loader = new ObjectLoader(_repository);
		var obj = await loader.Load(value.ObjectId, value.ChangesetId());
		var message = string.Format("Storage object renamed to {0}", obj.DisplayName);
		MessageBox.Show(message, "Ascon.Pilot.SDK.NotificationsSample");
	}

	public void OnError(Exception error)
	{
		
	}

	public void OnCompleted()
	{
		
	}
}

public class ObjectLoader : IObserver<IDataObject>
{
	private readonly IObjectsRepository _repository;
	private IDisposable _subscription;
	private TaskCompletionSource<IDataObject> _tcs;
	private long _changesetId;

	public ObjectLoader(IObjectsRepository repository)
	{
		_repository = repository;
	}

	public Task<IDataObject> Load(Guid id, long changesetId = 0)
	{
		_changesetId = changesetId;
		_tcs = new TaskCompletionSource<IDataObject>();
		_subscription = _repository.SubscribeObjects(new[] {id}).Subscribe(this);
		return _tcs.Task;
	}

	public void OnNext(IDataObject value)
	{
		if (value.State != DataState.Loaded) 
			return;

		if(value.LastChange() < _changesetId)
			return;

		_tcs.TrySetResult(value);
		_subscription.Dispose();
	}

	public void OnError(Exception error) { }
	public void OnCompleted() { }
}
```



<a name="IObjectCardHandler"/>

### Интерфейс IObjectCardHandler

Интерфейс позволяет реализовать работу с атрибутами, отображаемыми в карточке объекта. Для этого необходимо реализовать интерфейс из пакета `Ascon.Pilot.SDK.IObjectCardHandler` и обязательно пометить класс, реализующий интерфейс **IObjectCardHandler**, атрибутом `[Export]`.

Метод `Handle` вызывается при каждом построении карточки объекта: при показе диалогов создания и редактирования объекта, при выборе объектов, для которых отображается карточка, в Обозревателе документов, а так же в диалоге автоимпорта при выборе нового родителя для создаваемого документа или селектировании документа для замены файла. В случае, если карточка находится в режиме редактирования существующего объекта, свойство `EditingObject` аргумента `context` возвращает сам объект, атрибуты которого показаны в карточке; и `null`, если карточка показана для создания нового объекта. Возвращаемое значение типа boolean никак не используется в реализации метода на стороне приложения Pilot и может быть любым.

Метод `OnValueChanged` вызывается при изменении пользователем значения какого-либо из отображаемых атрибутов в карточке объекта. Аргумент `sender` возвращает атрибут, значение которого было изменено. Аргумент `args` позволяет получить доступ к аргументам события: предыдущему и новому значению атрибута, а также контексту карточки объекта. `modifier` позволяет отреагировать на изменения и установить новое значение дополнительно одному или нескольким атрибутам. Изменения в значениях атрибутов, которые были сделаны с помощью `modifier`, не приводят к вызовам метода `OnValueChanged`. Возвращаемое значение типа boolean никак не используется в реализации метода на стороне приложения Pilot и может быть любым.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ObjectCardHandlerSample
{
    [Export(typeof(IObjectCardHandler))]
    public class ObjectCardHandler : IObjectCardHandler
    {
        public bool Handle(IAttributeModifier modifier, ObjectCardContext context)
        {
        	var isObjectModification = context.EditiedObject != null;
        }
        
        public bool OnValueChanged(IAttribute sender, AttributeValueChangedEventArgs args, IAttributeModifier modifier)
        {
        }
    }
}
```



<a name="IAttributeModifier"/>

#### Редактирование атрибутов в карточке IAttributeModifier

Интерфейс позволяет установить новое значения для атрибутов элемента перед тем, как они будут показаны в карточке объекта.

##### IAttributeModifier.SetValue

Метод позволяет задать новое значение атрибуту.

```cpp
void IAttributeModifier.SetValue(string name, string value);
```
где:
 - `name` - уникальное наименование атрибута типа.
 - `value` - новое значение атрибута.

Метод имеет несколько перегрузок:
```cpp
void IAttributeModifier.SetValue(string name, string value);
void IAttributeModifier.SetValue(string name, long value);
void IAttributeModifier.SetValue(string name, double value);
void IAttributeModifier.SetValue(string name, DateTime value);
void IAttributeModifier.SetValue(string name, decimal value);
void IAttributeModifier.SetValue(string name, Guid value);
void IAttributeModifier.SetValue(string name, int[] value);
void IAttributeModifier.SetValue(string name, Guid[] value);
void IAttributeModifier.SetValue(string name, string[] value);
```


<a name="ObjectCardContext"/>

#### Контекст карточки ObjectCardContext

Контекст отображения карточки.

##### ObjectCardContext.CurrentObjectId

Поле содержит идетификатор объекта, для которого показана карточка. Если карточка показана для создания нового объекта, то поле содержит идентификатор, который будет присвоен новому объекту при создании.

```cpp
Guid ObjectCardContext.CurrentObjectId { get; }
```

##### ObjectCardContext.DisplayAttributes

Поле содержит список отображаемых в карточке атрибутов. В этот список не входят сервисные атрибуты.

```cpp
IEnumerable<IAttribute> ObjectCardContext.DisplayAttributes { get; }
```

##### ObjectCardContext.AttributeValues

Словарь текущих значений атрибутов карточки объекта, где ключ это имя атрибута, а значение - это значение атрибута.

```cpp
IDictionary<string, object> ObjectCardContext.AttributeValues { get; }
```

##### ObjectCardContext.Type

Поле содержит описание типа элемента, для которого показана карточка.

```cpp
IType ObjectCardContext.Type { get; }
```

##### ObjectCardContext.EditiedObject

Текущий редактируемый объект, для которого показана карточка. Null, если карточка показана для создания нового объекта.

```cpp
IDataObject ObjectCardContext.EditiedObject { get; }
```

##### ObjectCardContext.Parent

Поле содержит описание родительского элемента.

```cpp
IDataObject ObjectCardContext.Parent { get; }
```

##### ObjectCardContext.IsReadOnly

True, если карточка в режиме только для чтения, и атрибуты недоступны для изменения.

```cpp
bool ObjectCardContext.IsReadOnly { get; }
```




<a name="IObjectChangeHandler"/>

#### IObjectChangeHandler
Интерфейс `IObjectChangeHandler` позволяет подписаться на изменения, сделанные пользователем.

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace Sample
{
    [Export(typeof(IObjectChangeHandler))]
    public class AnnulHandler : IObjectChangeHandler
    {
        private readonly string _annuledUserStateId;

        [ImportingConstructor]
        public AnnulHandler(IObjectsRepository repository)
        {
            var userStates = repository.GetUserStates();
            var annuledUserState = userStates.FirstOrDefault(x => x.Name == Const.AnnuledUserState);
            if (annuledUserState != null)
                _annuledUserStateId = annuledUserState.Id.ToString();
        }

        public void OnChanged(IEnumerable<DataObjectChange> changes)
        {
            if(string.IsNullOrEmpty(_annuledUserStateId))
                return;

            foreach (var change in changes)
            {
                foreach (var attribute in change.New.Attributes)
                {
                    if (Equals(attribute.Value, _annuledUserStateId))
                    {
                        object oldValue = null;
                        change.Old?.Attributes.TryGetValue(attribute.Key, out oldValue);

                        if (!Equals(oldValue, _annuledUserStateId))
                            OnAnnuled(change.New);
                    }
                }
            }
        }

        private void OnAnnuled(IDataObject obj)
        {
            ...
        }
    }
}
```

#### `IObjectChangeHandler.OnChanged`

Данный метод вызывается после того, как пользователь делает какое-либо изменение. В метод `OnChanged` параметром передается список произошедших изменений.

<a name="DataObjectChange"/>

#### Класс DataObjectChange
Представляет собой изменение над одним объектом

  ##### DataObjectChange.Old
Состояние объекта до изменения. Равно `null` в случае создания объекта.

  ##### DataObjectChange.New
Состояние объекта после изменения.


<a name="IObjectChangeProcessor"/>

#### IObjectChangeProcessor
Интерфейс `IObjectChangeProcessor` позволяет редактировать и отменять изменения, сделанные пользователем. Примеры использования данного интерфейса можно найти в проекте **SubtreeUserStateAnuller**.

```cpp
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Ascon.Pilot.SDK;

namespace SubtreeUserStateAnuller
{
    [Export(typeof(IObjectChangeProcessor))]
    public class ChangeProcessor : IObjectChangeProcessor
    {
        public bool ProcessChanges(IEnumerable<DataObjectChange> changes, IObjectModifier modifier)
        {
            //проверяем первое изменение
            var change = changes.First();

            //получаем pdf документы из изменения
            var newPdfs = change.New.ActualFileSnapshot.Files.Select(f => f.Name).Where(f => f.EndsWith("pdf"));
            var oldPdfs = change.Old.ActualFileSnapshot.Files.Select(f => f.Name).Where(f => f.EndsWith("pdf"));

            //если для объекта приложен pdf файл - отменяем изменения, иначе применяем
            if (newPdfs.Except(oldPdfs).Any())
                return false;

            return true;
        }
    }
}
```

```cpp
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using Ascon.Pilot.SDK;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace SubtreeUserStateAnuller
{
    [Export(typeof(IObjectChangeProcessor))]
    public class AnnulHandler : IObjectChangeProcessor
    {
        public const string Xaml = "<TextBlock Foreground=\"Red\" FontSize=\"20\">АННУЛИРОВАНО</TextBlock>";

        private readonly string _annuledUserStateId;

        [ImportingConstructor]
        public AnnulHandler(IObjectsRepository repository)
        {
            var userStates = repository.GetUserStates();
            var annuledUserState = userStates.FirstOrDefault(x => x.Name == Const.AnnuledUserState);
            if (annuledUserState != null)
                _annuledUserStateId = annuledUserState.Id.ToString();
        }

        public bool ProcessChanges(IEnumerable<DataObjectChange> changes, IObjectModifier modifier)
        {
            if (string.IsNullOrEmpty(_annuledUserStateId))
                return true;

            foreach (var change in changes)
            {
                foreach (var attribute in change.New.Attributes)
                {
                    if (Equals(attribute.Value.ToString(), _annuledUserStateId))
                    {
                        object oldValue = null;
                        change.Old?.Attributes.TryGetValue(attribute.Key, out oldValue);

                        if (!Equals(oldValue, _annuledUserStateId))
                            AddGraphicLayer(change.New, modifier);
                    }
                }
            }

            return true;
        }

        private void AddGraphicLayer(IDataObject dataObject, IObjectModifier modifier)
        {
            ...
        }
    }
}

```

#### Методы

```cpp
bool ProcessChanges(IEnumerable<DataObjectChange> changes, IObjectModifier modifier);
```
Данный метод вызывается после того, как пользователь делает какое-либо изменение.

где:
    `changes` - список изменений. Подробнее см. [DataObjectChange](#DataObjectChange);
    `modifier` - объект модификатора, с помощью которого можно вносить изменения в объект. Подробнее см. [IObjectModifier](#IObjectModifier);

Метод возвращает флаг, который показывает, следует ли применять изменения.

<a name="IFileHandler"/>

### Интерфейс IFileHandler
Интерфейс позволяет перхватить обработку файлов прикладываемых к документу. Для этого необходимо реализовать интерфейс `Ascon.Pilot.SDK.IFileHandler` и пометить класс, реализующий интерфейс, атрибутом `[Export]`
####Методы

```cpp
bool Handle(string inputFile, out List<string> outputFiles, FileHandlerSource source);
```
где:
    `inputFile` - путь к обрабатываему файлу;
    `outputFiles` - колекция путей к новым файлам после обработки;
    `source` - контекст срабатывания метода `Handle`:
  - `FileHandlerSource.ObjectCreation` диалог создания нового объекта;
  - `FileHandlerSourc.Publication` публикация документа;
  - `FileHandlerSource.AutoImport` диалог автоимпорта.

Метод возвращает флаг, который показывает, следует ли применять результат обработки файлов.

```cpp
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace Ascon.Pilot.SDK.FileHandlerSample
{
    [Export(typeof(IFileHandler))]
    class FileHandler : IFileHandler
    {
        public bool Handle(string inputFile, out List<string> outputFiles, FileHandlerSource source)
        {
            outputFiles = new List<string>();

            if (!File.Exists(inputFile))
                return false;

            if (!string.Equals(Path.GetExtension(inputFile), ".pdf", StringComparison.OrdinalIgnoreCase))
                return false;
            //some logic for pdf format as example
            return true;
        }
    }
}

```

<a name="AutomationInterfaces"/>

## 3. Интерфейсы для скриптов автоматизации

<a name="IAutomationActivity"/>

##### Автоматизация. Пользовательские действия IAutomationActivity

Для того, чтобы написать свое пользовательское действие для автоматизации процессов, в SDK придусмотрен ряд интерфейсов.

#### Интерфейс IAutomationActivity

Интерфейс IAutomationActivity позволяет встроится в процесс автоматизации и обработать полученный изменения. Пример использования данного интерфейса можно найти в проекте SignMeAsActivity.

Пример встраивания в процесс автоматизации:
```cpp

...
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Automation;

namespace SignMeAsActivity
{
    [Export(typeof(IAutomationActivity))]
    public class SignMeAsActivity : AutomationActivity
    {
        private readonly IDigitalSigner _digitalSigner;

        [ImportingConstructor]
        public SignMeAsActivity(IDigitalSigner digitalSigner)
        {
            _digitalSigner = digitalSigner;
        }

        // Имя activity должно совпадать с именем указанным в настройках автоматизации в myAdmin
        public override string Name => nameof(SignMeAsActivity);

        // Переопределим метод
        public override async Task RunAsync(IObjectModifier modifier, IAutomationBackend backend, IAutomationEventContext context, TriggerType triggerType)
        {
            // Код действия здесь...
            ...
        }

        ...
    }

}

```

`AutomationActivity` - класс реализующий интерфейс `IAutomationActivity`. Устанавливает стандартные настройки действия по умолчанию. Например такие свойства как: `SourceTypes`, `Target`, `RelationType` и д.р.

В классе действия (activity) также можно воспользоваться импортированием зависимостей в конструктор с помощью атрибута `ImportingConstructor`.

#### Свойства

```cpp
string Name { get; }
```
Свойство возвращает имя действия (activity).

> !!! Внимение !!!  Возвращаемое имя должно совпадать с именем, указанным в скрипте автоматизации (json), задаваемом в настройках автоматизации в myAdmin.

```cpp
Dictionary<string, object> Params { get; }
```
Свойство возвращает набор параметров, заданных в настройках атоматизации в myAdmin для этого действия (activity). 

```cpp
List<string> SourceTypes { get; }
```
Свойство возвращает набор типов для которых сработает действие. Типы задаются в настройках атоматизации в myAdmin при описании этого действия (activity).

```cpp
TargetType? Target { get; }
```
Свойство возвращает значение на каго распространяется дейтсвие. Подробнее смотри [TargetType](#TargetType)

```cpp
ObjectRelationType? RelationType { get; }
```
Свойство возвращает значение связи, на которые распространяется это действие (activity). Подробнее смотри [ObjectRelationType](#ObjectRelationType)

```cpp
Dictionary<string, string> Errors { get; }
```
Свойство возвращает список ошибок при вызове действия (activity).

```cpp
RelationFilterByChangeKind? RelationFilterByChangeKind { get; }
```
Свойство возвращает значение, указывающее на какой тип изменения связей реагирует действие (activity). Значение заполняется в myAdmin.

#### Методы

```cpp
Task RunAsync(IObjectModifier modifier, IAutomationBackend backend, IAutomationEventContext context, TriggerType triggerType);
```
Метод вызывается когда срабатывает действие.

где:
    `modifier` - объект модификатора, с помощью которого можно вносить изменения в объект. Подробнее см. [IObjectModifier](#IObjectModifier);

    `backend` - объект доступа к различным типам или объектам в системе. Подробнее см. [IAutomationBackend](#IAutomationBackend);

    `context` - объект доступа к инициирующему изменению и его свойствам Подробнее см. [IAutomationEventContext](#IAutomationEventContext);

    `triggerType` - тип триггера. Подробнее см. [TriggerType](#TriggerType);


<a name="IAutomationEventContext"/>

##### Интерфейс IAutomationEventContext.

Интерфейс описывает контекст события автоматизации.

#### Свойства ####

```cpp
IDataObject Source { get; }
```
Свойство возвращает инициирующий событие элемент. 

```cpp
IEnumerable<IChange> Changes { get; }
```
Свойство возвращает все изменения, произошедшие в процессе изменения инициирующего элемента.

```cpp
IPerson InitiatingPerson { get; }
```
Свойство возвращает пользователя инициирующего изменения.

```cpp
DateTime EventDate { get; }
```
Свойство возвращает время начала изменения. Дата указа в UTC.


<a name="IAutomationBackend"/>

##### Интерфейс IAutomationBackend.

Интерфейс описывает объект доступа к данным доступным для действий.

#### Методы

```cpp
Guid GetDatabaseId();
```
Метод возвращает идентификатор базы данных.

```cpp
IDictionary<int, IType> GetTypes();
```
Метод возвращает словарь с описанием доступных типов элементов

```cpp
IPerson CurrentPerson();
```
Метод возвращает текущего пользователя

```cpp
IDictionary<int, IPerson> GetPeople();
```
Метод возвращает словарь с описанием доступных пользователей

```cpp
 IDictionary<int, IOrganisationUnit> GetOrganisationUnits();
```
Метод возвращает словарь с описанием доступных организационных единиц

```cpp
 IDataObject GetObject(Guid id);
```
Метод возвращает запрошенный элемент.


<a name="TriggerType"/>

##### Перечисление  TriggerType
Перечисление описывает тип возможных триггеров в системе

#### Значения

```cpp
 None = 0
```
Значение по умолчанию
```cpp
 Client = 1
```
Триггер срабатывает только на клиенте
```cpp
 Server = 2
```
Триггер срабатывает только на сервере
```cpp
 ServerAndClient = Client | Server
```
Триггер срабатывает и на клиенте и на сервере


<a name="TargetType"/>

##### Перечисление  TargetType

Перечисление описывает типы, на которые должно срабатывать действие (activity)

#### Значения

```cpp
 Self
```
Если в описании автоматизации указан тип `Self`, то действие (activity) должно срабатывать над элементом, инициировавшем изменения. 

```cpp
 Relations
```
Если в описании автоматизации указан тип `Relations`, то действие (activity) должно срабатывать над элементами, которые связаны с инициировавшем изменения по типу связи указанному в свойстве RelationType действия (activity). Подробнее  [ObjectRelationType](#ObjectRelationType)

<a name="RelationFilterByChangeKind"/>

##### Перечисление  RelationFilterByChangeKind

Перечисление описывает типы возможных изменений связей у инициирующего элемента.

#### Значения

```cpp
 Added
```
Если в описании автоматизации указан тип `Added`, то действие (activity) должно срабатывать при добавлении связей.

```cpp
 Removed
```
Если в описании автоматизации указан тип `Removed`, то действие (activity) должно срабатывать при удалении связей.

<a name="ActivityExtensions"/>

##### Вспомогательный класс ActivityExtensions

#### Методы

```cpp
public static IEnumerable<IDataObject> GetObjectsToExecute(IAutomationEventContext context, IAutomationBackend backend, IAutomationActivity sourceActivity)
```

Метод возвращает элементы в зависимости от настроек `RelationFilterByChangeKind`, `TargetTypes` и `OjectRelationType` в описании действия в myAdmin.




<a name="DataManagementInterfaces"/>

## Интерфейсы управления данными

Для получения доступа к интерфейсам их необходимо передать в конструктор. Можно передавать любые из перечисленных ниже интерфейсов. При этом, конструктор обязательно необходимо пометить атрибутом `[ImportingConstructor]`.

```cpp
[Export(typeof(IObjectContextMenu))]
public class ModifyObjectsPlugin : IObjectContextMenu
{
     private readonly IObjectModifier _modifier;
     private readonly Repository _repository;

     [ImportingConstructor]
     public ModifyObjectsPlugin(IObjectModifier modifier, IObjectsRepository repository)
     {
         _modifier = modifier;
         _repository = repository;
     }
}
```

<a name="IObjectsRepository"/>

### Интерфейс IObjectsRepository

Интерфейс позволяет получить доступ к элементам, типам элементов и организационной структуре. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IObjectsRepository.GetCurrentPerson
Метод позволяет получить информацию о текущем пользователе, вошедшем в систему.

```cpp
IPerson IObjectsRepository.GetCurrentPerson();
```
Возвращает объект типа `IPerson`.


##### IObjectsRepository.GetOrganisationUnit

Метод позволяет получить информацию об организационной единице.

```cpp
IOrganisationUnit IObjectsRepository.GetOrganisationUnit(int id);
```
где:
- `id` - идентификатор должности или подразделения.

Возвращает объект типа `IOrganisationUnit`.


##### IObjectsRepository.GetOrganisationUnits

Метод позволяет получить список организационных единиц.

```cpp
IEnumerable<IOrganisationUnit> IObjectsRepository.GetOrganisationUnits();
```
где:
  - `id` - идентификатор должности или подразделения.


##### IObjectsRepository.GetPeople

Метод позволяет получить список всех пользователей.

```cpp
IEnumerable<IPerson> IObjectsRepository.GetPeople();
```


##### IObjectsRepository.GetPerson

Метод позволяет получить информацию о пользователе по идентификатору.

```cpp
IPerson IObjectsRepository.GetPerson(int id);
```
где:
  - `id` - идентификатор пользователя.


##### IObjectsRepository.GetType

Метод позволяет получить информацию о типе элемента по идентификатору или по имени.

```cpp
IType IObjectsRepository.GetType(int id);
```
где:
  - `id` - идентификатор типа элемента.

```cpp
IType IObjectsRepository.GetType(string name);
```
где:
  - `name` - имя типа элемента.


##### IObjectsRepository.GetTypes

Метод позволяет получить список всех типов.

```cpp
IEnumerable<IType> IObjectsRepository.GetTypes();
```


##### IObjectsRepository.SubscribeObjects

Метод предназначен для получения элементов по их идентификаторам. Метод работает асинхронно. Возвращает стандартный интерфейс [IObservable](https://msdn.microsoft.com/en-us/library/dd990377.aspx), который позволяет подписаться на изменения запрошенных элементов. Все запрошенные элементы будут загружены в методе `OnNext`.

```cpp
IObservable<IDataObject> IObjectsRepository.SubscribeObjects(IEnumerable<Guid> ids);
```
где:
  - `ids` - список идентификаторов элементов.

Пример:
```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ObjectsSample
{
    [Export(typeof(IDataPlugin))]
    public class ObjectsSamplePlugin : IDataPlugin, IObserver<IDataObject>
    {
        private readonly IObjectsRepository _repository;
        private readonly List<IDataObject> _elements;

        [ImportingConstructor]
        public ObjectsSamplePlugin(IObjectsRepository repository)
        {
            _repository = repository;
            _elements = new List<IDataObject>();
            //Получить корневой элемент
            _repository.SubscribeObjects(new[] { SystemObjectIds.RootObjectId }).Subscribe(this);
        }

        public void OnNext(IDataObject value)
        {
        	_elements.Clear();
            if (!_elements.Exists(e => e.Id == value.Id))
                _elements.Add(value);
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
```
В данном примере описан сценарий получения элементов асинхронно с использованием метода `SubscribeObjects`. В данном подходе при реализации интерфейса `IObserver<IDataObject>` необходимо оставить пустой реализацию методов `OnError` и `OnCompleted` во избежании ошибок в будущем. В методе `OnNext` реализовано сохранение запрошенных элементов для дальнейшего использования.


##### IObjectsRepository.SubscribeOrganisationUnits

Метод предназначен для подписки на изменения организационных единиц (должности и подразделения). Метод возвращает стандартный интерфейс [IObservable](https://msdn.microsoft.com/en-us/library/dd990377.aspx), который позволяет подписаться на изменения запрошенных объектов. Все запрошенные объекты будут загружены в методе `OnNext`.

```cpp
IObservable<IOrganisationUnit> IObjectsRepository.SubscribeOrganisationUnits();
```
Пример:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ObjectsSample
{IOrganisationUnit.Title
    [Export(typeof(IDataPlugin))]
    public class ObjectsSamplePlugin : IDataPlugin, IObserver<IOrganisationUnit>
    {
        [ImportingConstructor]
        public ObjectsSamplePlugin(IObjectsRepository repository)
        {
            repository.SubscribeOrganisationUnits().Subscribe(this);
        }

        public void OnNext(IOrganisationUnit value)
        {
        	//TODO update organization unit
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
```
В данном примере описан сценарий обновления организационной единицы с использованием метода `SubscribeOrganisationUnits`. В данном подходе при реализации интерфейса `IObserver<IOrganisationUnit>` необходимо оставить пустой реализацию методов `OnError` и `OnCompleted` во избежании ошибок в будущем. В методе `OnNext` необходимо реализовать обновление организационной единицы.


##### IObjectsRepository.SubscribePeople

Метод предназначен для подписки на изменения информации о пользователях. Метод возвращает стандартный интерфейс [IObservable](https://msdn.microsoft.com/en-us/library/dd990377.aspx), который позволяет подписаться на изменения запрошенных объектов. Все запрошенные объекты будут загружены в методе `OnNext`.

```cpp
IObservable<IPerson> IObjectsRepository.SubscribePeople();
```
Пример:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ObjectsSample
{
    [Export(typeof(IDataPlugin))]
    public class ObjectsSamplePlugin : IDataPlugin, IObserver<IPerson>
    {
        [ImportingConstructor]
        public ObjectsSamplePlugin(IObjectsRepository repository)
        {
            repository.SubscribePeople().Subscribe(this);
        }

        public void OnNext(IPerson value)
        {
        	//TODO update person info
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
```
В данном примере описан сценарий обновления информации о пользователе с использованием метода `SubscribePeople`. В данном подходе при реализации интерфейса `IObserver<IPerson>` необходимо оставить пустой реализацию методов `OnError` и `OnCompleted` во избежании ошибок в будущем. В методе `OnNext` необходимо реализовать обновление организационной единицы.


##### IObjectsRepository.SubscribeTypes

Метод предназначен для подписки на изменения описания типов элементов. Метод возвращает стандартный интерфейс [IObservable](https://msdn.microsoft.com/en-us/library/dd990377.aspx), который позволяет подписаться на изменения запрошенных типов. Все запрошенные типы будут загружены в методе `OnNext`.

```cpp
IObservable<IType> IObjectsRepository.SubscribeTypes();
```
Пример:

```cpp
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;

namespace ObjectsSample
{
    [Export(typeof(IDataPlugin))]
    public class ObjectsSamplePlugin : IDataPlugin, IObserver<IType>
    {
        [ImportingConstructor]
        public ObjectsSamplePlugin(IObjectsRepository repository)
        {
            repository.SubscribeTypes().Subscribe(this);
        }

        public void OnNext(IType value)
        {
        	//TODO update element type info
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
```
В данном примере описан сценарий обновления информации о типе элемента с использованием метода `SubscribeTypes`. В данном подходе при реализации интерфейса `IObserver<IType>` необходимо оставить пустой реализацию методов `OnError` и `OnCompleted` во избежании ошибок в будущем. В методе `OnNext` необходимо реализовать обновление описания типа.

  
##### IObjectsRepository.SubscribeNotification

Метод предназначен для получения уведомлений выбранного типа. Метод работает асинхронно. Возвращает стандартный интерфейс [IObservable](https://msdn.microsoft.com/en-us/library/dd990377.aspx), который позволяет подписаться на получения уведомлений. Полученные уведомление будут передаваться в метод `OnNext`.

```cpp
IObservable<INotification> SubscribeNotification(NotificationKind kind);
```
где:
  - `kind` - тип уведомлений для подписки.

##### IObjectsRepository.GetUserStates

Метод предназначен для получения списка всех пользовательских состояний, зарегистрированных в базе.

##### IObjectsRepository.GetUserStateMachines

Метод предназначен для получения списка всех машин состояний, зарегистрированных в базе.

##### IObjectsRepository.GetReportItems

Метод предназначен для получения списка всех доступных текущему пользователю отчетов.

##### IObjectsRepository.GetCurrentAccess

Метод предназначен для получения информации о доступе пользователя к определенному объекту.

```cpp
AccessLevel GetCurrentAccess(Guid objectId, int personId);
```
где:
  - `objectId` - идентификатор объекта
  - `personId` - идентификатор пользователя
  
##### IObjectsRepository.GetStoragePath

Метод возвращает путь к файлу или папке, смонтированной на Pilot-Storage

```cpp
string GetStoragePath(Guid objId);
```

##### IObjectsRepository.GetStorageObjects

Предоставляет информацию о файлах и папках на Pilot-Storage, находящихся по запрошенному абсолютному пути.

```cpp
IEnumerable<IStorageDataObject> GetStorageObjects(IEnumerable<string> paths);
```

##### IObjectsRepository.Mount

Монтирует элемент на Pilot-Storage. На Pilot-Storage могут быть смонтированы только элементы типов, помеченных флагом "Может монтироваться на диск". Т.к. метод выполняется асинхронно, смонтированный элемент может быть недоступен на Pilot-Storage сразу после вызова метода.

```cpp
void Mount(Guid objId);
```

##### IObjectsRepository.MountAsync

Монтирует элемент на Pilot-Storage. На Pilot-Storage могут быть смонтированы только элементы типов, помеченных флагом "Может монтироваться на диск". 

```cpp
Task MountAsync(Guid objId);
```

##### IObjectsRepository.GetHistoryItems
Метод предназначен для получения элементов истории объекта. Метод работает асинхронно. Возвращает стандартный интерфейс [IObservable](https://msdn.microsoft.com/en-us/library/dd990377.aspx), который позволяет подписаться на получения элементов истории по одному объекту. Полученные элементы будут передаваться в метод `OnNext`.

```cpp
IObservable<IHistoryItem> GetHistoryItems(IEnumerable<Guid> ids);
```
где:
  - `ids` - список идентификаторов элементов истории по одному объекту
  
##### IObjectsRepository.LoadCleanableFileIds
Метод предназначен для получения списка файлов, содержимое которых может быть безопасно удалено для освобождения места в локальном файловом хранилище. Использование данного метода демонстрируется в примере Ascon.Pilot.SDK.ClearCacheDaemon.

```cpp
Task<IEnumerable<Guid>> LoadCleanableFileIds();
```
  
  
<a name="IMessagesRepository"/>

### Интерфейс IMessagesRepository

Интерфейс позволяет получить доступ к сообщениям и чатам

##### IMessagesRepository.LoadMessagesAsync
Загрузка списка сообщений по чату. Перед вызовом этого метода для нового чата, неоходимо вызвать метод LoadChatAsync для этого чата.

```cpp
Task<List<IChatMessage>> LoadMessagesAsync(Guid chatId, DateTime dateFromUtc, DateTime dateToUtc, int maxNumber);
```
где:
- `chatId` - идентификатор чата
- `dateFromUtc` - минимальная серверная дата создания сообщения, попадающего в выборку
- `dateToUtc` - максимальная серверная дата создания сообщения, попадающего в выборку
- `maxNumber` - максимальное количество сообщений, попадающих в выборку

##### IMessagesRepository.LoadChatMembersAsync
Загрузка участников чата

```cpp
Task<IReadOnlyCollection<IChatMember>> LoadChatMembersAsync(Guid chatId, DateTime dateFromUtc);
```
где:
- `chatId` - идентификатор чата
- `dateFromUtc` - минимальная серверная дата создания сообщения, попадающего в выборку


##### IMessagesRepository.SendMessageAsync
Отправка сообщения

```cpp
Task SendMessageAsync(IChatMessage message);
```
где:
- `IChatMessage` - объект сообщения, полученный из `IMessageBuilder`
	

##### IMessagesRepository.LoadChatAsync
Получение чата по идентифкатору

```cpp
Task<IChat> LoadChatAsync(Guid id);
```
где:
- `IChat` - объект чата

<a name="IObjectModifier"/>
### Интерфейс IObjectModifier

Интерфейс предназначен для создания, изменения и удаление элементов. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.


##### IObjectModifier.Create

Метод позволяет создать новый элемент заданного типа.

```cpp
IObjectBuilder IObjectModifier.Create(IDataObject parent, IType type);
```
где:
 - `parent` - родительский элемент, в котором создается новый элемент.
 - `type` - тип создаваемого элемента.

Метод возвращает объект типа `IObjectBuilder`, который позволяет заполнить элемент атрибутами и задать дополнительные свойства. Подробнее смотри интерфейс IObjectBuilder


##### IObjectModifier.Edit

Метод позволяет отредактировать заданный элемент.

```cpp
IObjectBuilder IObjectModifier.Edit(IDataObject object);
```
где:
 - `object` - редактируемые элемент.

Метод возвращает объект типа `IObjectBuilder`, который позволяет заполнить элемент атрибутами и задать дополнительные свойства. Подробнее смотри интерфейс IObjectBuilder


##### IObjectModifier.Move

Метод позволяет переместить элемент из текущего родительского элемента в другой.

```cpp
void IObjectModifier.Move(IDataObject object, IDataObject newParent);
```
где:
 - `object` - текущий элемент, который надо переместить;
 - `newParent` - новый родительский элемент.


##### IObjectModifier.Delete

Метод позволяет удалить элемент в корзину.

```cpp
void IObjectModifier.Delete(IDataObject object);
```
где:
 - `object` - элемент, который надо удалить.

##### IObjectModifier.DeleteById

Метод позволяет удалить элемент в корзину.

```cpp
void IObjectModifier.DeleteById(Guid objectId);
```
где:
 - `objectId` - идентификатор элемента, который надо удалить.
 
##### IObjectModifier.ChangeState

Метод позволяет изменить статус элемента.

```cpp
void IObjectModifier.ChangeState(IDataObject @object, ObjectState state);
```
где:
 - `object` - элемент, статус которого нужно изменить
 - `state` - новый статус элемента.

##### IObjectModifier.CreateLink

Метод позволяет связать два элемента выбранной связью.

```cpp
void IObjectModifier.CreateLink(IRelation relation1, IRelation relation2);
```
где:
 - `relation1` - описание связи для первого объекта
 - `relation2` - описание связи для второго объекта

Пример:
```cpp
using Ascon.Pilot.SDK;

class ObjectsSample
{
    private IObjectModifier _modifier;

    {...}

    private void CreateSourceFileLink(ObjectsViewContext context)
    {
        var object1 = context.SelectedObjects.First();
        var object2 = context.SelectedObjects.Last();

        //Create relations
        var relationId = Guid.NewGuid();
        var relationName = "RelationName";
        var relationType = ObjectRelationType.Custom;

        var relation1 = new Relation
        {
            Id = relationId,
            Type = relationType,
            Name = relationName,
            TargetId = object2.Id
        };

        var relation2 = new Relation
        {
            Id = relationId,
            Type = relationType,
            Name = relationName,
            TargetId = object1.Id
        };

        _modifier.CreateLink(relation1, relation2);
        _modifier.Apply();
    }

    {...}
}
```

##### IObjectModifier.RemoveLink

Метод позволяет удалить связь между объектами.

```cpp
void IObjectModifier.RemoveLink(IDataObject obj, IRelation relation);
```
где:
 - `obj` - объект, из которого будет удалена связь
 - `relation` - удаляемая связь

Пример:
```cpp
using Ascon.Pilot.SDK;

class ObjectsSample
{
    private IObjectModifier _modifier;

    {...}

    private void RemoveSourceFileLink(ObjectsViewContext context)
    {
		var selected = context.SelectedObjects.First();

		foreach (var relation in selected.Relations.Where(x => x.Type == ObjectRelationType.SourceFiles))
		{
			_modifier.RemoveLink(selected, relation);
		}
        _modifier.Apply();
    }

    {...}
}
```

##### IObjectModifier.Apply

Применить все сделанные над элементами изменения. Без вызова этого метода изменения не будут применены.

```cpp
void IObjectModifier.Apply();
```

##### IObjectModifier.Clear

Очистить все изменения, сделанные над элементами, без применения. Возвращает объект modifier к своему изначальному состоянию.

```cpp
void IObjectModifier.Clear();
```


<a name="IObjectBuilder"/>

### Интерфейс IObjectBuilder

Интерфейс обеспечивает возможность наполнения или редактирования элементов атрибутами и дополнительными свойствами.

#### Методы

```cpp
IObjectBuilder SetAttribute(string name, string value);
```
Метод задает или изменяет атрибут с заданным именем.
где:
 - `name` - имя атрибута.
 - `value` - новое значение атрибута.

Метод имеет несколько перегрузок:
`IObjectBuilder SetAttribute(string name, string value);` - для установки строкового значения атрибута.
`IObjectBuilder SetAttribute(string name, int value);` - для установки целочисленного значения атрибута.
`IObjectBuilder SetAttribute(string name, long value);` - для установки целочисленного 64-разрядного значения атрибута.
`IObjectBuilder SetAttribute(string name, DateTime value);` - для установки значения атрибута времени.
`IObjectBuilder SetAttribute(string name, double value);` - для установки цифрового значения атрибута с плавающей точкой.
`IObjectBuilder SetAttribute(string name, decimal value);` - для установки денежного значения атрибута.
`IObjectBuilder SetAttribute(string name, Guid value);` - для установки значения атрибута идентификатора `Guid`.
`IObjectBuilder SetAttribute(string name, int[] value);` - для установки значения атрибута список идентификаторов типа `int`.
`IObjectBuilder SetAttribute(string name, string[] value);` - для установки значения атрибута список идентификаторов типа `string`.

```cpp
IObjectBuilder RemoveAttribute(string name);
Метод удаляет атрибут с заданным именем.
где:
 - `name` - имя атрибута.
 
```cpp
IObjectBuilder AddRelation(string name, ObjectRelationType type, Guid sourceId, Guid targetId, DateTime versionId);
```
Метод добавляет новую связь к актуальной версии объекта.
где:
 - `name` - имя связи.
 - `type` - тип связи.
 - `sourceId` - идентификатор первого объекта.
 - `targetId` - идентификатор второго объекта.
 - `versionId` - идентификатор версии второго объекта.

```cpp
IObjectBuilder AddOrReplaceFile(string name, Stream stream, IFile file, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime);
```
Метод добавляет или заменяет файл к заданному элементу.
где:
 - `name` - имя файла.
 - `stream` - поток файла.
 - `file` - описание файла.
 - `creationTime` - дата создания файла.
 - `lastAccessTime` - дата последнего чтения из файла.
 - `lastWriteTime` - дата последнего изменения файла.

```cpp
IObjectBuilder ReplaceFileInSnapshot(DateTime snapshotCreated, Guid fileToReplace, string name, Stream stream, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime);
```
Метод заменяет файл в заданном снимке файлов (данное действие доступно только пользователю с правами администратора).
где:
 - `snapshotCreated` - временная метка снимка файлов.
 - `fileToReplace` - идентификатор заменяемого файла.
 - `name` - имя файла.
 - `stream` - поток файла.
 - `creationTime` - дата создания файла.
 - `lastAccessTime` - дата последнего чтения из файла.
 - `lastWriteTime` - дата последнего изменения файла.

```cpp
IObjectBuilder AddFile(string path);
```
Метод добавляет файл к заданному элементу.
где:
 - `path` - путь до файла, который надо добавить.

```cpp
IObjectBuilder AddFile(string name, Stream stream, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime);
```
где:
 - `name` - имя добавляемого файла.
 - `stream` - поток добавляемого файла.
 - `creationTime` - дата создания файла.
 - `lastAccessTime` - дата последнего доступа к файлу.
 - `lastWriteTime` - дата последнего доступа на запись в файл.

```cpp
IObjectBuilder RemoveFile(Guid fileId);
```
Метод-расширение интерфейса IObjectBuilder. удаляет файл из ActualFilesSnapshot элемента.
где:
 - `fileId` - идентификатор файла, который надо удалить.

```cpp
IObjectBuilder AddAccessRecords(int orgUnitId, AccessLevel level, DateTime validThrough, AccessInheritance inheritance, AccessType type);
```
Метод добавляет права доступа на элемент.
где:
 - `orgUnitId` - идентификатор должности или подразделения, на которое будет добавлено право доступа.
 - `level` - уровень доступа к элементу.
 - `validThrough` - срок действия права.
 - `inheritance` - уровень наследования прав доступа.
  - `AccessInheritance.None` - без наследования
  - `AccessInheritance.InheritUntilSecret` - наследование для общедоступных (скрытые элементы прерывают цепочку наследования прав доступа)
  - `AccessInheritance.InheritWholeSubtree` - наследование для всех (запись прав доступа будет наследоваться для всех дочерних элементов, в том числе скрытых). Добавление или удаление прав доступа с этой опцией доступно только администратору базы данных.
 - `type` - тип записи прав доступа. Доступные значения:
  - `AccessType.Allow` - разрешительное право доступа
  - `AccessType.Deny` - запретительное право доступа. Добавление или удаление запретительных прав доступа доступно только администратору базы данных.

```cpp
IObjectBuilder AddAccessRecords(int orgUnitId, AccessLevel level, DateTime validThrough, AccessInheritance inheritance, AccessType type, int[] typeIds);
```
Метод добавляет права доступа на элемент.
где:
 - `orgUnitId` - идентификатор должности или подразделения, на которое будет добавлено право доступа.
 - `level` - уровень доступа к элементу.
 - `validThrough` - срок действия права.
 - `inheritance` - уровень наследования прав доступа.
  - `AccessInheritance.None` - без наследования
  - `AccessInheritance.InheritUntilSecret` - наследование для общедоступных (скрытые элементы прерывают цепочку наследования прав доступа)
  - `AccessInheritance.InheritWholeSubtree` - наследование для всех (запись прав доступа будет наследоваться для всех дочерних элементов, в том числе скрытых). Добавление или удаление прав доступа с этой опцией доступно только администратору базы данных.
 - `type` - тип записи прав доступа. Доступные значения:
  - `AccessType.Allow` - разрешительное право доступа
  - `AccessType.Deny` - запретительное право доступа. Добавление или удаление запретительных прав доступа доступно только администратору базы данных.
 - `typeIds` - идентификаторы типов для которых будет добавлено право доступа, null - значит все типы.

```cpp
IObjectBuilder RemoveAccessRecords(Func<IAccessRecord, bool> predicate);
```
Метод удаляет права доступа на элемент по предикату.
где:
 - `predicate` - предикат, соответствующие которому права доступа будут удалены из элемента.

```cpp
IObjectBuilder MakeSecret();
```
Метод позволяет сделать элемент скрытым.

```cpp
IObjectBuilder MakePublic();
```
Метод позволяет сделать элемент общедоступным.


```cpp
IObjectBuilder CreateFileSnapshot(string reason);
```
Метод позволяет создать снимок файлов и указать причину. Текущие файлы будут перемещены в историю.
где:
 - `reason` - причина создания снимка файлов.

```cpp
IObjectBuilder MakeSnapshotActual(string reason, IFilesSnapshot snapshot);
```
Метод позволяет сделать выбранный снимок файлов текущим.
где:
 - `reason` - причина создания снимка файлов.
 - `snapshot` - снимок, который надо сделать текущим (актуальным).


```cpp
IObjectBuilder AddSubscriber(int personId);
```
Метод позволяет подписаться на изменения элемента.
где:
 - `personId` - идентификатор пользователя.


```cpp
IObjectBuilder RemoveSubscriber(int personId);
```
Метод позволяет отписаться от изменений элемента.
где:
 - `personId` - идентификатор пользователя.

```cpp
IObjectBuilder SetIsDeleted(bool isDeleted);
```
Метод позволяет задать элементу состояние удалено безвозвратно.
где:
 - `isDeleted` - флаг указывающий удалить объект или нет.

```cpp
IObjectBuilder SetIsInRecycleBin(bool isInRecycleBin);
```
Метод позволяет задать элементу состояние удалено корзину.
где:
 - `isInRecycleBin` - флаг указывающий удалить объект в корзину или нет.

```cpp
IObjectBuilder SetParent(Guid parentId);
```
Метод позволяет задать элементу нового родителя.
где:
 - `parentId` - идентификатор нового родителя.

```cpp
IObjectBuilder SetType(IType type);
```
Метод позволяет задать элементу тип.
где:
 - `type` - тип элемента.

>**ВНИМАНИЕ!**
Рекомендуется использовать этот метод только для восстановления безвозвратно удаленных объектов. В остальных случаях это может привести к неработоспособности системы.

```cpp
IObjectBuilder SetCreator(int creatorId);
```
Метод позволяет задать создателя элемента.
где:
 - `creatorId` - идентификатор пользователя (не должности).
 
 ```cpp
IObjectBuilder SetCreationDate(DateTime dateTime);
```
Метод позволяет задать дату создания элемента (доступно только при создании нового элемента).
где:
 - `dateTime` - дата/время создания в UTC.

```cpp
IObjectBuilder Lock();
```
Метод позволяет заблокировать объект для изменений текущим пользователем.

```cpp
IObjectBuilder Unlock();
```
Метод позволяет разблокировать объект.

```cpp
IObjectBuilder SaveHistoryItem();
```
Если метод SaveHistoryItem был вызван в процессе конструирования изменения, при применении изменения предыдущее состояние объекта будет сохранено в IDataObject.HistoryItems.

```cpp
ISignatureModifier SetSignatures(Predicate<IFile> selectFilesPredicate);
```
Метод позволяет задать новые запросы на подпись к выбранным файлам.
где:
 - `selectFilesPredicate` - функция выбора набора файлов.

Возвращает объект типа [ISignatureModifier](#ISignatureModifier) с помощью, которого можно изменить набор запросов на подпись.

<a name="IMessagesBuilder"/>

### Интерфейс IMessagesBuilder

Интерфейс обеспечивает возможность создания и наполнения дополнительными данными сообщений.

#### Методы

```cpp
 IChatMessage Message { get; }
```
Вовзвращает полученное в результате создания сообщение
 
```cpp
IMessageBuilder CreateChatMessage(MessageType type, Guid chatId);
```
Создает сообщение
где:
 - `type` - тип сообщения.
 - `chatId` - идентификатор чата.

```cpp
IMessageBuilder AddText(string text);
```
Заполняет `Message.Data` для текстового сообщения
где:
 - `text` - текст сообщения.

```cpp
IMessageBuilder AddText(string text, bool isReadOnly);
```
Заполняет `Message.Data` для текстового сообщения
где:
 - `text` - текст сообщения.
 - `isReadOnly` - определяет, будет ли текстовое сообщение доступно для редактирования.


```cpp
IMessageBuilder AddChatCreationData(string name, string description, ChatKind type);
```
Заполняет `Message.Data` данными для создания чата
где:
 - `name` - имя чата.
 - `description` - описание чата
 - `type` - тип чата

```cpp
IMessageBuilder AddChatRelation(ChatRelationType relationType, Guid objectId);
```
Заполняет `Message.Data` данными для создания связи чата с объектом
где:
 - `relationType` - тип связи.
 - `objectId` - идентификатор связываемого объекта.
 
```cpp
IMessageBuilder AddChatMemberData(int personId, bool isNotifiable, bool isAdmin)
```
Заполняет `Message.Data` данными для добавляения и изменения параметров участника чата
где:
 - `personId` - идентификатор участника чата.
 - `isNotifiable` - >признак получения участником уведомлений по чату
 - `isAdmin` -признак администратора чата.
 
```cpp
IMessageBuilder DeleteChatMember(int personId);
```
Заполняет `Message.Data` данными для удаления участника чата
где:
 - `personId` - идентификатор участника чата.

 
#### Свойства

```cpp
IDataObject DataObject { get; }
```
Свойство возвращает пустую болванку редактируемого элемента с заполненным идентификатором элемента и идентификатором родительского элемента.

<a name="ISignatureModifier"/>

### Интерфейс ISignatureModifier

Интерфейс прадназначен для изменения запросов на подпись в объекте.

#### Методы

```cpp
ISignatureBuilder Add(Guid id);
```
где:
 - `id` - идентификатор новго запроса на подпись.

Метод добавляет новый запрос на подпись. Возвращаемое значение [ISignatureBuilder](#ISignatureBuilder).

```cpp
ISignatureModifier Remove(Predicate<ISignatureRequest> findSignature);
```
где:
 - `findSignature` - фильтр;

Метод метод удаляет запросы на подпись согласно заданному фильтру.


```cpp
void SetSign(Guid id, string sign);
```
где:
 - `id` - идентификатор запроса на подпись;
 - `sign` - подпись (идентификатор файла подписи в формате string);

Метод добавляет подпись (идентификатор файла подписи в формате string) к запросу.

```cpp
ISignatureModifier SetPublicKeyOid(Guid id, string publicKeyOid);
```
где:
 - `id` - идентификатор запроса на подпись;
 - `publicKeyOid` - идентификатор криптоалгоритма;

Метод заполняет идентификатор криптоалгоритма, которым была создана подпись.

```cpp
ISignatureModifier SetLastSignCadesType(Guid id, CadesType cadesType);
```
где:
 - `id` - идентификатор запроса на подпись;
 - `cadesType` - [CadesType](#CadesType) последней подписи в запросе;
 
Метод заполняет [CadesType](#CadesType) последней подписи в запросе.

```cpp
ISignatureModifier SetIsAdvancementRequired(Guid id, bool isAdvancementRequired);
```
где:
 - `id` - идентификатор запроса на подпись;
 - `isAdvancementRequired` - определяет, нужно ли подписи усовершенствование;

Метод заполняет поле в запросе, определяющее нужно ли подписи усовершенствование;

<a name="ISignatureBuilder"/>

### Интерфейс ISignatureBuilder

Интерфейс предназначен для изменения запроса на подпись.

#### Методы

```cpp
ISignatureBuilder WithDatabaseId(Guid databaseId);
```
где:
 - `databaseId` - идентификатор базы данных.

Метод изменяет идентификатор базы данных.

```cpp
ISignatureBuilder WithPositionId(int positionId);
```
где:
 - `positionId` - идентификатор должности.

Метод изменяет должность у запроса на подпись.

```cpp
ISignatureBuilder WithRole(string role);
```
где:
 - `role` - роль подписанта.

Метод изменяет роль подписанта.

```cpp
ISignatureBuilder WithSign(string sign);
```
где:
 - `sign` - цифровая подпись в формате Base64.

Метод добавляет информацию о подписи в запрос (sign будет добавлен в коллекцию ISignatureRequest.Signs).

```cpp
ISignatureBuilder WithRequestSigner(string requestSigner);
```
где:
 - `requestSigner` - отображаемое имя должности.

Метод изменяет отображаемое имя должности подписанта.

```cpp
ISignatureBuilder WithIsAdditional(bool value);
```
где:
 - `value` - значение.

Метод изменяет флаг, указывающий на то, является ли запрос на подпись виртуальным.

```cpp
ISignatureBuilder WithObjectId(Guid objectId);
```
где:
 - `objectId` - идентификатор объекта.

Метод изменяет идентификатор объекта.

<a name="IFileProvider"/>

### Интерфейс IFileProvider
Интерфейс обеспечивает работу с телами файлов. Получить интерфейс можно через конструктор, помеченный атрибутом `[ImportingConstructor]`.

##### IFileProvider.Exists
Метод показывает, существует-ли тело файла с заданным идентификатором файла.
```cpp
bool IFileProvider.Exists(Guid fileId);
```
где:
 - `fileId` - идентификатор файла.

##### IFileProvider.IsFull
Метод показывает, полностью-ли загружен файл с сервера. 
```cpp
bool IFileProvider.IsFull(Guid fileId);
```
где:
 - `fileId` - идентификатор файла.

##### IFileProvider.OpenRead
Метод открывает заданный файл на чтение. Сигнатура метода:
```cpp
Stream IFileProvider.OpenRead(IFile file);
```
где:
 - `file` - открываемый файл.

Получение результирующего `Stream` не запускает скачивание тела файла с сервера, а возвращает **"ленивый"** поток. Рассмотрим на примере:
Предположим, что у нас на клиенте ранее не запрашивался file длиной 100 байт.
```cpp
var stream = fileProvider.OpenRead(file1); // возвращается "ленивый" поток, с сервера ничего не запрашивается
var buffer = new byte[50];
stream.Read(buffer, 0, buffer.Length); // поток понимает, что этих 50 байт у него нет, скачивает их с сервера и кэширует, буффер заполняется данными. Если нет соединения с сервером, будет сгенерировано исключение.
stream.Read(buffer, 0, buffer.Length); // поток перед этим вызовом уже на 50 позиции, так что будут скачаны следующие 50 байт, и они также будут закэшированы. Теперь файл прочитан и закэширован полностью (все 100 байт), и вызов IsFull после этого вернет true.
```
##### IFileProvider.GetFileSizeOnDisk
```cpp
long IFileProvider.GetFileSizeOnDisk(Guid fileId)
```
Получает объем занимаемого файлом места на диске в локальном файловом хранилище. Не обязательно соответствует размеру файла на сервере, т.к. файл на клиенте может быть не загружен совсем или загружен частично.

##### IFileProvider.DeleteLocalFile
```cpp
void IFileProvider.DeleteLocalFile(Guid fileId)
```
Удаляет содержимое файла из локального файлового хранилища. Файл на сервере при этом остается, и при следующем обращении к файлу на клиенте с помощью метода IFileProvider.OpenRead файл будет загружен на клиент повторно.
Данный метод может быть использован для очистки занимаемого файлом места в локальном файловом хранилище.
 
<a name="IPilotStorageListener"/>

### Интерфейс `IPilotStorageListener`
Интерфейс позволяет отслеживать изменения состава папок и состояний файлов и папок на **Pilot-Storage**. Данный интерфейc должен быть реализован классом на стороне расширения, и класс должен быть помечен атрибутом `[Export(typeof(IPilotStorageListener))]`. Данный интерфейc содержит методы:

##### IPilotStorageListener.FileAdded
Метод будет вызван при добавлении/появлении файла на **Pilot-Storage**
```cpp
void FileAdded(Guid id, string path, StorageObjectState state, NotificationTrigger trigger);
```
где:
 - `id` - идентификатор объекта, связанного с файлом на **Pilot-Storage**.
 - `path` - полный путь к добавленному файлу.
 - `state` - текущее состояние файла.
 - `trigger` - определяет, был ли файл добавлен на **Pilot-Storage** вследствие изменения (добавления текущим или другим пользователем) или загрузки структуры файлов и папок при монтировании проекта (в том числе, при старте приложения).
 
##### IPilotStorageListener.FileRemoved
Метод будет вызван, если по каким-то причинам (удаление/перемещение/права доступа и т.д.) файл больше не существует на **Pilot-Storage**
```cpp
void FileRemoved(Guid id, string path, NotificationTrigger trigger);
```
где:
 - `id` - идентификатор объекта, связанного с файлом на **Pilot-Storage**.
 - `path` - полный путь к удаленному файлу.
 - `trigger` - не имеет практического смысла в случае `FileRemoved`, оставлен для возможности дальнейшего расширения.
 
##### IPilotStorageListener.FileEdited
Метод будет вызван при редактировании содержимого файла на **Pilot-Storage**. Метод `FileEdited` может быть вызван несколько раз при однократном редактировании файла инструментом, т.к. запись в файл, как правило, ведется блоками фиксированного размера, зависящего от инструмента, редактирующего файл. Метод будет вызван при записи каждого такого блока.
```cpp
void FileEdited(Guid id, string path, StorageObjectState state, NotificationTrigger trigger);
```
где:
 - `id` - идентификатор объекта, связанного с файлом на **Pilot-Storage**.
 - `path` - полный путь к изменяемому файлу.
 - `state` - текущее состояние файла.
 - `trigger` - не имеет практического смысла в случае `FileEdited`, оставлен для возможности дальнейшего расширения.
 
##### IPilotStorageListener.DirectoryAdded
Метод будет вызван при добавлении/появлении папки на **Pilot-Storage**
```cpp
void DirectoryAdded(Guid id, string path, StorageObjectState state, NotificationTrigger trigger);
```
где:
 - `id` - идентификатор объекта, связанного с папкой на **Pilot-Storage**.
 - `path` - полный путь к добавленной папке.
 - `state` - текущее состояние папки.
 - `trigger` - определяет, была ли папка добавлена на **Pilot-Storage** вследствие изменения (добавления текущим или другим пользователем) или загрузки структуры файлов и папок при монтировании проекта (в том числе, при старте приложения). 
 
##### IPilotStorageListener.DirectoryRemoved
Метод будет вызван, если по каким-то причинам (удаление/перемещение/права доступа и т.д.) папка больше не существует на **Pilot-Storage**
```cpp
void DirectoryRemoved(Guid id, string path, NotificationTrigger trigger);
```
где:
 - `id` - идентификатор объекта, связанного с папкой на **Pilot-Storage**.
 - `path` - полный путь к удаленной папке.
 - `trigger` - не имеет практического смысла в случае `DirectoryRemoved`, оставлен для возможности дальнейшего расширения.
 
##### IPilotStorageListener.FileStateChanged
Метод будет вызван при изменении состояния файла на **Pilot-Storage**
```cpp
void FileStateChanged(Guid id, string path, StorageObjectState state, NotificationTrigger trigger);
```
где:
 - `id` - идентификатор объекта, связанного с файлом на **Pilot-Storage**.
 - `path` - полный путь к измененному файлу.
 - `state` - новое состояние файла.
 - `trigger` - определяет, произошло ли изменение состояния файла на **Pilot-Storage** вследствие изменения данных или загрузки структуры файлов и папок при монтировании проекта (в том числе, при старте приложения). 
 
##### IPilotStorageListener.DirectoryStateChanged
Метод будет вызван при изменении состояния папки на **Pilot-Storage**
```cpp
void DirectoryStateChanged(Guid id, string path, StorageObjectState state, NotificationTrigger trigger);
```
где:
 - `id` - идентификатор объекта, связанного с папкой на **Pilot-Storage**.
 - `path` - полный путь к измененной папке.
 - `state` - новое состояние папки.
 - `trigger` - определяет, произошло ли изменение состояния папки на **Pilot-Storage** вследствие изменения данных или загрузки структуры файлов и папок при монтировании проекта (в том числе, при старте приложения).  

<a name="IPilotStorageCommandController"/>

### Интерфейс `IPilotStorageCommandController`
Интерфейс позволяет вызывать команды **Pilot-Storage**. Получить интерфейс можно через конструктор, помеченный атрибутом `[ImportingConstructor]`.

##### IPilotStorageCommandController.Execute
Вызов команды **Pilot-Storage**
```cpp
void Execute(PilotStorageCommand command, params string[] paths);
```
где:
 - `command` - вызываемая команда.
 - `paths` - список полных путей к файлам или папкам, для которых будет вызвана команда.
 
##### IPilotStorageCommandController.CanExecute
Проверяет возможность вызова команды **Pilot-Storage** для указанных файлов и папок.
```cpp
void CanExecute(PilotStorageCommand command, params string[] paths);
```
где:
 - `command` - команда.
 - `paths` - список полных путей к файлам или папкам, для которых будет проверена возможность вызова команды.

<a name="PilotStorageCommand"/>

### Список доступных команд

Список доступных команд описан перечислением `PilotStorageCommand`.

`Commit` - отправка изменений по файлу(или файлам в папке) на сервер
`Download` - загрузка содержимого файла (или файлов в папке) с сервера
`GetLatestVersion` - обновление содержимого файла (или файлов в папке) на самую свежую версию
`Revert` - отмена изменений по файлу (или файлам в папке).
`Subsribe` - подписка на изменения по файлу/папке.
`Unsubsribe` - отписка от изменений по файлу/папке.
`Unmount` - размонтирование. Работает только для папок верхнего уровня.
`Lock` - блокировка файла.
`Unlock` - разблокировка файла.
`Freeze` - заморозка файла/папки.
`Unfreeze` - разморозка файла/папки.
`Publish` - публикация файла в ECM документ.
`PublishAndCommit` - публикация файла в ECM документ с последующей отправкой изменений по файлу на сервер.
`AppendPublish` - публикация файла добавлением новых страниц в ECM документ.
`AppendPublishAndCommit` - публикация файла добавлением новых страниц в ECM документ с последующей отправкой изменений по файлу на сервер.
`UpdateFilesAttributes` - заполнение полей в файле значениями атрибутов ECM документа.
`Discard` - отмена непринятых сервером изменений по файлу/папке.

<a name="IClientInfo"/>

### Интерфейс IClientInfo
Интерфейс позволяет получить различную информацию о клиенте. Получить интерфейс можно через конструктор, помеченный атрибутом `[ImportingConstructor]`.

#### Методы
```cs
ClientType GetClientType();
```
Метод позволяет получить тип клиента. Подробнее см [ClientType](#ClientType)

#### Свойства
```cs
string AutoimportDirectory { get; set; }
```
Получить путь до папки автоимпорта.

```cs
Uri ConnectionString { get; }
```
Получить адрес подключения к серверу **Pilot-Server**.


<a name="XPSInterfaces"/>

## 5. Интерфейсы работы с XPS

<a name="IXpsRender"/>

### Интерфейс IXpsRender
Интерфейс позволяет рендерить XPS документы в растровое изображение. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IXpsRender.RenderXpsToBitmap

Метод возвращает список отрендеренных страниц в формате Stream.

```cpp
IEnumerable<Stream> IFileProvider.RenderXpsToBitmap(Stream xpsStream, double quality = 1.0);
```
где:
 - `xpsStream` - поток XPS документы.
 - `quality` - качество, в котором будет рэндериться (от 0 до 6).

<a name="IFileSaver"/>

### Интерфейс `IFileSaver`
Интерфейс позволяет сохранять документы (повторяет логику работы команды "Отправить -> На диск"). Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IFileSaver.SaveFileAsync

Метод сохраняет файлы документа по указанному пути.

```cpp
	Task<IList<string>> IFileSaver.SaveFileAsync(IDataObject dataObject, string outputFilePath, FileSaverOptions options = null);
```
где:
 - `dataObject` - документ с файлами.
 - `outputFilePath` - путь сохранения файлов.
 - `options` - дополнительные параметры для IFileSaver.

 ##### IFileSaver.SaveFileVersionAsync

Метод сохраняет файлы документа выбранной версии по указанному пути.

```cpp
	Task<IList<string>> IFileSaver.SaveFileVersionAsync(IDataObject dataObject, DateTime snapshot, string outputFilePath, FileSaverOptions options = null);
```
где:
 - `dataObject` - документ с файлами.
 - `snapshot` - Created timestamp версии IFilesSnapshot для сохранения.
 - `outputFilePath` - путь сохранения файлов.
 - `options` - дополнительные параметры для IFileSaver.
 
<a name="FileSaverOptions"/>

##### Class FileSaverOptions
Дополнительные параметры для `IFileSaver`.

**Свойства**

```cpp
GraphicLayerOption GraphicLayerOption { get; set; }
```
Определяет, как вшивать элементы в XPS файл при сохранении.

<a name="InjectKind"/>

##### GraphicLayerOption Enum

Определяет, как вшить графические слои, метки и штрихкоды в XPS файл при сохранении.

**Поля**
```cpp
InjectFloatingRelatable
```
Вшить в зависимости от параметра IsFloating у элементов.

```cpp
ForceInject
```
Вшить все элементы.

```cpp
KeepAllAsFloating
```
Добавить к файлу все элементы как плавающие.

```cpp
LoseGraphicLayers
```
Ничего не вшивать и не добавлять к документу, кроме подписей.

<a name="IXpsViewer"/>

### Интерфейс IXpsViewer

Интерфейс позволяет получить номер текущей страницы просмотрщика документов и произвести увеличение/уменьшение масштаба на элементе графического слоя или аннотации. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IXpsViewer.CurrentPageNumber

Свойство возвращает номер открытой в просмотрщике страницы (от 0 до N-1)

 ##### IXpsViewer.ZoomToElement

Метод производит увеличение/уменьшение масштаба на элементе графического слоя или аннотации по его идентификатору

```cpp
void ZoomToElement(Guid id, double scale = 1);
```
где:
 - `id` - Guid графического элемента или аннотации.
 - `scale` - изменение масштаба (от 0 до 1). 1 - максимальный масштаб

##### IXpsViewer.SubscribeLeftMouseClick

Метод позволяет _добавить_ обработчик клика левой кнопки мыши.

```cpp
void SubscribeLeftMouseClick(IMouseLeftClickListener leftMouseClickListener);
```
где:
 - `leftMouseClickListener` - объект реализующий интерфейс [IMouseLeftClickListener](#IMouseLeftClickListener)

##### IXpsViewer.UnsubscribeLeftMouseClick

Метод позволяет _удалить_ обработчик клика левой кнопки мыши.

##### IXpsViewer.SetGraphicLayerElementsVisibility

Метод позволяет скрыть или показать указанные по идентификатору графические элементы.

```cpp
void SetGraphicLayerElementsVisibility(IList<Guid> elementIds, bool hidden);
```
где:
 - `elementIds` - Guid'ы графических элементов.
 - `hidden` - true - скрывать, false - показывать

<a name="IXpsMerger"/>

### Интерфейс IXpsMerger

Интерфейс позволяет сохранять скомпонованный XPS документ по указанному пути. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IXpsMerger.SaveMergedXps

Метод сохраняет скомпонованный документ по пути и производит подписку на событие завершения сохранения

```cpp
void SaveMergedXps(string filepath, IXpsMergerSaveListener saveListener);
```
где:
 - `filepath` - путь для сохранения.
 - `saveListener` - обработчик события завершения сохранения документа
 
<a name="IXpsMergerSaveListener"/>

### Интерфейс IXpsMergerSaveListener

Интерфейс позволяет обработать завершение сохранения скомпонованного XPS документа. Для использования необходимо создать класс, унаследованный от абстрактного `XpsMergerSaveListener`, и передать его аргументом в метод `IXpsMerger.SaveMergedXps`

##### IXpsMergerSaveListener.OnSaveCompleted

Метод вызывается при завершении сохранения документа

```cpp
void OnSaveCompleted(string filepath);
```
где:
 - `filepath` - путь до сохраненного документа.

<a name="IGraphicLayerElement"/>

### IGraphicLayerElement

Интерфейс представляет элемент графического слоя на XPS документах. Каждый графический элемент представлен в виде 2 файлов, хранящихся в объекте.
Первый файл содержит описательную часть графического элемента, а именно xml-сериализованный объект, унаследованный от `IGraphicLayerElement`. Имя первого файла должно быть создано по правилу `GraphicLayerElementConstants.GRAPHIC_LAYER_ELEMENT + ElementId`, где 
 - `ElementId` - идентификатор графического элемента,

Второй файл представляет содержимое графического элемента (XAML или BITMAP). Имя второго файла должно быть создано по правилу `GraphicLayerElementConstants.GRAPHIC_LAYER_ELEMENT_CONTENT + ContentId`, где
 - `ContentId` - идентификатор содержимого графического элемента

#### Свойства

```cpp
Guid ElementId { get; }
```
Идентификатор графического элемента

```cpp
Guid ContentId { get; }
```
Идентификатор содержимого графического элемента

```cpp
 double OffsetY { get; }
```
Отступ по вертикали в точках

```cpp
 double OffsetX { get; }
```
Отступ по горизонтали в точках

```cpp
Point Scale { get; }
```
Масштаб по X и Y, по-умолчанию (1,1)

```cpp
double Angle { get; }
```
Угол поворота

```cpp
int PositionId { get; }
```
Идентификатор пользователя, у которого есть права на редактирования элемента (если 0, то редактировать могут все)

```cpp
int PageNumber { get; }
```
Номер страницы, на которую наложен графический элемент (от 0)

```cpp
VerticalAlignment VerticalAlignment { get; set; }
```
Вертикальное выравнивание

```cpp
HorizontalAlignment HorizontalAlignment { get; set; }
```
Горизонтальное выравнивание

```cpp
string ContentType { get; set; }
```
Тип содержимого графического элемента (`GraphicLayerElementConstants.XAML` или `GraphicLayerElementConstants.BITMAP`)

```cpp
bool IsFloating { get; set; }
```
Определяет, является ли графический элемент плавающим (будет ли он встраиваться в документ при создании запроса на подпись или отправке на диск). Если `true`, то графический элемент никогда не будет вшит и всегда будет доступен для перемещения и удаления.

<a name="GraphicLayerElementConstants"/>

### GraphicLayerElementConstants

Класс с константами для работы с графическим слоем

#### Свойства

```cpp
public const string GRAPHIC_LAYER_ELEMENT = "PILOT_GRAPHIC_LAYER_ELEMENT_";
```
Константа для обозначения графического элемента

```cpp
public const string GRAPHIC_LAYER_ELEMENT_CONTENT = "PILOT_CONTENT_GRAPHIC_LAYER_ELEMENT_";
```
Константа для обозначения содержимого графического элемента.

```cpp
public const string XAML = "xaml";
```
Константа для обозначения содержимого XAML.

```cpp
public const string BITMAP = "bitmap";
```
Константа для обозначения содержимого растровой графики.

```cpp
public static string Version = "2";
```
Константа для обозначения версии описания графического элемента.

<a name="ProviderInterfaces"/>

## 6. Интерфейсы управления

Для получения доступа к интерфейсам их необходимо передать в конструктор. Можно передавать любые из перечисленных ниже интерфейсов. При этом конструктор необходимо пометить атрибутом `[ImportingConstructor]`.

```cpp
[Export(typeof(IObjectContextMenu))]
public class ModifyObjectsPlugin : IObjectContextMenu
{
     private readonly ITabServiceProvider _
     viceProvider;

     [ImportingConstructor]
     public ModifyObjectsPlugin(ITabServiceProvider tabServiceProvider)
     {
         _tabServiceProvider = tabServiceProvider;
     }
}
```


<a name="ITabServiceProvider"/>

### ITabServiceProvider

Интерфейс обеспечивает работу с вкладками главного окна **клиента Pilot-ICE/ECM**. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

#### Методы

```cpp
IEnumerable<string> GetTabPageTitles();
```
Метод возвращает список заголовков открытых вкладок.

```cpp
string ITabServiceProvider.GetActiveTabPageTitle();
```
Метод возвращает заголовок активной вкладки.

```cpp
void ITabServiceProvider.ActivateTabPage(string title);
```
Вызов метода активирует вкладку с заданным заголовком.
где:
 - `title` - заголовок вкладки для активации.

```cpp
void ITabServiceProvider.OpenNewTabPage(bool isInOtherPane = false);
```
Метод открывает вкладку *Новая вкладка* в текущей панели вкладок или в новой.
где:
 - `isInOtherPane` - флаг, указывающий где открыть новую вкладку:
 	+ `true` - открыть вкладку в новой панели вкладок;
 	+ `false` - открыть вкладку в текущей панели вкладок. Значение по умолчанию.

```cpp
void ITabServiceProvider.OpenStandardTab(StandardTabType tabType);
```
Метод открывает новую стандартную вкладку в текущей панели вкладок. 
где:
 - `tabType` - тип стандартной вкладки:
    + `Docs` - обозреватель документов;
    + `Reports` - отчеты;
    + `Chats` - переписка;
    + `XpsMerger` - компоновщик документов;
    + `Tasks` - задания.

```cpp
void ITabServiceProvider.OpenTabPage(string title, FrameworkElement view, bool isInOtherPane = false);
```
Метод открывает новую вкладку с заданным контентом в текущей панели вкладок или в новой.
где:
 - `title` - локализованный заголовок открываемой вкладки;
 - `view` - контент, который будет отображаться в открываемой вкладке;
 - `isInOtherPane` - флаг, указывающий где открыть новую вкладку:
 	+ `true` - открыть вкладку в новой панели вкладок.
 	+ `false` - открыть вкладку в текущей панели вкладок. Значение по умолчанию.

```cpp
void ITabServiceProvider.CloseTabPage(string title);
```
Метод закрывает первую открытую вкладку с заданным заголовком.
где:
 - `title` - локализованный заголовок закрываемой вкладки.

```cpp
void ITabServiceProvider.UpdateTabPageContent(string oldTitle, string newTitle, FrameworkElement view);
```
Метод обновляет контент в открытой вкладке с заданным заголовком.
где:
 - `oldTitle` - локализованный заголовок вкладки, в которой меняется контент;
 - `newTitle` - новый локализованный заголовок вкладки;
 - `view` - новый контент, который будет отображаться во вкладке.
 
```cpp
void ITabServiceProvider.UpdateTabPageContent(Guid tabId, string title, FrameworkElement view);
```
Метод обновляет контент и заголовок вкладки, которая определяется ее идентификатором.
где:
 - `tabId` - идентификатор вкладки, в которой меняется заголовок и контент;
 - `title` - новый локализованный заголовок вкладки;
 - `view` - новый контент, который будет отображаться во вкладке.

```cpp
void ITabServiceProvider.UpdateActiveTabPageContent(FrameworkElement view);
```
Метод обновляет контент в текущей открытой вкладке.
где:
 - `view` - новый контент, который будет отображаться во вкладке.

```cpp
void ITabServiceProvider.ShowElement(Guid id);
```
Метод открывает вкладку (или использует существующую) и переходит к указаному объекту
где:
 - `id` - идентификатор объекта для навигации.

```cpp
void ITabServiceProvider.ShowElement(Guid id, bool openNewTab);
```
Метод открывает вкладку (или использует существующую) и переходит к указаному объекту
где:
 - `id` - идентификатор объекта для навигации;
 - `openNewTab` - флаг, указывающий на необходимость открытия новой вкладки.

```cpp
void ITabServiceProvider.ShowElement(Guid id, DateTime version);
```
Метод открывает вкладку (или использует существующую) и переходит к указаной версии объекта
где:
 - `id` - идентификатор объекта для навигации;
 - `version` - дата создания версии.

```cpp
void ITabServiceProvider.ShowElement(Guid id, DateTime version, object[] args);
```
Перегруженый вариант метода для перехода к объекту, принимающий список аргументов для навигации. Используется для перехода к объекту отчёта в случае, если необходимо не только открыть указанный объект, но и передать значения параметров построения отчёта.
где:
 - `id` - идентификатор объекта отчёта;
 - `version` - дата создания версии, не применимо для отчётов, заполняется DateTime.MinValue;
 - `args` - параметры навигации. Для передачи определенных объектов в качестве параметров построения отчёта нужно заполнить массив идентификаторамми данных объектов типа Guid;

```cpp
void ITabServiceProvider.UpdateActiveTabPageTitle(string title);
```
Метод позволяет обновить заголовок активной вкладки.
где:
 - `title` - старый заголовок вкладки;
 
 
 <a name="ITabStateHandler"/>

### ITabStateHandler

Интерфейс используется для сохранения состояния вкладок, принадлежащих модулю расширения, при выходе из приложения Pilot, а так же восстановления вкладок расширения и их состояния при последующем запуске приложения Pilot. Для примера реализации функционала сохранения и восстановления открытых вкладок расширения смотри пример OpenSpacePlugin из состава модулей SDK.

#### Методы

```cpp
bool TrySaveTabState(Guid tabId, out Guid extensionId, out string state, out string tabTitle);
```
Метод, позволяющий сохранить состояние вкладки, если она принадлежит модулю расширения. Вызывается **клиентом Pilot** непосредственно перед закрытием. В этот момент модуль расширения может проверить, принадлежит ли ему вкладка с указанным идентификатором, и если да, передать информацию, которая может быть использована для восстановления данной вкладки расширения после перезапуска. Должен возвращать `true` в случае, если вкладка с указанным идентификатором принадлежит данному модулю расширения (если он её ранее создавал), и если данную вкладку необходимо будет автоматически открыть заново, когда **клиент Pilot** будет запущен вновь. В случае, если вкладка с указанным идентификатором не принадлежит модулю расширения, либо не должна быть сохранена для последующего восстановления, метод должен вернуть начение false.
где:
 - `tabId` - идентификатор вкладки, которая открыта на момент выхода из приложения. Используется для утсановления факта принадлежности данной вкладки текущему модулю расширения;
 - `extensionId` - идентификатор модуля расширения, который необходимо вернуть в **клиент Pilot**. С помощью данного идентификатора возможно будет принять решение, принадлежит ли сохраненное состояние вкладки данному модулю расширения на этапе запуска **клиента Pilot**;
 - `state` - сериализованное в виде строки состояние вкладки, которое необходимо вернуть в **клиент Pilot**. Эта строка будет передана модулю расширения в момент запуска приложения для восстановлния состояния вкладки расширения;
 - `tabTitle` - сзаголовок вкладки, который необходимо вернуть в **клиент Pilot**. Это заголовок будет использоваться при восстановлении вклдаки после запуска приложения до тех пор, пока модуль расширения не обновит его в соответсвии с внутренней логикой восстановления состояния;


```cpp
bool TryAddTabAndRestoreState(Guid extensionId, string state, Guid tabId);
```
Метод, который вызывается **клиентом Pilot** при запуске и восстановлении набора сохраненных ранее вкладок. В этот момент модуль расширения должен проверить, совпадает ли идентификатор расширения, с которым модуль сохраняет состояниес своих вкладок, с идентификатором, который передан из **клиента Pilot**. Если идентификаторы совпадают и модуль успешно восстановил состояние вкладки, метод должен вернуть значение `true`. Если же информация о состоянии вкладки не принадлежит данному модулю, либо логика не позволяет восстановить данную вкладу, модуль должен вернуть занчение `false`.

 - `extensionId` - идентификатор расширения, который был использован при сохранении состояния вкладки;
 - `state` - сериализованная в виде строки информация, которая позволяет восстановить состояние вкладки;
 - `tabId` - идентификатор пустой вкладки, которая подготовлена **системой Pilot**, для замещения восстановленной версией вкладки модуля расширения. Метод `ITabServiceProvider.UpdateTabPageContent` должен быть использован для того, чтобы показать вновь созданную модулем расширения вкладку взамен текущей пустой вкладки с переданным идентификатором.
 
 
<a name="IPilotDialogService"/>

### IPilotDialogService

Интерфейс предоставляет работу со встроенными диалоговыми и всплывающими окнами **клиента Pilot-ICE/ECM**. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

#### Свойства

```cpp
tring AccentColor { get; }
```
Возвращает текущий главный (акцентный) цвет приложения.

```cpp
ThemeNames Theme { get; }
```
Возвращает текущую [схему](#ThemeNames) приложения.

#### Методы

```cpp
void ShowSharingSettingsDialog(IEnumerable<Guid> objectIds);
```
Метод показывает диалог управления общим доступом к элементам.
где:
 - `objectIds` - список выбранных идентификаторов элементов.

```cpp
IEnumerable<IOrganisationUnit> ShowPositionSelectorDialog(IPilotDialogOptions options = null);
```
Метод показывает диалог выбора пользователей и должностей.
где:
 - `options` - настройки диалога.

Диалог возвращает список выбранных должностей и пользователей.

```cpp
IEnumerable<IDataObject> ShowDocumentsSelectorDialog(IPilotDialogOptions options = null);
```
Метод показывает диалог выбора элементов из обозревателя проектов.
где:
 - `options` - настройки диалога.
Метод возвращает список выбранных элементов.

```cpp
IEnumerable<IDataObject> ShowDocumentsSelectorDialogAndNavigate(Guid parentId, IPilotDialogOptions options = null);
```
Метод показывает диалог выбора элементов из обозревателя проектов (с навигацией в указанную параметром папку).
где:
 - `parentId` - идентификатор родительского элемента, навигация в который будет выполнена при открытии диалога.
 - `options` - настройки диалога.
Метод возвращает список выбранных элементов.

```cpp
IEnumerable<IDataObject> ShowDocumentsSelectorByObjectTypeDialog(IEnumerable<int> objectTypeIds, IPilotDialogOptions options = null);
```
Метод показывает диалог выбора элементов указанного типа из обозревателя проектов.
где:
 - `objectTypeIds` - Id типов, которые могут быть выбраны
 - `options` - настройки диалога.
Метод возвращает список выбранных элементов.

```cpp
IEnumerable<IDataObject> ShowDocumentsSelectorByObjectTypeDialogAndNavigate(IEnumerable<int> objectTypeIds, Guid parentId, IPilotDialogOptions options = null);
```
Метод показывает диалог выбора элементов указанного типа из обозревателя проектов (с навигацией в указанную параметром папку).
где:
 - `objectTypeIds` - Id типов, которые могут быть выбраны
 - `parentId` - идентификатор родительского элемента, навигация в который будет выполнена при открытии диалога.
 - `options` - настройки диалога.
Метод возвращает список выбранных элементов.

```cpp
IEnumerable<IDataObject> ShowTasksSelectorDialog2(IPilotDialogOptions options = null);
```
Метод показывает диалог выбора заданий.
где:
 - `options` - настройки диалога.
Метод возвращает список выбранных объектов заданий.

```cpp
IEnumerable<IDataObject> IPilotDialogService.ShowReferenceBookDialog(string referenceBookConfiguration, string serializationId = null, IPilotDialogOptions options = null);
```
Метод показывает диалог выбора элементов из строкового справочика
где:
 - `referenceBookConfiguration` - строка конфигурации справочника (может быть получена из дополнительных параметров соответсвующего атрибута);
 - `serializationId` - идентификатор, использующийся для сохранения и восстановления состояния диалога (список развернутых узлов дерева);
 - `options` - дополнительные параметры показа диалога;
 
```cpp
IEnumerable<IDataObject> IPilotDialogService.ShowElementBookDialog(string elementBookConfiguration, string serializationId = null, IPilotDialogOptions options = null);
```
Метод показывает диалог выбора элементов для атрибута **Справочник элементов**
где:
 - `elementBookConfiguration` - строка конфигурации справочника элементов (может быть получена из дополнительных параметров соответсвующего атрибута);
 - `serializationId` - идентификатор, использующийся для сохранения и восстановления состояния диалога (список развернутых узлов дерева);
 - `options` - дополнительные параметры показа диалога;

```cpp
void ShowTaskDialog(int taskTypeId, IObjectModifier modifier, IPilotDialogOptions options = null);
```
Метод показывает диалог задания с предустановленными изменениями из `modifier`.
где:
 - `taskId` - идентификатор объекта задания для которого нужно показать диалог;
 - `modifier` - объект модификатора, с помощью которого можно вносить изменения в объект;
 - `options` - дополнительные параметры показа диалога;

```cpp
void ShowEditTaskDialog(Guid taskId, bool isReadonly, DialogOptions options = null);
```
Метод показывает диалог редактирования задания
где:
 - `taskId` - идентификатор объекта задания для которого нужно показать диалог редактирования;
 - `isReadonly` - флаг, который определяет, должна ли карточка задания быть показана в режиме только для чтения;
 - `options` - дополнительные параметры показа диалога;

```cpp
void ShowObjectDialog(Guid parentId, int objectTypeId, IReadOnlyDictionary<Guid, INChange> changes, bool showDocumentPreview);
```
Метод показывает диалог создания нового объекта
где:
 - `parentId` - идентификатор родительского объекта, в детях которого должен быть создан новый объект;
 - `objectTypeId` - тип создаваемого объекта;
 - `changes` - предзаполненные изменения объекта. Поддерживается только предзаполнение атрибутов значниями, которые будут показаны в карточке объекта;
 - `showDocumentPreview` - флаг, который указывает, показывать ли в диалоге создания объекта панель для добавлния документов;

```cpp
void ShowEditObjectDialog(Guid id, bool showDocumentPreview);
```
Метод показывает диалог редактирования карточки существующего объекта
где:
 - `id` - идентификатор объекта;
 - `showDocumentPreview` - флаг, который указывает, показывать ли в диалоге создания объекта панель для добавлния документов;

```cpp
void ShowBalloon(string title, string message, PilotBalloonIcon icon);
```
Метод показывает всплывающее окно с заданным заголовком, текстом и иконкой.
где:
 - `title` - заголовок всплывающего окна;
 - `message` - текст всплывающего окна;
 - `icon` - [иконка](#PilotBalloonIcon) всплывающего окна;

```cpp
IPilotDialogOptions NewOptions();
```
Создает параметры диалога по умолчанию.


<a name="ThemeNames"/>

### ThemeNames Enum

Задает значения, которые управляют схемой приложения.

#### Поля
```cpp
Jedi
```
Светлая схема приложения.

```cpp
Sith
```
Темная схема приложения.


<a name="PilotBalloonIcon"/>

### PilotBalloonIcon Enum

Задает значения, которые определяют иконку всплывающего окна.

#### Поля
```cpp
None
```
Иконка отсутствует.

```cpp
Info
```
Иконка, которая характеризует вывод информационного сообщения во всплывающем окне.

```cpp
Warning
```
Иконка, которая характеризует вывод предупреждения во всплывающем окне.

```cpp
Error
```
Иконка, которая характеризует вывод ошибки во всплывающем окне.

<a name="MessageType"/>

####MessageType Enum
Задает значения, которые определяют тип сообщения.

```cpp
TextMessage
```
Текстовое сообщение.

```cpp
ChatCreation
```
Сообщение создания чата.

```cpp
ChatMembers
```
Сообщение редактирования участников чата: добавление/удаление/редактирование.

```cpp
MessageRead
```
Сообщение о прочтении другого сообщения.

```cpp
MessageAnswer
```
Сообщение-ответ на другое сообщение.

```cpp
ChatChanged
```
Сообщение редактирования чата.

```cpp
ChatRelation
```
Сообщение о добавлении связи.

```cpp
MessageDropUnreadCounter
```
Сообщение сбросе счетчика непрочитанных сообщений.

```cpp
EditTextMessage
```
Сообщение о редактировании другого сообщения.


<a name="ChatKind"/>

####ChatKind Enum
Задает значения, которые определяют тип чата.

```cpp
Personal
```
Чат для личной переписки.

```cpp
Group
```
Групповой чат.

```cpp
ObjectRelated
```
Чат по элементу.

<a name="ChatRelationType"/>

####ChatRelationType Enum
Задает значения, которые определяют тип связи чата с объектом.

```cpp
Relation
```
Связь
```cpp
Attach
```
Вложение

<a name="ICertificate"/>

####ICertificate
Описывает сертификат

```cpp
string Thumbprint
```
Уникальный идентификатор сертификата

```cpp
DataTime ValidToDate
```
Дата окончания действия сертификата

```cpp
DataTime ValidFromDate
```
Дата начала действия сертификата

```cpp
string Issuer
```
Центр сертификации, выпустивший сертификат

```cpp
string Subject
```
Субъект сертификата

```cpp
string PublicKeyOid
```
Идентификатор алгоритма публичного ключа сертификата


<a name="CadesType"/>
####CadesType Enum

```cpp
Unknown
```
Неизвестная подпись.

```cpp
NotCades
```
Не являющаяся Cades подпись.

```cpp
CadesBes
```
Базовая подпись без штампов времени и ссылок на сертификаты.

```cpp
CadesEpes
```
Добавляется политика подписи.

```cpp
CadesT
```
Добавляется штамп времени.


```cpp
CadesC
```
Добавляется полные значения сертификатов и CRL для самодостаточной проверки.

```cpp
CadesXLT1
```
Добавляются ссылки на сертификаты и CRL, а также архивный штамп времени.

```cpp
CadesXLT2
```
Добавляются полные значения сертификатов и CRL(как в CADES-C).

```cpp
CadesA
```
Архивная подпись, включающая все необходимые данные для долговременной проверки подписи.

<a name="IEventAggregator"/>

### Интерфейс IEventAggregator

Интерфейс обеспечивает возможность подписываться на разные события возникающие в клиенте Pilot-ICE/ECM. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.


##### IEventAggregator.Subscribe

Метод подписывает заданного подписчика на события.

```cpp
void IEventAggregator.Subscribe(object subscriber);
```
где:
 - `subscriber` - подписчик. Должен реализовывать один или несколько интерфейсов событий

Пример:
```cpp
using Ascon.Pilot.SDK;

namespace EventSample
{
	class EventSubscriber : IHandle<UnloadedEventArgs>, IHandle<LoadedEventArgs>
    {
        private readonly IEventAggregator _eventAggregator;

        public EventSubscriber(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public void Handle(UnloadedEventArgs message)
        {
            //TODO do something when unloading extension
        }

        public void Handle(LoadedEventArgs message)
        {
            //TODO do something when loading extension
        }
    }
}
```
В примере показано как можно подписаться на события загрузки и выгрузки расширения.


##### IEventAggregator.Unsubscribe

Метод отписывается от  ранее подписанных событий;

```cpp
void IEventAggregator.Unsubscribe(object subscriber);
```
где:
 - `subscriber` - подписчик. Должен реализовывать один или несколько интерфейсов событий.

<a name="IConnectionStateProvider"/>

### Интерфейс IConnectionStateProvider

Интерфейс для получения информации о состоянии подключения к Pilot-Server


##### IConnectionStateProvider.IsOnline

Метод, определяющий, установлено ли соединение клиента с Pilot-Server

```cpp
bool IsOnline();
```

##### IConnectionStateProvider.Subscribe

Подписка на изменение состояния подключения клиента с Pilot-Server

```cpp
IObservable<bool> Subscribe();
```
<a name="IObjectCardControlProvider"/>
### Интерфейс IObjectCardControlProvider
Интерфейс служит для встраивания карточки объекта Pilot в интерфейс расширений. Может использоваться как для отображения карточки уже существующего объекта, так и для того, чтобы показать карточку создания нового объекта заданного типа. Получить этот интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

#### Методы
`GetObjectCardControl` Метод используется для получения элементов отображения и управления карточкой объекта. Возвращает объект типа `ObjectCardControl`, который предоставляет два свойства: котрол типа FrameworkElement, который при встраивании в визуальное дерева расширения отображает карточку объекта; и объект типа [`IObjectCardControlInteraction`](#IObjectCardControlInteraction), который используется для управления карточкой: получение введеных пользователем значений атрибутов, переход в режим только для чтения, и других возможностей.

Метод имеет два варианта перегрузки для встраивания карточки либо в режиме создания нового объекта, либо в режиме редактирования существующего объекта. Встраивание карточки в режиме создания нового объекта осуществляется при помощи следующего варианта метода:

```cs
 ObjectCardControl GetObjectCardControl(
     Guid newObjectId,
     IType newObjectType,
     Guid parentObjectId,
     IDictionary<string, object> initAttributes);
```
где:
 - `newObjectId` - идентификатор, который будет назначен новому объекту после создания;
 - `newObjectType` - тип нового объекта;
 - `parentObjectId` - идентификатор родительского объекта, в детях которого будет создан новый объект;
 - `initAttributes` - список предопределенных значений атрибутов, которыми будет инициализирована карточка нового объекта;

Встраивание карточки в режиме редактирования существующего объекта осуществляется другим вариантом метода:

```cs
ObjectCardControl GetObjectCardControl(Guid objectId, bool isReadonly);
```
где:
 - `objectId` - идентификатор объекта, для которого будет показана карточка;
 - `isReadonly` - флаг, который управляет тем, будет ли карточка показана в режиме только для чтения.;
 
<a name="IObjectCardControlInteraction"/>
### Интерфейс IObjectCardControlInteraction
Служит для взаимодействия расширения с карточкой объекта, которая была встроена с помощью метода `GetObjectCardControl` интерфейса [`IObjectCardControlProvider`](#IObjectCardControlProvider).

#### Методы
`OnObjectCardUpdated` Событие, которое позволяет отслеживать момент изменения значения атрибутов в карточки объекта.
```cs
 event EventHandler OnObjectCardUpdated
```
`GetIsChildDialogOpen` Возвращает true, если в настоящий момент остаётся открытым дочерний диалог, который был вызван из карточки объекта. Например, диалог выбора организационных единиц.
```cs
bool GetIsChildDialogOpen();
```
`GetIsValidInput` Позволяет узнать, являются ли все введеные значения атрибутов в карточке валидными.
```cs
bool GetIsValidInput();
```
`GetIsDataChanged` Возвращает true, если значение хотя бы одного атрибута было изменено.
```cs
bool GetIsDataChanged();
```
`GetValues` Используется для получения текущих значений атрибутов в карточке объекта.
```cs
IDictionary<string, ObjectCardValue> GetValues();
```
`SetIsReadOnly` Устанавливает значение фалага, который определяет, находится ли карточка объекта в режиме только для чтения.
```cs
void SetIsReadOnly(bool value);
```
`HideCreatedByInfo` Скрывает строку в интерфейсе карточки, которая отображает автора и время создания объекта.
```cs
void HideCreatedByInfo();
```
`UseCompactScrollbar` Переключает карточку в режим использования компактного вертикального скроллбара.
```cs
void UseCompactScrollbar();
```
<a name="IChatControlsProvider"/>
### Интерфейс IChatControlsProvider

Интерфейс обеспечивает возможность встроить элемент пользовательского интерфейса "Чаты" в интерфейс расширений. Получить этот интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

#### Методы

Метод позволяет получить элемент пользовательского интерфеса отвечающий за работу чатов привязанных к заданному элементу,
```cs
ChatControl GetObjectChatsControl(Guid objectId, IChatControlsOptions options = null);
```
где:
 - `objectId` - идентификатор элемента, к которому привязана переписка (чат).
 - `options` - дополнительные настройки элемента "Чаты".

Возвращает объект типа `ChatControl`, который предоставляет два свойства: объект типа `FrameworkElement`, который отображает элемент интерфейса "Чаты" и интерфейс `IChatControlsInteraction`, который используется для взаимодействия с с ним.

Метод создания дополнительных настроек элемента "Чаты".
```cs
IChatControlsOptions NewChatControlOptions();
```

<a name="IChatControlsOptions"/>
#### Интерфейс IChatControlsOptions
Интерфейс обеспечивает возможность настроить отображение элемента "Чаты".

##### Методы
Метод позволяет задать видимость инофрмационной панели элемента "Чаты"
```cs
IChatControlsOptions WithShowChatInfo(bool value);
```
Метод позволяет задать видимость заголовка объекта по которому создан чат
```cs
IChatControlsOptions WithShowObjectCaption(bool value);
```

<a name="IPilotServiceProvider"/>

### Интерфейс IPilotServiceProvider

Интерфейс обеспечивает возможность зарегистрировать и получить сервисы определенного типа (включая зарегистрированные другими модулями). Получить этот интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IPilotServiceProvider.GetServices

Метод позволяет получить сервисы заданного типа.

```cs
IEnumerable<TService> GetServices<TService>();
```
где:
 - `TService` - тип сервиса.

##### IPilotServiceProvider.Register

Метод позволяет зарегистрировать экземпляр сервиса заданного типа.

```cs
void Register<TService>(TService service);
```
где:
 - `TService` - тип сервиса;
 - `service` - экземпляр сервиса заданного типа.
 
<a name="IDocumentExplorer"/>

### Интерфейс IDocumentExplorer

Интерфейс позволяет управлять влкадкой с просмотрщиком документов. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IDocumentExplorer.UpdateDocumentPreview

Метод обновляет текущую вкладку с просмотрщиком документов.

```cpp
void UpdateDocumentPreview();
```

<a name="AttributeInterfaces"/>
## 7. Интерфейсы для работы с конфигурацией атрибутов: форматирование, нумераторы, справочники ##

Каждый из пользовательских атрибутов может быть дополнен конфигурацией. Конфигурация представляет собой текстовое поле, с помощью которого могут быть заданы специальные настройки атрибута:
- способ форматирования атрибутов объекта при отображении;
- описание нумератора;
- описание справочника;

Для работы с конфигурацией атрибута можно использовать интерфейс IAttributeFormatParser, который предоставляет методы для десериализации настроек, хранящихся в строке конфигурации атрибута. Более полную информацию о форматировании атрибутов, нумераторах и справочниках смотри в справке к Pilot-ICE. 

Пример *Ascon.Pilot.SDK.ObjectsSample* использует этот интерфейс для построения карточки объекта, включающей в себя отображение атрибутов, которым назначен нумератор, и атрибутов типа справочник; а так же для построения отоброжаемого имени объектов.


<a name="IAttributeFormatParser"/>

### Интерфейс `IAttributeFormatParser`

Позволяет работать с конфигурайией атрибутов: получать формат отображения атрибутов, а так же десериализованное описание нумераторов и справочников.

Для получения доступа к интерфейсу поиска его необходимо передать в конструктор. При этом конструктор необходимо пометить атрибутом [ImportingConstructor].

<a name="AttributesFormat"/>

##### string AttributesFormat(string format, Dictionary<string, object> attributes)

Осуществляет форматирование значениий атрибутов объекта в соответсвии с указанным форматом. Например, используется для получения отображаемого имени объекта. Строка форматирования может быть получена с помощью метода GetAttributeFormat, который описан ниже.

```cpp
string objAttributesDisplayString = _parser.AttributesFormat(format, objectAttributes);
```
где:
 - `objAttributesDisplayString ` - значение, полученное при применении приведенной строки форматирования к текущему состоянию объекта;
 - `_parser ` - экземляр объекта, реализующего интерфейс IAttributeFormatParser;
 - `format ` - приведенная строка форматирования атрибутов;
 - `objectAttributes ` - текущее состояние объекта: коллекция пар <ИмяАтрибута, ЗначениеАтрибута>;


<a name="GetAttributeFormat"/>

##### string GetAttributeFormat(string configuration)

Возвращает строку форматирования атрибута на основе конфигурации атрибута.

```cpp
string format = _parser.GetAttributeFormat(configuration);
```
где:
 - `format ` - строка форматирования атрибутов объекта;
 - `_parser ` - экземляр объекта, реализующего интерфейс IAttributeFormatParser;
 - `configuration ` - строка конфигурации атрибута;


<a name="GetAttributeFormatCulture"/>

##### string GetAttributeFormatCulture(string configuration)

Возвращает имя культуры, которая должна использоваться, при форматировании значения атрибута.

```cpp
string culture = _parser.GetAttributeFormatCulture(configuration);
```
где:
 - `culture ` - строка культуры форматирования арибутов объекта;
 - `_parser ` - экземляр объекта, реализующего интерфейс IAttributeFormatParser;
 - `configuration ` - строка конфигурации атрибута;


<a name="TryParseNumeratorDeclaration"/>

##### bool TryParseNumeratorDeclaration(string declaration, out IEnumerable<INumeratorInfo> numeratorInfo)

Метод для получения десериализованного описания нумераторов из строки конфигурации атрибута. Возвращает true, если строка конфигурации содержит описание нумераторов и его удалось успешно десериализовать; false в обратном случае.

```cpp
IEnumerable<INumeratorInfo> numeratorsCollection;
bool success = _parser.TryParseNumeratorDeclaration(configuration, out numeratorsCollection);
```
где:
 - `success ` - результат успешной десериализации описания нумераторов из приведенной строки конфигурации атрибута;
 - `_parser ` - экземляр объекта, реализующего интерфейс IAttributeFormatParser;
 - `configuration ` - строка конфигурации атрибута;
 - `numeratorsCollection ` - коллекция десериализованных описаний нумераторов, содержавшихся в приведенной строке конфигурации;


<a name="PreviewNumeratorFormat"/>

##### string PreviewNumeratorFormat(string configuration, Dictionary<string, string> values);

Применяет нумератор к текущему состоянию объекта и возвращает строку, которая может использоваться для предпросмотра значения атрибута, которое будет назначено сервером при создании объекта и применения к нему нумератора. Используется для визуализации значения атрибута в карточке создания нового объекта, которое будет назначено сервером.

```cpp
string attributeValuePreview = _parser.PreviewNumeratorFormat(configuration, objectAttributes);
```
где:
 - `attributeValuePreview  ` - значение атрибута, которое будет получено на стороне сервера, при применении нумератора к текущему состоянию;

объекта;
 - `configuration ` - строка конфигурации нумератора;
 - `objectAttributes ` - текущее состояние объекта: коллекция пар <ИмяАтрибута, ЗначениеАтрибута>;


<a name="TryParseReferenceBookConfiguration"/>

##### bool TryParseReferenceBookConfiguration(string configuration, out IReferenceBookConfiguration referenceBookConfiguration);

Метод для получения десериализованного описания справочника из строки конфигурации атрибута. Возвращает true, если строка конфигурации содержит описание справочника и его удалось успешно десериализовать; false в обратном случае.

```cpp
IReferenceBookConfiguration referenceBookConfiguration;
bool success = _parser.TryParseReferenceBookConfiguration(configuration, out referenceBookConfiguration);
```
где:
 - `success ` - результат успешной десериализации описания справочника из приведенной строки конфигурации атрибута;
 - `_parser ` - экземляр объекта, реализующего интерфейс IAttributeFormatParser;
 - `configuration ` - строка конфигурации атрибута;
 - `referenceBookConfiguration ` - десериализованное описание справочника, содержавшееся в приведенной строке конфигурации;

<a name="TryParseElementBookConfiguration"/>

##### bool TryParseElementBookConfiguration(string configuration, out IElementBookConfiguration elementBookConfiguration);

Метод для получения десериализованного описания атрибута **Справочник элементов**. Возвращает true, если строка конфигурации содержит описание справочника и его удалось успешно десериализовать; false в обратном случае.

```cpp
IElementBookConfiguration elementBookConfiguration;
bool success = _parser.TryParseElementBookConfiguration(configuration, out elementBookConfiguration);
```
где:
 - `success ` - результат успешной десериализации описания справочника из приведенной строки конфигурации атрибута;
 - `_parser ` - экземляр объекта, реализующего интерфейс IAttributeFormatParser;
 - `configuration ` - строка конфигурации атрибута;
 - `elementBookConfiguration ` - десериализованное описание справочника, содержавшееся в приведенной строке конфигурации. Интерфейфс [IElementBookConfiguration](#IElementBookConfiguration);


<a name="TryParseUserStateConfiguration"/>

##### bool TryParseUserStateConfiguration(string configuration, out IUserStateConfiguration userStateConfiguration);

Метод для получения десериализованного описания состояния из строки конфигурации атрибута. Возвращает true, если строка конфигурации содержит описание состояния и его удалось успешно десериализовать; false в обратном случае.

```cpp
IUserStateConfiguration userStateConfiguration
bool success = _parser.TryParseUserStateConfiguration(configuration, out userStateConfiguration);
```
где:
 - `success ` - результат успешной десериализации описания состояния из приведенной строки конфигурации атрибута;
 - `_parser ` - экземляр объекта, реализующего интерфейс IAttributeFormatParser;
 - `configuration ` - строка конфигурации атрибута;
 - `userStateConfiguration ` - десериализованное описание состояния, содержавшееся в приведенной строке конфигурации;
 
 <a name="DeferredNumeratorAttributeValuePrefix"/>

##### bool DeferredNumeratorAttributeValuePrefix { get };

Свойство для получаения префикса, указывающего, что значение данного атрибута нумератора требует отложенной регистрации.


<a name="INumeratorInfo"/>

### Интерфейс INumeratorInfo

Описание нумератора.

#### string DisplayName { get; }
Отображаемое имя нумератора.


#### string Configuration { get; }
Конфигурация нумератора, используется как параметр метода [PreviewNumeratorFormat](#PreviewNumeratorFormat), для получения предпросмотра значния атрибута, к которому применяется нумератор.

<a name="IReferenceBookConfiguration"/>

### Интерфейс IReferenceBookConfiguration

Конфигурация справочника, которая описывает его тип, поведение и правила формирования списка его элементов.

<a name="IReferenceBookConfiguration.Kind"/>

#### RefBookKind Kind { get; }
Свойство типа [RefBookKind](#RefBookKind), определяет тип справочника.

<a name="IReferenceBookConfiguration.Values"/>

#### IEnumerable&lt;string&gt; Values { get; }
Свойство, которое определяет коллекцию элементов справочника, если свойство [Kind](#IReferenceBookConfiguration.Kind) имеет значение [RefBookKind.Enum](#RefBookKind.Enum).

<a name="IReferenceBookConfiguration.Source"/>

#### Guid Source { get; }
Идентификатор объекта базы данных, дочерние объкты которого являются элементами справочника, если свойство [Kind](#IReferenceBookConfiguration.Kind) имеет значение [RefBookKind.Object](#RefBookKind.Object).


#### IEnumerable&lt;string&gt; ElementsTypes { get; }
Список имен типов, из объектов которого должны формироваться элементы справочника, если свойство [Kind](#IReferenceBookConfiguration.Kind) имеет значение [RefBookKind.Object](#RefBookKind.Object). Если имен список пуст, то в формировании элементов справочника учавствуют дочерние элементы любого типа.

#### bool IsEditable { get; }
Определяет, имеет ли пользователь возможность вручную отредактировать значение выбранного из элементов справочника. Если свойство имеет значение false, то редактор атрибута не допускает ручной ввод значений, и значение атрибута может быть лишь выбрано из списка. В противном случае, пользователь может вводить значение атрибута вручную, а список элементов справочника используется как список значений для быстрого заполнения.

#### bool CreateLink { get; }
Определяет, добавлять ли связь нового объекта с выбранным из справочника элементом. Если свойство имеет значение false, то при выборе элемента из справочника задается значение атрибута объекта, при этом свзяь между объектом и элементом справочника не создается. В противном случае наряду с заданием значения атрибута будет создана связь между объектом и выбранным элементом справочика.

#### string StringFormat { get; }
Строка форматирования, которая должна использоваться для формирования значения атрибута, при выборе элемента справочника.

#### bool AllowMultiSelect { get; }
Флаг, определяет доступен ли мультивыбор в диалоге выбора справочника или нет. 

<a name="RefBookKind"/>
### Перечисление RefBookKind

Указывает тип справочника, то есть один из возможных вариантов заполнения списка возможных значений справочника.

<a name="RefBookKind.Enum"/>
```cpp
RefBookKind.Enum;
```

Справочник, элементами которого является предопределенный набор строковых значений. Смотри свойство *Values* интерфеса *IReferenceBookConfiguration*.

<a name="RefBookKind.Object"/>
```cpp
RefBookKind.Object;
```

Справочник, элементами которого являются дочерние элементы определенного объекта в базе данных. Этот объект определяется идентификатором, значение которого возвращается свойством *Source* интерфеса *IReferenceBookConfiguration*.

```cpp
RefBookKind.OrgUnit;
```
Справочник, который предоставляет выбор элементов оргструктуры базы данных.

```cpp
RefBookKind.Type;
```
Справочник, который предоставляет выбор типов из базы данных.



<a name="RefBookEditorType"/>

### Перечисление RefBookEditorType

Указывает тип редактора, который должен использоваться для редактирования значения атрибута, для которого назначен справочник.

```cpp
RefBookEditorType.Dialog;
```
Редактор атрибута должен представлять собой поле ввода с кнопкой, которая вызывает новый диалог для выбора значений из элементов справочника.

```cpp
RefBookEditorType.ComboBox;
```

В качестве редактора значения атрибута должен использоваться комбобокс, выпадающий список которого состоит из элементов справочника.

<a name="IElementBookConfiguration"/>

### Интерфейс IElementBookConfiguration

Конфигурация атрибута **Справочник элементов**

#### IElementBookDescription Description { get; }

Интерфейс [IElementBookDescription](#IElementBookDescription) с основными свойствами атрибута **Справочник элементов**.

#### IElementBookAutoFill AutoFill { get; }

Интерфейс [IElementBookAutoFill](#IElementBookAutoFill) с описанием автозаполнения атрибута **Справочник элементов**.


<a name="IElementBookDescription"/>
### Интерфейс IElementBookDescription

Контейнер с основными свойствами атрибута **Справочник элементов**.

#### Guid Source { get; }
Идентификатор объекта базы данных, дочерние объкты которого являются элементами справочника.

#### IEnumerable&lt;string&gt; ElementsTypes { get; }
Список имен типов, из объектов которого должны формироваться элементы справочника. Если имен список пуст, то в формировании элементов справочника учавствуют дочерние элементы любого типа.

#### bool AllowMultiSelect { get; }
Определяет, доступен ли мультивыбор в диалоге выбора справочника или нет. 

#### string StringFormat { get; }
Строка форматирования, которая должна использоваться для формирования значения атрибута, при выборе элемента справочника.

#### bool ShowInListView { get; }
Определяет, будет ли показан атрибут в списке элементов.

#### bool EnablePopupCommands { get; }
Определяет, будет ли активен атрибут в списке элементов.

#### bool CreateLink { get; }
Определяет, добавлять ли связь к объекту с выбранным элементом из справочника.

#### bool AutoFill { get; }
Определяет, следует ли заполнять поля атрибутов, совпадающих по имени с указанными в справочнике.

<a name="IElementBookAutoFill"/>
### Интерфейс IElementBookAutoFill

Контейнер с описанием автозаполнения атрибута **Справочник элементов**. Для получения результата автозаполнения используйте интерфейс [IElementBookAutoFillWorker](#IElementBookAutoFillWorker).

#### AutoFillTriggerType TriggerType { get; }
Определяет, на какое событие срабатывает автозаполнение. 

#### AutoFillValueSource ValueSource { get; }
Определяет, откуда будет взято значение для автозаполнения.

#### List&lt;string&gt; ParentTypeNames { get; }
Если в **ValueSource** указано `AutoFillValueSource.Parent`, то значение для заполнения будет взято у первого родителя вложения, соответствующего типу из списка.

<a name="AutoFillValueSource"/>
### Перечисление AutoFillValueSource

#### Self
Определяет, откуда будет взято значение для автозаполнения атрибута **Справочник элементов**

#### Parent
Значением атрибута будет один из родителей вложения (по умолчанию).


<a name="IUserStateConfiguration"/>

### Интерфейс IUserStateConfiguration

Конфигурация состояния.

####Guid StateMachineId{ get; }####
Идентификатор машины состояний, к которой относится атрибут.

<a name="NumeratorExample"/>
#### Пример создания объекта с нумератором ####
Для заполнения атрибута типа нумератор при создании объекта необходимо:

1. Получить метаданные атрибута создаваемого объекта. `IDataObject.Type.Attributes` -> найти по имени (или другому признаку) нужный атрибут;

2. Получить значение поля `IAttribute.Configuration` и распарсить его. Поле должно содержать описание возможных строк формата данного атрибута. Ниже приведен наиболее сложный пример такого описания:
`Входящий документ, ВХ-{Counterc11cbdead2d047399127798d2d1ea07f:d5}-{CurrentDate:yyyy}`
`Исходящий документ, ИС-{Counterc11cbdead2d047399127798d2d1ea07f:d5}-{CurrentDate:yyyy}`

3. Строками разделены различные возможные описания нумератора. Таких строк может быть 1 или более. Каждая строка содержит следующие данные, разделенные между собой знаком `","`:
	- Название описания (может быть пропущено)
	- Строка форматирования описания

4. Из распарсенных данных из пункта 2 выбираем нужное нам описание (если оно одно, то без выбора) и получаем значение его строки форматирования. Например, из вышеприведенного примера мы выберем
`ИС-{Counterc11cbdead2d047399127798d2d1ea07f:d5}-{CurrentDate:yyyy}`

5. Присваиваем значению атрибута создаваемого объекта значение строки форматирования из пункта 3. Описание нумератора также содержит флаг IsDeferredRegistration, который указывает на то, является ли нумертор отложенным. В таком случае, чтобы создать объект со отложенной регистрацией нумератора, нужно присвоить значению атрибута значние строки форматирования из пункта 3. с префиксом отложенной регистрации. Значение этого префикса можно получить используя свойство IAttributeFormatParser.DeferredNumeratorAttributeValuePrefix

6. Создаем объект, после прохождения объекта через сервер атрибут будет заполен согласно выбранной строке форматирования.

Пример:
```cpp
using System.Linq;
using System.Text;
using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.DataObjectWrappers;
using Ascon.Pilot.SDK.Menu;
using NumeratorPlugin.Wrappers;

namespace NumeratorPlugin
{
    [Export(typeof(IMenu<ObjectsViewContext>))]
    public class MainModule : IMenu<ObjectsViewContext>
    {
        private readonly IObjectsRepository _repository;
        private readonly IObjectModifier _modifier;
        private readonly IAttributeFormatParser _attributeFormatParser;
        private const string CREATE_COMMAND = "CreateCommand";

        [ImportingConstructor]
        public MainModule(IObjectsRepository repository, IObjectModifier modifier, IAttributeFormatParser attributeFormatParser)
        {
            _repository = repository;
            _modifier = modifier;
            _attributeFormatParser = attributeFormatParser;
        }

        public void Build(IMenuBuilder builder, ObjectsViewContext context)
        {
            builder.AddItem(CREATE_COMMAND, 0).WithHeader("Create element with numerator");
        }

        public void OnMenuItemClick(string name, ObjectsViewContext context)
        {
            const string typeName = "DocumentTypeWithNumerator";
            const string attributeName = "NumeratorAttribute";

            var type = _repository.GetType(typeName);
            var numberAttribute = type.Attributes.First(att => att.Name == attributeName);
            if (_attributeFormatParser.TryParseNumeratorDeclaration(numberAttribute.Configuration2(), out var numInfos))
            {
                var parent = context.SelectedObjects.FirstOrDefault();
                foreach (var ni in numInfos)
                {
                    
                    var value = ni.IsDeferredRegistration
                        ? $"{_attributeFormatParser.DeferredNumeratorAttributeValuePrefix}{ni.Configuration}"
                        : ni.Configuration;

                    _modifier.Create(parent, type).SetAttribute(attributeName, value);
                }
            }

            _modifier.Apply();
        }
    }
}
```

<a name="ITypeConfigurationParser"/>

### Интерфейс ITypeConfigurationParser

Каждый из пользовательских типов может быть дополнен конфигурацией. Конфигурация представляет собой текстовое поле, с помощью которого могут быть заданы специальные настройки типа.
Для работы с конфигурацией типа можно использовать интерфейс `ITypeConfigurationParser`, который предоставляет методы для десериализации настроек, хранящихся в конфигурации типа.

<a name="ITypeConfigurationParser"/>

### Интерфейс `ITypeConfigurationParser`

Позволяет работать с конфигурайией типов.

Для получения доступа к интерфейсу его необходимо передать в конструктор. При этом конструктор необходимо пометить атрибутом `[ImportingConstructor]`.

<a name="TryParseRemarksConfiguration"/>

##### bool TryParseRemarksConfiguration(string configuration, out IRemarksConfiguration remarksConfiguration);


Метод для получения десериализованного описания видов замечаний из конфигурации типа. Возвращает true, если строка конфигурации содержит описание видов замечаний и его удалось успешно десериализовать; false в обратном случае.

```cs
IRemarksConfiguration remarksConfiguration;
bool success = parser.TryParseRemarksConfiguration(configuration, out remarksConfiguration);
```
где:
 - `success` - результат успешной десериализации;
 - `parser` - экземляр объекта, реализующего интерфейс ITypeConfigurationParser;
 - `configuration` - конфигурация типа;
 - `remarksConfiguration ` - объект описывающий виды замечаний из конфигурации типа;


<a name="IRemarksConfiguration"/>

### Интерфейс IRemarksConfiguration

Конфигурация видов замечаний

```cы
IList<RemarkKind> Kinds { get; }
```
Свойство, которе содержит список видов замечаний у заданного типа замечания. См [RemarkKind](#RemarkKind)

<a name="RemarkKind"/>

### Тип RemarkKind

Тип содержит описание видов замечаний из конфигурации типа.

#### Конструкторы

```cs
public RemarkKind(string colorHex, string kind)
```
где:
 - `colorHex` - цвет замечания в **HEX** (например `#FFFFDABA`);
 - `kind` - вид замечания;

#### Свойства

```cs
public string ColorHex { get; }
```
Свойство возвращает цвет замечания в ** HEX **;

```cs
public string Kind { get; }
```
Свойство возвращает вид замечания.


<a name="ITransitionManager"/>

### Интерфейс ITransitionManager

Интерфейс для работы с атрибутом типа Состояние.

##### IEnumerable<ITransition> GetAvailableTransitions(IAttribute attribute, IDictionary<string, object> attributes, IPerson person)

Метод для получения доступных переходов машины состояний. 

```cpp
IEnumerable<INumeratorInfo> transitions = _transitionManager.GetAvailableTransitions(attribute, attributes, person);
```
где:
 - `transitions ` - список доступных переходов машины состояний;
 - `attribute ` - тип атрибута Состояние;
 - `attributes ` - значение атрибута Состояние;   
 - `person ` - пользователь, для которого будут посчитаны доступные переходы;

##### Guid GetStartingState(IAttribute attribute)

Метод для получения начального положения машины состояний. 

```cpp
Guid stateId = _transitionManager.GetStartingState(attribute);
```
где:
 - `stateId ` - идентификатор состояния;
 - `attribute ` - тип атрибута Состояние;
 
##### bool IsTransitionAvailable(ITransition transition, IDictionary<string, object> attributes, IPerson person)

Вычисляет, доступен ли переход в состояние. 

```cpp
bool result = _transitionManager.IsTransitionAvailable(transition, attributes, person);
```
где:
 - `result ` - результат;
 - `transition ` - объект типа ITransition;
 - `attributes ` - значение атрибута Состояние;   
 - `person ` - пользователь, для которого будут посчитаны доступные переходы;

<a name="IElementBookAutoFillWorker"/>
### Интерфейс IElementBookAutoFillWorker

Интерфейс содержит логику автозаполнения атрибута **Справочник элементов**. Описание настройки автозаполнения – [IElementBookAutoFill](#IElementBookAutoFill).
Для получения доступа к интерфейсу его необходимо добавить как параметр в конструктор класса плагина. При этом конструктор необходимо пометить атрибутом `[ImportingConstructor]`.

#### Task&lt;string[]&gt; ComputeElementBookValueAsync(string configuration, Guid triggerObjectId);

Вычисляет значение атрибута **Справочник элементов** с помощью настройки автозаполнения. 
```cpp
string[] result = await _autoFill.ComputeElementBookValueAsync(configuration, triggerObjectId);
```
где:
 - `configuration ` - конфигурация атрибута;
 - `triggerObjectId ` - идентификатор вложения;   
 
<a name="CommonSettings"/>

## 8. Управление общими настройками

Общие настройки представляют собой персонифицированное хранилище типа "ключ-значение", где ключ - название настройки, а значение - ее значение. Список ключей системных настроек описан в классе [SystemSettingsKeys](#SystemSettingsKeys). Данные хранилища общих настроек синхронизируются с сервером. Значения настроек могут быть записаны как для конкретного пользователя, так и для элемента организационной структуры, причем настройки, записанные для конкретного пользователя, имеют приоритет перед настройками для элемента организационной структуры, в который входит должность, на которой состоит пользователь.

<a name="ISettingsFeature"/>

### Интерфейс ISettingsFeature
Интерфейс ISettingsFeature позволяет зарегистрировать в диалоге общих настроек новый ключ. Класс, реализующий данный интерфейс, должен быть помечен атрибутом [Export(typeof(ISettingsFeature))].

#### string Key { get; }
Ключ настройки.

#### string Title { get; }
Отображаемое имя настройки.

<a name="ISettingsFeatureEditor"/>

#### FrameworkElement Editor { get; }
Визуальный элемент, используемый для редактирования настроек данного типа.

#### void SetValueProvider(ISettingValueProvider settingValueProvider)####
Данный метод вызывается со стороны приложения перед показом [Editor](#ISettingsFeatureEditor) и позволяет получить объект [ISettingValueProvider](#ISettingValueProvider) для редактирования значения настройки.

<a name="IPersonalSettings"/>

### Интерфейс IPersonalSettings
Импортируемый интерфейс, позволяющий получать значение настроек и менять значения настроек текущего пользователя.

#### IObservable<KeyValuePair<string, string>> SubscribeSetting(string key)
Подписка на значение настройки по передаваемому ключу.

#### void ChangeSettingValue(string key, string value)
Изменение значения настройки по ключу.

#### string GetPersonalSettingValue(string key)
Получение значения персональной настройки для текущего пользователя по ключу.

#### IReadOnlyCollection<string> GetCommonSettingValue(string key)
Получение  по ключу списка значений настройки для всех элементов оргструктуры, в которые входит текущий пользователь.

<a name="ISettingValueProvider"/>

### Интерфейс ISettingValueProvider
Позволяет получать и устанавливать значение настройки для диалога общих настроек.

#### string GetValue()
Получение текущего значения настройки.

#### void SetValue(string value)
Установка значения настройки.

<a name="SystemSettingsKeys"/>

### Класс SystemSettingsKeys
Класс SystemSettingsKeys описывает ключи системных общих настроек.

#### string FavoritesFeatureKey { get; }
Список избранного в обозревавателе документов.

#### string PilotStorageDriveLetter { get; }
Предпочтительная буква монтирования диска Pilot-Storage.

#### string AgreementRolesFeatureKey { get; }
Список ролей для согласования.

#### string MountedItemsList { get; }
Список смонтированных проектов.

#### string AutoBlockingFileExtensionsFeatureKey { get; }
Список расширений файлов на Pilot-Storage, для которых будет работать функционал автоблокировки.

#### string DocsAutoFillFeatureKey { get; }
Настройки автозаполнения полей файлов.


<a name="SearchInterfaces"/>

## 9. Поиск

Поисковая система Pilot предоставляет богатый язык запросов. Построение запросов осуществляется с помощью конструкторов запросов (IQueryBuilder). Поиск можно осуществлять как по полям объектов так и по ключевому слову или части слова (фразы). Основными элементами языка запросов являются так называемые "[условия поиска](#MainSearchTerms)".
Чтобы осуществить поиск необходимо:
- получить конструктор поисковых запросов из интерфейса ISearchService.
- построить запрос с помощью полученного конструктора поисковых запросов с использованием "условий поиска".
- выполнить поиск вызвав метод Search и передать конструктор поисковых запросов в качестве параметра.

Конструкторы запросов имеют три ключевых метода для построения поисковых запросов:
 - `Must` - заданное условие обязательно должно выполниться.
 - `MustAnyOf` - должно выполняться одно из заданных условий.
 - `MustNot` - заданное условие не должно выполниться.

Чтобы задать поиск по определенному полю объекта используйте условия поиска для объектов [ObjectFields](#ObjectFields) и конструктор запросов для объектов (см [GetObjectQueryBuilder](#ObjectQueryBuilder))
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.TypeId.BeAnyOf(16,17));
```
Для любых объектов в базе Pilot предусмотрен поиск по ключевым словам. Такой поиск осуществляется с помощью **"условия поиска AllText"**.
Поиск по ключевому слову:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.AllText.Be("Проект*"));
```
Поиск по ключевым словам поддерживает одиночный (?) и многократный (*) подстановочные символы.

Для поиска с использованием одного символа подстановки используется служебный символ `?`. Например, чтобы найти слова "test" и "text" вы можете использовать следующую запись:
```cs
ObjectFields.AllText.Be("te?t")
```
Для поиска с использованием многократного символа подстановки используется служебный символ `*`.  Например, чтобы найти слова "test", "tests" или "tester" вы можете использовать следующую запись:
```cs
ObjectFields.AllText.Be("test*")
```
Также есть возможность использования символа многократной подстановки в середине или в начале слова. Например:
```cs
ObjectFields.AllText.Be("te*t")
```
**ВНИМАНИЕ!**
Не рекомендуется использовать подстановочные символы в начале слова. Это приведет к полному  перебору поисковых индексов, что может привести к сильным задержкам выдачи результата.



<a name="ISearchService"/>

### Интерфейс ISearchService

Интерфейс работы с поисковыми запросами.
Для получения доступа к интерфейсу поиска его необходимо добавить как параметр в конструктор класса плагина. При этом конструктор необходимо пометить атрибутом `[ImportingConstructor]`.

Чтобы осуществить поиск объектов необходимо предварительно составить поисковый запрос с помощью выбранного конструктора запроса. После задания условий в конструкторе запросов с помощью [условий поиска](#MainSearchTerms), его необходимо передать как параметр в метод [Search](#Search). Существует четыре типа конструкторов поисковых запросов:
- конструктор запросов для объектов "Обозревателя документов" (см [GetObjectQueryBuilder](#ObjectQueryBuilder));
- конструктор запросов для заданий (см [GetTaskQueryBuilder](#TaskQueryBuilder));
- универсальный конструктор запросов (см [GetEmptyQueryBuilder](#EmptyQueryBuilder));
- конструктор поисковой строки для "умной папки" (см [GetSmartFolderQueryBuilder](#SmartFolderQueryBuilder))

<a name="Search"/>

#### IObservable< ISearchResult> Search(IQueryBuilder query)

С помощью этого метода осуществляется поиск объектов по заданным критериям в конструкторе запросов. Результатом вызова этого метода является поставщик push-уведомлений результатов поиска cм. [ISearchResult](#ISearchResult).

```cpp
IObservable<ISearchResult> Search(IQueryBuilder query);
```

<a name="RunRefreshableSearch"/>

#### IObservable< ISearchResult> RunRefreshableSearch(IQueryBuilder query)

Метод запускает обновляемый поиск. По обновляемому поиску будут приходить нотификации каждый раз, когда будет изменяться результат поисковой выборки. Результатом вызова этого метода является поставщик push-уведомлений результатов поиска cм. [ISearchResult](#ISearchResult).

**ВНИМАНИЕ!**
Обновляемые поисковые запросы создают дополнительную нагрузку на сервер, должны использоваться только при необходимости и в случае потери интереса к результатам обновляемого поиска требуется выполнить отписку через Dispose Observable коллекции.

```cpp
IObservable<ISearchResult> RunRefreshableSearch(IQueryBuilder query);
```

<a name="ObjectQueryBuilder"/>
#### IQueryBuilder GetObjectQueryBuilder() ####

Метод получения предварительно сконфигурированного конструктора запросов для пользовательских объектов.
В результат поисковой выдачи не попадают:
  - объекты системных и сервисных типов;
  - удаленные объекты.

```cpp
IQueryBuilder ISearchService.GetObjectQueryBuilder();
```

<a name="EmptyQueryBuilder"/>

#### IQueryBuilder GetEmptyQueryBuilder()

Метод получения простого конструктора запросов. Это универсальный конструктор запросов без предварительных условий. Этот конструктор можно использовать для поиска любых объектов в базе, включая объекты системных типов, например файл или папка на Pilot-Storage.

```cpp
IQueryBuilder ISearchService.GetEmptyQueryBuilder();
```

<a name="SmartFolderQueryBuilder"/>

##### ISearchService.GetSmartFolderQueryBuilder

Метод получения конструктора строки запроса для `умной папки`.


```cpp
ISmartFolderQueryBuilder ISearchService.GetSmartFolderQueryBuilder();
```

<a name="AnnotationQueryBuilder"/>

##### ISearchService.GetAnnotationQueryBuilder

Метод получения конструктора строки запроса для поиска по замечаниям.


```cpp
ISmartFolderQueryBuilder ISearchService.GetSmartFolderQueryBuilder();
```

<a name="ISmartFolderQueryBuilder"/>

### Интерфейс ISmartFolderQueryBuilder

Интерфейс создания поискового запроса `умной папки`.


##### ISmartFolderQueryBuilder.WithSearchMode

Задает режим поиска (по атрибутам или по файлам)

```cpp
ISmartFolderQueryBuilder WithSearchMode(SearchMode searchMode);
```
где:
 - `searchMode` - режим поиска.


##### ISmartFolderQueryBuilder.WithKeyword

Задает ключевое слово для поиска

```cpp
ISmartFolderQueryBuilder WithKeyword(string keyword);
```
где:
 - `keyword` - ключевое слово.


##### ISmartFolderQueryBuilder.WithQuotedKeyword

Задает ключевое слово для точного поиска.

```cpp
ISmartFolderQueryBuilder WithQuotedKeyword(string keyword);
```
где:
 - `keyword` - ключевое слово.


##### ISmartFolderQueryBuilder.WithType

Задает тип элемента для поиска.

```cpp
ISmartFolderQueryBuilder WithType(int typeId);
```
где:
 - `typeId` - идентификатор типа.

##### ISmartFolderQueryBuilder.WithTypes

Задает список типов элементов для поиска.

```cpp
ISmartFolderQueryBuilder WithTypes(IEnumerable<int> typeIds);
```
где:
 - `typeIds` - список идентификаторов типов.

##### ISmartFolderQueryBuilder.WithState

Задает состояние элементов для поиска.

```cpp
ISmartFolderQueryBuilder WithState(ObjectState state);
```
где:
 - `state` - состояние объекта.

##### ISmartFolderQueryBuilder.WithAuthor

Задает автора (создателя) элемента для поиска.

```cpp
ISmartFolderQueryBuilder WithAuthor(int authorId);
```
где:
 - `authorId` - идентификатор пользователя.



##### ISmartFolderQueryBuilder.WithAuthors

Задает авторов (создателей) элементов для поиска.

```cpp
ISmartFolderQueryBuilder WithAuthors(IEnumerable<int> authorIds);
```
где:
 - `authorIds` - список идентификаторов пользователя.


##### ISmartFolderQueryBuilder.WithCreatedToday

Задает время создания элементов "сегодня" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedToday();
```

##### ISmartFolderQueryBuilder.WithCreatedYesterday 

Задает время создания элементов "вчера" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedYesterday();
```

##### ISmartFolderQueryBuilder.WithCreatedThisWeek 

Задает время создания элементов "на этой неделе" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedThisWeek();
```

##### ISmartFolderQueryBuilder.WithCreatedLastWeek 

Задает время создания элементов "на прошлой неделе" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedLastWeek();
```

##### ISmartFolderQueryBuilder.WithCreatedThisMonth 

Задает время создания элементов "в этом месяце" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedThisMonth();
```


##### ISmartFolderQueryBuilder.WithCreatedLastMonth 

Задает время создания элементов "в прошлом месяце" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedLastMonth();
```


##### ISmartFolderQueryBuilder.WithCreatedThisYear 

Задает время создания элементов "в этом году" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedLastYear();
```


##### ISmartFolderQueryBuilder.WithCreatedLastYear 

Задает время создания элементов "в прошлом году" для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedThisYear();
```


##### ISmartFolderQueryBuilder.WithCreatedInRange 

Задает диапазон дат создания элементов для поиска.

```cpp
ISmartFolderQueryBuilder WithCreatedInRange(DateTime fromUtc, DateTime toUtc);
```
где:
 - `fromUtc` - начальная дата в UTC.
 - `toUtc` - конечная дата  в UTC.


##### ISmartFolderQueryBuilder.ToString 

Преобразует поисковый запрос в строку для последующего сохранения ее в соответствующем атрибуте `умной папки`

```cpp
string ToString();
```
<a name="SmartFolderCreationExample"/>

##### Пример создания "умной папки" 

```cpp
using Ascon.Pilot.SDK;

namespace SmartFolderSample
{
    [Export(typeof(IDataPlugin))]
    public class SmartFolderSample : IDataPlugin
    {
        private readonly ISearchService _searchService;
        private readonly IObjectsRepository _repository;
        private readonly IObjectModifier _modifier;

        [ImportingConstructor]
        public SmartFolderSample(ISearchService searchService, IObjectsRepository repository, IObjectModifier modifier)
        {
            _searchService = searchService;
            _repository = repository;
            _modifier = modifier;
        }

        public void CreateSmartFolder(IDataObject parent)
        {
            _modifier.Create(parent, GetSmartFolderType())
                     .SetAttribute(SystemAttributeNames.SMART_FOLDER_TITLE, "title")
                     .SetAttribute(SystemAttributeNames.SEARCH_CRITERIA, GetSmartFolderSearchString())
                     .SetAttribute(SystemAttributeNames.SEARCH_CONTEXT_OBJECT_ID, parent.Id.ToString());
            _modifier.Apply();
        }

        private string GetSmartFolderSearchString()
        {
            var searchString = string.Empty;
            var type = _repository.GetType("Project");
            if (type == null)
                return searchString;

            var builder = _searchService.GetSmartFolderQueryBuilder();
            builder.WithSearchMode(SearchMode.Attributes)
                   .WithType(type.Id)
                   .WithKeyword("5000").ToString();
            searchString = builder.ToString();
            return searchString;
        }

        private IType GetSmartFolderType()
        {
            return _repository.GetType(SystemTypeNames.SMART_FOLDER);
        }
    }
}

```

<a name="IAnnotationQueryBuilder"/>

### Интерфейс IAnnotationQueryBuilder

Интерфейс создания поискового запроса по замечаниям.

##### IAnnotationQueryBuilder.WithKeyword

Задает ключевое слово для поиска

```cpp
IAnnotationQueryBuilder WithKeyword(string keyword);
```
где:
 - `keyword` - ключевое слово.


##### IAnnotationQueryBuilder.WithQuotedKeyword

Задает ключевое слово для точного поиска.

```cpp
IAnnotationQueryBuilder WithQuotedKeyword(string keyword);
```
где:
 - `keyword` - ключевое слово.


##### IAnnotationQueryBuilder.WithType

Задает тип элемента для поиска.

```cpp
IAnnotationQueryBuilder WithType(int typeId);
```
где:
 - `typeId` - идентификатор типа.

##### IAnnotationQueryBuilder.WithTypes

Задает список типов элементов для поиска.

```cpp
IAnnotationQueryBuilder WithTypes(IEnumerable<int> typeIds);
```
где:
 - `typeIds` - список идентификаторов типов.
 
##### IAnnotationQueryBuilder.WithCurrentVersion

Задаёт поиск замечаний только по текущий версии документа (по умолчанию - поиск по всем версиям).

```cpp
IAnnotationQueryBuilder WithCurrentVersion();
```

<a name="IQueryBuilder"/>

### Интерфейс IQueryBuilder

Универсальный интерфейс конструктора поисковых запросов.


##### IQueryBuilder.Must

Используйте этот метод для [условий поиска](#MainSearchTerms), которые ДОЛЖНЫ быть в объектах.

```cpp
IQueryBuilder Must(ISearchTerm term);
```
где:
   - `term` - условие поиска. (например: ObjectFields.AllText.Be("Hello*"))


##### IQueryBuilder.MustAnyOf 

Используйте этот метод, если какое-либо из указанных [условий поиска](#MainSearchTerms) ДОЛЖНО быть в объектах.

```cpp
IQueryBuilder MustAnyOf(params ISearchTerm[] terms);
```
где:
   - `terms` - условия поиска. (например: ObjectFields.TypeId.BeAnyOf(new[] {1, 2, 3})).

##### IQueryBuilder.MustNot 

Используйте этот метод, если [условие поиска](#MainSearchTerms) НЕ ДОЛЖНО быть в объектах.

```cpp
IQueryBuilder MustNot(ISearchTerm term);
```
где:
   - `terms` - условие поиска. (например: TaskFields.State.Be(State.None)).


##### IQueryBuilder.SortBy 

Используйте этот метод для того, чтобы установить поле и направление сортировки результатов поиска.

```cpp
IQueryBuilder SortBy(INamedField field, ListSortDirection direction);
```
где:
   - `field` - поле, по которому необходимо отсортировать результаты поиска (Например: ObjectFields.TypeId).
   - `direction` - направление сортировки.


<a name="IQueryBuilderMaxResults"/>

##### IQueryBuilder.MaxResults

Используйте этот метод для того, чтобы ограничить количество результатов. Значение по умолчанию 250.

```cpp
IQueryBuilder MaxResults(int result);
```
где:
   - `result` - максимальное количество результатов поиска.


##### IQueryBuilder.InContext 

Используйте этот метод для того, чтобы установить контекст поиска.

```cpp
IQueryBuilder InContext(Guid id);
```
где:
   - `id` - идентификатор объекта.


<a name="ISearchResult"/>

### Интерфейс ISearchResult 

Интерфейс предоставляет доступ к результатам поиска.

##### ISearchResult.Result 

Список идентификаторов найденных объектов.

```cpp
IEnumerable<Guid> Result { get; }
```

##### ISearchResult.Total 

Общее количество найденных объектов. Так как поисковая выдача ограничена с помощью [IQueryBuilder.MaxResults](#IQueryBuilderMaxResults), в общем случае значение Total может быть больше количества результатов Result. Если требуется найти больше объектов, установите большее значение [IQueryBuilder.MaxResults](#IQueryBuilderMaxResults) и перезапустите поиск.

```cpp
long Total { get; }
```

##### ISearchResult.Kind 

Источник результата поиска.

```cpp
SearchResultKind Kind { get; }
```

##### Перечисление SearchResultKind 

Источник результата поиска.

Результаты были получены из локального хранилища клиента.
```cpp
SearchResultKind.Local
```
Результаты были получены с удаленного сервера.
```cpp
SearchResultKind.Remote
```

<a name="MainSearchTerms"/>

### Предустановленные условия поиска 

<a name="ObjectFields"/>

### Условия поиска для объектов ObjectFields 

##### ObjectFields.Id 

Используйте это условие, чтобы задать поиск по определенному идентификатору объекта.
Например:
```cs
var id  = new Guid("01A1028B-79BC-45C7-8A59-6191E8ADFE39");
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.Id.Be(id));
```

##### ObjectFields.ParentId 

Используйте это условие, чтобы задать поиск по определенному идентификатору родительского объекта.
Например:
```cs
var id  = new Guid("01A1028B-79BC-45C7-8A59-6191E8ADFE39");
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.ParentId.Be(id));
```

##### ObjectFields.TypeId 

Используйте это условие, чтобы задать поиск по типу объекта.
Например: зададим условие поиска для объектов типа 16 ИЛИ 17
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.TypeId.BeAnyOf(16,17));
```

##### ObjectFields.ObjectState 

Используйте это условие, чтобы задать поиск по состоянию объекта.
Например: зададим условие поиска для объектов в состоянии заморозка
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.ObjectState.BeAnyOf(ObjectState.Frozen));
```

Дргугой пример, поиск объектов, находящихся в корзине:
```cs
var builder = _searchService.GetEmptyQueryBuilder();
builder.Must(ObjectFields.ObjectState.BeAnyOf(ObjectState.InRecycleBin));
```

Или наоборот: фильтрация объектов, помещенных в корзину, которые не должны отображаться в результатах поиска:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.ObjectState.BeAnyOf(ObjectState.Frozen, ObjectState.Alive));
```

Дргугой пример: поиск заблокированных объектов
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.ObjectState.BeAnyOf(ObjectState.LockRequested, ObjectState.LockAccepted));
```

##### ObjectFields.CreatorId

Используйте это условие, чтобы задать поиск по создателю объекта.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.CreatorId.Be(25));
```

##### ObjectFields.CreatedDate

Используйте это условие, чтобы задать поиск по дате создания объектов. Дату и время необходимо задавать в формате UTC.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
var fromDate = new DateTime(2016, 5, 5).ToUniversalTime();
var toDate = DateTime.Today.ToUniversalTime();
builder.Must(ObjectFields.CreatedDate.BeInRange(fromDate, toDate));
```

##### ObjectFields.IsSecret

Используйте это условие, чтобы искать скрытые или общедоступные объекты.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.IsSecret.Be(true));
```

##### ObjectFields.AllText

Используйте это условие, чтобы искать объекты по слову или части слова. Поиск осуществляется по атрибутам объекта.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.AllText.Be("Проект*"));
```
##### ObjectFields.SnapshotsCreated

Используйте это условие, чтобы искать объекты по дате создания версии файла (например, XPS документа или файла на Pilot-Storage).
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.SnapshotsCreated.BeInRange(from,to));
```

##### ObjectFields.AllSnapshotsReason

Используйте это условие, чтобы искать документы по причине создания версии (как актуальной, так и любой из предыдущих).
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.AllSnapshotsReason.Be("Обновлен*"));
```

##### ObjectFields.SignatureAwaitingBy

Используйте это условие, чтобы искать документы, ожидающие подписания от одной или нескольких должностей.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.SignatureAwaitingBy.Be(gipPositionId));
```

##### ObjectFields.SignedBy

Используйте это условие, чтобы искать документы, подписанные каким-либо пользователем.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.SignedBy.Be(gipPositionId));
```

##### ObjectFields.StateChangedPersonId

Используйте это условие, чтобы искать объекты, заблокированные каким-либо пользователем.
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(ObjectFields.ObjectState.BeAnyOf(ObjectState.LockRequested, ObjectState.LockAccepted));
builder.Must(ObjectFields.StateChangedPersonId.Be(gipPersonId));
```

<a name="AttributeFields"/>

### Условия поиска по произвольным атрибутам AttributeFields

##### AttributeFields.String 

Используйте это условие, чтобы искать объекты по атрибуту типа строка.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(AttributeFields.String("AttributeName").Be("Искать*"));
```

##### AttributeFields.DateTime 

Используйте это условие, чтобы искать объекты по атрибуту типа дата. Дату и время необходимо задавать в формате UTC.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
var fromDate = new DateTime(2016, 5, 5).ToUniversalTime();
var toDate = DateTime.Today.ToUniversalTime();
builder.Must(AttributeFields.DateTime("AttributeName").BeInRange(fromDate, toDate));
```

##### AttributeFields.Double 

Используйте это условие, чтобы искать объекты по атрибуту типа вещественное число.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(AttributeFields.Double("AttributeName").Be(2.2));
```

##### AttributeFields.Integer 

Используйте это условие, чтобы искать объекты по атрибуту типа целое число.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(AttributeFields.Integer("AttributeName").Be(2));
```

##### AttributeFields.Bool 

Используйте это условие, чтобы искать объекты по атрибуту типа булевое значение.
Например:
```cs
var builder = _searchService.GetObjectQueryBuilder();
builder.Must(AttributeFields.Bool("AttributeName").Be(true));
```


<a name="AllTerms"/>

### Общие условия поиска

##### Условие Be 

Используйте это условие для точного поиска значения.
```cs
ISearchTerm Be(T value);
```
Например:
```cs
var builder = _searchService.GetTaskQueryBuilder();
builder.Must(TaskFields.ExecutorPositionId.Be(42));
```

##### Условие BeAnyOf 

Поиск значений удовлетворяющих одному из условий.
```cs
ISearchTerm BeAnyOf(params T[] values);
```
Например:
```cs
var builder = _searchService.GetTaskQueryBuilder();
builder.Must(TaskFields.ExecutorPositionId.BeAnyOf(45,42,15));
```

##### Условие BeInRange 

Поиск значений находящихся в заданном диапазоне значений.
```cs
ISearchTerm BeInRange(T from, T to);
```
Например:
```cs
var builder = _searchService.GetTaskQueryBuilder();
var fromDate = new DateTime(2016, 5, 5).ToUniversalTime();
var toDate = DateTime.Today.ToUniversalTime();
builder.Must(TaskFields.DateOfCompletion.BeInRange(fromDate, toDate));
```

##### Условие ContainsAll 

Поиск значений соответствующих всем заданным условиям.
```cs
ISearchTerm ContainsAll(params T[] values);
```
Например:
```cs
var builder = _searchService.GetTaskQueryBuilder();
builder.Must(TaskFields.AllText.ContainsAll("задание", "согласование"));
```

<a name="ObjectSearchExample"/>

##### Пример создания поискового запроса 

```cpp
using Ascon.Pilot.SDK;

namespace SearchSample
{
    [Export(typeof(IDataPlugin))]
    public class SearchSamplePlugin : IDataPlugin, IObserver<ISearchResult>
    {
        private readonly IObjectsRepository _repository;
        private long _total;

        [ImportingConstructor]
        public SearchSamplePlugin(ISearchService searchService, IObjectsRepository repository)
        {
            _repository = repository;
            var currentPerson = repository.GetCurrentPerson();
            var fromDate = new DateTime(2016,1,1).ToUniversalTime();
            var to = new DateTime(2016,12,31).ToUniversalTime();
            //Построим запрос на поиск объектов:
            // - созданные текущим пользователем
            // - объекты только типа "Project"
            // - созданные в 2016 году
            // - сортировать результаты по дате создания. По возрастанию.
            // - вывести только 50 результатов
            var qBuilder = searchService.GetObjectQueryBuilder();
            qBuilder.Must(ObjectFields.CreatorId.Be(currentPerson.Id))
                    .Must(ObjectFields.TypeId.Be(GetProjectTypeId()))
                    .Must(ObjectFields.CreatedDate.BeInRange(fromDate, to))
                    .SortBy(ObjectFields.CreatedDate, ListSortDirection.Ascending)
                    .MaxResults(50);

            searchService.Search(qBuilder).Subscribe(this);
        }

        public void OnNext(ISearchResult value)
        {
            var items = value.Result;
            _total = value.Total;
            _repository.SubscribeObjects(items).Subscribe(SomeObjectsLoader);
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }

        private int GetProjectTypeId()
        {
            //Сработает только если в конфигурации базы данных присутствует тип с именем Project
            var type = _repository.GetType("Project");
            if (type == null)
                throw new Exception("Type Project not found");

            return type.Id;
        }
    }
}

```



<a name="Commands"/>

## 10. Команды расширений (устаревший, использовать IPilotServiceProvider)

В клиенте Pilot предусмотрена возможность взаимодействия расширений между собой. Т.е. любое расширение может вызвать команду другого расширения и использовать результат работы вызванной команды. Для того, чтобы разработчики смогли воспользоваться командой мы рекомендуем добавить описание входящих и исходящих параметров. Как сериализовать и десериализовать параметры и результат работы команды.

<a name="ICommandHandler"/>

### ICommandHandler

Интерфейс предназначен для реализации обработчика команды в расширении

#### Свойства 

```cs
Guid CommandId { get; }
```
Свойство возвращает уникальный идентификатор команды.

```cs
string Description { get; }
```
Свойство возвращает описание команды.
Чтобы разработчики сторонних расширений смогли воспользоваться вашими командами в  описании команды необходимо описать формат входящих параметров и формат результата работы команды.

#### Методы 

```cs
byte[] Handle (byte[] args);
```
где:
 - `args` - параметры команды

Метод выполняет команду.


<a name="ICommandInvoker"/>

### ICommandInvoker 

Интерфейс предназначен для получения информации о доступных командах в системе и вызове их.
Для получения доступа к интерфейсу вызова команд его необходимо добавить как параметр в конструктор класса плагина. При этом конструктор необходимо пометить атрибутом `[ImportingConstructor]`.

#### Методы 

```cs
IEnumerable<CommandDescription> GetAvailableCommands();
```
Метод возвращает [описания](#CommandDescription) доступных команд в системе.

```cs
byte[] Invoke (Guid commandId, byte[] args);
```
где:
 - `commandId` - уникальный идентификатор команды
 - `args` - параметры вызова команды.

Метод позволяет вызвать команду из другого расширения. В случае отсутствия команды будет выброшено исключение типа `CommandHandlerNotFoundException`

Описание и результат выполнения команды смотрите спрашивайте у разработчиков расширений. Мы рекомендуем описывать команды и параметры в свойстве Description обработчика команды.


<a name="CommandDescription"/>

### CommandDescription 

Тип описания команды расширения.

#### Конструкторы 

```cs
public CommandDescription(Guid commandId, string description)
```
где:
 - `commandId` - уникальный идентификатор команды
 - `description` - подробное описание команды команды.


#### Свойства 

```cs
public Guid CommandId { get; }
```
Свойство получает уникальный идентификатор команды

```cs
public string Description { get; }
```
Свойство получает подробное описание команды.


<a name="DataTypesInterfaces"/>

## 11. Типы данных Pilot 

К данным интерфейсам относятся объекты, которые содержат информацию об элементах, пользователях, заданиях и т.п.

<a name="IDataObject"/>

### Интерфейс IDataObject 

Интерфейс предназначен для просмотра информации об элементе.


##### IDataObject.Id 

Поле `Id` содержит идентификатор элемента.
```cpp
Guid IDataObject.Id { get; }
```


##### IDataObject.ParentId 

Поле `ParentId` содержит идентификатор родительского элемента.

```cpp
Guid IDataObject.ParentIdId { get; }
```


##### IDataObject.Created 

Поле `Created` содержит дату создания элемента.

```cpp
DateTime IDataObject.Created { get; }
```


##### IDataObject.Attributes 

Поле `Attributes` содержит словарь атрибутов элемента. Где значением атрибута могут быть следующие типы: `string`, `int`, `DateTime` и `double`.

```cpp
IDictionary<string, object> IDataObject.Attributes { get; }
```


##### IDataObject.DisplayName 

Поле `DisplayName` содержит отображаемое имя элемента, собранное из доступных атрибутов по определенному правилу.

```cpp
string IDataObject.DisplayName { get; }
```


##### IDataObject.Type 

Поле `Type` содержит тип элемента.

```cpp
IType IDataObject.Type { get; }
```


##### IDataObject.Creator 

Поле `Creator` содержит информацию о пользователе, создавшем текущий элемент.

```cpp
IPerson IDataObject.Creator { get; }
```


##### IDataObject.Children 

Поле `Children` содержит список идентификаторов дочерних элементов.

```cpp
ReadOnlyCollection<Guid> IDataObject.Children { get; }
```

##### IDataObject.Relations 

Поле `Relations` содержит список всех связей элемента с другими элементами.

```cpp
ReadOnlyCollection<IRelation> IDataObject.Relations { get; }
```

##### IDataObject.RelatedSourceFiles 

Поле `RelatedSourceFiles` содержит список идентификаторов элементов, связанных с текущим связью типа "Исходный файл"

```cpp
ReadOnlyCollection<Guid> IDataObject.RelatedSourceFiles { get; }
```

##### IDataObject.RelatedTaskInitiatorAttachments 

Поле `RelatedTaskInitiatorAttachments` содержит список идентификаторов элементов, связанных с текущим связью типа "Вложение в задание от инициатора"

```cpp
ReadOnlyCollection<Guid> IDataObject.RelatedTaskInitiatorAttachments { get; }
```

##### IDataObject.RelatedTaskMessageAttachments 

Поле `RelatedTaskMessageAttachments` содержит список идентификаторов элементов, связанных с текущим связью типа "Вложение в сообщение по заданию"

```cpp
ReadOnlyCollection<Guid> IDataObject.RelatedTaskMessageAttachments { get; }
```

##### IDataObject.TypesByChildren 

С помощью поле `TypesByChildren` можно определить тип ребенка элемента не загружая его.

```cpp
IDictionary<Guid, int> IDataObject.TypesByChildren { get; }
```

##### IDataObject.State 

Поле `State` содержит информацию о текущем статусе элемента. Загружен элемент в кеш клиента или нет. Подробнее см. [DataState](#DataState)

```cpp
DataState IDataObject.State { get; }
```

##### IDataObject.ObjectStateInfo 

Поле `ObjectStateInfo` содержит информацию о текущем статусе [ObjectState](#ObjectState) объекта и информацию о том, кто и когда совершил последнее изменения статуса. Подробнее см. [DataState](#DataState).

```cpp
DataState IDataObject.State { get; }
```

##### IDataObject.SynchronizationState 

Поле `SynchronizationState` содержит информацию о текущем статусе синхронизации элемента с сервером. Подробнее см. [SynchronizationState](#SynchronizationState)

```cpp
SynchronizationState IDataObject.SynchronizationState { get; }
```

##### IDataObject.Files 

Поле `Files` содержит список доступных файлов элемента. Содержит только информацию о файле. Тело файла необходимо загружать отдельно. Подробнее см. [IFileProvider](#IFileProvider)

```cpp
ReadOnlyCollection<IFile> IDataObject.Files { get; }
```

##### IDataObject.Access2 

Поле `Access2` содержит информацию о текущих правах доступа к этому элементу. Подробнее см [IAccessRecord](#IAccessRecord).

```cpp
ReadOnlyCollection<IAccessRecord> IDataObject.Access2 { get; }
```

##### IDataObject.IsSecret 

Поле `IsSecret` содержит значение указывающее является ли этот элемент скрытым.

```cpp
 bool IDataObject.IsSecret { get; }
```

##### IDataObject.ActualFileSnapshot 

Поле `ActualFileSnapshot` содержит информацию о текущих фалах элемента. Подробнее см. [IFilesSnapshot](#IFilesSnapshot).

```cpp
 IFilesSnapshot IDataObject.ActualFileSnapshot { get; }
```

##### IDataObject.PreviousFileSnapshots 

Поле `PreviousFileSnapshots` содержит информацию об изменениях файлов элемента. Это история изменения файлов элемента. Подробнее см. [IFilesSnapshot](#IFilesSnapshot).

```cpp
 ReadOnlyCollection<IFilesSnapshot> IDataObject.PreviousFileSnapshots { get; }
```

##### IDataObject.Subscribers 

Поле `Subscribers` содержит список пользователей, которые подписаны на изменения этого элемента.

```cpp
 ReadOnlyCollection<int> IDataObject.Subscribers { get; }
```

##### IDataObject.LastChange 

Метод расширения `LastChange` позволяет получить инкрементальный идентификатор набора изменений, последним изменившего данный элемент. Может быть использован для ожидания загрузки новой версии элемента при получении нотификации (см. [INotification.ChangesetId](#INotificationChangesetId)).

```cpp
 public static long LastChange(this IDataObject dataObject);
```



<a name="IType"/>

### Интерфейс IType 

Интерфейс предназначен для просмотра информации о типе элемента.


##### IType.Id 

Поле `Id` содержит идентификатор типа элемента.
```cpp
Guid IType.Id { get; }
```


##### IType.Name 

Поле `Name` содержит внутренне имя типа элемента.
```cpp
string IType.Name { get; }
```


##### IType.Title 

Поле `Title` содержит локализованное имя типа элемента.
```cpp
string IType.Title { get; }
```


##### IType.Sort 

Поле `Sort` содержит порядковый номер сортировки.
```cpp
int IType.Sort { get; }
```


##### IType.HasFiles 

Поле `HasFiles` показывает может ли данный тип содержать файлы.
```cpp
bool IType.HasFiles { get; }
```


##### IType.Children 

Поле `Children` содержит идентификаторы дочерних типов.
```cpp
ReadOnlyCollection<int> IType.Children { get; }
```


##### IType.Attributes 

Поле `Attributes` содержит информацию об атрибутах, которые может содержать элемент данного типа.
```cpp
ReadOnlyCollection<IAttribute> IType.Attributes { get; }
```


##### IType.DisplayAttributes 

Поле `DisplayAttributes` содержит информацию об отображаемых в *Обозревателе проектов* атрибутах.
```cpp
ReadOnlyCollection<IAttribute> IType.DisplayAttributes { get; }
```


##### IType.SvgIcon 

Поле `SvgIcon` содержит массив байт иконки типа в формате svg.
```cpp
byte[] IType.SvgIcon { get; }
```


##### IType.IsMountable

Флаг `IsMountable` указывает может ли элемент данного типа монтироваться на диск (Pilot - Storage).
```cpp
bool IType.IsMountable { get; }
```


##### IType.Kind

Поле `Kind` содержит вид типа элемента.
```cpp
TypeKind IType.Kind { get; }
```


##### IType.IsDeleted

Поле `IsDeleted` указывает удален ли текущий тип. В системе **Pilot** типы не удаляются из базы данных, а только помечаются как удаленные.
```cpp
bool IType.IsDeleted { get; }
```


##### IType.IsService 

Поле `IsService` указывает является ли текущий тип сервисным. Если данное свойство установлено в `true`, то элементы такого типа не будут отображаться в *Обозревателе проектов*. Доступ к элементам такого типа можно получить только из кода.
```cpp
bool IType.IsService { get; }
```

<a name="IAttribute"/>

### Интерфейс IAttribute 

Интерфейс предназначен для просмотра информации об атрибутах типа элемента.


##### IAttribute.Name

Поле `Name` содержит внутреннее название атрибута типа.
```cpp
string IAttribute.Name { get; }
```


##### IAttribute.Title 

Поле `Title` содержит локализованное название атрибута типа.
```cpp
string IAttribute.Title { get; }
```


##### IAttribute.IsObligatory 

Поле `IsObligatory` показывает является ли данный атрибут обязательным для заполнения при создании нового элемента.
```cpp
bool IAttribute.IsObligatory { get; }
```

##### IAttribute.DisplaySortOrder 

Поле `DisplaySortOrder` содержит порядковый номер сортировки атрибута типа.
```cpp
int IAttribute.DisplaySortOrder { get; }
```


##### IAttribute.ShowInObjectsExplorer 

Поле `ShowInObjectsExplorer` показывает будет ли данный атрибут виден в *Обозревателе проектов*.
```cpp
bool IAttribute.ShowInObjectsExplorer { get; }
```

##### IAttribute.JoinWithPrevious 

Флаг `JoinWithPrevious` управляет отображением атрибута в карточке объекта. При наличии свободного места атрибут с установленным флагом `JoinWithPrevious` будет объединен с предыдущим в одну строку.
```cpp
bool IAttribute.JoinWithPrevious { get; }
```


##### IAttribute.IsService 

Поле `IsService` показывает является ли данный атрибут сервисным.
```cpp
bool IAttribute.IsService { get; }
```


##### IAttribute.Configuration 

Поле `Configuration` содержит дополнительную информацию типа. Например этот атрибут используется для описания меток штрих-кода или описания справочников.
```cpp
string IAttribute.Configuration { get; }
```


##### IAttribute.DisplayHeight 

Поле `DisplayHeight` содержит высоту атрибута, которую занимает атрибут в карточке редактирования и просмотра свойств элемента в *Обозревателе проектов*. По умолчанию атрибуты занимают одну строку.
```cpp
int IAttribute.DisplayHeight { get; }
```

##### IAttribute.Obligatory

Поле `Obligatory` содержит значение показывающее, является ли атрибут обязательным к заполнению при создании.
```cpp
bool Obligatory { get; }
```

##### IAttribute.GroupTitle 

Поле `GroupTitle` содержит название группы, в которую будет сгруппирован данный атрибут при отображении в карточке объекта.
```cpp
string IAttribute.GroupTitle { get; }
```

<a name="IFile"/>

### Интерфейс IFile 

Интерфейс предназначен для просмотра информации о файлах, которые содержит элемент.

##### IFile.Id 

Поле `Id` содержит идентификатор файла
```cpp
Guid IFile.Id { get; }
```

##### IFile.Size 

Поле `Size` содержит информацию о размере файла.
```cpp
long IFile.Size { get; }
```


##### IFile.Md5

Поле `Md5` содержит хеш-код файла, вычисленный по алгоритму хеширования md5
```cpp
string IFile.Md5 { get; }
```


##### IFile.Name

Поле `Name` содержит имя файла с расширением.
```cpp
string IFile.Name { get; }
```


##### IFile.Modified 

Поле `Modified` содержит время последнего изменения файла.
```cpp
DateTime IFile.Modified { get; }
```


##### IFile.Created 

Поле `Created` содержит время создания файла.
```cpp
DateTime IFile.Created { get; }
```


##### IFile.Accessed 

Поле `Accessed` содержит время последнего доступа к файлу.
```cpp
DateTime IFile.Accessed { get; }
```


##### IFile.Signatures 

Поле `Signatures` содержит список цифровых подписей, поставленных на файл.
```cpp
ReadOnlyCollection<ISignatureRequest> IFile.SignaturesRequests { get; }
```


<a name="ISignatureRequest"/>

### ISignatureRequest

Интерфейс предназначен для просмотра информации о запросах на подпись и цифровых подписях, поставленных на файл.

#### Свойства

```cs
Guid Id { get; }
```
Свойство содержит идентификатор цифровой подписи.

```cs
Guid ObjectId { get; }
```
Свойство содержит идентификатор задания на согласование.

```cs
Guid DatabaseId { get; }
```
Свойство содержит идентификатор базы данных, к которой привязана должность пользователя.

```cs
int PositionId { get; }
```
Свойство содержит идентификатор должности пользователя, который подписал или должен подписать документ.

```cs
string Role { get; }
```
Свойство содержит информацию о роли пользователя, который подписал или должен подписать документ.

```cs
IReadOnlyList<string> Signs { get; }
```
Свойство содержит список подписей, связанных с запросом.

```cs
string RequestedSigner { get; }
```
Свойство содержит информацию о должности пользователя, который должен подписать документ.


#### Методы расширения

```cs
public static bool IsAdditional(this ISignatureRequest signature)
```
Метод возвращает значение, указывающее виртуальный запрос на подпись или нет.


<a name="IOrganisationUnit"/>

### Интерфейс IOrganisationUnit 

Интерфейс предназначен для просмотра информации о должностях и структурных подразделениях организации.


##### IOrganisationUnit.Id 

Поле `Id` содержит идентификатор должности или структурного подразделения.
```cpp
int IOrganisationUnit.Id { get; }
```


##### IOrganisationUnit.Kind 

Поле `Kind` возвращает тип элемента организационной структуры.
```cpp
OrganisationUnitKind IOrganisationUnit.Kind { get; }
```


##### IOrganisationUnit.Title 

Поле `Title` содержит локализованное наименование должности или структурного подразделения.
```cpp
string IOrganisationUnit.Title { get; }
```


##### IOrganisationUnit.IsDeleted 

Поле `IsDeleted` показывает удалена ли должность или структурное подразделение. В системе *Pilot* организационные единицы не удаляются из базы данных, а только помечаются как удаленные.
```cpp
bool IOrganisationUnit.IsDeleted { get; }
```

##### IOrganisationUnit.IsChief

Метод расширения `IsChief` показывает, является ли должность руководящей.
```cpp
bool IOrganisationUnit.IsChief { get; }
```

##### IOrganisationUnit.Children

Поле `Children` содержит идентификаторы дочерних должностей или структурных подразделений.
```cpp
ReadOnlyCollection<int> IOrganisationUnit.Children { get; }
```

##### IOrganisationUnit.Person

Метод расширения Person возвращает идентификатор пользователя, занимающего должность. Если должность вакантна, значение будет -1.

##### IOrganisationUnit.VicePersons

Метод расширения `VicePersons` возвращает список идентификаторов пользователей-заместителей на должности. Порядок идентификаторов в списке определяет порядок вступления заместителей в должность при неактивном состоянии пользователя.

##### IOrganisationUnit.GroupPerson

Метод расширения GroupPersons возвращает список идентификаторов пользователей в группе.

<a name="IPerson"/>

### Интерфейс IPerson 

Интерфейс предназначен для просмотра информации о пользователе системы *Pilot*.


##### IPerson.Id 

Поле `Id` содержит уникальный идентификатор пользователя.
```cpp
int IPerson.Id { get; }
```


##### IPerson.Login 

Поле `Login` содержит имя пользователя (login).
```cpp
string IPerson.Login { get; }
```


##### IPerson.DisplayName 

Поле `DisplayName` содержит отображаемое имя пользователя. Например ФИО.
```cpp
string IPerson.DisplayName { get; }
```


##### IPerson.Email 

Метод расширения `Email` возвращает электронную почту пользователя
```cpp
static string Email(this IPerson person)
```


##### IPerson.Phone 

Метод расширения `Phone` возвращает телефон пользователя
```cpp
static string Phone(this IPerson person)
```


##### IPerson.Positions 

Поле `Positions` содержит список должностей, которые в данный момент занимает пользователь.
```cpp
ReadOnlyCollection<IPosition> IPerson.Positions { get; }
```


##### IPerson.MainPosition 

Поле `MainPosition` содержит основную должность пользователя.
```cpp
IPosition IPerson.MainPosition { get; }
```


##### IPerson.Comment 

Поле `Comment` содержит дополнительные сведения о пользователе.
```cpp
string IPerson.Comment { get; }
```

##### IPerson.IsInactive 

Метод расширения `IsInactive` возвращает `true`, если статус пользователя "Недоступен".

##### IPerson.Sid 

Поле `Sid` содержит уникальный идентификатор доменной учетной записи пользователя.
```cpp
string IPerson.Sid { get; }
```


##### IPerson.IsDeleted 

Поле `IsDeleted` показывает удален пользователь или нет. В системе *Pilot* информация пользователях не удаляется из базы данных. Пользователи помечаются как удаленные.
```cpp
bool IPerson.IsDeleted { get; }
```


##### IPerson.IsAdmin 

Поле `IsAdmin` показывает является ли текущий пользователь администратором.
```cpp
bool IPerson.IsAdmin { get; }
```


##### IPerson.ActualName 

Поле `ActualName` содержит актуальное имя пользователя. По умолчанию это поле выводит информацию из поля `DisplayName`. Если поле `DisplayName` не заполнено, то данное поле выводит информацию из поля `Login`.
```cpp
string IPerson.ActualName { get; }
```

##### IPerson.Groups 

Метод расширения Groups возвращает список идентификаторов групп, в которых состоит пользователь

##### IPerson.AllOrgUnits 

Метод расширения `AllOrgUnits` возвращает список идентификаторов всех должностей, подразделений и групп, в которых состоит пользователь.


<a name="IPosition"/>

### Интерфейс IPosition 

Интерфейс предназначен для просмотра информации о должности пользователя.


##### IPosition.Order 

Поле `Order` содержит порядковый номер должности пользователя. Основная должность имеет наименьший порядковый номер.
```cpp
int IPosition.Order { get; }
```


##### IPosition.Position 

Поле `Position` содержит идентификатор должности пользователя.
```cpp
int IPosition.Position { get; }
```


<a name="IAccess"/>

### Интерфейс IAccess 

Интерфейс предназначен для просмотра информации об уровне доступа.


##### IAccess.AccessLevel 

Содержит уровень доступа к элементу.

```cpp
AccessLevel IAccess.AccessLevel { get; }
```


##### IAccess.ValidThrough 

Содержит дату, до которой действует право доступа.

```cpp
DateTime IAccess.ValidThrough { get; }
```


##### IAccess.IsInheritable 

Флаг, указывающий будут ли наследовать дочерние элементы текущее право доступа.

```cpp
bool IAccess.IsInheritable { get; }
```


##### IAccess.IsInherited 

Флаг, указывающий унаследовано право от родительского элемента или нет.

```cpp
bool IAccess.IsInherited { get; }
```

<a name="IAccessRecord"/>

### Интерфейс IAccessRecord 
Интерфейс предназначен для просмотра информации о правах доступа на элемент.

##### IAccessRecord.OrgUnitId 
Содержит идентификатор должности для которого действует указанный уровень доступа.
```cpp
int IAccessRecord.OrgUnitId { get; }
```

##### IAccessRecord.Access 
Содержит информацию об уровене доступа. Подробнее см. [IAccess](#IAccess)
```cpp
IAccess IAccessRecord.Access { get; }
```

##### IAccessRecord.RecordOwner 
Содержит информацию о должности, которая установила указанный уровень доступа.
```cpp
int IAccessRecord.RecordOwner { get; }
```

##### IAccessRecord.InheritanceSource 
Содержит идентификатор элемента, с которого текущий элемент получил унаследованное право доступа.
```cpp
Guid IAccessRecord.InheritanceSource { get; }
```


<a name="IStorageDataObject"/>

### Интерфейс IStorageDataObject 

Интерфейс предназначен для просмотра информации об элементе Pilot-Storage.


##### IStorageDataObject.DataObject 

Поле `DataObject` содержит полную информацию об элементе.
```cpp
IDataObject IStorageDataObject.DataObject { get; }
```


##### IStorageDataObject.Path 

Поле `Path` содержит абсолютный путь до элемента на *Pilot-Storage*.

```cpp
string IStorageDataObject.Path { get; }
```


##### IStorageDataObject.State 

Поле `State` содержит текущее состояние элемента.

```cpp
StorageObjectState IStorageDataObject.State { get; }
```

##### IStorageDataObject.IsDirectory 

Поле `IsDirectory` показывает является ли текущий элемент папкой.

```cpp
bool IStorageDataObject.IsDirectory { get; }
```

<a name="IStateInfo"/>

### Интерфейс IStateInfo 

Интерфейс предназначен для просмотра информации о состоянии объекта.

##### IStateInfo.State 

Поле `State` содержит ифнормацию о состоянии объекта.
```cpp
ObjectState IStateInfo.State { get; }
```

##### IStateInfo.Date 

Поле `Date` содержит ифнормацию дате последнего изменения состояния объекта.
```cpp
DateTime IStateInfo.Date { get; }
```

##### IStateInfo.PersonId 

Поле `PersonId` содержит идентификатор пользователя, изменившего состояние объекта последним.
```cpp
int IStateInfo.PersonId { get; }
```

##### IStateInfo.PositionId 

Поле `PositionId` содержит идентификатор позиции пользователя, изменившего состояние объекта последним.
```cpp
int IStateInfo.PositionId { get; }
```

<a name="IRelation"/>

### Интерфейс IRelation 

Интерфейс предоставляет доступ к свойствам связи элемента.

##### IRelation.Id 

Поле `Id` содержит идентификатор связи. У двух связанных между собой объектов должны быть связи с одинаковым идентификатором.

```cpp
Guid IRelation.Id { get; }
```

##### IRelation.TargetId 

Поле TargetId содержит идентификатор элемента, с которым связан текущий объект.

```cpp
Guid IRelation.TargetId { get; }
```

##### IRelation.Type 

Поле `Type` содержит тип связи. Подробнее см [ObjectRelationType](#ObjectRelationType)

```cpp
ObjectRelationType IRelation.Type { get; }
```

##### IRelation.Name 

Поле `Name` содержит имя связи.

```cpp
string IRelation.Name { get; }
```

##### IRelation.VersionId 

Поле `VersionId` содержит дату создания версии документа.

```cpp
DateTime IRelation.VersionId { get; }
```

<a name="IFilesSnapshot"/>

### Интерфейс IFilesSnapshot 

Интерфейс содержит информацию о файлах элемента.

##### IFilesSnapshot.Created 
Поле `Created` содержит дату создания снимка файлов.
```cpp
DateTime IFilesSnapshot.Created { get; }
```

##### IFilesSnapshot.CreatorId 
Поле `CreatorId` содержит идентификатор пользователя сделавшего снимок файлов.
```cpp
int IFilesSnapshot.CreatorId { get; }
```

##### IFilesSnapshot.Reason 
Поле `Reason` содержит информацию о причине создания снимка файлов.
```cpp
string IFilesSnapshot.Reason { get; }
```

##### IFilesSnapshot.Files 
Поле `Files` содержит список файлов снимка.
```cpp
ReadOnlyCollection<IFile> IFilesSnapshot.Files { get; }
```

<a name="ILicenseInfo"/>

### Интерфейс ILicenseInfo 

Интерфейс содержит информацию о лицензии для определенного продукта.

##### ILicenseInfo.ProductId 
Поле `ProductId` содержит номер продукта.
```cpp
int ILicenseInfo.ProductId { get; }
```

##### ILicenseInfo.IsExpired 
Поле `IsExpired` содержит информацию просрочена ли лицензия.
```cpp
bool ILicenseInfo.IsExpired { get; }
```

##### ILicenseInfo.IsCheated 
Поле `IsCheated` содержит информацию о корректности файла лицензии.
```cpp
bool ILicenseInfo.IsCheated { get; }
```

##### ILicenseInfo.ExpirationDate 
Поле `ExpirationDate` дату истечения лицензии.
```cpp
DateTime ILicenseInfo.ExpirationDate { get; }
```

##### ILicenseInfo.MaxLicensesCount 
Поле `MaxLicensesCount` содержит количество рабочих мест, выделенных для данного продукта.
```cpp
int ILicenseInfo.MaxLicensesCount { get; }
```

<a name="IUserState"/>

### Интерфейс IUserState 

Интерфейс `IUserState` описывает пользовательское состояние

##### IUserState.Id 
Поле `Id` содержит уникальный идентификатор пользовательского состояния. Данный идентификатор в виде строки записывается как значение атрибута в объект, находящийся в данном состоянии.
```cpp
Guid IUserState.Id { get; }
```

##### IUserState.Name 
Поле `Name` содержит уникальное внутреннее имя пользовательского состояния.
```cpp
string IUserState.Name { get; }
```

##### IUserState.Title 
Поле `Title` содержит отображаемое имя пользовательского состояния.
```cpp
string IUserState.Title { get; }
```

##### IUserState.Icon 
Поле `Icon` содержит иконку пользовательского состояния в формате SVG.
```cpp
byte[] IUserState.Icon { get; }
```

##### IUserState.Color 
Поле `Color` содержит цвет пользовательского состояния из перечисления возможных цветов.
```cpp
UserStateColors IUserState.Color { get; }
```

##### IUserState.IsDeleted 
Поле `IsDeleted` показывает, было ли удалено пользовательское состояние.
```cpp
bool IUserState.IsDeleted { get; }
```

<a name="IUserStateMachine"/>

### Интерфейс IUserStateMachine 
Интерфейс `IUserStateMachine` описывает машину состояний

##### IUserStateMachine.Id 
Поле `Id` содержит уникальный идентификатор машины состояния. Данный идентификатор в виде строки записывается в `IAttribute.Configuration` для связи атрибута типа "Состояние" с машиной состояний.
```cpp
Guid IUserStateMachine.Id { get; }
```

##### IUserStateMachine.Title 
Поле `Title` содержит имя машины состояний.
```cpp
string IUserStateMachine.Title { get; }
```

##### IUserStateMachine.StateTransitions 
Поле `StateTransitions` содержит список переходов машины состояний. Ключом словаря является идентификатор состояния, значением - список переходов, доступных для данного состояния.
```cpp
IDictionary<Guid, IEnumerable<ITransition>> IUserStateMachine.StateTransitions { get; }
```

### Интерфейс ITransition 
Интерфейс `ITransition` описывает переход в машине состояний


##### ITransition.StateTo 
Поле `StateTo` содержит идентификатор состояния, в которое доступен переход.
```cpp
Guid ITransition.StateTo { get; }
```

##### ITransition.DisplayName 
Поле `DisplayName` содержит название команды в пользовательском интерфейcе, которая осуществляет данный переход.
```cpp
string ITransition.DisplayName { get; }
```

##### ITransition.AvailableForPositionsSource 
Поле `AvailableForPositionsSource` определяет права доступа на данный переход. По умолчанию (при значении `null`), данный переход доступен всем пользователям, у которых есть права на редактирование объекта. Значения, отличные от `null`, позволяют наложить дополнительные ограничения на доступность перехода. В данный массив строк могут быть записаны значения двух типов:
1) Локальные роли. Локальные роли начинаются с символа `&` и указывают на атрибут типа **"Организационная единица"**, в котором будут записаны идентификаторы должностей и подразделений, имеющих права доступа на данный переход.
2) Идентификаторы элементов организационной структуры. Выглядят как целое число и указывают, какие должности или подразделения будут иметь права доступа на данный переход.
```cpp
string[] ITransition.AvailableForPositionsSource { get; }
```

<a name="IReportItem"/>

### Интерфейс IReportItem 
Интерфейс `IReportItem` описывает объект типа отчет.

##### IReportItem.Id 
Поле `Id` содержит уникальный идентификатор объекта.
```cpp
Guid IReportItem.Id { get; }
```

##### IReportItem.Name
Поле `Name` содержит текущее имя отчета.
```cpp
string IReportItem.Name { get; }
```

##### IReportItem.Type
Поле `Type` возвращает тип отчета.
```cpp
IType IReportItem.Type { get; }
```

##### IReportItem.SourceTypesNames 
Поле `SourceTypesNames` возвращает список имен типов объектов, для которых данный отчет может быть построен. То есть список допустимых типов объектов, которые могут использоваться как параметр отчета.
```cpp
HashSet<string> IReportItem.SourceTypesNames { get; }
```

<a name="IMouseLeftClickListener"/>

### Интерфейс IMouseLeftClickListener 

Интерфейс обработчика клика левой кнопки мыши по документу.

##### IMouseLeftClickListener.OnLeftMouseButtonClick 
Метод вызывается при клике **_левой_** кнопки мыши
```cpp
void OnLeftMouseButtonClick(XpsRenderClickPointContext pointContext);
```
где `pointContext` - [XpsRenderClickPointContext](#XpsRenderClickPointContext)

##### IMouseLeftClickListener.OnLeftMouseButtonClick
Метод вызывается при клике **_левой_** кнопки мыши
```cpp
void OnLeftMouseButtonClick(XpsRenderClickPointContext pointContext);
```
где `pointContext` - [XpsRenderClickPointContext](#XpsRenderClickPointContext)

<a name="IHistoryItem"/>

### Интерфейс IHistoryItem 

Интерфейс описывает элемент истории изменения объекта

##### IHistoryItem.Id 
Поле `Id` содержит идентификатор элемента.
```cpp
Guid Id { get; }
```
##### IHistoryItem.ObjectId 
Поле `ObjectId` содержит идентификатор объекта.
```cpp
Guid ObjectId { get; }
```
##### IHistoryItem.Reason 
Поле `Reason` содержит причину изменения объекта.
```cpp
string Reason { get; }
```
##### IHistoryItem.Created 
Поле `Created` содержит дату и время создания изменения объекта.
```cpp
DateTime Created { get; }
```
##### IHistoryItem.CreatorId 
Поле `CreatorId` содержит идентификатор пользователя, создавшего изменение.
```cpp
int CreatorId { get; }
```
##### IHistoryItem.Object 
Поле `Object` содержит состояние объекта на момент изменения.
```cpp
IDataObject Object { get; }
```

<a name="IChatMessage"/>

### Интерфейс IChatMessage 

Интерфейс описывает сообщение

##### IChatMessage.Id 
Идентификатор сообщения.
```cpp
Guid Id { get; }
```

##### IChatMessage.Data 
Специфичные для определенного типа сообщения данные
```cpp
byte[] Data { get; }
```

##### IChatMessage.CreatorId 
Идентификатор пользователя, создателя сообщения
```cpp
int CreatorId { get; }
```

##### IChatMessage.ServerDateUtc 
Серверная дата создания сообщения в UTC
```cpp
DateTime? ServerDateUtc { get; }
```

##### IChatMessage.ClientDateUtc 
Клиентская дата создания сообщения в UTC
```cpp
DateTime ClientDateUtc { get; }
```

##### IChatMessage.ChatId 
Идентификатор чата
```cpp
Guid ChatId { get; }
```

##### IChatMessage.RelatedMessageId 
Идентификатор связанного сообщения
```cpp
Guid? RelatedMessageId { get; }
```

##### IChatMessage.Type 
Тип сообщения
```cpp
MessageType Type { get; }
```
<a name="IChat"/>

### Интерфейс IChat 

Интерфейс описывает чат

##### IChat.Id 
Идентификатор чата.
```cpp
Guid Id { get; }
```

##### IChat.Name 
Название чата.
```cpp
 string Name { get; }
```

##### IChat.Description 
Описание чата.
```cpp
string Description { get; }
```

##### IChat.CreatorId 
Идентификатор создателя чата
```cpp
int CreatorId { get; }
```

##### IChat.Type 
Тип чата
```cpp
ChatKind Type { get; }
```

##### IChat.LastMessageId 
Идентификатор последнего сообщения в чате
```cpp
 Guid? LastMessageId { get; }
```

##### IChat.CreationDateUtc 
Дата сощдания чата в UTC
```cpp
DateTime CreationDateUtc { get; }
```


##### IChatMessage.RelatedMessages 
Коллекция связанных сообщений. Сообщения RelatedMessageId которых, равен идентифкатору данного сообщения
```cpp
IReadOnlyCollection<IChatMessage> RelatedMessages { get; }
```

##### IChatMessage.GetData();
Данные текстового сообщения
```cpp
T GetData<T>();
```
Где `T` один из типов из списка:
`ITextMessageData` для текстового сообщения


<a name="ITextMessageData"/>

### Интерфейс ITextMessageData 

Интерфейс описывает данные текстового сообщения
##### ITextMessageData.Text 
Текст сообщения
```cpp
 string Text { get; }
```

##### ITextMessageData.Attachments 
Вложения в сообщения
```cpp
List<IChatRelation> Attachments { get; }
```
##### ITextMessageData.IsReadOnly 
Определяет, доступно ли редактирование для сообщения
```cpp
 string IsReadOnly { get; }
```

<a name="IChatRelation"/>

### Интерфейс IChatRelation 

Интерфейс описывает данные связи сообщения с объектом
##### ITextMessageData.ObjectId 
Идентификатор объекта
```cpp
 Guid ObjectId { get; set; }
```
##### ITextMessageData.Type 
Тип связи
```cpp
ChatRelationType Type { get; set; }
```
##### ITextMessageData.MessageId 
идентификатор сообщения, создавшего связь
```cpp
Guid? MessageId { get; set; }
```
##### ITextMessageData.IsDeleted 
Признак удаленной связи
```cpp
bool IsDeleted { get; set; }
```

<a name="IChatMember"/>

### Интерфейс IChatMember 

Интерфейс описывает данные участника чата
##### IChatMember.ChatId 
Идентификатор чата
```cpp
 Guid ChatId { get; }
```

##### IChatMember.PersonId 
Идентификатор пользователя
```cpp
int PersonId { get;}
```

##### IChatMember.IsAdmin 
Признак администратора чата
```cpp
bool IsAdmin { get;}
```

##### IChatMember.IsDeleted 
Признак удаленного участника
```cpp
bool IsDeleted { get; }
```

##### IChatMember.IsNotifiable 
Признак подписки на уведомления
```cpp
bool IsNotifiable { get;}
```

##### IChatMember.DateUpdatedUtc 
Дата создания
```cpp
DateTime DateUpdatedUtc { get; }
```

##### IChatMember.IsViewerOnly 
Признак пользователя, не являющегося участником чата, но просматривающего его
```cpp
bool IsViewerOnly { get; }
```

<a name="Enums"/>

## Перечисления

<a name="DataState"/>

### Перечисление DataState
Перечисление состояний элемента.

##### DataState.Unknown
Не известное состояние элемента. Все создаваемые элементы получают данное состояние.
```cpp
DataState.Unknown;
```

##### DataState.NoData
Элемент существует, но не загружен в память клиента.
```cpp
DataState.NoData;
```

##### DataState.Loaded
Элемент существует и загружен в память клиента.
```cpp
DataState.Loaded;
```

##### DataState.NonExistent
Элемента не существует.
```cpp
DataState.NonExistent;
```

<a name="SynchronizationState"/>

### Перечисление SynchronizationState
Перечисление состояний синхронизации элемента с сервером.

##### SynchronizationState.Synchronized 
Элемент синхронизирован с сервером. Элемент на сервере идентичен элементу в кеше клиента.
```cpp
SynchronizationState.Synchronized;
```

##### SynchronizationState.AwaitingForSynchronization 
Элемент в ожидании синхронизации с сервером.
```cpp
SynchronizationState.AwaitingForSynchronization;
```

##### SynchronizationState.SynchronizationAborted 
Изменения внесенные в элемент по каким-то причинам были отклонены сервером. Например отсутствие прав на изменение атрибутов элемента.
```cpp
SynchronizationState.SynchronizationAborted;
```

<a name="TypeKind"/>

### Перечисление TypeKind 
Перечисление видов типа элемента.

##### TypeKind.User 
Тип элемента является пользовательским и элементы данного типа могут отображаться в **Обозревателе проектов**.
```cpp
TypeKind.User;
```

##### TypeKind.System 
Тип элемента является системным и элементы данного типа не отображаются в **Обозревателе проектов**.
```cpp
TypeKind.System;
```

<a name="ObjectState"/>

### Перечисление ObjectState 
Перечисление состояний элемента.

##### ObjectState.Alive 
Состояние по умолчанию. Элемент "живой".
```cpp
ObjectState.Alive;
```

##### ObjectState.InRecycleBin 
Элемент удален в корзину
```cpp
ObjectState.InRecycleBin;
```

##### ObjectState.DeletedPermanently 
Элемент удален безвозвратно
```cpp
ObjectState.DeletedPermanently;
```

##### ObjectState.Frozen 
Элемент заморожен
```cpp
ObjectState.Frozen;
```

##### ObjectState.LockRequested 
Запрошено разрешение на блокировку
```cpp
ObjectState.LockRequested;
```

##### ObjectState.LockAccepted 
Элемент заблокирован
```cpp
ObjectState.LockAccepted;
```


<a name="State"/>

### Перечисление State 
Перечисление состояний элемента задания.

##### State.None 
Черновик
```cpp
State.None;
```

##### State.Assigned 
Задание выдано
```cpp
State.Assigned;
```

##### State.InProgress 
Задание взято в работу
```cpp
State.InProgress;
```

##### State.Revoked 
Задание отозвано
```cpp
State.Revoked;
```

##### State.OnValidation 
Задание находится на проверке
```cpp
State.OnValidation;
```

##### State.Completed 
Задание выполнено
```cpp
State.Completed;
```


<a name="AccessLevel"/>

### Перечисление AccessLevel 

Перечисление возможных уровней доступа к элементам.


##### AccessLevel.None 

Нет доступа к элементу.

```cpp
AccessLevel.None;
```

##### AccessLevel.View 

Уровень доступа, позволяющий просматривать свойства текущего элемента.

```cpp
AccessLevel.View;
```

##### AccessLevel.Create 

Уровень доступа, позволяющий создавать дочерние элементы.

```cpp
AccessLevel.Create;
```


##### AccessLevel.Edit 

Уровень доступа, позволяющий редактировать текущий элемент.

```cpp
AccessLevel.Edit;
```

##### AccessLevel.Agreement 

Уровень доступа, отвечающий за возможность аннотирования/подписания документов.

```cpp
AccessLevel.Agreement;
```

##### AccessLevel.Freeze 

Уровень доступа, позволяющий замораживать/размораживать текущий элемент.

```cpp
AccessLevel.Freeze;
```

##### AccessLevel.Share 

Уровень доступа, позволяющий делегировать права доступа для текущего элемента.

```cpp
AccessLevel.Share;
```

##### AccessLevel.ViewCreate 

Уровень доступа, позволяющий просматривать свойства текущего элемента и создавать дочерние элементы. Это комбинация двух уровней доступа `Create` и `View`.

```cpp
AccessLevel.ViewCreate;
```
`


##### AccessLevel.ViewEdit 

Полный доступ к элементу. Уровень доступа, позволяющий просматривать, редактировать свойства текущего элемента и создавать дочерние элементы. Это комбинация уровней доступа `Create`, `View` и `Edit`.

```cpp
AccessLevel.ViewEdit;
```


<a name="StorageObjectState"/>

### Перечисление StorageObjectState 
Перечисление состояний элемента **Pilot-Storage**.

```cpp
StorageObjectState.None = 0;
```
Неизвестное состояние элемента.

```cpp
StorageObjectState.NotLoaded = 1;
```
Элемент не загружен.

```cpp
StorageObjectState.Loaded = 2;
```
Элемент загружен.

```cpp
StorageObjectState.Outdated = 4;
```
Элемент скачан с сервера. Но на сервере есть более новая версия.


```cpp
StorageObjectState.NotSent = 8;
```
Элемент еще не был отправлен на сервер.

```cpp
StorageObjectState.Edited = 16;
```
Есть локальные изменения, которые не отправлены на сервер.

```cpp
StorageObjectState.Aborted = 32;
```
Изменения не принятые сервером.

```cpp
StorageObjectState.Temp = 64;
```
Временный элемент, который никогда не будет отправлен на сервер.

```cs
StorageObjectState.Locked = 128;
```
Элемент заблокирован.

```cpp
StorageObjectState.Downloading = 256;
```
Элемент в процессе загрузки.

```cpp
StorageObjectState.Conflicted = 512;
```
Элемент в состоянии конфликта.


<a name="ObjectRelationType"/>

### Перечисление ObjectRelationType 

##### ObjectRelationType.SourceFiles 
Тип связи с исходным файлом
```cpp
ObjectRelationType.SourceFiles;
```

##### ObjectRelationType.TaskInitiatorAttachments 
Тип связи, описывающий вложения в задание документов или файлов инициатором задания.
```cpp
ObjectRelationType.TaskInitiatorAttachments;
```

##### ObjectRelationType.MessageAttachments 
Тип связи, описывающий вложения в переписку по заданию документов или файлов.
```cpp
ObjectRelationType.MessageAttachments;
```

##### ObjectRelationType.Custom 
Пользовательский тип связи. Этот тип связи отображается во вкладке `"Связи"` в клиенте **Pilot**.
```cpp
ObjectRelationType.Custom;
```


<a name="OrganisationUnitKind"/>

### Перечисление OrganisationUnitKind 
Перечисление типов элементов организационной структуры.

##### OrganisationUnitKind.Department 
Подразделение организационной структуры. Может включать дочерние подразделения и должности. Пользователь не может быть назначен на подразделение.
```cpp
OrganisationUnitKind.Department;
```

##### OrganisationUnitKind.Position 
Должность организационной структуры. Не может иметь дочерних элементов. На должность может быть назначен пользователь и список заместителей.
```cpp
OrganisationUnitKind.Position;
```

##### OrganisationUnitKind.Group 
Группа организационной структуры. Может быть создана только на первом уровне организационной структуры, не может содержать дочерних элементов и иметь назначенного пользователя. Используется для объединения пользователей в группы.
```cpp
OrganisationUnitKind.Group;
```

<a name="IClientInfo"/>

### Перечисление IClientInfo 
Перечисление видов клиентов

```cs
IClientInfo.Ice = 0,
```
Pilot-ICE клиент

```cs
IClientInfo.IceEnterprise = 1,
```
Pilot-ICE Enterprise клиент

```cs
IClientInfo.ThreeDStorage = 2,
```
3D-Storage клиент

```cs
IClientInfo.Ecm = 3,
```
Pilot-ECM клиент

```cs
IClientInfo.Bim = 4,
```
Pilot-BIM клиент

<a name="PdfStamperMode"/>

### Перечисление PdfStamperMode
Перечисление режимов при наложении штампов на pdf-документы

```cs
PdfStamperMode.Preview = 0,
```
Отображение в просмотрщике документов

```cs
PdfStamperMode.Print = 1,
```
Вывод на печать

```cs
PdfStamperMode.Export = 2
```
Экспорт проекта

<a name="Events"/>

## События

<a name="OfflineEventArgs"/>

#### Событие OfflineEventArgs 

Аргументы события `OfflineEventArgs` присылаются расширению когда возникает событие перехода **клиента Pilot-ICE/ECM** в режим автономной работы.
```cpp
public class OfflineEventArgs : MarshalByRefObject { }
```

<a name="OnlineEventArgs"/>

#### Событие OnlineEventArgs

Аргументы события `OnlineEventArgs` присылаются расширению когда возникает событие перехода **клиента Pilot-ICE/ECM** в режим автономной работы в сети.
```cpp
public class OnlineEventArgs : MarshalByRefObject { }
```

<a name="CloseTabEventArgs"/>

#### Событие CloseTabEventArgs

Аргументы события `CloseTabEventArgs` присылаются расширению перед закрытием вкладки.
```cpp
public class CloseTabEventArgs : MarshalByRefObject { }
```
Идентификатор закрываемой вкладки.
```cpp
public Guid CloseTabEventArgs.TabId { get; }
```
Заголовок закрываемой вкладки.
```cpp
public string CloseTabEventArgs.TabTitle { get; }
```

<a name="OpenTabEventArgs"/>

#### Событие OpenTabEventArgs

Аргументы события `OpenTabEventArgs` присылаются расширению перед открытием новой вкладки.
```cpp
public class OpenTabEventArgs : MarshalByRefObject { }
```
Заголовок открываемой вкладки.
```cpp
public string OpenTabEventArgs.TabTitle { get; }
```
Идентификатор открываемой вкладки.
```cpp
public Guid OpenTabEventArgs.TabId { get; }
```

<a name="ActiveTabChangedEventArgs"/>

#### Событие **ActiveTabChangedEventArgs**

Аргументы события `ActiveTabChangedEventArgs` присылаются расширению при изменении активной вкладки.
```cpp
public class ActiveTabChangedEventArgs : MarshalByRefObject { }
```
Класс `ActiveTabChangedEventArgs` содержит поле с идентификатором активной вкладки.
```cpp
public Guid ActiveTabChangedEventArgs.ActiveTabId { get; }
```

<a name="TabUndockedEventArgs"/>

#### Событие **TabUndockedEventArgs**

Аргументы события `TabUndockedEventArgs` присылаются расширению при перемещении вкладки в отдельное окно.
```cpp
public class TabUndockedEventArgs : MarshalByRefObject { }
```
Класс `TabUndockedEventArgs` содержит поле с идентификатором вкладки.
```cpp
public Guid TabUndockedEventArgs.TabId { get; }
```
Класс `TabUndockedEventArgs` содержит поле с дескриптором окна.
```cpp
public IntPtr TabUndockedEventArgs.Handle { get; }
```

<a name="TabDockedEventArgs"/>

#### Событие **TabDockedEventArgs**

Аргументы события `TabDockedEventArgs` присылаются расширению при перемещении вкладки из отдельного окна в панель вкладок.
```cpp
public class TabDockedEventArgs : MarshalByRefObject { }
```
Класс `TabDockedEventArgs` содержит поле с идентификатором вкладки.
```cpp
public Guid TabDockedEventArgs.TabId { get; }
```

<a name="LoadedEventArgs"/>

#### Событие **LoadedEventArgs**

Аргументы события `LoadedEventArgs` присылаются расширению после загрузки **клиентом Pilot-ICE/ECM** данного расширения.
```cpp
public class LoadedEventArgs : MarshalByRefObject { }
```

<a name="UnloadedEventArgs"/>

#### Событие **UnloadedEventArgs**

Аргументы события `LoadedEventArgs` присылаются расширению перед выгрузкой **клиентом Pilot-ICE/ECM** данного расширения.
```cpp
public class UnloadedEventArgs : MarshalByRefObject { }
```

<a name="SystemConstants">

## Константы

<a name="SystemTypeNames"/>

#### SystemTypeNames 

##### PROJECT_FOLDER 
Имя системного типа **папка** для **Pilot-Storage**.
```cs
public const string PROJECT_FOLDER = "Project_folder";
```

##### PROJECT_FILE 
Имя системного типа **файл** для **Pilot-Storage**.
```cs
public const string PROJECT_FILE = "File";
```

##### EXTENSION 
Имя системного типа **расширение**.
```cs
public const string EXTENSION = "Extension";
```

##### EXTENSION_FOLDER 
Имя системного типа **папка расширений**.
```cs
public const string EXTENSION_FOLDER = "Extension_folder";
```

##### SHORTCUT 
Имя системного типа **ярлык**.
```cs
public const string SHORTCUT = "Shortcut_E67517F1-93F5-4756-B651-133B816D43C8";
```

##### REPORT 
Имя системного типа **Отчет**.
```cs
public const string REPORT = "Report_6088AF81-061E-456E-9225-CF65B7B25368";
```

##### REPORT_FOLDER 
Имя системного типа **Папка отчетов**.
```cs
public const string REPORT_FOLDER = "Report_Folder_F2CC6F1D-70E1-4E9B-B32F-BEB3E991318F";
```

##### SMART_FOLDER 
Имя системного типа **Умная папка**.
```cs
public const string SMART_FOLDER = "Smart_folder_type";
```

##### TASK_TEMPLATE_FOLDER 
Имя системного типа **Папка шаблонов заданий**.
```cs
public const string TASK_TEMPLATE_FOLDER = "Task_template_folder_A0A09765-E6FB-4272-87EE-37793283DBC5";
```

##### TASK_TEMPLATE 
Имя системного типа **Шаблон задания**.
```cs
public const string TASK_TEMPLATE = "Task_template_05339782-CCBA-4786-80C3-0F6C7E0EF3C5";
```

##### DOCUMENT_TEMPLATE_FOLDER 
Имя системного типа **Папка шаблонов документов**.
```cs
public const string DOCUMENT_TEMPLATE_FOLDER = "Document_template_folder_793D0CE8-65E6-484E-AAF9-7E095AF9DBD2";
```

##### DOCUMENT_TEMPLATE 
Имя системного типа **Шаблон документов**.
```cs
public const string DOCUMENT_TEMPLATE = "Document_template_89B9E233-A6F9-4B9C-B970-55B3B3A77CED";
```

##### GLOBAL_ROOT_TYPE_NAME 
Имя системного типа **Глобальный корень документов**.
```cs
public const string GLOBAL_ROOT_TYPE_NAME = "Root_object_type";
```

##### WORKFLOW_PREFIX 
Префикс для типа **Процесс**.
```cs
public const string WORKFLOW_PREFIX = "workflow_";
```

##### STAGE_PREFIX 
Префикс для типа **Этап** задания.
```cs
public const string STAGE_PREFIX = "stage_";
```

##### TASK_PREFIX 
Префикс для типа **Задание**.
```cs
public const string TASK_PREFIX = "task_";
```

##### BIM_PREFIX 
Префикс для типов системы BIM **BIM типы**.
```cs
public const string BIM_PREFIX = "bim_";
```

<a name="SystemAttributeNames"/>

#### SystemAttributeNames 
Константы для системных атрибутов.

##### DELETE_DATE 
Имя атрибута с датой удаления элемента.
```cs
public const string DELETE_DATE = "DeleteDate 9349ED7C-C2D7-4B8B-852A-83140F158611";
```

##### DELETE_INITIATOR_PERSON 
Имя атрибута с идентификатором пользователя, удалившего элемент.
```cs
public const string DELETE_INITIATOR_PERSON = "DeleteInitiatorPerson BE56ECD1-F4C5-40E2-A83C-274EAE4D02A9";
```

##### DELETE_INITIATOR_POSITION 
Имя атрибута с идентификатором должности пользователя, удалившего элемент.
```cs
public const string DELETE_INITIATOR_POSITION = "DeleteInitiatorPosition 35E355AC-97B3-40FC-9636-1648402040D4";
```

##### SEARCH_CRITERIA 
Имя атрибута с критерием поиска для **умной папки**
```cs
public const string SEARCH_CRITERIA = "SearchCriteria 52F6E73A-D736-49CD-8807-5AD955506A37";
```

##### SMART_FOLDER_TITLE 
Имя атрибута с именем для **умной папки**
```cs
public const string SMART_FOLDER_TITLE = "SmartFolderTitle 3AE3FFC8-A776-4E61-87D0-FFD8B50CBBA8";
```

##### SEARCH_CONTEXT_OBJECT_ID 
Имя атрибута с идентификатором объекта-контекста поиска для **умной папки**
```cs
public const string SEARCH_CONTEXT_OBJECT_ID = "SearchContextObjectId 257E6DB2-F3A3-4231-83D4-DB57C3FF059E";
```

##### IS_HIDDEN 
Имя атрибута, значение которого указывает на то, спрятана ли папка или файл на **Pilot-Storage**
```cs
public const string IS_HIDDEN = "IsHidden FDF5475C-93FA-41F3-8243-F1810854DEBD";
```

##### PROJECT_ITEM_NAME 
Имя атрибута с именем файла или папки на **Pilot-Storage**
```cs
public const string PROJECT_ITEM_NAME = "Title 4C281306-E329-423A-AF45-7B39EC30273F";
```

##### EXTENSION_FOLDER_NAME 
Имя атрибута с именем папки для расширений
```cs
public const string EXTENSION_FOLDER_NAME = "Extension_folder_name EE4DE4B7-FFB7-455E-8E15-C185CFDB34FA";
```

##### EXTENSION_NAME 
Имя атрибута с именем расширения
```cs
public const string EXTENSION_NAME = "Extension_name 53522556-0749-46DE-913E-EDA195AD1299";
```

##### EXTENSION_ADDITIONAL 
Имя атрибута с описанием расширения
```cs
public const string EXTENSION_ADDITIONAL = "Extension_additional 9A428222-2031-4F5E-A367-C5BAA18DFCA5";
```

##### SHORTCUT_OBJECT_ID 
Имя атрибута с идентификатором объекта, на который ссылается ярлык
```cs
public const string SHORTCUT_OBJECT_ID = "Shortcut_object_id_CF11FD82-3D56-4DBC-B7F8-DF91CA1F9885";
```

##### REPORT_FOLDER_NAME 
Имя атрибута с именем папки отчетов
```cs
public const string REPORT_FOLDER_NAME = "Report_folder_name_BDBDEDD1-BFCB-4E2C-BE44-3E4BEBBB58F9";
```

##### REPORT_NAME 
Имя атрибута с именем элемента отчет
```cs
public const string REPORT_NAME = "Report_name_D745D627-E7CD-4E3A-B30E-F9EDC1A09D77";        
```

##### REPORT_SOURCE_TYPES_NAMES 
Имя атрибута с типами элементов с данными для отчета
```cs
public const string REPORT_SOURCE_TYPES_NAMES = "Report_source_types_names_76e09b65-9785-40cc-986e-b35cd6c3a4fe";        
```

##### TASK_TEMPLATE_NAME 
Имя атрибута с именем шаблона задания
```cs
public const string TASK_TEMPLATE_NAME = "Task_template_name_CE25F101-E758-4A68-9F16-E73B72F39FBC";       
```

##### TASK_TEMPLATE_VALUE 
Имя атрибута с объектом шаблона задания
```cs
public const string TASK_TEMPLATE_VALUE = "Task_template_value_3C190DF3-3644-47F4-AF64-45C140207054";       
```

##### TASK_TEMPLATE_FOLDER_NAME 
Имя атрибута с именем папки шаблонов заданий
```cs
public const string TASK_TEMPLATE_FOLDER_NAME = "Task_template_folder_name_DC250687-50D3-40E9-8B9A-71ACD01D50F7";        
```

##### TAGS 
Имя атрибута с тегами
```cs
public const string TAGS = "Tags_BCA19031-E5A1-49D3-A55F-47B30A8F7243";      
```

##### DEFAULT_PUBLISH_FILE_ID 
Имя атрибута с идентификатором файла для публикации по умолчанию
```cs
public const string DEFAULT_PUBLISH_FILE_ID = "Default_publish_file_id_BA9EA041-93E8-4AE9-9E09-0C40221DE75D";
```

##### DOCUMENT_TEMPLATE_NAME 
Имя атрибута с именем шаблона документов
```cs
public const string DEFAULT_PUBLISH_FILE_ID = "Default_publish_file_id_BA9EA041-93E8-4AE9-9E09-0C40221DE75D";
```

##### DOCUMENT_TEMPLATE_TYPE_ID 
Имя атрибута с идентификатором типа шаблона документов
```cs
public const string DOCUMENT_TEMPLATE_TYPE_ID = "Document_template_type_id_0D8595C0-45CF-4C98-8AB2-EE5C9276DE3E";
```

##### DOCUMENT_TEMPLATE_FOLDER_NAME 
Имя атрибута с именем папки шаблонов документов
```cs
public const string DOCUMENT_TEMPLATE_FOLDER_NAME = "Document_template_folder_name_D6C7CC9A-0850-47B7-88D0-1045F8D5561D";
```


<a name="SystemStates"/>

#### SystemStates 
Константы для системных состояний.

##### TASK_REVOKED_STATE_ID 
Идентификатор состояния `Отозвано`
```cs
public static Guid TASK_REVOKED_STATE_ID = Guid.Parse("abdbe49a-7094-4084-9673-eb5fb3f95262");
```

##### TASK_NONE_STATE_ID 
Идентификатор состояния `Нет`
```cs
public static Guid TASK_NONE_STATE_ID = Guid.Parse("d8ae8c3a-6f46-45d2-835b-563fe2b47acd");
```

##### TASK_NO_EXECUTOR_ASSIGNED_STATE_NAME 
Константа `Исполнитель не назначен`
```cs
public static string TASK_NO_EXECUTOR_ASSIGNED_STATE_NAME = "noExecutorAssigned";
```

<a name="SystemObjectIds"/>

#### SystemObjectIds 
Идентификаторы системных объектов.

##### RootObjectId 
Идентификатор корневого объекта элеметов
```cs
public static readonly Guid RootObjectId = new Guid("00000001-0001-0001-0001-000000000001");
```

##### ExtensionRootObjectId 
Идентификатор корневого объекта расширений 
```cs
public static readonly Guid ExtensionRootObjectId = new Guid("E6519D37-1984-407E-96A0-1CD371F68F16");
```

##### ReportRootObjectId 
Идентификатор корневого объекта отчетов 
```cs
public static readonly Guid ReportRootObjectId = new Guid("7DAB217D-6E06-4C2C-AB77-B5EC9361415D");
```

##### TaskTemplateRootObjectId 
Идентификатор корневого объекта шаблонов заданий 
```cs
public static readonly Guid TaskTemplateRootObjectId = new Guid("DB4AB44C-B3D1-4F5A-B049-13CDF0C2EFE7");
```

##### DocumentTemplateRootId 
Идентификатор корневого объекта шаблонов документов 
```cs
public static readonly Guid DocumentTemplateRootId = new Guid("109DA1F5-7E1A-4DF4-95BD-1FD5AA023DD6");
```

<a name="SystemFileNames"/>

#### SystemFileNames 
Константы имен системных файлов. Эти константы можно использовать при фильтрации файлов элемента или добавлении нового.
Пример добавления миниатюры к элементу:
```cs
IObjectBuilder.AddFile(Path.GetFileName(filename) + SystemFileNames.THUMBNAIL_FILE_NAME_POSTFIX, bitmapStream, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
```

[Obsolete]
##### ANNOTATIONS_DEFINITION 
Имя файла с описанием замечания 
```cs
public const string ANNOTATIONS_DEFINITION = "Annotation";
```

##### GRAPHIC_LAYER_ELEMENT_DEFINITION 
Префикс имени файла с описанием графического элемента 
```cs
public const string GRAPHIC_LAYER_ELEMENT_DEFINITION = "PILOT_GRAPHIC_LAYER_ELEMENT_";
```

##### GRAPHIC_LAYER_ELEMENT_CONTENT 
Префикс имени файла с содержимым графического элемента 
```cs
public const string GRAPHIC_LAYER_ELEMENT_CONTENT = "PILOT_CONTENT_GRAPHIC_LAYER_ELEMENT_";
```

[Obsolete]
##### ANNOTATION_CHAT_MESSAGE 
Имя файла с содержимым сообщения к замечанию 
```cs
public const string ANNOTATION_CHAT_MESSAGE = "Note_Chat_Message";
```

##### DIGITAL_SIGNATURE 
Имя файла с цифроврой подписью 
```cs
public const string DIGITAL_SIGNATURE = "PilotDigitalSignature";
```

##### THUMBNAIL_FILE_NAME_POSTFIX 
Имя файла с миниатюрой для файла на **Pilot-Storage**
```cs
public const string THUMBNAIL_FILE_NAME_POSTFIX = "PilotThumbnail";
```

##### TEXT_LABELS_DEFINITION 
Имя файла с описанием текстовых меток
```cs
public const string TEXT_LABELS_DEFINITION = "PilotTextLabels";
```

##### BARCODE_DEFINITION 
Имя файла с описанием штрих-кода
```cs
 public const string BARCODE_DEFINITION = "PilotBarcode";
```

## Интерфейс IChatMessage
Интерфейс предназначен для просмотра информации о сообщении.

##### IChatMessage.Id 

Поле Id содержит идентификатор сообщения.
```cpp
Guid IChatMessage.Id { get; }
```
##### IChatMessage.Data

Поле Data содержит специфичные для определенного типа сообщения данные.
```cpp
byte[] IChatMessage.Data { get; }
```

##### IChatMessage.CreatorId

Поле CreatorId содержит идентификатор пользователя создавшего сообщение.
```cpp
int IChatMessage.CreatorId { get; }
```

##### IChatMessage.ServerDate

Поле ServerDate содержит содержит серверную дату-время создания собщения в формате UTC.
```cpp
DateTime? IChatMessage.ServerDate { get; }
```

##### IChatMessage.LocalDate

Поле LocalDate содержит клиентскую дату-время создания сообщения в формате UTC.
```cpp
DateTime IChatMessage.LocalDate { get; }
```

##### IChatMessage.ChatId

Поле ChatId содержит идентификатор чата.
```cpp
Guid IChatMessage.ChatId { get; }
```

##### IChatMessage.RelatedMessageId

Поле RelatedMessageId содержит идентификатор сообщения, с которым связано данное сообщение. Например: отредактированное, отвеченное, прочитанное сообщение.
```cpp
Guid IChatMessage.RelatedMessageId { get; }
```

##### IChatMessage.Type

Поле Type содержит тип сообщения. Подробнее см. [MessageType](#MessageType)
```cpp
Guid IChatMessage.RelatedMessageId { get; }
```

##### IChatMessage.RelatedMessages

Поле RelatedMessages содержит сообщения, которые ссылаются на данное сообщение.
```cpp
List<IChatMessage> IChatMessage.RelatedMessages{ get; }
```

##### IChatMessage.TextMessageData

Поле TextMessageData содержит данные специфичные для текстового сообщения. Подробнее см. [ITextMessageData](#ITextMessageData)
```cpp
ITextMessageData IChatMessage.TextMessageData{ get; }
```

<a name="TemplateInterfaces"/>

## 12. Интерфейсы для работы с шаблонами

Шаблоны хранятся в сериализованном виде как строковое значение специальных системных атрибутов. Для того чтобы получить содержимое шаблона необходимо десериализовать это строковое значение. 

<a name="ITaskTemplateParser"/>

### Интерфейс **ITaskTemplateParser**

Шаблоны заданий и процессов сохраняются как дочерние объекты корневого объекта шаблонов заданий, который можно получить по его системному идентификатору **SystemObjectIds.TaskTemplateRootObjectId**. Он может включать объекты двух типов: непосредсвенно шаблоны заданий и процессов - объекты системного типа **SystemTypeNames.TASK_TEMPLATE**; и папки шаблонов - объекты системного типа **SystemTypeNames.TASK_TEMPLATE_FOLDER**. Таким образом можно организовать древовидную систему хранения шаблонов.

Объекты шаблонов заданий и процессов хранят содержимое в сериализованном виде как строковое значение системного атрибута **SystemAttributeNames.TASK_TEMPLATE_VALUE**. Для десериализации шаблона задания и получения доступа к его составляющим используется интерфейс **ITaskTemplateParser**. Пример **Ascon.Pilot.SDK.TaskSample** включает в себя демонстрацию получения шаблонов заданий и использования этого интерфейса для выдачи заданий и процессов на основе этих шалонов.

Для получения доступа к интерфейсу поиска его необходимо передать в конструктор. При этом конструктор необходимо пометить атрибутом [ImportingConstructor].

<a name="Parse"/>

##### ITaskTemplateItem Parse(string stringTemplateValue)

Метод используется для получения составляющих шаблона задания или процесса из сериализованного в строку значения шаблона.

```cpp
var template = _taskTemplateParser.Parse(templateStr);
```
где:
 - `template ` - десериализованное представление шаблона, объект типа *ITaskTemplateItem*;
 - `_taskTemplateParser ` - экземпляр объекта, реализующего интерфейс ITaskTemplateParser;
 - `templateStr ` - исходное сериализованное строковое представление шаблона, которое было получено из атрибута объекта шаблона;
 
 <a name="ITaskTemplateItem"/>

### Интерфейс **ITaskTemplateItem**

Этот интерфейс позволяет получить доступ к составляющим элементам шаблона задания или процесса.

#### Свойства

```cpp
int TypeId { get; }
```
Идентификатор типа элемента, который описывает данный элемент шаблона: это может быть идентификатор типа задания, типа этапа процесса или идентификатор типа процесса. Позволяет узнать, объект какого типа должен быть создан на основе этого элемента шаблона.

```cpp
IEnumerable<ITaskTemplateItem> Children { get; }
```
Список дочерних элементов шаблона. Шаблон обычного задания состоит только из одного элемента, его коллекция дочерних элементов всегда пуста. Шаблон процесса отличается тем, что его свойство `Children` содержит список элементов шаблона, описывающих этапы процесса. Каждый из элементов шаблона этапа в свою очередь содержит коллекцию дочерних элементов, которые описывают шаблоны заданий данного этапа. Метод **MakeWorkflowFromTemplate** примера **Ascon.Pilot.SDK.TaskSample** демонстрирует работу с шаблоном процесса и последовательным получением шаблонов этапов и шаблонов заданий каждого этапа.

```cs
IDictionary<string, object> Attributes  { get; }
```
Список атрибутов и их значений, которыми обладает элемент шаблона. Так, в нем могут хранится заголовок и описание задания или процесса, список исполнителей и аудиторов задания и т.д.

```cs
IEnumerable<Guid> Attachments  { get; }
```
Список идентификаторов объектов, которые являются вложениями данного элемента шаблона.

<a name="IDocumentCryptoInterfaces"/>

## 13. Интерфейсы для реализации подписания документов сторонними криптоалгоритмами

С помощью интерфейсов подписания можно реализовать свою логику подписания и валидации ЭП документов.

<a name="IDocumentCryptoProvider"/>

### Интерфейс **IDocumentCryptoProvider**

>**ВНИМАНИЕ!**
Этот интерфейс является устаревшим. Используйте [ICryptoProvider](#ICryptoProvider)

Интерфейс позволяет зарегистрировать в Pilot свою логику подписания и валидации ЭП документов. Для этого необходимо реализовать интерфейс из пакета Ascon.Pilot.SDK `IDocumentCryptoProvider` и обязательно пометить класс, реализующий интерфейс **IDocumentCryptoProvider** атрибутом `[Export]`. 
В системе Pilot может быть только один зарегистрированный экземпляр IDocumentCryptoProvider.

```cpp

[Export(typeof(IDocumentCryptoProvider))]
public class DocumentCryptoProvider : IDocumentCryptoProvider
{
	...
}
```

##### bool IsKnownSignature(IDataObject document, IFilesSnapshot snapshot, ISignatureRequest signatureRequest);

Метод вызывается только для подписанных запросов на подпись и служит для определения, готов ли IDocumentCryptoProvider прочитать информацию из подписи. Если метод вернет false, для чтения информации об электронной подписи будут использованы базовые алгоритмы Pilot.
Аргументы:
 - `document ` - документ;
 - `snapshot ` - версия документа, для которой происходит чтение данных;
 - `signatureRequest ` - подписанный запрос на подпись, для которого необходимо будет прочитать информацию;
 
##### void Sign(IDataObject document, Stream xpsStream, IEnumerable<ISignatureRequest> signatureRequests);

Метод вызывается для подписания документа.
Аргументы:
 - `document ` - документ;
 - `xpsStream ` - поток XPS документа со встроенными графическими слоями. Соответствует содержимому XPS документа при сохранении на диск.
 - `signatureRequests` - запросы на подпись для подписания;
 
##### void ShowCertificate(IDataObject document, IFilesSnapshot snapshot, ISignatureRequest signatureRequest);

Метод вызывается при вызове команды "Показать сертификат".
Аргументы:
 - `document ` - документ;
 - `snapshot ` - версия документа, для которой происходит чтение данных.
 - `signatureRequest ` - подписанный запрос на подпись, для которого необходимо показать сертификат;
 
##### void ReadSignature(IDataObject document, Stream xpsStream, IFilesSnapshot snapshot, ISignatureRequest signatureRequest, IReadSignatureListener listener);

Метод вызывается для получения информации о подписанном запросе на подпись.
Аргументы:
 - `document ` - документ;
 - `xpsStream ` - поток XPS документа со встроенными графическими слоями. Соответствует содержимому XPS документа при сохранении на диск.
 - `snapshot ` - версия документа, для которой происходит чтение данных.
 - `signatureRequest ` - запрос на подпись, для которого будет выполнено чтение;
 - `listener ` - используется для заполнения информации о прочитанной электронной подписи;


<a name="ICryptoProvider"/>

### Интерфейс ICryptoProvider
Интерфейс позволяет зарегистрировать в Pilot свою логику подписания и валидации ЭП документов. Для этого необходимо реализовать интерфейс из пакета Ascon.Pilot.SDK `ICryptoProvider` и обязательно пометить класс, реализующий интерфейс **ICryptoProvider** атрибутом `[Export]`. 
В системе Pilot может быть несколько экземпляров ICryptoProvider.

```cpp

[Export(typeof(ICryptoProvider))]
public class CryptoProvider : ICryptoProvider
{
	...
}
```

#### Методы
```cs
IEnumerable<ICertificate> GetCertificates();
```
Метод возвращает список сертификатов, поддерживаемых расширением. Подробнее см [ICertificate](#ICertificate)

```cs
 void Sign(Guid documentId, IFile file, Stream stream, ICertificate cert, IEnumerable<ISignatureRequest> signatureRequests);
```
Метод подписывает документ. Подпись должня быть создана в формате base64 а также файлу подписи должно быть присвоено расширение `.sig`.
где:
 - `documentId` - идентификатор документа;
 - `file` - подписываемый файл;
 - `stream` - последовательность байт подписываемого файла;
 - `cert` - сертификат для подписания;
 - `signatureRequests` - список запросов на подпись;
 
```cs
 bool IsAlgorithmSupported(string algorithmOid);
```
Определяет, поддерживается ли криптоалгоритм расширением.
где:
 - `algorithmOid` - идентификатор алгоритма;
 
```cs
 bool IsAlgorithmSupported(byte[] sigBase64);
```
Определяет, поддерживается ли сертификат подписи расширением. Проверка необходима при импорте подписи, когда криптоалгоритм неизвестен.
где:
 - `sigBase64` - подпись в формате base64;


```cs
void ReadSignature(IDataObject document, Stream xpsStream, IFilesSnapshot snapshot, ISignatureRequest signatureRequest, IReadSignatureListener listener);
```

Метод вызывается для получения информации о подписанном запросе на подпись.
Аргументы:
 - `document ` - документ;
 - `xpsStream ` - поток документа; 
 - `snapshot ` - версия документа, для которой происходит чтение данных;
 - `signatureRequest ` - запрос на подпись, для которого будет выполнено чтение;
 - `listener ` - используется для заполнения информации о прочитанной электронной подписи;

```cs
 void ReadImportedSignature(Stream file, byte[] sigBase64, IImportedSignatureListener importedSignatureListener);
```

Метод вызывается для получения информации об импортируемой подписи.
Аргументы:
 - `file` - поток документа;
 - `sigBase64` - импортируемая подпись в формате base64;
 - `importedSignatureListener ` - используется для заполнения информации об импортируемой электронной подписи;

 <a name="IReadSignatureListener"/>

### Интерфейс **IReadSignatureListener**

Этот интерфейс используется для заполнения информации о прочитанной электронной подписи.

```cpp
void CertificateIsValid();
```
Вызовите этот метод, если сертификат, использованный для подписания, корректен.

```cpp
void CertificateIsInvalid(string message);
```
Вызовите этот метод, если сертификат, использованный для подписания, некорректен.

```cpp
void SignatureIsValid();
```
Вызовите этот метод, если подпись соответствует подписанному содержимому.

```cpp
void SignatureIsInvalid(string message);
```
Вызовите этот метод, если подпись не соответствует подписанному содержимому.

```cpp
void SetSignerName(string name);
```
Вызовите этот метод для вывода имени подписанта.

```cpp
void SetSignDate(string signDate);
```
Вызовите этот метод для вывода времени подписания.

```cpp
void SetCustomState(ImageSource icon, string title, string description);
```
Вызовите этот метод для установки пользовательского состояния запроса на подпись (иконка и тултип).

```cpp
void SetSignerNameForeground(Color color);
```
Вызовите этот метод для установки цвета текста имени подписанта.

```cpp
void ReadCompleted();
```
Вызовите этот метод для окончания чтения информации из электронной подписи.

<a name="ICertificateSelector"/>

### Интерфейс **ICertificateSelector**

Интерфейс ICertificateSelector используется для выбора сертификата для подписания через API. По умолчанию, если плагин, реализующий интерфейс [ICryptoProvider](#ICryptoProvider), при подписании вернёт несколько сертификатов, будет показан диалог выбора сертификата. 
Объявление экспортного типа ICertificateSelector в модуле расширения перекрывает базовое поведение и позволяет реализовать свою логику выбора сертификата. 
В системе Pilot может быть только один зарегистрированный экземпляр ICertificateSelector.


```cpp

[Export(typeof(ICertificateSelector))]
public class CustomCertificateSelector : ICertificateSelector
{
	...
}
```

<a name="IDigitalSigner"/>

### Интерфейс IDigitalSigner

Интерфейс позволяет получить управление над экземпляром плаигна, реализующего интерфейс [ICryptoProvider](#ICryptoProvider) и с его помощью подписывать запросы на подпись в документах. Получить интерфейс можно через конструктор помеченный атрибутом `[ImportingConstructor]`.

##### IDigitalSigner.Sign
Метод подписывает документ выбранным сертификатом.
```cpp
void Sign(Guid documentId, IFile file, Stream stream, ICertificate cert, IEnumerable<ISignatureRequest> signatureRequests);
```
где:
 - `documentId` - идентификатор документа.
 - `file` - подписываемый файл.
 - `stream` - стрим подписываемого файла.
 - `cert` - сертификат, с помощью которого будет выполнено подписание.
 - `signatureRequests` - подписываемые запросы на подпись.
 
##### IDigitalSigner.GetCertificate
Метод позволяет получить серитфикат для подписания.
```cpp
ICertificate GetCertificate();
```

 <a name="IImportedSignatureListener"/>

### Интерфейс **IImportedSignatureListener**

Этот интерфейс используется для заполнения информации об импортируемой электронной подписи.

```cpp
void CertificateIsValid();
```
Вызовите этот метод, если сертификат, использованный для подписания, корректен.

```cpp
void CertificateIsInvalid(string message);
```
Вызовите этот метод, если сертификат, использованный для подписания, некорректен.

```cpp
void SignatureIsValid();
```
Вызовите этот метод, если подпись соответствует подписанному содержимому.

```cpp
void SignatureIsInvalid(string message);
```
Вызовите этот метод, если подпись не соответствует подписанному содержимому.


```cpp
void SetPublicKeyOid(string oid);
```
Вызовите этот метод для того, чтобы установить идентификатор криптоалгоритма.

```cpp
void SetCadesType(CadesType cadesType);
```
Вызовите этот метод для того, чтобы установить тип [CadesType](#CadesType) импортируемой подписи.

```cpp
void ReadCompleted();
```
Вызовите этот метод для окончания чтения информации из электронной подписи.

<a name="PDFInterfaces"/>
## 14. Интерфейсы для работы с PDF документами

<a name="IPdfStamper"/>
### Интерфейс **IPdfStamper**

Интерфейс позволяет реализовать логику наложения штампов на PDF документы. Для этого необходимо реализовать интерфейс из пакета Ascon.Pilot.SDK `IPdfStamper` и обязательно пометить класс, реализующий интерфейс атрибутом `[Export]` (см. пример `PdfStamper` из комплекта SDK).

Метод вызывается при загрузке документа для отображения в интерфейсе Pilot. Возвращаемое значение учитывается при добавлении кнопки "Отобразить штампы" в панель инструментов просмотрщика документов: в случае возвращения `true` - кнопка будет добавлена, в противном случае нет. Подробнее см. PdfDocumentContext.
```cpp
bool CanAddStamps(PdfDocumentContext context);
```
где:
 - `context` - контекст отображаемого документа, подробнее см. [PdfDocumentContext](#PdfDocumentContext).

Метод вызывается при нажатии кнопки "Отобразить штампы", при отправке документа на печать, а также при выгрузке на диск и экспорте.
```cpp
void AddStamps(Stream stream, PdfStamperMode mode, PdfDocumentContext context);
```
где:
 - `stream` - поток файла;
 - `mode` - режим при добавлении штампов, подробнее см. [PdfStamperMode](#PdfStamperMode);
 - `context` - контекст документа;

Метод вызывается при перемещении штампа на документе.
```cpp
void OnStampPositionChanged(StampPositionArgs args);
```
где:
 - `args` - аргументы события перемещения штампа, подробнее см. [StampPositionArgs](#StampPositionArgs).

<a name="PdfDocumentContext"/>
### **PdfDocumentContext**

Класс, описывающий контекст отображаемого документа.

### Свойства

```cs
IDataObject Document { get; }
```
Объект документа

```cs
DateTime Version { get; }
```
Версия

<a name="StampPositionArgs"/>
### **StampPositionArgs**

Класс, описывающий аргументы события перемещения штампа на документе.

### Свойства

```cs
string StampId { get; }
```
Идентификатор штампа

```cs
double X { get; }
```
Координата `X` после перемещения

```cs
double Y { get; }
```
Координата `Y` после перемещения

```cs
PdfDocumentContext Context { get; }
```
Контекст документа
