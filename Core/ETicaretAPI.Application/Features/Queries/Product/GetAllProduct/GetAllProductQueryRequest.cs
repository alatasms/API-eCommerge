﻿using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
    {
        public int Page { get; set; } = 0;
        
        public int Size { get; set; } = 5;

        public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
        {
            readonly IProductReadRepository _productReadRepository;

            public GetAllProductQueryHandler(IProductReadRepository productReadRepository)
            {
                _productReadRepository = productReadRepository;
            }

            public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
            {
                var totalCount = _productReadRepository.GetAll().Count();
                var products = _productReadRepository.GetAll().Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Stock,
                    p.Price,
                    p.CreatedDate,
                    p.UpdatedDate
                }).Skip(request.Page * request.Size).Take(request.Size).ToList();

                return new()
                {
                    Products = products,
                    TotalCount = totalCount
                };
            }
        }
    }
}
