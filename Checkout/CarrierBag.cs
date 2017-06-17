﻿using System;

namespace Checkout.Core
{
	/// <summary>
	/// Carrier Bag class.
	/// </summary>
	public class CarrierBag : ICarrierBag
	{
		private readonly decimal BagPrice = CheckoutSettings.Default.CarrierBagPrice;

		private readonly int BagCapacity = CheckoutSettings.Default.CarrierBagCapacity;

		public CarrierBag()
		{
			
		}

		public decimal Charge { get; set; }

		public decimal CalculateBagCharge(int numberOfItems)
		{
			if (numberOfItems <= 0)
			{
				return Charge = 0;
			}

			var bagsRequired = Math.DivRem(numberOfItems, BagCapacity, out int remainder);
			if (remainder > 0)
			{
				// bag overlflow, so add another bag
				bagsRequired += 1;
			}

			var bagCharge = bagsRequired * BagPrice;
			return Charge = bagCharge;
		}
	}
}