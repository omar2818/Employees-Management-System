using DemoG02.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.BusinessLogic.Services.Classes
{
    public class AttachmentService : IAttachmentService
    {
        List<string> allowedExtensions = new List<string>() { ".png", ".Jpg", ".jpeg"};
        const int maxSize = 2 * 1024 * 1024;
        public string? Upload(IFormFile file, string folderName)
        {
            // 1. Check Extension
            var extension = Path.GetExtension(file.FileName); // ".png"
            if(!allowedExtensions.Contains(extension))
            {
                return null;
            }
            // 2. Check Size
            if(file.Length > maxSize || file.Length == 0)
            {
                return null;
            }
            // 3. Get Located Folder Path
            // D:\Route\C44\07 ASP.Net Core MVC\Session 03\Files\DemoG02 Solution\DemoG02.Presentation\wwwroot\Files\Images
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName);
            // 4. Make Attachment Name Unique
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            // 5. Get File Path
            var filePath = Path.Combine(folderPath, fileName);
            // 6. Create File Stream [unmanaged]
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            // 7. Copy File
            file.CopyTo(fs);
            // 8. Return File Name
            return fileName;
        }
        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

    }
}
