using System.Diagnostics.CodeAnalysis;

/// <summary>
/// https://themuuj.com/blog/2020/08/csharp-collection-deconstructing/
/// </summary>
public static class DeconstructExtensions {

  // deconstructs a collection to two values
  public static void Deconstruct<T>(
      this IEnumerable<T> collection,
      [MaybeNull] out T item1,
      [MaybeNull] out T item2) {

    var enumerator = collection.GetEnumerator();
    item1 = Next(enumerator);
    item2 = Next(enumerator);
  }

  // deconstructs a collection to three values
  public static void Deconstruct<T>(
      this IEnumerable<T> collection,
      [MaybeNull] out T item1,
      [MaybeNull] out T item2,
      [MaybeNull] out T item3) {

    var enumerator = collection.GetEnumerator();
    item1 = Next(enumerator);
    item2 = Next(enumerator);
    item3 = Next(enumerator);
  }

  // deconstructs a collection to four values
  public static void Deconstruct<T>(
      this IEnumerable<T> collection,
      [MaybeNull] out T item1,
      [MaybeNull] out T item2,
      [MaybeNull] out T item3,
      [MaybeNull] out T item4) {

    var enumerator = collection.GetEnumerator();
    item1 = Next(enumerator);
    item2 = Next(enumerator);
    item3 = Next(enumerator);
    item4 = Next(enumerator);
  }

  // deconstructs a collection to five values
  public static void Deconstruct<T>(
      this IEnumerable<T> collection,
      [MaybeNull] out T item1,
      [MaybeNull] out T item2,
      [MaybeNull] out T item3,
      [MaybeNull] out T item4,
      [MaybeNull] out T item5) {

    var enumerator = collection.GetEnumerator();
    item1 = Next(enumerator);
    item2 = Next(enumerator);
    item3 = Next(enumerator);
    item4 = Next(enumerator);
    item5 = Next(enumerator);
  }

  // deconstructs a collection to six values
  public static void Deconstruct<T>(
      this IEnumerable<T> collection,
      [MaybeNull] out T item1,
      [MaybeNull] out T item2,
      [MaybeNull] out T item3,
      [MaybeNull] out T item4,
      [MaybeNull] out T item5,
      [MaybeNull] out T item6) {

    var enumerator = collection.GetEnumerator();
    item1 = Next(enumerator);
    item2 = Next(enumerator);
    item3 = Next(enumerator);
    item4 = Next(enumerator);
    item5 = Next(enumerator);
    item6 = Next(enumerator);
  }

  // helper method to advance enumerator and return next value
  [return: MaybeNull]
  private static T Next<T>(IEnumerator<T> enumerator) {
    return enumerator.MoveNext() ? enumerator.Current : default;
  }
}