using System.Diagnostics.CodeAnalysis;

namespace ObjectCopyExample.ObjectCopy;

public static class ObjectCopyExtensions
{
    public static T InjectFrom<T, TS>([DisallowNull] this T target, [DisallowNull] TS source)
    {
        var sourceProperties = source.GetType().GetProperties().Where(pr => pr.CanRead).OrderBy(p => p.Name);
        var targetProperties = target.GetType().GetProperties().Where(pr => pr.CanWrite).OrderBy(p => p.Name);
        var targetFields = target.GetType().GetFields().OrderBy(p => p.Name);

        foreach (var sourceInfo in sourceProperties)
        {
            var targetInfo = targetProperties.SingleOrDefault(pi => pi.Name.Equals(sourceInfo.Name) &&
                pi.PropertyType == sourceInfo.PropertyType);

            if (targetInfo == null)
            {
                var targetField = targetFields.SingleOrDefault(fi => fi.Name.Equals(sourceInfo.Name) &&
                    fi.FieldType == sourceInfo.PropertyType);

                if (targetField != null)
                    targetField.SetValue(target, sourceInfo.GetValue(source));
            }
            else
                targetInfo.SetValue(target, sourceInfo.GetValue(source));
        }

        return target;
    }
}
