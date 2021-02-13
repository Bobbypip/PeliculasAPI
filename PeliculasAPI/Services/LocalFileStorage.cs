using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Services
{
    public class LocalFileStorage : IFilesStorage
    {
        // _env to get the wwwroot path to save the files there
        private readonly IWebHostEnvironment _env;
        // _httpContextAccessor to get the domain where the webAPI is published
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorage(
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor )
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string container, string path)
        {
            if (path != null)
            {
                var fileName = Path.GetFileName(path);
                string fileDirectory = Path.Combine(_env.WebRootPath, container, fileName);

                if (File.Exists(fileDirectory))
                {
                    File.Delete(fileDirectory);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string path, string contentType)
        {
            await DeleteFile(container, path);
            return await SaveFile(content, extension, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(path, content);

            var uno = _httpContextAccessor.HttpContext.Request.Scheme;
            var dos = _httpContextAccessor.HttpContext.Request.Host;

            var actualURL = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var forDBURL = Path.Combine(actualURL, container, fileName).Replace("\\","/");

            return forDBURL;
        }
    }
}
