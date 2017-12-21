using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LivingDoc.MDMerge
{
    public class MDStruct
    {
        private string _headerPattern = null;
        private Regex _headerRegex = null;

        public string HeaderPattern
        {
            get
            {
                return _headerPattern;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("Шаблон имени заголовка не может быть пустым");

                _headerPattern = value;
                _headerRegex = new Regex(_headerPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
        }
   
        public MDMergeStrategy MergeStrategy { get; protected set; }  
        
        public List<MDStruct> Substruct { get; protected set; } 

        public MDStruct()
        {
            this.Substruct = new List<MDStruct>();
        }

        public static MDStruct LoadFromXml(XElement xml)
        {
            MDStruct mdStruct = new MDStruct();

            XAttribute xHeader = xml.Attribute("header");
            if (xHeader == null || string.IsNullOrWhiteSpace(xHeader.Value))
            {
                throw new FormatException("Шаблон заголовка не может быть пустым (элемент: //mdstruct[header])");
            }
            mdStruct.HeaderPattern = xHeader.Value;

            XAttribute xStrategy = xml.Attribute("strategy");
            if (xStrategy != null && !string.IsNullOrWhiteSpace(xStrategy.Value))
            {
                mdStruct.MergeStrategy = (MDMergeStrategy)Enum.Parse(typeof(MDMergeStrategy), (string) xStrategy.Value, true);
            }    
                                                        
            foreach (XElement child in xml.Elements("paragraph"))
            {
                mdStruct.Substruct.Add(LoadFromXml(child));
            }

            return mdStruct;
        }

        public bool IsMatchHeader(string header)
        {
            if (this._headerRegex == null)
                throw new InvalidOperationException("Шаблон заголовка не задан.");

            return _headerRegex.IsMatch(header);
        }
    }
}
