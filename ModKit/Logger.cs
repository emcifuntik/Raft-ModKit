using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ModKit
{
    public class Logger
    {
        private String logFilePath = null;

        public Logger Debug
        {
            get
            {
                Print("[Debug] ");
                return this;
            }
            set { }
        }

        public Logger Error
        {
            get
            {
                Print("[Error] ");
                return this;
            }
            set { }
        }

        public Logger Info
        {
            get
            {
                Print("[Info] ");
                return this;
            }
            set { }
        }
        public Logger(String logFile)
        {
            logFilePath = logFile;

        }
        public void Print(String text)
        {
            File.AppendAllText(logFilePath, text);
        }

        public void PrintLn(String text)
        {
            File.AppendAllText(logFilePath, text + Environment.NewLine);
        }

        public static Logger operator+(Logger l, String text) {
            l.PrintLn(text);
            return l;
        }
    }
}
