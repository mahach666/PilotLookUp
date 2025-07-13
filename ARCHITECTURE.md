# Архитектура PilotLookUp

## Обзор

PilotLookUp - это WPF приложение, построенное на архитектуре MVVM с использованием SimpleInjector для dependency injection. Приложение позволяет просматривать и анализировать объекты Pilot-ICE.

## Основные компоненты

### 1. Контейнер сервисов (ServiceContainer)

**Расположение**: `src/Utils/ServiceContainer.cs`

Контейнер использует SimpleInjector и создает новый экземпляр для каждого окна приложения, что позволяет избежать проблем с блокировкой контейнера.

```csharp
// Создание нового контейнера для каждого окна
var container = ServiceContainer.CreateContainer(objectsRepository, tabServiceProvider);

// Установка глобальных сервисов (опционально)
ServiceContainer.SetGlobalServices(objectsRepository, tabServiceProvider);
```

**Ключевые особенности**:
- Каждое окно получает свой собственный контейнер
- Внешние сервисы (IObjectsRepository, ITabServiceProvider) регистрируются в каждом контейнере
- Базовые сервисы настраиваются один раз для всех контейнеров

### 2. Фабрика ViewModels (ViewModelFactory)

**Расположение**: `src/Model/Services/ViewModelFactory.cs`

Фабрика создает ViewModels с использованием dependency injection:

```csharp
public interface IViewModelFactory
{
    LookUpVM CreateLookUpVM(ObjectSet dataObjects = null);
    SearchVM CreateSearchVM();
    TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject);
    AttrVM CreateAttrVM(PilotObjectHelper selectedObject);
    MainVM CreateMainVM();
}
```

### 3. Провайдер ViewModels (ViewModelProvider)

**Расположение**: `src/Model/Services/ViewModelProvider.cs`

Создает ViewModels, которые требуют параметры в конструкторе:

```csharp
public interface IViewModelProvider
{
    LookUpVM CreateLookUpVM(ObjectSet dataObjects = null);
    TaskTreeVM CreateTaskTreeVM(PilotObjectHelper selectedObject);
    AttrVM CreateAttrVM(PilotObjectHelper selectedObject);
    MainVM CreateMainVM(INavigationService navigationService, IViewModelFactory viewModelFactory);
}
```

### 4. Создатель SearchVM (SearchViewModelCreator)

**Расположение**: `src/Model/Services/SearchViewModelCreator.cs`

Специализированный сервис для создания SearchVM:

```csharp
public interface ISearchViewModelCreator
{
    SearchVM CreateSearchVM(INavigationService navigationService);
}
```

### 5. Сервис навигации (NavigationService)

**Расположение**: `src/Model/Services/NavigationService.cs`

Управляет навигацией между страницами в MainVM:

```csharp
public interface INavigationService
{
    void NavigateToLookUp(ObjectSet selectedObjects = null);
    void NavigateToSearch();
    void NavigateToTaskTree(PilotObjectHelper selectedObject);
    void NavigateToAttr(PilotObjectHelper selectedObject);
}
```

## Регистрация сервисов

### Базовые сервисы (регистрируются в каждом контейнере)

```csharp
// Основные сервисы
container.Register<IRepoService, RepoService>(Lifestyle.Singleton);
container.Register<ICustomSearchService, SearchService>(Lifestyle.Singleton);
container.Register<ITabService, TabService>(Lifestyle.Singleton);
container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
container.Register<ITreeItemService, TreeItemService>(Lifestyle.Singleton);
container.Register<IDataObjectService, DataObjectService>(Lifestyle.Singleton);

// Фабрики и провайдеры
container.Register<ISearchViewModelCreator, SearchViewModelCreator>(Lifestyle.Singleton);
container.Register<IViewModelProvider, ViewModelProvider>(Lifestyle.Singleton);
container.Register<INavigationService, NavigationService>(Lifestyle.Singleton);
container.Register<IViewModelFactory, ViewModelFactory>(Lifestyle.Singleton);

// ViewModels (только те, которые не требуют параметров)
container.Register<LookUpVM>(Lifestyle.Transient);

// Views
container.Register<MainView>(Lifestyle.Transient);
```

### Внешние сервисы (регистрируются для каждого окна)

```csharp
// Регистрируются как экземпляры для каждого окна
container.RegisterInstance(objectsRepository);
container.RegisterInstance(tabServiceProvider);
```

## Создание окна

Процесс создания нового окна:

```csharp
// 1. Создание контейнера для окна
var container = ServiceContainer.CreateContainer(objectsRepository, tabServiceProvider);

// 2. Получение сервиса навигации
var navigationService = container.GetInstance<INavigationService>();

// 3. Настройка начальной страницы
navigationService.NavigateToLookUp(selectedObjects);

// 4. Создание ViewModel и View
var viewModelFactory = container.GetInstance<IViewModelFactory>();
var mainVM = viewModelFactory.CreateMainVM();
var window = container.GetInstance<MainView>();
window.DataContext = mainVM;
window.Show();
```

