﻿using Translate;

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Welcome to .po File Translator Application V1.0.0");
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("if you want to exit use CTRL+C");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Please, Enter Google Cloud API key: ");
Console.ForegroundColor = ConsoleColor.Gray;
Console.WriteLine("https://support.google.com/googleapi/answer/6158862?hl=en");
Operation.APIKey = Console.ReadLine();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Please, Enter Project Name: ");
Operation.ProjectName = Console.ReadLine();
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("Operation has started. Please wait...");

Operation.Translate();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Operation finished.");
Console.ForegroundColor = ConsoleColor.White;
Console.ReadKey();