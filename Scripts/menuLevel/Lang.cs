using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class Lang {
	private Hashtable strings;
	// Use this for initialization

	public Lang (TextAsset path, string language){
			setLanguage(path, language);
	}

    public Lang(string path, string language)
    {
            setLanguageWeb(path, language);
    }

    public int getNumberOfLanguages(TextAsset path){
		XmlDocument xml = new XmlDocument ();
		xml.LoadXml (path.text);

		return xml.DocumentElement.ChildNodes.Count ;
	}

    public int getNumberOfLanguages(string path)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(new StringReader(path));

        return xml.DocumentElement.ChildNodes.Count;
    }

    public void setLanguage(TextAsset path, string language){
		XmlDocument xml = new XmlDocument ();
		xml.LoadXml (path.text);

		strings = new Hashtable ();
		XmlElement element = (XmlElement)xml.DocumentElement.SelectSingleNode (language);
		if (element != null) {
			IEnumerator elemNum = element.GetEnumerator ();
			while (elemNum.MoveNext()) {
				XmlElement xmlItem = (XmlElement)elemNum.Current;
				strings.Add (xmlItem.GetAttribute ("name"), xmlItem.InnerText);
			}
		} else {
			Debug.LogError("The specified language does not exist: " + language);
		}
	}

	public void setLanguageWeb (string path, string language){
		XmlDocument xml = new XmlDocument ();
		xml.Load (new StringReader(path));
		
		strings = new Hashtable ();
		XmlElement element = (XmlElement)xml.DocumentElement.SelectSingleNode (language);
		if (element != null) {
			IEnumerator elemNum = element.GetEnumerator ();
			while (elemNum.MoveNext()) {
				XmlElement xmlItem = (XmlElement)elemNum.Current;
				strings.Add (xmlItem.GetAttribute ("name"), xmlItem.InnerText);
			}
		} else {
			Debug.LogError("The specified language does not exist: " + language);
		}
	}

	public string getString (string name){
		if (!strings.ContainsKey(name)) {
			Debug.LogError("The specified string does not exist: " + name);
			
			return "";
		}
		
		return strings [name].ToString ();;
	}
}
