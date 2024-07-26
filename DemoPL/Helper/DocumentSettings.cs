using System;
using System.IO;
using Microsoft.AspNetCore.Http;

using System.IO;

namespace DemoPL.Helper
{
    public class DocumentSettings
    {

        public static string UploadFile(IFormFile file ,string folderName)
        {
            //1.Get location folder path 
            //  string folderpath = "C:\\Users\\Emad\\source\\repos\\MVC Solution DEMO\\DemoPL\\wwwroot\\files\\" + folderName;
            // string folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files" ,folderName);
            //2.get file name and make it unique 

            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            //3.Get File Path --> folderpath + filename

            string filePath = Path.Combine(folderPath, fileName);

            //4.save file as stream 

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;




        }
        

        public static void DeleteFile(string fileName , string folderName)
        {
            string filePath=Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\files",folderName,fileName);
            
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}
