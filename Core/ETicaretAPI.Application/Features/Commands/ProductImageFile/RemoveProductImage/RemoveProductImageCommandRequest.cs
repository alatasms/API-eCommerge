using ETicaretAPI.Application.Abstraction.Storage;
using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Domain.Entities;


namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandRequest : IRequest<RemoveProductImageCommandResponse>
    {
        [FromRoute]
        public string Id { get; set; }
        [FromQuery]
        public string imageId { get; set; }


        public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
        {
            readonly IProductReadRepository _productReadRepository;
            readonly IProductWriteRepository _productWriteRepository;
            readonly IProductImageFileWriteRepository _fileWriteRepository;
            readonly IStorageService _storageService;

            public RemoveProductImageCommandHandler(IProductReadRepository productReadRepository,
                IProductWriteRepository productWriteRepository,
                IProductImageFileWriteRepository fileWriteRepository,
                IStorageService storageService)
            {
                _productReadRepository = productReadRepository;
                _productWriteRepository = productWriteRepository;
                _fileWriteRepository = fileWriteRepository;
                _storageService = storageService;
            }

            public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
            {
                E.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                    .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

                E.ProductImageFile? productImageFile = product?.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.imageId));
                if (productImageFile != null)
                    product?.ProductImageFiles.Remove(productImageFile);
                await _productWriteRepository.SaveAsync();

                await _storageService.DeleteAsync("product-images", productImageFile.FileName);
                _fileWriteRepository.Remove
                    (productImageFile);
                return new();
            }
        }
    }
}
