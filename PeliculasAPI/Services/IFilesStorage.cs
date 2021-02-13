using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Services
{
    public interface IFilesStorage
    {
        Task<string> EditFile(byte[] content, string extension, string container, string path, string contentType);
        Task DeleteFile(string container, string path);
        Task<string> SaveFile(byte[] content, string extension, string container, string contentType);
    }
}
