namespace PilotLookUp.Enums
{
    public enum OccurKind
    {
        // Обязательное условие (Must); отрицание задается оператором («Не равно»...)
        Must,
        // Альтернатива: все Should-строки собираются в один MustAnyOf
        Should
    }
}
