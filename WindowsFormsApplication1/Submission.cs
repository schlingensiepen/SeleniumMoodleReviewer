using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Submission
    {
        public string Path;
        public string LocalPath;
        public string StudentName;
        public string[] Chunks;
        public string DataPath;

        public IEnumerable<string> ToStringElements()
        {
            yield return Path;
            yield return LocalPath;
            yield return StudentName;
            yield return DataPath;
            foreach (string chunk in Chunks)
                yield return chunk;
        }
        public override string ToString()
        {
            return string.Join("\n", ToStringElements());
        }

        public Submission(string path)
        {
            Path = path;
            LocalPath = System.IO.Path.GetFileName(path);
            StudentName = LocalPath.Split(new[] {'_'})[0];
            Chunks = StudentName.Split(new[] {' '});
            DataPath = System.IO.Path.Combine(Path, "data");
        }
        
        public string value = null;
        public string msg = null;

        private void listDirectory(StringBuilder target, string path, string prefix = "  ")
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                listDirectory(target, dir, prefix + "  ");
            }
            foreach (var file in Directory.GetFiles(path))
            {
                target.AppendLine(prefix + System.IO.Path.GetFileName(file));
            }
        }

        public string initStructure()
        {
            string[] zipFiles = Directory.GetFiles(Path).ToArray();
            if (zipFiles.Length != 1) throw new Exception("Zu viele Dateien in der Abgabe.");
            string zipFile = zipFiles[0];
            if (!Directory.Exists(DataPath))
                ZipFile.ExtractToDirectory(zipFile, DataPath);
            StringBuilder sb = new StringBuilder();
            listDirectory(sb, DataPath);
            return sb.ToString();            
        }

        public IEnumerable<string> findFiles(string Folder, string[] extensions)
        {
            foreach (var directory in Directory.GetDirectories(Folder))
            {
                foreach (var res in findFiles(directory, extensions))
                {
                    yield return res;
                }                
            }
            foreach (var extension in extensions)
            {
                foreach (var file in Directory.GetFiles(Folder, extension))
                {
                    yield return file;
                }
            }
        }


        public IEnumerable<string> findFiles(string[] extensions)
        {
            return findFiles(DataPath, extensions);
        }

        public void openFiles(string Folder, string[] extensions)
        {
            foreach (var directory in Directory.GetDirectories(Folder))
            {
                openFiles(directory, extensions);
            }
            foreach (var extension in extensions)
            {
                foreach (var file in Directory.GetFiles(Folder, extension))
                {
                    Process.Start(file);
                }
            }
        }

        public void openFiles(string[] extensions)
        {
            openFiles(DataPath, extensions);
        }
    }
}
