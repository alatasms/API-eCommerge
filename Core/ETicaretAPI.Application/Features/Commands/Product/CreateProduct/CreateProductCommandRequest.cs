﻿using ETicaretAPI.Application.Abstraction.Hubs;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }

        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
        {
            readonly IProductWriteRepository _productWriteRepository;
            readonly IProductHubService _productHubService;

            public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService)
            {
                _productWriteRepository = productWriteRepository;
                _productHubService = productHubService;
            }

            public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
            {
                await _productWriteRepository.AddAsync(new()
                {
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock
                });

                await _productWriteRepository.SaveAsync();
                await _productHubService.ProductAddedMessageAsync($"{request.Name} isminde bir ürün eklendi.");
                return new();
            }
        }
    }
}
