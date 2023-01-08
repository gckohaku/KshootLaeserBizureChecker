using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.Devices;

namespace KshootLaeserBizureChecker
{
	internal static class LaeserCheck
	{
		public static void Check(string fileName, ref TextBox textBox)
		{
			(string Left, string Right) previous_laeser_positions = ("", "");
			(bool Left, bool Right) is_laeser_throughs = (false, false);
			string pos_char = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmno";
			Regex notes_regex = new Regex(@"^\d{4}\|\d{2}\|..$", RegexOptions.Compiled);
			int measure_count = 0;
			List<string> errors = new List<string>();
			List<string> warnings = new List<string>();

			File.Create(fileName.Substring(0, fileName.Length - 4) + "_checked.ksh").Close();

			using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
			using (StreamWriter sw = new StreamWriter(fileName.Substring(0, fileName.Length - 4) + "_checked.ksh", false, Encoding.UTF8))
			{
				while (!sr.EndOfStream)
				{
					string line_str = sr.ReadLine();

					if (line_str == "--")
					{
						measure_count++;
					}
					else if (notes_regex.IsMatch(line_str))
					{
						string left = line_str[8].ToString();
						string right = line_str[9].ToString();

						if (left != "-" && left != ":")
						{

							if (previous_laeser_positions.Left == "")
							{
								previous_laeser_positions.Left = left;
								is_laeser_throughs.Left = true;
							}
							else
							{
								int previous = pos_char.IndexOf(previous_laeser_positions.Left);
								int current = pos_char.IndexOf(left);

								if (is_laeser_throughs.Left)
								{
									if (IsBizure(previous, current))
									{
										errors.Add(measure_count + "小節目 Left : " + previous_laeser_positions.Left + " -> " + left);
										sw.WriteLine("// _eLeft : " + previous_laeser_positions.Left + " -> " + left);
									}
									else if (IsWarning(previous, current))
									{
										warnings.Add(measure_count + "小節目 Left : " + previous_laeser_positions.Left + " -> " + left);
										sw.WriteLine("// _wLeft : " + previous_laeser_positions.Left + " -> " + left);
									}
								}

								previous_laeser_positions.Left = left;
							}
						}
						else
						{
							if ((left == ":") != (is_laeser_throughs.Left))
							{
								is_laeser_throughs.Left = !is_laeser_throughs.Left;
							}

						}

						if (right != "-" && right != ":")
						{

							if (previous_laeser_positions.Right == "")
							{
								previous_laeser_positions.Right = right;
								is_laeser_throughs.Right = true;
							}
							else
							{
								int previous = pos_char.IndexOf(previous_laeser_positions.Right);
								int current = pos_char.IndexOf(right);

								if (is_laeser_throughs.Right)
								{
									if (IsBizure(previous, current))
									{
										errors.Add(measure_count + "小節目 Right : " + previous_laeser_positions.Right + " -> " + right);
										sw.WriteLine("// _eRight : " + previous_laeser_positions.Right + " -> " + right);
									}
									else if (IsWarning(previous, current))
									{
										warnings.Add(measure_count + "小節目 Right : " + previous_laeser_positions.Right + " -> " + right);
										sw.WriteLine("// _wRight : " + previous_laeser_positions.Right + " -> " + right);
									}
								}

								previous_laeser_positions.Right = right;
							}
						}
						else
						{
							if ((right == ":") != (is_laeser_throughs.Right))
							{
								is_laeser_throughs.Right = !is_laeser_throughs.Right;
							}

						}
					}
					sw.WriteLine(line_str);
					Console.WriteLine(line_str);
				}

				textBox.Text = "検出微ズレ\r\n";

				foreach (string error in errors)
				{
					textBox.Text += error + "\r\n";
				}
				textBox.Text += "\r\n微ズレの可能性あり\r\n";
				foreach (string warning in warnings)
				{
					textBox.Text += warning + "\r\n";
				}
			}
		}

		internal static bool IsBizure(int before, int after)
		{
			return Math.Abs(before - after) == 1;
		}

		internal static bool IsWarning(int before, int after)
		{
			return Math.Abs(before - after) == 2;
		}
	}
}
