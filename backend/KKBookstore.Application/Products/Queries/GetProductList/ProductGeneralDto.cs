﻿using KKBookstore.Application.Common.Models;

namespace KKBookstore.Application.Products.Queries.GetProductList;

public record ProductGeneralDto : BaseDto
{
    public string Name { get; set; }
    public int ProductTypeId { get; set; }
    public string Description { get; set; }
    public bool IsBook { get; set; }
    public string ThumbnailImageUrl { get; set; }
    public decimal MinUnitPrice { get; set; }
    public decimal MinRecommendedRetailPrice { get; set; }
    public decimal AverageRating { get; set; }
}