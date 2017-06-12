﻿namespace Checkout.Core.Tests
{
    using Data;
	using Moq;
	using NUnit.Framework;

    /// <summary>
    /// Test fixture for checkout.
    /// </summary>
    [TestFixture]
    public class CheckoutTests
    {
        /// <summary>
        /// The checkout.
        /// </summary>
        private Checkout _checkout;

		/// <summary>
		/// The product repository.
		/// </summary>
		private Mock<IProductRepository> _mockProductRepository;

        /// <summary>
        /// Called before each test is run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
			_mockProductRepository = new Mock<IProductRepository>();

			_mockProductRepository.Setup(x => x.GetProductBySkuCode("A"))
				.Returns(new Product
				{
					Sku = "A",
					UnitPrice = 50,
					Description = "Pineapple",
					SpecialOffer = new SpecialOffer
					{
						IsAvailable = true,
						Quantity = 3,
						Discount = 20
					}
				});

			_mockProductRepository.Setup(x => x.GetProductBySkuCode("B"))
				.Returns(
				new Product
				{
					Sku = "B",
					UnitPrice = 30,
					Description = "Mango",
					SpecialOffer = new SpecialOffer
					{
						IsAvailable = true,
						Quantity = 2,
						Discount = 15
					}
				});

			_mockProductRepository.Setup(x => x.GetProductBySkuCode("C"))
				.Returns(
				new Product
				{
					Sku = "C",
					UnitPrice = 20,
					Description = "Kiwi",
					SpecialOffer = new SpecialOffer
					{
						IsAvailable = false
					}
				});

			_mockProductRepository.Setup(x => x.GetProductBySkuCode("D"))
				.Returns(
				new Product
				{
					Sku = "D",
					UnitPrice = 15,
					Description = "Melon",
					SpecialOffer = new SpecialOffer
					{
						IsAvailable = false
					}
				});

			_checkout = new Checkout(_mockProductRepository.Object);
        }

        [Test]
        public void VerifyThatNoScannedItemsReturnsZeroPrice()
        {
            _checkout.Scan(string.Empty);
            Assert.AreEqual(0, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyScannedItemReturnsCorrectPrice()
        {
            _checkout.Scan("A");
            Assert.AreEqual(50, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyTwoScannedItemsCalculatesTotalPrice()
        {
            _checkout.Scan("A");
            _checkout.Scan("B");
            Assert.AreEqual(80, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyNoScannedItemIsIgnore()
        {
            _checkout.Scan("A");
            _checkout.Scan(string.Empty);
            _checkout.Scan("B");
            Assert.AreEqual(80, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyAllScannedProductsReturnsCorrectTotalPrice()
        {
			_checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("C");
            _checkout.Scan("D");
            Assert.AreEqual(115, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyThatDiscountIsAppliedToScannedItems()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            Assert.AreEqual(130, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyThatDiscountIsAppliedThenAdditionalItem()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            Assert.AreEqual(180, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyThatMultipleDiscountsAppliedToScannedItemsInAnyOrder()
        {
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("A");
            Assert.AreEqual(175, _checkout.GetTotalPrice());
        }

        [Test]
        [ExpectedException(typeof(InvalidProductException))]
        public void VerifyThatAnInvalidScannedItemReturnsAnError()
        {
			_mockProductRepository.Setup(x => x.GetProductBySkuCode("Z")).Throws<InvalidProductException>();
			_checkout.Scan("Z");
        }

        [Test]
        public void VerifyThatDiscountIsAppliedMultipleTimesWithSameProduct()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            Assert.AreEqual(260, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyThatDiscountIsAppliedMultipleTimesWithMultipleProducts()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("B");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("B");
            Assert.AreEqual(350, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyThatDiscountedItemsDoesNotTriggerDiscount()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            Assert.AreEqual(100, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyThatScannedItemCancelHasNoEffectOnEmptyBasket()
        {
            _checkout.CancelScan("A");
            Assert.AreEqual(0, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyThatScannedItemCanBeCancelled()
        {
            _checkout.Scan("A");
            _checkout.CancelScan("A");
            Assert.AreEqual(0, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyNoScanCancelledItemIsIgnored()
        {
            _checkout.CancelScan(string.Empty);
            Assert.AreEqual(0, _checkout.GetTotalPrice());
        }

        [Test]
        public void VerifyTotalDiscountsApplied()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            Assert.AreEqual(130, _checkout.GetTotalPrice());
            Assert.AreEqual(20, _checkout.TotalDiscount);
        }

        [Test]
        public void VerifyTotalDiscountsNotAppliedMessage()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.GetTotalPrice();
            Assert.AreEqual("You did not save any money on your shopping today.", _checkout.GetTotalDiscounts());
        }

        [Test]
        public void VerifyTotalDiscountsAppliedMessage()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.GetTotalPrice();
            Assert.AreEqual("You saved £20 on your shopping today.", _checkout.GetTotalDiscounts());
        }

        [Test]
        [Ignore]
        public void VerifySubTotalBeforeDiscountsApplied()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.GetTotalPrice();
            Assert.AreEqual(150, _checkout.SubTotal);
        }
    }
}