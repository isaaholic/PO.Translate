using Google.Cloud.Translation.V2;
using System.Diagnostics;
using System.Text;

namespace Translate
{
    // 1.1.0
    // 1.1.1
    public static class Operation
    {
        private static StreamReader? stream;
        private static List<string> lines = new List<string>();
        public static TranslationClient client { private get; set; } // Google Cloud API Key
        private static bool isOpen = false;
        private static string newFilePath;

        public static string Path { get; private set; } // path of Folder
        public static string FileName { get; private set; } // without .po
        public static int FileLineCount { get; private set; } // count of lines of file



        public static void OpenFile(string fileName, string folderPath)
        {
            try
            {
                stream = new StreamReader(folderPath + "\\" + fileName + ".po");
                FileLineCount = File.ReadAllLines(folderPath + "\\" + fileName + ".po").Length;
                Path = folderPath;
                FileName = fileName;
                isOpen = true;
                newFilePath = $"{Path}\\{FileName}_new.po";
            }
            catch (Exception)
            {
                // Add Log
            }
        }

        public static void CreateClientFromApiKey(string apiKey)
        {
            client = TranslationClient.CreateFromApiKey(apiKey);
            // Add Log
        }

        private static void DeleteNewFile()
        {
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }
            using var sw = new StreamWriter(newFilePath);
        }

        public static async void Translate()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (isOpen)
            {
                List<string> unTranslatedLines = new();
                DeleteNewFile();

                while (true)
                {
                    var line = stream.ReadLine();

                    if (line == null)
                        break;

                    StringBuilder poLine = new();
                    StringBuilder unTranslatedLine = new();
                    TranslationResult? response;
                    string detectedLine;

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
                            if (unTranslatedLines[unTranslatedLines.Count - 1] != unTranslatedl)
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
                    //Console.WriteLine($"Progress - {lines.Count}/{FileLineCount}"); // AddLog
                }
                await File.WriteAllLinesAsync(newFilePath, lines);
            }
            stopwatch.Stop();
            // Add Log
        }
    }
}