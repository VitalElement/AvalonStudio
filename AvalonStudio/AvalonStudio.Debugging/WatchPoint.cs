namespace AvalonStudio.Debugging
{
    using Mono.Debugging.Client;
    using System.Xml;

    public class WatchPoint : BreakEvent
    {
        public WatchPoint(string expression)
        {
            TriggerOnWrite = true;
            Expression = expression;
        }

        protected WatchPoint(XmlElement elem, string baseDir) : base(elem, baseDir)
        {
            string s = elem.GetAttribute("expression");

            if (!string.IsNullOrEmpty(s))
            {
                Expression = s;
            }

            s = elem.GetAttribute("read");

            bool read = false;

            if (string.IsNullOrEmpty(s) || !bool.TryParse(s, out read))
            {
                TriggerOnRead = false;
            }
            else
            {
                TriggerOnRead = read;
            }

            s = elem.GetAttribute("write");

            bool write = false;

            if (string.IsNullOrEmpty(s) || !bool.TryParse(s, out write))
            {
                TriggerOnWrite = true;
            }
            else
            {
                TriggerOnWrite = write;
            }
        }

        public override XmlElement ToXml(XmlDocument doc, string baseDir)
        {
            XmlElement elem = base.ToXml(doc, baseDir);

            if (!string.IsNullOrEmpty(Expression))
            {
                elem.SetAttribute("expression", Expression);
            }

            elem.SetAttribute("read", TriggerOnRead.ToString());
            elem.SetAttribute("write", TriggerOnWrite.ToString());

            return elem;
        }

        public string Expression { get; set; }

        public bool TriggerOnRead { get; set; }

        public bool TriggerOnWrite { get; set; }
    }
}
