using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Utils;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Model;

internal record BaseSubject
{
    private string Subject { get; }
    public Lazy<SubjectType> SubjectType { get; }

    private BaseSubject(string subject)
    {
        Subject = subject;
        SubjectType = new Lazy<SubjectType>(() => SubjectUtils.GetSubjectType(subject));
    }

    public static BaseSubject Create(string subject) => new(subject);

    public override string ToString()
    {
        var subjectType = SubjectType.Value.GetStringValue<int>();

        var s = Subject == "\"\"" ? "\"\"" : $"\"{Subject}\"";

        return $"{subjectType.StringValue} {s}";
    }
}
