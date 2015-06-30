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
                    if (author != "")
                        moveDirectory(input[i], author);
                }
                backgroundWorker.ReportProgress((i * 100) / input.Length - 1);
                output = Directory.GetDirectories(model.Output);
                input = Directory.GetDirectories(model.Input);
            }
        }

        private void moveDirectory(string p, string author)
        {
            bool toMove = false;
            int i = 0;
            while (i < output.Length)
            {
                if (Path.GetFileName(output[i]) == author)
                {
                    toMove = true;
                    break;
                }
                i++;
            }
            if (toMove == false)
            {
                Directory.CreateDirectory(model.Output + '\\' + author);
                Directory.Move(p, model.Output + "\\" + author + "\\" + Path.GetFileName(p));
            }
            else
                Directory.Move(p, output[i] + "\\" + Path.GetFileName(p));
        }

        private string getBracket(string p)
        {
            string tmp = Path.GetFileName(p);
            return Regex.Match(tmp, @"(?<=\[)(.*?)(?=\])").Groups[0].Value;
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
