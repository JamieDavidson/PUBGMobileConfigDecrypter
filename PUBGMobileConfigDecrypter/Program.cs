using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUBGMobileConfigDecrypter
{
    class Program
    {
        private const string StringToStrip = "+CVars=";
        private const string UserCustomPath = @".\UserCustom.ini";
        static void Main(string[] args)
        {
            var processMode = ProcessMode.None;
            var displayHelp = false;
            var hasFilePath = false;

            var optionSet = new OptionSet
            {
                {
                    "d|decrypt",
                    "Specifies that the file should be decrypted",
                    d => processMode = d != null ? ProcessMode.Decrypt : processMode
                },
                {
                    "e|encrypt",
                    "Specifies that the file should be encrypted",
                    e => processMode = e != null ? ProcessMode.Encrypt : processMode
                },
                {
                    "f|file",
                    "File path of the file to be encrypted/decrypted",
                    i => hasFilePath = i != null
                },
                {
                    "h|help",
                    "Show this message and exit",
                    v => displayHelp = v != null
                }
            };

            List<string> extra;
            try
            {
                extra = optionSet.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try 'crypter --help' for more information.");
                return;
            }

            if (processMode == ProcessMode.None)
            {
                Console.WriteLine("You must specify whether to encrypt or decrypt the file.");
                ShowHelp(optionSet);
                Environment.Exit(0);
            }

            if (!hasFilePath)
            {
                Console.WriteLine("Could not find path to file in arguments.");
                ShowHelp(optionSet);
                Environment.Exit(0);
            }

            if (displayHelp)
            {
                ShowHelp(optionSet);
                Environment.Exit(0);
            }

            if (processMode == ProcessMode.Encrypt)
            {
                EncryptFile(extra.First());
            }
            else
            {
                DecryptFile(extra.First());
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: crypter [OPTIONS]");
            Console.WriteLine("Encrypts or decrypts a PUBG Mobile ini file.");
            Console.WriteLine("May not work for future versions if encryption method changes");
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void EncryptFile(string filePath)
        {
            var stringBuilder = new StringBuilder();
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Could not find file at {filePath}, exiting.");
                Environment.Exit(0);
            }

            var text = File.ReadAllLines(filePath);

            for (var i = 0; i < text.Length; i++)
            {
                var line = text[i];
                if (line.StartsWith("["))
                {
                    stringBuilder.AppendLine(line);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    stringBuilder.AppendLine();
                    continue;
                }

                var strippedLine = line.Replace(StringToStrip, string.Empty);
                stringBuilder.Append(StringToStrip);
                for (var j = 0; j < strippedLine.Length; j++)
                {
                    try
                    {
                        var encryptedCharacter = Encryption.Encrypt(strippedLine[j]);
                        stringBuilder.Append(encryptedCharacter);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Error on line {i+1}, character {j + 1 + StringToStrip.Length}, {e.Message}");
                        throw;
                    }
                }

                stringBuilder.AppendLine();
            }

            File.WriteAllText(UserCustomPath, stringBuilder.ToString());
        }

        static void DecryptFile(string filePath)
        {
            var stringBuilder = new StringBuilder();
            if (!File.Exists(UserCustomPath))
            {
                Console.WriteLine("Could not find UserCustom.ini in directory, place it in the same folder as this executable.");
                Environment.Exit(0);
            }

            var text = File.ReadAllLines(UserCustomPath);
            for (var i = 0; i < text.Length; i++)
            {
                var line = text[i];
                if (line.StartsWith("["))
                {
                    stringBuilder.AppendLine(line);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    stringBuilder.AppendLine();
                    continue;
                }

                var strippedLine = line.Replace(StringToStrip, string.Empty);
                if (strippedLine.Length % 2 != 0)
                {
                    Console.WriteLine($"Error on file line {i+1}, lenth of CVar was not divisible by 2.");
                    Environment.Exit(0);
                }

                stringBuilder.Append(StringToStrip);
                for (var j = 0; j < strippedLine.Length / 2; j++)
                {
                    var character = new string(strippedLine.Skip(2*j).Take(2).ToArray());
                    try
                    {
                        var decryptedCharacter = Encryption.Decrypt(character);
                        stringBuilder.Append(decryptedCharacter);
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Invalid character found at line {i}, character {j*2}, {exception.Message}");
                        Environment.Exit(0);
                    }
                }

                stringBuilder.Append(Environment.NewLine);
            }

            File.WriteAllText(filePath, stringBuilder.ToString());
        }
    }

    public enum ProcessMode
    {
        None,
        Encrypt,
        Decrypt
    }
}
