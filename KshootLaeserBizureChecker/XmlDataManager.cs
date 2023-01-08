using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace KshootLaeserBizureChecker
{
	internal class XmlDataManager
	{
		internal static void Store(string fileName)
		{
			XDocument xml = XDocument.Load(@"./setting.xml");
			XElement fileNameXml = (from item in xml.Elements("settingDatas") select item).First();
			fileNameXml.Element("fileName").Value = fileName;
			xml.Save(@"./setting.xml");
		}

		internal static string GetFileName()
		{
			XDocument xml = XDocument.Load(@"./setting.xml");
			XElement fileNameXml = (from item in xml.Elements("settingDatas") select item).First();
			return fileNameXml.Element("fileName").Value;
		}
	}
}
