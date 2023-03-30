using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
        {
            readonly IProductWriteRepository _productWriteRepository;
            readonly IProductReadRepository _productReadRepository;

            public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository = null)
            {
                _productWriteRepository = productWriteRepository;
                _productReadRepository = productReadRepository;
            }

            public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
            {
                P.Product product = await _productReadRepository.GetByIdAsync(request.Id);
                product.Stock = request.Stock;
                product.Name = request.Name;
                product.Price = request.Price;
                await _productWriteRepository.SaveAsync();
                return new();
            }
        }
    }
}
