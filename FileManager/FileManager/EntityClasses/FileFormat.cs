namespace FileManager.EntityClasses
{
    class FileFormat
    {
        public string Name { get; set; }

        public string Format { get; set; }

        public string FullName {
            get
            {
                if (Format == "*") return Name;
                return Name + "(" + Format + ")";
            }
        }
    }

    static class FileFormats
    {
        public static FileFormat TextFormat = new FileFormat() { Name = "Text Files", Format = "*.txt" };
        public static FileFormat HtmlFormat = new FileFormat() { Name = "Html Files", Format = "*.html"};
        public static FileFormat XmlFormat = new FileFormat() { Name = "Xml Files", Format = "*.xml" };
        public static FileFormat AnyFormat = new FileFormat() { Name = "All Files", Format = "*" };
    }
}
