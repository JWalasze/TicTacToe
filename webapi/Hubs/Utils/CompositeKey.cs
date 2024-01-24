namespace webapi.Hubs.Utils;

public class CompositeKey<T>
{
    public T Key1 { get; }

    public T Key2 { get; }

    public CompositeKey(T key1, T key2)
    {
        Key1 = key1;
        Key2 = key2;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key1, Key2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is CompositeKey<T> otherKey)
            return (Equals(Key1, otherKey.Key1) ||
                    Equals(Key1, otherKey.Key2)) &&
                   (Equals(Key2, otherKey.Key1) ||
                    Equals(Key2, otherKey.Key2));

        return false;
    }
}