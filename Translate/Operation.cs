using Google.Cloud.Translation.V2;
using System.Diagnostics;
using System.Text;

namespace Translate
{
    // 1.1.0
    public static class Operation
    {
        private static StreamReader? stream;
        private static List<string> lines = new List<string>();

        public static string Path { get; set; } // path of Folder
        public static string APIKey { private get; set; } // Google Cloud API Key
        public static string FileName { get; set; } // without .po
        public static int FileLineCount { get; set; } // count of lines of file


        private static bool Open()
        {
            if (File.Exists(Path + "\\" + FileName + ".po"))
            {
                stream = new StreamReader(Path + "\\" + FileName + ".po");
                fileLineCount = File.ReadAllLines(Path + "\\" + FileName + ".po").Length;
                return true;
            }
            return false;
        }

        public static async void Translate()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool isOpen = Open();
            if (isOpen)
            {
                var client = TranslationClient.CreateFromApiKey(APIKey);
                List<string> unTranslatedLines = new List<string>();

                if (File.Exists($"{Path}\\{FileName}_new.po"))
                {
                    File.Delete($"{Path}\\{FileName}_new.po");
                }
                {
                    using var sw = new StreamWriter($"{Path}\\{FileName}_new.po");
                }
                while (true)
                {
                    var line = stream.ReadLine();

                    if (line == null)
                        break;

                    StringBuilder poLine = new StringBuilder();
                    StringBuilder unTranslatedLine = new();
                    TranslationResult? response;
                    string detectedLine;
                    bool getOver = false;

                    if (line.Length > 5 && line.Remove(5, line.Length - 5) == "msgid")
                    {
                        int count = 5;
                        if (line[5] == '_')
                            count = 12;
                        else unTranslatedLines.Clear();
                        lines.Add(line);
                        foreach (var character in line.Remove(0, count))
                        {
                            unTranslatedLine.Append(character);
                        }
                        unTranslatedLines.Add(unTranslatedLine.ToString());
                    }
                    else if (line.Length > 7 && line.Remove(7, line.Length - 7) == "msgstr ")
                    {
                        poLine.Append("msgstr");
                        bool once = false;
                        foreach (var unTranslatedl in unTranslatedLines)
                        {
                            if (unTranslatedl[unTranslatedl.Length - 1] == unTranslatedl[unTranslatedl.Length - 2] && unTranslatedl[unTranslatedl.Length - 1] == '"')
                            {
                                response = client.TranslateText(unTranslatedl.ToString().Remove(unTranslatedl.Length - 1, 1), LanguageCodes.Azerbaijani, LanguageCodes.English);
                                poLine.Append(response.TranslatedText.Replace('“', '"').Replace('”', '"') + '"');
                            }
                            else
                            {
                                response = client.TranslateText(unTranslatedl.ToString(), LanguageCodes.Azerbaijani, LanguageCodes.English);
                                poLine.Append(response.TranslatedText.Replace('“', '"').Replace('”', '"'));
                            }
                            if (unTranslatedLines[unTranslatedLines.Count-1] != unTranslatedl)
                                poLine.AppendLine();
                            if (once)
                            {
                                stream.ReadLine();
                            }
                            else
                                once = true;
                        }
                        unTranslatedLines.Clear();
                        lines.Add(poLine.ToString());
                    }
                    else if (line.Length > 7 && line.Remove(7, line.Length - 7) == "msgstr[")
                    {
                        int index = int.Parse(line[7].ToString());
                        poLine.Append("msgstr[" + index + ']');

                        if (unTranslatedLines[index][unTranslatedLines[index].Length - 1] == unTranslatedLines[index][unTranslatedLines[index].Length - 2] && unTranslatedLines[index][unTranslatedLines[index].Length - 1] == '"')
                        {
                            response = client.TranslateText(unTranslatedLines[index].ToString().Remove(unTranslatedLines[index].Length - 1, 1), LanguageCodes.Azerbaijani, LanguageCodes.English);
                            poLine.Append(response.TranslatedText.Replace('“', '"').Replace('”', '"') + '"');
                        }
                        else
                        {
                            response = client.TranslateText(unTranslatedLines[index].ToString(), LanguageCodes.Azerbaijani, LanguageCodes.English);
                            poLine.Append(response.TranslatedText.Replace('“', '"').Replace('”', '"'));
                        }
                        lines.Add(poLine.ToString());
                    }
                    else if (line.Length != 0 && line[0] == '"')
                    {
                        lines.Add(line);
                        unTranslatedLines.Add(line);
                    }
                    else
                    {
                        lines.Add(line);
                        unTranslatedLines.Clear();
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    //Console.WriteLine($"Progress - {lines.Count}/{fileLineCount}");
                }
                await File.WriteAllLinesAsync(Path + "\\" + FileName + "_new.po", lines);
            }
            stopwatch.Stop();
            Console.WriteLine($"Time Elapsed: {stopwatch.Elapsed}");
        }
    }
}