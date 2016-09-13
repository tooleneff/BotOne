using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BotOne.Helpers
{
    public  class WordsHelper
    {
        private readonly  string AdjectiveFile = HttpContext.Current.Server.MapPath("~/Text/") + "Adjectives.txt";
        private readonly string NounsFile = HttpContext.Current.Server.MapPath("~/Text/") + "Nouns.txt";
        public WordsHelper()
        {
            if (!File.Exists(NounsFile))
            {
                File.Create(NounsFile);
            }
            if (!File.Exists(AdjectiveFile))
            {
                File.Create(AdjectiveFile);
            }
        }
        public  string  GetNoun()
        {
            
            var a = File.ReadAllLines(NounsFile);
            if (a.Length==0)
            {
                return "";
            }
            Random b = new Random(1);
            var c = b.Next(0, a.Length - 1);
            return a[c];
        }
        public void AddNoun(string item)
        {
            var a = File.ReadAllLines(NounsFile);
            if (!a.Contains(item))
            {
                File.AppendAllLines(NounsFile,new string[1] { item });
            }
        }
        public void AddAdjective(string item)
        {
            var a = File.ReadAllLines(AdjectiveFile);
            if (!a.Contains(item))
            {
                File.AppendAllLines(AdjectiveFile, new string[1] { item });
            }
        }
        public string GetAdjective()
        {
            var a = File.ReadAllLines(AdjectiveFile);
            if (a.Length == 0)
            {
                return "";
            }
            Random b = new Random(1);
            var c = b.Next(0, a.Length - 1);
            return a[c];
        }
    }
}