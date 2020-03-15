using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

// ReSharper disable All

namespace Google_Photo_Metadata_Resolver
{
    class Program
    {
        private static Dictionary<string, string> _filesAndMetaDataDictionary = new Dictionary<string, string>();

        private static Dictionary<string, DateTime> _foldersNameAsDataDictionary =
            new Dictionary<string, DateTime>();

        private static readonly string generalDirectoryPath = @"e:\Photo&Video\googlePhotoResolving\";
        private static readonly CultureInfo Culture = new System.Globalization.CultureInfo("uk-UA");
        private static readonly string logFilePath = @"C:\Users\stani\Desktop\new.txt";
        private static readonly string noPairFilesLog = @"C:\Users\stani\Desktop\noPairFiles.txt";
        private static readonly string NonSupportedFileFormatLog = @"C:\Users\stani\Desktop\exeptedFormats.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Looking for pair file-metadata");
            Console.WriteLine("\n");

            GetSubDirectories(generalDirectoryPath);

            Console.WriteLine("\n");
            Console.WriteLine("Start rewrite metadata?You will try edit {0} files",
                _filesAndMetaDataDictionary.Count + _foldersNameAsDataDictionary.Count);
            Console.Read();
            Console.WriteLine("\n");

            //MetadataEditor(_filesAndMetaDataDictionary);
            Console.WriteLine("\n");
            Console.WriteLine("Editing Finished");
            Console.WriteLine("\n");
            Console.WriteLine("Uploading data to txt file");
            Console.WriteLine("\n");

            WriteToFileMethod(_filesAndMetaDataDictionary);

            Console.WriteLine("\n");
            Console.WriteLine("Finished, press any key");
            Console.Read();
        }

        static void GetSubDirectories(string generalDirectory)
        {
            string[] subdirectoriesEntries = Directory.GetDirectories(generalDirectory);
            foreach (string oneDirectoryPath in subdirectoriesEntries)
            {
                OneDirectoryResolver(oneDirectoryPath);
                Console.WriteLine(oneDirectoryPath + ": ");
            }
        }

        static void OneDirectoryResolver(string oneDirectoryPath)
        {
            string[] currentFolderFilesList = Directory.GetFiles(oneDirectoryPath);
            Dictionary<string, string> metadataFilesDictionary = new Dictionary<string, string>();
            Dictionary<string, string> filesDictionary = new Dictionary<string, string>();
            foreach (string t in currentFolderFilesList)
            {
                try
                {
                    if (t.EndsWith(".json"))
                        metadataFilesDictionary.Add(t, oneDirectoryPath);
                    if (t.EndsWith(".jpg") || t.EndsWith(".mp4"))
                        filesDictionary.Add(t, oneDirectoryPath);
                    else
                        File.WriteAllText(NonSupportedFileFormatLog, t);
                }
                catch (Exception e)
                {
                    Console.WriteLine("OneDirectoryResolver exception");
                }
            }

            _filesAndMetaDataDictionary = DuplicateFinder(metadataFilesDictionary, filesDictionary);
        }

        private static Dictionary<string, string> DuplicateFinder(Dictionary<string, string> metadataFilesDictionary,
            Dictionary<string, string> filesDictionary)
        {
            List<string> filesList = filesDictionary.Keys.ToList();
            List<string> metadataFilesList = metadataFilesDictionary.Keys.ToList();

            for (int i = 0; i < filesList.Count; i++)
            {
                for (int j = 0; j < metadataFilesList.Count; j++)
                {
                    try
                    {
                        if (metadataFilesList[j].Contains(filesList[i]))
                        {
                            _filesAndMetaDataDictionary.Add(filesList[i], metadataFilesList[j]);
                            metadataFilesList.RemoveAt(j);
                            filesList.RemoveAt(i);
                        }
                        else
                        {
                            _foldersNameAsDataDictionary.Add(filesList[i], FolderNameDateTimeParser(filesList[i]));
                            metadataFilesList.RemoveAt(j);
                            filesList.RemoveAt(i);
                        }
                    }
                    catch (SystemException exception)
                    {
                        Console.Write("!");
                    }
                }
            }

            //TODO: return _foldersNameAsDataDictionary;
            Console.WriteLine("\n");
            return _filesAndMetaDataDictionary;
        }

        static void WriteToFileMethod(Dictionary<string, string> dictionary)
        {
            foreach (var valuePair in _filesAndMetaDataDictionary)
            {
                File.AppendAllText(logFilePath, valuePair.Key + ": " + valuePair.Value + "\n");
                Console.Write("+");
            }

            foreach (var parsedMetadata in _foldersNameAsDataDictionary)
            {
                File.AppendAllText(noPairFilesLog, parsedMetadata.Key + ": " + parsedMetadata.Value + "\n");
                Console.Write("*");
            }
        }

        static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static DateTime FolderNameDateTimeParser(string folderName)
        {
            CultureInfo culture = new System.Globalization.CultureInfo("uk-UK");
            string format = "yyyy-MM-dd";

            if (folderName.Length > 10)
                folderName = folderName.Remove(10);
            return DateTime.ParseExact(folderName, format, culture);
        }

        static void MetadataEditor(Dictionary<string, string> dictionary)
        {
            foreach (var keyValuePair in _filesAndMetaDataDictionary)
            {
                Metadata jsonConverter = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(keyValuePair.Value));
                try
                {
                    DateTime dateTime = UnixTimeStampToDateTime(jsonConverter.PhotoTakenTime.Timestamp);
                    if (File.GetCreationTime(keyValuePair.Key) != dateTime ||
                        File.GetLastWriteTime(keyValuePair.Key) != dateTime ||
                        File.GetLastAccessTime(keyValuePair.Key) != dateTime)
                    {
                        File.SetCreationTime(keyValuePair.Key, dateTime);
                        File.SetLastWriteTime(keyValuePair.Key, dateTime);
                        File.SetLastAccessTimeUtc(keyValuePair.Key, dateTime);
                        Console.Write("+");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                catch (System.FormatException formatException)
                {
                    Console.WriteLine(formatException);
                }
            }

            foreach (var VARIABLE in _foldersNameAsDataDictionary)
            {
                try
                {
                    File.SetCreationTime(VARIABLE.Key, VARIABLE.Value);
                    File.SetLastWriteTime(VARIABLE.Key, VARIABLE.Value);
                    File.SetLastAccessTimeUtc(VARIABLE.Key, VARIABLE.Value);
                    Console.Write("+");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}