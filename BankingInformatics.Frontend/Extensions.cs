namespace BankingInformatics.Frontend;

public static class Extensions
{
    public static IEnumerable<IEnumerable<(T Value, int Index, int ColumnWidth)>> SepareteIntoRows<T>(
        this IEnumerable<T> items, int itemsInRow) => items
        .Select((item, index) => (item, index))
        .GroupBy(pair => pair.index / itemsInRow)
        .Select(group => group
            .Select(item => (item.item, item.index, 12 / itemsInRow)));

    public static string CutIfMoreThan(this string text, int count) => text.Length < count ?
        text :
        $"{string.Concat(text.Take(count))}...";
}
