using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShop.Utility
{
    public class FileHelper : IFileHelper
    {
        public void SaveFile(string filePath, IFormFile file)
        {          
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
    }
}
