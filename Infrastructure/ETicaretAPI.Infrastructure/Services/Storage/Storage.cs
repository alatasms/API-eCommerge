using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public abstract class Storage
    {
        //protected delegate bool HasFile(string pathOrContainerName, string fileName);

        public async Task<string> FileRenameAsync(string pathOrContainerName, string fileName)
        {
            string fileNewName = await Task.Run<string>(async () =>
             {
                 string extension = Path.GetExtension(fileName);
                 return $"{Path.GetFileNameWithoutExtension(fileName).Replace(" ", "-")}-{Guid.NewGuid()}{extension}";

             });
            return fileNewName;
            
        }
    }
}
