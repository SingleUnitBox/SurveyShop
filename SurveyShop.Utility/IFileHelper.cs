using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShop.Utility
{
    public interface IFileHelper
    {
        void SaveFile(string filePath, IFormFile file);
    }
}
