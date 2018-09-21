using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Diagnostics;

namespace PasswordChecker
{
	public class Program
	{
		public static async Task<int> Main()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(
				@"  ___                                     _    ___  _              _             
 | _ \ __ _  ___ _____ __ __ ___  _ _  __| |  / __|| |_   ___  __ | |__ ___  _ _ 
 |  _// _` |(_-<(_-<\ V  V // _ \| '_|/ _` | | (__ | ' \ / -_)/ _|| / // -_)| '_|
 |_|  \__,_|/__//__/ \_/\_/ \___/|_|  \__,_|  \___||_||_|\___|\__||_\_\\___||_|  
                                                                                 ");
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("Jesse Mallen, 2018");
			Console.WriteLine("Using HIBP's Password API");
			Console.WriteLine(
				"https://haveibeenpwned.com/API/v2#PwnedPasswords");
			Console.ForegroundColor = ConsoleColor.White;

			while (true)
			{
				await CheckPassword();

				Console.WriteLine(
					Environment.NewLine + "Check another? (y/n): ");
				var key = Console.ReadKey(true);
				Console.WriteLine();
				if (key.KeyChar == 'y' || key.KeyChar == 'Y')
				{
					await CheckPassword();
				}
				else
				{
					break;
				}
			}

			Console.WriteLine("Exiting...");
			return 0;
		}

		private static string ReadPassword()
		{
			var pass = "";
			do
			{
				var key = Console.ReadKey(true);
				// Backspace Should Not Work
				if (key.Key != ConsoleKey.Backspace
				    && key.Key != ConsoleKey.Enter)
				{
					pass += key.KeyChar;
					Console.Write("*");
				}
				else
				{
					if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
					{
						pass = pass.Substring(0, (pass.Length - 1));
						Console.Write("\b \b");
					}
					else if (key.Key == ConsoleKey.Enter)
					{
						break;
					}
				}
			}
			while (true);

			Console.WriteLine();
			return pass;
		}

		private static async Task CheckPassword()
		{
			Console.WriteLine(Environment.NewLine + "Enter password: ");
			var password = ReadPassword();
			var sha1 = SHA1.Create();
			var hashedBytes =
				sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
			var sBuilder = new StringBuilder();
			foreach (var t in hashedBytes)
			{
				sBuilder.Append(t.ToString("x2"));
			}

			var hashed = sBuilder.ToString();
			var prefix = hashed.Substring(0, 5);
			var suffix = hashed.Substring(5);
			Debug.WriteLine($"{password}: {hashed}");
			Debug.WriteLine($"{prefix} {suffix}");

			var client = new HttpClient
			{
				BaseAddress = new Uri("https://api.pwnedpasswords.com/range/")
			};

			var resp = await client.GetAsync(prefix);

			var results = (await resp.Content.ReadAsStringAsync())
				.Split("\r\n")
				.ToDictionary(
					str => prefix + str.Split(':')[0].ToLowerInvariant(),
					str => str.Split(':')[1]);
			results.TryGetValue(hashed, out var countString);
			int.TryParse(countString, out var count);

			Console.WriteLine($"Password appears in breaches {count} times.");
		}

//		private static void SampleConsoleColors()
//		{
//			Console.ForegroundColor = ConsoleColor.DarkBlue;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.DarkBlue;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.DarkCyan;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.DarkGray;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.DarkGreen;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.DarkMagenta;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.DarkRed;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.DarkYellow;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.Blue;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.Cyan;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.Gray;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.Green;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.Magenta;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.Red;
//			Console.WriteLine("This is a sentence.");
//			Console.ForegroundColor = ConsoleColor.Yellow;
//			Console.WriteLine("This is a sentence.");
//		}
	}
}
