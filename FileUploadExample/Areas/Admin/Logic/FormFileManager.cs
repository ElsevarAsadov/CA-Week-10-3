using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Areas.Admin.Logic
{
    public class FormFileManager
    {


        public static void SaveFile(IFormFile file, string filePath, FileMode modes)
        {
            try
            {
                Path.GetFullPath(filePath);

            }
            catch (Exception exp) {

                throw new Exception($"File path format is invalid.Path: {filePath}");
            }

            
            string fileDir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(fileDir)) {

                Directory.CreateDirectory(fileDir);
            }

            using (FileStream fs = new FileStream(filePath, modes))
            {
                file.CopyTo(fs);
            }
        }
        

        public static void DeleteFile(IFormFile file)
        {
            System.IO.File.Delete(file.FileName);
        }

        public static void DeleteFile(string filePath)
        {
            System.IO.File.Delete(filePath);  
          
        }
        
        /// <summary>
        /// Converts file name to FILENAME[max:maximumLength]-GUID_ID.EXTENSION
        /// </summary>
        /// <param name="filename">filename with extension</param>
        /// <param name="maximumLength">Maximum file length default is INF+ (not including extension and GUID)</param>
        /// <returns>Formatted String</returns>
        public static string ConvertUniqueName(string filename, int? maximumLength = null)
        {
            string filenameOutExtension = Path.GetFileNameWithoutExtension(filename);
            //includes DOT
            string extension = Path.GetExtension(filename);

            if(filenameOutExtension == String.Empty || extension == String.Empty)
            {
                throw new Exception($"Invalid file format. filename: {filename}");
            }


            string nonTruncated = $"{filenameOutExtension}-[{Guid.NewGuid()}]{extension}";
            if (maximumLength != null)
            {

                if(filenameOutExtension.Length > maximumLength)
                {
                    filenameOutExtension = filenameOutExtension.Substring(0, (int) maximumLength);
                }

                return $"{filenameOutExtension}-[{Guid.NewGuid()}]{extension}";
            }
            else
            {
                return nonTruncated;
            }
        }

       
    }
}
