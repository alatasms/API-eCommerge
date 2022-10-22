using ETicaretAPI.Application.Abstraction.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName)
        
          =>  File.Delete($"{path}\\{fileName}");
        

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)

            => File.Exists($"{path}\\{fileName}");

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine
                (_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string filename, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName =
                    Path.GetFileNameWithoutExtension(file.FileName);

                //string fileNewName = await FileRenameAsync
                //    (Path.GetFileNameWithoutExtension(file.FileName));

                await CopyFileAsync($"{uploadPath}\\{fileNewName}-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}", file);
               
                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
            }

            return datas;

            //todo eğer ki yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığnıa dair uyarı oluşturup firlatılması gerekiyor.
        }

        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                //FileStream IDisposable olduğu için using dersek işlem bitince dispose edilir.
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }
    }
}