## Преимущества архитектуры

1. **Изоляция окон**: Каждое окно имеет свой контейнер, что предотвращает конфликты
2. **Гибкость**: Легко добавлять новые ViewModels и сервисы
3. **Тестируемость**: Все зависимости инжектируются, что упрощает unit-тестирование
4. **Масштабируемость**: Архитектура поддерживает добавление новых функций
5. **Отсутствие блокировки контейнера**: Каждый контейнер валидируется только один раз

## Структура ViewModels

### MainVM
- Главная ViewModel приложения
- Управляет навигацией между страницами
- Содержит команды для переключения между вкладками

### LookUpVM
- Отображает список объектов
- Поддерживает поиск и фильтрацию
- Позволяет открывать новые окна для выбранных объектов

### SearchVM
- Предоставляет интерфейс для поиска объектов
- Использует ICustomSearchService для выполнения поиска

### TaskTreeVM
- Отображает дерево задач
- Показывает иерархию объектов

### AttrVM
- Отображает атрибуты выбранного объекта
- Позволяет редактировать свойства объекта

## Интеграция с Pilot-ICE MEF

App класс интегрируется с Pilot-ICE через MEF (Managed Extensibility Framework). Конструктор App получает зависимости от Pilot-ICE и сохраняет их в приватных полях:

```csharp
[ImportingConstructor]
public App(IObjectsRepository objectsRepository,
    ITabServiceProvider tabServiceProvider,
    IPilotDialogService pilotDialogService)
{
    AppDomain.CurrentDomain.AssemblyResolve += Resolver.ResolveAssembly;
    _objectsRepository = objectsRepository;
    _tabServiceProvider = tabServiceProvider;
    _theme = pilotDialogService.Theme;
}
```

Для доступа к нашим сервисам используется глобальный контейнер, созданный в конструкторе App. Это обеспечивает сохранение состояния сервисов между вызовами:

```csharp
private static Container _globalContainer;

[ImportingConstructor]
public App(IObjectsRepository objectsRepository,
    ITabServiceProvider tabServiceProvider,
    IPilotDialogService pilotDialogService)
{
    // Создаем глобальный контейнер для сервисов, которые должны сохранять состояние
    _globalContainer = ServiceContainer.CreateContainer(objectsRepository, tabServiceProvider);
}

private void ItemClick(string name)
{
    try
    {
        var menuService = _globalContainer.GetInstance<IMenuService>();
        menuService.HandleMenuItemClick(name);
    }
    catch (System.Exception ex)
    {
        // Обработка ошибок
    }
}
```

Этот подход обеспечивает:
- Совместимость с MEF системой Pilot-ICE
- Сохранение чистой архитектуры
- Изоляцию наших сервисов от внешних зависимостей
- Отсутствие конфликтов с MEF инъекцией зависимостей
- Сохранение состояния сервисов между вызовами (важно для SelectionService)
- Правильную работу LookSelected функциональности

## Принцип инверсии зависимостей (DIP)

### Фабрики для создания объектов

Для соблюдения принципа инверсии зависимостей созданы фабрики для создания объектов, которые требуют зависимостей:

#### IPilotObjectHelperFactory
```csharp
public interface IPilotObjectHelperFactory
{
    PilotObjectHelper Create(string name, string stringId, object lookUpObject, bool isLookable);
}
```

#### IObjectSetFactory
```csharp
public interface IObjectSetFactory
{
    ObjectSet Create(MemberInfo memberInfo);
}
```

### Внедрение зависимостей через конструкторы

Все сервисы теперь получают зависимости через конструкторы:

```csharp
public class SearchService : ICustomSearchService
{
    private readonly IObjectsRepository _objectsRepository;
    private readonly IObjectSetFactory _objectSetFactory;

    public SearchService(IObjectsRepository objectsRepository, IObjectSetFactory objectSetFactory)
    {
        _objectsRepository = objectsRepository;
        _objectSetFactory = objectSetFactory;
    }
}
```

### Использование фабрик

Вместо прямого создания объектов используется фабрика:

```csharp
// Было:
var objectSet = new ObjectSet(null);

// Стало:
var objectSet = _objectSetFactory.Create(null);
```

### Регистрация фабрик в DI

```csharp
container.Register<IPilotObjectHelperFactory, PilotObjectHelperFactory>(Lifestyle.Singleton);
container.Register<IObjectSetFactory, ObjectSetFactory>(Lifestyle.Singleton);
```

### Преимущества

1. **Устранение статических зависимостей**: Объекты больше не зависят от глобальных сервисов
2. **Тестируемость**: Легко создавать моки и тестировать компоненты изолированно
3. **Гибкость**: Можно легко заменить реализации через DI
4. **Соблюдение DIP**: Зависимости инжектируются через абстракции 