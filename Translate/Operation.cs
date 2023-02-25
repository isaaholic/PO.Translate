using Google.Cloud.Translation.V2;
using System.Diagnostics;
using System.Text;

namespace Translate
{
    public static class Operation
    {
        private static StreamReader? stream;
        private static readonly string path = Directory.GetCurrentDirectory() + "\\poFile\\co.po";
        private static int fileLineCount;
        //private static List<string> preparedList;

        public static string APIKey { private get; set; }
        public static string? ProjectName { get; set; }


        private static bool Open()
        {
            if (File.Exists(path))
            {
                stream = new StreamReader(path);
                fileLineCount = File.ReadAllLines(path).Length;
                return true;
            }
            return false;
        }

        public static async void Translate()
        {
            bool isOpen = Open();
            if (isOpen)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //preparedList = new List<string>()
                //{
                //    "msgid \"\"",
                //    "msgstr \"\"",
                //    "\"MIME-Version: 1.0\\n\"",
                //    "\"Content-Type: text/plain; charset=UTF-8\\n\"",
                //    "\"Content-Transfer-Encoding: 8bit\\n\"",
                //    "\"X-Generator: isaaholic\\n\"",
                //    $"\"Project-Id-Version: {ProjectName}\\n\"",
                //    "\"Language: az\\n\""
                //};

                if (File.Exists(Directory.GetCurrentDirectory() + "\\poFile\\new.po"))
                {
                    File.Delete(Directory.GetCurrentDirectory() + "\\poFile\\new.po");
                }
                {
                    using var sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\poFile\\new.po");
                }
                List<string> lines = new();
                //foreach (var line in preparedList)
                //{
                //    lines.Add(line);
                //    stream.ReadLine();
                //}
                var client = TranslationClient.CreateFromApiKey(APIKey);
                while (true)
                {
                    StringBuilder unTranslatedLine = new();
                    StringBuilder poLine = new StringBuilder();
                    TranslationResult? response;
                    string detectedLine;
                    bool getOver = false;

                    var line = stream.ReadLine();

                    if (line == null)
                        break;

                    if (line.Length > 5)
                        detectedLine = line.Remove(5, line.Length - 5);
                    else detectedLine = line;

                    switch (detectedLine)
                    {
                        case "msgid":
                            poLine.Append("msgid");
                            foreach (var character in line.Remove(0, 5))
                            {
                                poLine.Append(character);
                                unTranslatedLine.Append(character);
                            }
                            lines.Add(poLine.ToString());
                            if (poLine.ToString()[poLine.ToString().Length - 3] == '\\' && poLine.ToString()[poLine.ToString().Length - 2] == 'n')
                            {
                                getOver = true;
                                poLine.Clear();
                                line = stream.ReadLine();
                                unTranslatedLine.AppendLine();
                                foreach (var character in line)
                                {
                                    poLine.Append(character);
                                    unTranslatedLine.Append(character);
                                }
                                lines.Add(poLine.ToString());
                            }
                            poLine.Clear();

                            line = stream.ReadLine();
                            detectedLine = line.ToString().Remove(5, line.Length - 5);
                            if (detectedLine == "msgst")
                            {
                                poLine.Append("msgstr");
                                if (unTranslatedLine[unTranslatedLine.Length - 1] == unTranslatedLine[unTranslatedLine.Length - 2] && unTranslatedLine[unTranslatedLine.Length - 1] == '"')
                                {
                                    response = client.TranslateText(unTranslatedLine.ToString().Remove(unTranslatedLine.Length - 1, 1), LanguageCodes.Azerbaijani, LanguageCodes.English);
                                    poLine.Append(response.TranslatedText+'"');
                                }
                                else
                                {
                                    response = client.TranslateText(unTranslatedLine.ToString(), LanguageCodes.Azerbaijani, LanguageCodes.English);
                                    poLine.Append(response.TranslatedText.Replace('“', '"').Replace('”', '"'));
                                }
                                lines.Add(poLine.ToString());
                                if (getOver == true)
                                    stream.ReadLine();
                            }
                            else
                            {
                                poLine.Append(line);
                                lines.Add(poLine.ToString());
                            }
                            break;
                        case "msgst":
                            unTranslatedLine.Append(line);
                            response = client.TranslateText(unTranslatedLine.ToString(), LanguageCodes.Azerbaijani, LanguageCodes.English);
                            poLine.Append(response.TranslatedText.Replace('“','"').Replace('”','"'));
                            lines.Add(poLine.ToString());
                            break;
                        default:
                            lines.Add(line);
                            break;
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Progress - {lines.Count}/{fileLineCount}");
                }
                await File.WriteAllLinesAsync(Directory.GetCurrentDirectory() + "\\poFile\\new.po", lines);
                stopwatch.Stop();
                Console.WriteLine($"Time Elapsed: {stopwatch.Elapsed}");
            }
        }
    }
}