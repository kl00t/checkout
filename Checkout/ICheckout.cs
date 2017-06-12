﻿namespace Checkout.Core
{
    /// <summary>
    /// Checkout interface definition.
    /// </summary>
    interface ICheckout
    {
        /// <summary>
        /// Scans the specified item.
        /// </summary>
        /// <param name="item">The name of scanned item.</param>
        void Scan(string item);

        /// <summary>
        /// Cancels the scanned scan.
        /// </summary>
        /// <param name="item">The item.</param>
        void CancelScan(string item);

        /// <summary>
        /// Gets the total price.
        /// </summary>
        /// <returns>Returns the total price as a whole number.</returns>
        int GetTotalPrice();

        /// <summary>
        /// Gets the total discounts.
        /// </summary>
        /// <returns>Returns the total discounts message.</returns>
        string GetTotalDiscounts();
    }
}