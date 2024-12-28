namespace FredsWorkmate.Util
{
    public static class ListExtensions
    {
        public static void MoveItem<T>(this List<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];

            list.RemoveAt(oldIndex);

            if (newIndex > oldIndex)
            {
                // the actual index could have shifted due to the removal
                newIndex--;
            }

            list.Insert(newIndex, item);
        }

        public static void MoveItem<T>(this List<T> list, T item, int newIndex)
        {
            var oldIndex = list.IndexOf(item);
            if (oldIndex > -1)
            {
                list.RemoveAt(oldIndex);

                if (newIndex > oldIndex)
                {
                    // the actual index could have shifted due to the removal
                    newIndex--;
                }

                list.Insert(newIndex, item);
            }
        }
    }
}
