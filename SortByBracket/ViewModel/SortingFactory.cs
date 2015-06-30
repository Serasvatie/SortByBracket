using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SortByBracket.ViewModel
{
    public class SortingFactory
    {
        private Model.PathAndWords model;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private string[] output;
        private string[] input;

        public SortingFactory(Model.PathAndWords model, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            // TODO: Complete member initialization
            this.model = model;
            this.backgroundWorker = backgroundWorker;
        }

        public void DoSort()
        {
            output = Directory.GetDirectories(model.Output);
            input = Directory.GetDirectories(model.Input);
            for (int i = 0; i < input.Length; i++)
            {
                Console.WriteLine(i);
                if (checkException(input[i]) == 0)
                {
                    string author = getBracket(input[i]);
                    Console.WriteLine(author);
                    if (author != "")
                        moveDirectory(input[i], author);
                }
                backgroundWorker.ReportProgress((i * 100) / input.Length);
                output = Directory.GetDirectories(model.Output);
                input = Directory.GetDirectories(model.Input);
            }
        }

        private void moveDirectory(string p, string author)
        {
            bool toMove = false;
            string tmp = Path.GetDirectoryName(p);
            for (int i = 0; i < output.Length; i++)
            {
                if (tmp == author)
                {
                    toMove = true;
                    break;
                }
            }
            if (toMove == false)
            {
                Directory.CreateDirectory(author);
            }
            Directory.Move(p, author);
        }

        private string getBracket(string p)
        {
            string tmp = Path.GetDirectoryName(p);
            return Regex.Match(tmp, @"\[([^)]*)\]").Value;
        }

        private int checkException(string p)
        {
            string tmp = Path.GetFileName(p);
            foreach (string exception in this.model.Words)
            {
                if (tmp.Contains(exception))
                {
                    return -1;
                }
            }
            return 0;
        }
    }
}
