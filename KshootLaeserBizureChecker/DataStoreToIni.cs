using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace KshootLaeserBizureChecker
{
	internal static class DataStoreToIni
	{
		[DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

		internal static void Store(string fileName)
		{
			WritePrivateProfileString("data", "fileName", fileName, @"./setting.ini");
		}
	}
}
