using ETicaretAPI.Application.Abstraction.Storage;
using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Domain.Entities;


namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandRequest : IRequest<UploadProductImageCommandResponse>
    {
        public string Id { get; set; }
        public IFormFileCollection? Files { get; set; }

        public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
        {
            IStorageService _storageService;
            IProductReadRepository _productReadRepository;
            IProductImageFileWriteRepository _productImageFileWriteRepository;

            public UploadProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository, IStorageService storageService)
            {
                _productImageFileWriteRepository = productImageFileWriteRepository;
                _productReadRepository = productReadRepository;
                _storageService = storageService;
            }

            public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
            {
                var datas = await _storageService.UploadAsync("product-images", request.Files);

                E.Product product = await _productReadRepository.GetByIdAsync(request.Id);
                await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new E.ProductImageFile()
                {
                    FileName = d.fileName,
                    Path = d.pathOrContainerName,
                    Storage = _storageService.StorageName,
                    Product = new List<E.Product>()
                {
                    product
                }

                }).ToList());
                await _productImageFileWriteRepository.SaveAsync();

                return new();
            }
        }
    }
}
