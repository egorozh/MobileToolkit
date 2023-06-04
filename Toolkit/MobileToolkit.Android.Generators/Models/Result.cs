namespace MobileToolkit.Android.Generators.Models;


internal sealed record Result<TValue>(TValue Value) where TValue : IEquatable<TValue>?;