// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

using var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var input = sr.ReadToEnd();
var tokens = input.Replace(Environment.NewLine, " ").Split(' ');
Console.WriteLine($"Tokens: {tokens.Length}");
