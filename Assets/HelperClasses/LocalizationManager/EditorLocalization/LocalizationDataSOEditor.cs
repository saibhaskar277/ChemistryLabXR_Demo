#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageDatabase))]
public class LanguageDatabaseSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        LanguageDatabase database = (LanguageDatabase)target;

        if (GUILayout.Button("Export All XML"))
        {
            foreach (var language in database.languages)
            {
                if (language == null) continue;
                ExportToXML(language);
            }

            Debug.Log("✅ Exported all localization XML files");
        }

        if (GUILayout.Button("Import All XML"))
        {
            foreach (var language in database.languages)
            {
                if (language == null) continue;
                ImportFromXML(language);
            }

            Debug.Log("✅ Imported all localization XML files");
        }

        if (GUILayout.Button("Open Localization Folder")) { EditorUtility.RevealInFinder(LocalizationPaths.RESOURCES_FULL_PATH); }
    }


    private void ExportToXML(LocalizationDataSO data)
    {
        if (data == null)
        {
            Debug.LogError("LocalizationDataSO is null!");
            return;
        }

        string folderPath = LocalizationPaths.RESOURCES_FULL_PATH;

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = data.languageName.ToString().ToLower();

        string path = Path.Combine(
            folderPath,
            fileName + LocalizationPaths.XML_EXTENSION
        );

        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true
        };

        HashSet<LocalizationKey> usedKeys = new();

        using (XmlWriter writer = XmlWriter.Create(path, settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Localization");

            foreach (var entry in data.entries)
            {
                if (!usedKeys.Add(entry.key))
                {
                    Debug.LogError($"Duplicate key: {entry.key}");
                    continue;
                }

                writer.WriteStartElement("Entry");
                writer.WriteAttributeString("key", entry.key.ToString());
                writer.WriteAttributeString("value", entry.value);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        Debug.Log($"✅ Exported XML: {path}");
        AssetDatabase.Refresh();
    }

    private void ImportFromXML(LocalizationDataSO data)
    {
        string fileName = data.languageName.ToString().ToLower();

        string path = Path.Combine(
            LocalizationPaths.RESOURCES_FULL_PATH,
            fileName + LocalizationPaths.XML_EXTENSION
        );

        if (!File.Exists(path))
        {
            Debug.LogError($"XML file not found: {path}");
            return;
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNodeList nodes = doc.SelectNodes("//Entry");

        List<LocalizationEntry> importedEntries = new();

        foreach (XmlNode node in nodes)
        {
            string keyString = node.Attributes["key"]?.Value;
            string value = node.Attributes["value"]?.Value;

            if (!System.Enum.TryParse(keyString, out LocalizationKey key))
            {
                Debug.LogWarning($"Invalid key found in XML: {keyString}");
                continue;
            }

            importedEntries.Add(new LocalizationEntry
            {
                key = key,
                value = value
            });
        }

        data.entries = importedEntries;

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();

        Debug.Log($"✅ Imported XML into {data.languageName}");
    }


}
#endif