
namespace MyApp
{
    public static class Extensions
    {
        public static T GetMax<T>(this IEnumerable<T> collection, Func<T, float> getValue) where T : class
        {
            float maxValue = float.MinValue;
            T maxElement = null;

            foreach (T item in collection)
            {
                float value = getValue(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = item;
                }
            }
            Console.WriteLine($"Максимальный элемент: {maxElement}");
            return maxElement;
        }
    }

    public class MyClass
    {
        public int Value { get; set; }
    }

    // -------------------------------------------------------------------
    public class FileSearcher
    {
        public event EventHandler<FileFoundEventArgs> FileFound;

        public bool _cancelSearch;

        public void SearchDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                _cancelSearch = false;
                SearchDirectoryInternal(directory);
            }
            else
            {
                Console.WriteLine($"Директория {directory} не существует.");
            }
        }

        private void SearchDirectoryInternal(string directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                OnFileFound(new FileFoundEventArgs(file));
                if (_cancelSearch)
                    return;
            }

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                SearchDirectoryInternal(subDirectory);
                if (_cancelSearch)
                    return;
            }
        }

        protected virtual void OnFileFound(FileFoundEventArgs e)
        {
            FileFound?.Invoke(this, e);
        }
    }

    public class FileFoundEventArgs : EventArgs
    {
        public string FileName { get; }

        public FileFoundEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }

    // -------------------------------------------------------------------

    class Program
    {
        static void Main(string[] args)
        {
            // Пример использования функции GetMax
            List<MyClass> list =
            [
                new MyClass { Value = 10 },
                new MyClass { Value = 5 },
                new MyClass { Value = 20 },
                new MyClass { Value = 15 }
            ];

            MyClass maxElement = list.GetMax(x => x.Value);
            


            // Вывод сообщений при срабатывании событий
            FileSearcher searcher = new FileSearcher();
            searcher.FileFound += EventFileFound;
            searcher.SearchDirectory(@"C:\Users\hunimuni\Downloads");

            void EventFileFound(object sender, FileFoundEventArgs e)
            {
                Console.WriteLine($"Найден файл: {e.FileName}");

                // Условие для отмены поиска
                if (e.FileName.EndsWith(".txt"))
                {
                    ((FileSearcher)sender)._cancelSearch = true;
                }
            }
        }
    }
    
}