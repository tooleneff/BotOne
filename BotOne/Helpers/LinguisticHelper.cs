using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProjectOxford.Linguistics;
using Microsoft.ProjectOxford.Linguistics.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BotOne.Helpers
{
    public class LinguisticHelper
    {
        private static readonly LinguisticsClient Client = new LinguisticsClient("f9bb567f3862439e80cb14cb487e23e7");

        
        private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public async Task<string> ChangeWords(string inputString)
        {
            var Dict = new Dictionary<string, List<string>>();
            List<ReplacementPair> pairs = new List<ReplacementPair>();
            
            // Analyze text with all available analyzers
            var analyzeTextRequest = new AnalyzeTextRequest()
            {
                Language = "en",
                AnalyzerIds = new Guid[] {LangAnalyzers.List["Tree"]},
                Text = inputString
            };

            var analyzeTextResults = await AnalyzeText(analyzeTextRequest);
            var resultsAsJson = JsonConvert.SerializeObject(analyzeTextResults, Formatting.Indented, jsonSerializerSettings);
            var resultAsObject = JsonConvert.DeserializeObject<List<LAResponse>>(resultsAsJson);
            string[] posResults = new string[0];
            foreach (var item in resultAsObject)
            {
                if (item.analyzerID == LangAnalyzers.List["Tree"])
                {
                    posResults = item.result.ToString().Split('(', ')');

                    Regex ItemRegex = new Regex(@"\((\w+) (\w+)\)", RegexOptions.Compiled);
                    foreach (Match ItemMatch in ItemRegex.Matches(item.result.ToString()))
                    {
                        var x = ItemMatch.Groups[1].ToString();
                        var y = ItemMatch.Groups[2].ToString();
                        if (!Dict.ContainsKey(x)) Dict.Add(x, new List<string>());
                        Dict[x].Add(y);
                    }

                }
            }

            foreach (var item in posResults)
            {
                var wHelper = new WordsHelper();
                if (item.Contains("JJ "))
                {
                    var s = item.Replace("JJ ", "");
                    var pair = new ReplacementPair { Source = s, Target = wHelper.GetAdjective() };
                    wHelper.AddAdjective(s);
                    if (pairs.Count(x=>x.Source==s)==0) pairs.Add(pair);
                }
                else if (item.Contains("NN "))
                {
                    var s = item.Replace("NN ", "");
                    var pair = new ReplacementPair { Source = s, Target = wHelper.GetNoun() };
                    wHelper.AddNoun(s);
                    if (pairs.Count(x => x.Source == s) == 0) pairs.Add(pair);
                }
            }
            string outputString = inputString;
            foreach (var item in pairs)
            {
                outputString = outputString.Replace(item.Source, item.Target);
            }
            return outputString;
        }

        /// <summary>
        /// List analyzers synchronously.
        /// </summary>
        /// <returns>An array of supported analyzers.</returns>
        private static Analyzer[] ListAnalyzers()
        {
            try
            {
                return Client.ListAnalyzersAsync().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to gather list of analyzers", exception as ClientException);
            }
        }

        /// <summary>
        /// Analyze text synchronously.
        /// </summary>
        /// <param name="request">Analyze text request.</param>
        /// <returns>An array of analyze text result.</returns>
        private static async Task<AnalyzeTextResult[]> AnalyzeText(AnalyzeTextRequest request)
        {
            try
            {
                return await Client.AnalyzeTextAsync(request);
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to analyze text", exception as ClientException);
            }
        }
    }
}