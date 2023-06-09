﻿using ETicaretAPI.Application.Repositories;
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

            public CreateProductCommandHandler(IProductWriteRepository productWriteRepository)
            {
                _productWriteRepository = productWriteRepository;
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
                return new();
            }
        }
    }
}
