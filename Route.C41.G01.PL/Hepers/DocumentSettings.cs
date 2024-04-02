using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Route.C41.G01.PL.Hepers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile formFile,string folderName)
        {
            // 1. Get Located Folder Path
            //string folderPath = $"D:\\courses\\ASP.Net Route C41\\7. ASP.Net Core MVC\\Session 03\\Assignements\\Route.C41.G01\\Route.C41.G01.PL\\wwwroot\\Files\\{folderName}";
            //string folderPath = $"{Directory.GetCurrentDirectory()}wwwroot\\Files\\{folderName}";
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 2. Get Fille Name and Make it Unique
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";

            // 3. Get File Path
            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save File as Stream[Data Per Time]
            using var fileStream = new FileStream(filePath, FileMode.Create);

            formFile.CopyTo(fileStream);

            return fileName;

        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
