using System;
using System.IO;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;

namespace AdsSystem.Libs
{
    public class FileStorage
    {
        public static string PutFile(IFormFile file, IModelWithId model)
        {
            var basePath = GetBasePath();
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            
            var modelFolder = Path.Combine(basePath, model.GetType().Name);
            
            if (!Directory.Exists(modelFolder))
                Directory.CreateDirectory(modelFolder);
            
            var extension = Path.GetExtension(file.FileName).Replace(".", "");
            
            var path = Path.Combine(modelFolder, model.Id + "." + extension);
            using (var fileStream = new FileStream(path, FileMode.Create)) 
                file.CopyTo(fileStream);
            
            return extension;
        }

        private static string GetBaseLink() => "uploads";
        private static string GetBasePath() => Path.Combine(Environment.CurrentDirectory, "public", GetBaseLink());
        
        public static string GetLink(IModelWithId model, string format) => 
            Path.Combine(GetBaseLink(), model.GetType().Name, model.Id + "." + format);
    }
}