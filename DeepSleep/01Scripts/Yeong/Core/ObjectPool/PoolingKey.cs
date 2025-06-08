using System;
using UnityEngine;


public class PoolingKey : IEquatable<PoolingKey>
{
    public string Key { get; }
    public Enum Enum { get; }
    public PoolingKey(Enum enumValue)
    {
        Key = $"{enumValue}";
        Enum = enumValue;
    }

    public override string ToString() => Key;

    public override bool Equals(object obj) => obj is PoolingKey other && Key == other.Key;
    public bool Equals(PoolingKey other) => Key == other.Key;
    public override int GetHashCode() => Key.GetHashCode();

    public static implicit operator string(PoolingKey key) => key.Key;
}
