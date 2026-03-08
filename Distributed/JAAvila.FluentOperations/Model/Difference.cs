namespace JAAvila.FluentOperations.Model;

internal record Difference(int Row, int Index)
{
    public static Difference New(int row, int index) => new(row, index);
}
