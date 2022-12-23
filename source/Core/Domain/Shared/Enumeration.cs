
using System.Reflection;

namespace Domain.Shared;

/// <summary>
/// An extensible value type that declares a set of named constants
/// </summary>
/// <remarks>Mimics the enum as a class</remarks>
/// <typeparam name="TEnum">The type of the enum</typeparam>
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    /// <summary>
    /// A list of the potential values of the Enumeration type
    /// </summary>
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    /// <summary>
    /// The value of the integral type
    /// </summary>
    public int Id { get; protected init; }
    /// <summary>
    /// The name of the integral type
    /// </summary>
    public string Name { get; protected init; } = string.Empty;

    /// <summary>
    /// Constructor requires all values to be populated
    /// </summary>
    /// <param name="value">value of the integral type</param>
    /// <param name="name">name of the integral type</param>
    protected Enumeration(int value, string name)
    {
        Id = value;
        Name = name;
    }

    /// <summary>
    /// Retrieves an integral type from it's associated value
    /// </summary>
    /// <param name="value">value of the integral type</param>
    /// <returns>an integral type</returns>
    public static TEnum? FromValue(int value)
    {
        return Enumerations.TryGetValue(
            value, 
            out TEnum? enumeration) 
                ? enumeration 
                : default;
    }
    /// <summary>
    /// Retrieves an integral type from it's associated name
    /// </summary>
    /// <param name="name">name of the integral type</param>
    /// <returns>an integral type</returns>
    public static TEnum? FromName(string name)
    {
        return Enumerations
            .Values
            .SingleOrDefault(x => x.Name == name);
    }
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
            return false;

        return GetType() == other.GetType() &&
            Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Use reflection to find public static fields and assign them to a dictionary representing all the integral values of an Enumeration
    /// </summary>
    /// <returns>a dictionary of fields in the Enum</returns>
    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Id);
    }
    public static IEnumerable<TEnum> GetValues()
    {
        return Enumerations.Values;
    }
}
