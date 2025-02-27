using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCS;
using static GrokCS.ParseGrokResponse;
using static GrokCS.GetRateLimit;

var rateLimit = new GetRateLimit();
string result = await rateLimit.Execute(); // Await the async method
Console.WriteLine(result);