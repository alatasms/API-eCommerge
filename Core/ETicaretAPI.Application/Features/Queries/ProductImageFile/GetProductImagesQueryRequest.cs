﻿using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Domain.Entities;


namespace ETicaretAPI.Application.Features.Queries.ProductImageFile
{
    public class GetProductImagesQueryRequest : IRequest<List<GetProductImagesQueryResponse>>
    {
        public string Id { get; set; }

        public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
        {
            readonly IProductReadRepository _productReadRepository;
            readonly IConfiguration _configuration;

            public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
            {
                _productReadRepository = productReadRepository;
                _configuration = configuration;
            }

            public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
            {
                E.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                    .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

                return product?.ProductImageFiles.Select(p => new GetProductImagesQueryResponse
                {

                    Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                    FileName = p.FileName,
                    Id = p.Id

                }).ToList();
            }
        }
    }
}
