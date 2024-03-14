using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
//using S

namespace BinanceTikerPrice
{
    internal class Program
    {
        static HttpClient HttpClient = new HttpClient();

        static async Task<string> BinanceTiker()
        {
            UriBuilder uriBilder = new UriBuilder("https", "www.binance.com", 443, "fapi/v1/ticker/price");
            HttpResponseMessage response = await HttpClient.GetAsync(uriBilder.Uri);
            var result = response.Content.ReadAsStringAsync();

            //Console.WriteLine(await response.Content.ReadAsStringAsync());

            return await result.ConfigureAwait(false);

        }
        static async Task Main()
        {
            while (true)
            {
                var tick = await BinanceTiker();
                using JsonDocument doc = JsonDocument.Parse(tick);
                JsonElement root = doc.RootElement;

                var cur = root.EnumerateArray();

                while (cur.MoveNext())
                {
                    var c = cur.Current;
                    var props = c.EnumerateObject();
                    bool flag = false;
                    while (props.MoveNext())
                    {
                        var prop = props.Current;

                        if (prop.Name == "symbol")
                            if (prop.Value.ToString() == "BTCUSDT")
                                flag = true;

                        if (flag)
                            Console.Write($"{prop.Name}: {prop.Value}\t");
                    }
                    flag = false;
                }
                Console.WriteLine();
                Thread.Sleep(2000);
            }
        }
    }
}
