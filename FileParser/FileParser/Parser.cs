using System;
using System.IO;
using System.Text;
using System.Linq;
namespace FileParser
{
    public class Parser
    {
        private const char CSV_SEPARATOR = ',';
        private const char TSV_SEPARATOR = '\t';

        public Parser()
        {
        }

        public void ValidateFile(FileExtension fileExt, string file, string pathString, int fieldNum)
        {
            var separator = fileExt.Equals(FileExtension.CSV) ? CSV_SEPARATOR : TSV_SEPARATOR;
            var fileName = Path.GetFileNameWithoutExtension(file);
            var goodFile = Path.Combine(pathString, $"{fileName}_Good_Matches.{fileExt}");
            var errorFile = Path.Combine(pathString, $"{fileName}_Errors.{fileExt}");

            using (StreamReader sr = new StreamReader(file))
            {
                using (FileStream goodFileStream = new FileStream(goodFile, FileMode.Append, FileAccess.Write))
                {
                    using (FileStream errorFileStream = new FileStream(errorFile, FileMode.Append, FileAccess.Write))
                    {
                        string headerLine = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            var line = sr.ReadLine();
                            var fields = line.Split(separator);
                            var input = String.Join(",", fields) + "\n";

                            //We want to make sure it matches the number of fields, as well as ensure there is a first and last name
                            if (fields.Length == fieldNum && !String.IsNullOrWhiteSpace(fields[0]) && !String.IsNullOrWhiteSpace(fields[fieldNum - 1]))
                            {
                                goodFileStream.Write(Encoding.ASCII.GetBytes(input), 0, ASCIIEncoding.ASCII.GetByteCount(input));
                            }
                            else
                            {
                                errorFileStream.Write(Encoding.ASCII.GetBytes(input), 0, ASCIIEncoding.ASCII.GetByteCount(input));
                            }
                        }
                    }
                }
            }
        }
    }
}
