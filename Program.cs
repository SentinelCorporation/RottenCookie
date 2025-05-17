using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RottenCookieApp
{
    class Program
    {
        // Console colors for output
        static readonly ConsoleColor NormalColor = ConsoleColor.Green;
        static readonly ConsoleColor ErrorColor = ConsoleColor.Red;

        static void Main()
        {
            PrintBinaryCookieArt();
            WriteColoredLine("\nWelcome to \x1b[1mRottenCookie\x1b[0m\n", NormalColor); // bold via ANSI escape

            while (true)
            {
                WriteColoredLine("Choose an option:", NormalColor);
                WriteColoredLine("1. Serialize JSON to cookie string", NormalColor);
                WriteColoredLine("2. Deserialize cookie string to JSON", NormalColor);
                WriteColoredLine("3. Exit", NormalColor);
                Console.Write("Enter option (1, 2 or 3): ");

                string choice = Console.ReadLine()?.Trim();

                if (choice == "1")
                {
                    SerializeJsonToCookie();
                }
                else if (choice == "2")
                {
                    DeserializeCookieToJson();
                }
                else if (choice == "3")
                {
                    WriteColoredLine("Exiting RottenCookie. Goodbye!", NormalColor);
                    break;
                }
                else
                {
                    WriteColoredLine("Invalid option. Please try again.\n", ErrorColor);
                }
            }
        }

        static void SerializeJsonToCookie()
        {
            WriteColoredLine("\nPaste the JSON object to serialize (single line or multi-line). Finish input by an empty line:", NormalColor);
            string jsonInput = ReadMultilineInput();

            try
            {
                // Validate JSON
                var obj = JToken.Parse(jsonInput);

                // Serialize to JSON string then URL encode
                string jsonString = JsonConvert.SerializeObject(obj);
                string encodedCookie = WebUtility.UrlEncode(jsonString);

                WriteColoredLine("\nSerialized cookie string:", NormalColor);
                Console.WriteLine(encodedCookie);
                WriteColoredLine("", NormalColor);
            }
            catch (JsonReaderException jex)
            {
                WriteColoredLine($"Invalid JSON format: {jex.Message}\n", ErrorColor);
            }
            catch (Exception ex)
            {
                WriteColoredLine($"Error serializing JSON: {ex.Message}\n", ErrorColor);
            }
        }

        static void DeserializeCookieToJson()
        {
            WriteColoredLine("\nPaste the encoded cookie string to deserialize:", NormalColor);
            string encodedCookie = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(encodedCookie))
            {
                WriteColoredLine("Input cannot be empty.\n", ErrorColor);
                return;
            }

            try
            {
                // URL decode and parse JSON
                string jsonString = WebUtility.UrlDecode(encodedCookie);

                // Parse JSON dynamically
                var obj = JToken.Parse(jsonString);

                string prettyJson = obj.ToString(Formatting.Indented);

                WriteColoredLine("\nDeserialized JSON object:\n", NormalColor);
                Console.WriteLine(prettyJson);
                WriteColoredLine("", NormalColor);
            }
            catch (JsonReaderException jex)
            {
                WriteColoredLine($"Invalid cookie JSON format: {jex.Message}\n", ErrorColor);
            }
            catch (Exception ex)
            {
                WriteColoredLine($"Error deserializing cookie: {ex.Message}\n", ErrorColor);
            }
        }

        /// <summary>
        /// Reads multiline input until an empty line is entered.
        /// </summary>
        /// <returns>Concatenated string input</returns>
        static string ReadMultilineInput()
        {
            string line;
            string input = "";
            while (true)
            {
                line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                input += line;
            }
            return input;
        }

        /// <summary>
        /// Writes colored text line
        /// </summary>
        static void WriteColoredLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a binary cookie shape made of 1's and 0's in green
        /// </summary>
        static void PrintBinaryCookieArt()
        {
            string[] cookieArt = new[]
            {
                "  01010101  11110000  01010101  ",
                " 10101010  10001111  10101010  ",
                "  01010101  11110000  01010101  ",
                "   111111111111111111111111    ",
                "  1001100110011001100110011001  ",
                "  0110011001100110011001100110  ",
                "   111111111111111111111111    "
            };

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var line in cookieArt)
            {
                Console.WriteLine(line);
            }
            Console.ResetColor();
        }
    }
}
