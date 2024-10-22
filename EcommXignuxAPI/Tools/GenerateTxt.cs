using System;
using System.IO;

namespace EventHubLastMilleApi
{
    public static class GenerateTxt
    {
        public static void saveZplText(string content, string nameFile, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            DateTime dtime = DateTime.Now;

            FileStream fs = new FileStream(string.Concat(folderPath, nameFile),
                                           FileMode.OpenOrCreate,
                                           FileAccess.Write
                                          );


            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(content);
            m_streamWriter.Flush();
            m_streamWriter.Close();
        }
    }

}
