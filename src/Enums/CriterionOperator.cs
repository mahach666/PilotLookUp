namespace PilotLookUp.Enums
{
    public enum CriterionOperator
    {
        Equals,
        NotEquals,
        ContainsAll,
        NotContainsAll,
        InRange,
        Exists,
        NotExists
    }

    public static class CriterionOperatorExtensions
    {
        // Отрицательные операторы строят тот же терм, что и их позитивная пара,
        // но строка уходит в MustNot вместо Must
        public static bool IsNegative(this CriterionOperator op) =>
            op == CriterionOperator.NotEquals
            || op == CriterionOperator.NotContainsAll
            || op == CriterionOperator.NotExists;

        public static CriterionOperator ToPositive(this CriterionOperator op) => op switch
        {
            CriterionOperator.NotEquals => CriterionOperator.Equals,
            CriterionOperator.NotContainsAll => CriterionOperator.ContainsAll,
            CriterionOperator.NotExists => CriterionOperator.Exists,
            _ => op
        };
    }
}
