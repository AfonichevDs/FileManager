using FileManager.EntityClasses;

namespace FileManager
{
    class Buffer
    {
        public IExplorerObject ExpObj { get; private set; }
        public bool IsCuted { get; set; }

        private static Buffer instance;

        private Buffer()
        { }

        public static Buffer getInstance()
        {
            if (instance == null)
                instance = new Buffer();
            return instance;
        }

        public void Push(IExplorerObject item, bool IsCuted)
        {
            ExpObj = item;
            this.IsCuted = IsCuted;
        }
    }
}
