using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SortByBracket.Model
{
    public class PathAndWords
    {
        private List<string> _words;
        private string input;
        private string output;

        public PathAndWords()
        {
            input = "Input directory";
            output = "Output directory";
            _words = new List<string>();
        }

        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        public string Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
            }
        }

        public List<string> Words
        {
            get
            {
                return _words;
            }
        }
    }
}
