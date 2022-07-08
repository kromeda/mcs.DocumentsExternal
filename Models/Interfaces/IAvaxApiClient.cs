using Models;
using System;
using System.Threading.Tasks;

namespace DocumentsExternal.Models.Interfaces
{
    public interface IAvaxApiClient
    {
        Task<FileDocumentView> GetNotificationFile(Guid id);
    }
}
