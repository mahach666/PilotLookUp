namespace PilotLookUp.Core
{
    public static class PilotTimeouts
    {
        // Страховочный таймаут ожидания объекта/истории, мс. Отсутствие объекта
        // определяется по NonExistent, а не по таймауту; SDK доставляет ответы
        // через UI-поток, который при открытии окна занят ~1 с
        public const int ObjectLoadMs = 5000;
    }
}
