namespace PlainGOAP
{
    public static class ArrayComparer
    {
        public static int GetHashCode<T>(T[] array)
        {
            if (array != null)
                unchecked
                {
                    var hash = 17;

                    foreach (var item in array)
                        hash = hash * 23 + (item != null ? item.GetHashCode() : 0);

                    return hash;
                }

            return 0;
        }

        public static bool Equals<T>(T[] firstArray, T[] secondArray)
        {
            if (ReferenceEquals(firstArray, secondArray))
                return true;

            if (firstArray == null || secondArray == null || firstArray.Length != secondArray.Length)
                return false;

            for (var i = 0; i < firstArray.Length; i++)
                if (!Equals(firstArray[i], secondArray[i]))
                    return false;

            return true;

        }
    }
}
