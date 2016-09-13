using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotOne.Helpers
{
    public static class LangAnalyzers
    {
        public static Dictionary<string, Guid> List
        {
            get
            {
                var s = new Dictionary<string, Guid>();
                s.Add("POS_Tags", Guid.Parse("4fa79af1-f22c-408d-98bb-b7d7aeef7f04"));
                s.Add("Tree", Guid.Parse("22a6b758-420f-4745-8a3c-46835a67c0d2"));
                s.Add("Tokens", Guid.Parse("08ea174b-bfdb-4e64-987e-602f85da7f72"));
                return s;
            }
        }
    }
    public class LAResponse
    {
        public Guid analyzerID { get; set; }
        public object result { get; set; }
    }
    public class ReplacementPair
    {
        public string Source { get; set; }
        public string Target { get; set; }
    }
}