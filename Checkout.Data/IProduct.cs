﻿namespace Checkout.Data
{
    /// <summary>
    /// Product interface definition.
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// Gets or sets the sku.
        /// </summary>
        /// <value>
        /// The sku.
        /// </value>
        string Sku { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        /// <value>
        /// The unit price.
        /// </value>
        int UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        /// <value>
        /// The product description.
        /// </value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the special offer.
        /// </summary>
        /// <value>
        /// The special offer.
        /// </value>
        SpecialOffer SpecialOffer { get; set; }
    }
}