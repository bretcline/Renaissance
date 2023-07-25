using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micros.DataLoader.Parsers
{
    public class LineParser
    {
        private Dictionary<string, int> m_Elements = null;
        private Dictionary<string, string> m_Replacements = null;

        public int ParseOrder { get; set; }
        public bool SingleElement { get; set; }
        public string ItemIndex { get; set; }
        public string Replacements { get; set; }
        public string DescriptionPrefix { get; set; }
        public string DescriptionSuffix { get; set; }

        public string RegEx { get; set; }
        public string PropertyName { get; set; }

        public Dictionary<string, int> GetElements()
        {
            if (null == m_Elements)
            {
                m_Elements = new Dictionary<string, int>();
                if (!string.IsNullOrWhiteSpace(ItemIndex))
                {
                    var elements = ItemIndex.Split(',');
                    foreach (var element in elements)
                    {
                        var values = element.Split('=');
                        m_Elements[values[0]] = int.Parse(values[1]);
                    }
                }
                else
                {
                    m_Elements[this.PropertyName] = 2;
                }
            }
            return m_Elements;
        }
        public Dictionary<string, string> GetReplacementValues()
        {
            if (null == m_Replacements && !string.IsNullOrWhiteSpace(Replacements))
            {
                m_Replacements = new Dictionary<string, string>();
                var elements = Replacements.Split(',');
                foreach (var element in elements)
                {
                    var values = element.Split('=');
                    m_Replacements[values[0]] = values[1];
                }
            }
            return m_Replacements;
        }
    }
    
    public class TransactionElements
    {
        public int TransactionType { get; set; }
        public string TextIdentifier { get; set; }
    }


    public class ParserConfig
    {
        public List<LineParser> Cleanup { get; set; }
        public string SectionMarker { get; set; }
        public List<List<LineParser>> SectionParsers { get; set; }
        public string DateFormat { get; set; }

        public List<TransactionElements> TransactionTypes { get; set; }
    }
}
