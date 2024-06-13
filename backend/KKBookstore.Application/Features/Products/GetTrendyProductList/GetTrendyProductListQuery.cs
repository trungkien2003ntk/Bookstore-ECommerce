﻿using AutoMapper;
using FluentValidation.Validators;
using KKBookstore.Application.Common.Interfaces;
using KKBookstore.Domain.Aggregates.ProductTypeAggregate;
using KKBookstore.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using static KKBookstore.Application.Features.Products.GetTrendyProductList.GetTrendyProductListResponse;

namespace KKBookstore.Application.Features.Products.GetTrendyProductList;

public record GetTrendyProductListQuery(int? ProductTypeId = default) : IRequest<Result<GetTrendyProductListResponse>>;

// todo: considering merge this query with GetProductListQuery
public class GetTrendyProductListQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetTrendyProductListQuery, Result<GetTrendyProductListResponse>>
{
    public async Task<Result<GetTrendyProductListResponse>> Handle(GetTrendyProductListQuery request, CancellationToken cancellationToken)
    {
        // now as initialize, return a list of random products
        var numberOfTrendyProducts = 12;

        var boughtProductIds = await dbContext.OrderLines
            .Include(ol => ol.Sku)
            .Select(ol => ol.Sku.ProductId)
            .ToListAsync(cancellationToken);

        var topMostPurchasedProductIds = boughtProductIds
            .GroupBy(id => id)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Take(numberOfTrendyProducts)
            .ToList();

        var productGeneralQueryable = dbContext.Products
            .Where(p => topMostPurchasedProductIds.Contains(p.Id))
            .Include(p => p.ProductImages)
            .Include(p => p.ProductType)
            .Include(p => p.Ratings)
            .Include(p => p.Skus)
            .Select(p =>
                new ProductSummary
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductTypeId = p.ProductTypeId,
                    ProductTypeName = p.ProductType.DisplayName,
                    IsBook = p.IsBook,
                    SoldCount = dbContext.OrderLines
                        .Include(ol => ol.Sku)
                        .Count(ol => ol.Sku.ProductId == p.Id),
                    MinUnitPrice = p.Skus.Min(s => s.UnitPrice),
                    MinRecommendedRetailPrice = p.Skus.Min(s => s.RecommendedRetailPrice),
                    AverageRating = Convert.ToDecimal(p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue) : 0),
                    IsActive = p.IsActive,
                    ThumbnailImageUrl = p.GetFirstThumbnailImageUrl()
                })
            .AsQueryable();


        var productGeneralDtos = await productGeneralQueryable.ToListAsync(cancellationToken);

        static int comparison(ProductSummary y, ProductSummary x) => x.SoldCount.CompareTo(y.SoldCount);
        productGeneralDtos.Sort(comparison);

        var response = new GetTrendyProductListResponse
        {
            Items = productGeneralDtos
        };

        return response;
    }
}