using AutoMapper;
using Moq;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Services;
using PexitaMVC.Application.Exceptions;
namespace PexitaMVC.Tests
{
    public class BillServiceTests
    {
        private readonly Mock<IBillRepository> _billRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly BillService _billService;
        private readonly BillModel bill;
        private readonly List<PaymentModel> paymentModels;
        private readonly BillDTO billDTO;
        private readonly UserModel owner = new() { Id = "1", UserName = "Owner", Name = "Eli" };
        private readonly UserModel payer = new() { Id = "2", UserName = "John", Name = "John" };
        private readonly SubUserDTO ownerDTO;
        private readonly SubUserDTO payerDTO;
        public BillServiceTests()
        {
            _billRepositoryMock = new Mock<IBillRepository>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _billService = new BillService(
                _billRepositoryMock.Object,
                _mapperMock.Object,
                _userRepositoryMock.Object
            );
            ownerDTO = new() { ID = owner.Id, Username = owner.UserName, Name = owner.Name };
            payerDTO = new() { ID = payer.Id, Username = payer.UserName, Name = payer.Name };
            paymentModels = [new() { Id = 1, UserId = "1", Amount = 20, IsPaid = true }, new() { Id = 2, Amount = 5, UserId = "2", IsPaid = false }];
            bill = new() { Id = 1, Title = "Dinner", Owner = owner, OwnerID = owner.Id, TotalAmount = 25, BillPayments = paymentModels };
            billDTO = new BillDTO()
            {
                Title = bill.Title,
                TotalAmount = bill.TotalAmount,
                Owner = new SubUserDTO() { ID = owner.Id, Name = owner.Name, Username = owner.UserName },
                Payments =
                    [
                        new() { Amount = paymentModels[0].Amount, IsPaid = true, BillID = bill.Id, UserID = paymentModels[0].UserId, Payer = ownerDTO },
                        new() {ID = paymentModels[1].Id, Amount = paymentModels[1].Amount, IsPaid = paymentModels[1].IsPaid, UserID = paymentModels[1].UserId, Payer = payerDTO }
                    ],
            };

        }

        [Fact]
        public async Task AddBillAsync_GivenValidBill_ShouldAddBillAndReturnBillDTO()
        {
            // Arrange
            var billCreateDTO = new BillCreateDTO
            {
                OwnerID = owner.Id,
                Title = "Dinner",
                Usernames = new Dictionary<string, double> { { "John", 50.0 } },
                TotalAmount = 50.0
            };

            _mapperMock.Setup(m => m.Map<BillModel>(billCreateDTO)).Returns(bill);
            _userRepositoryMock.Setup(u => u.GetByID(int.Parse(billCreateDTO.OwnerID))).Returns(owner);
            _userRepositoryMock.Setup(u => u.GetUsersByUsernamesAsync(It.IsAny<ICollection<string>>())).ReturnsAsync([owner, payer]);
            _billRepositoryMock.Setup(b => b.AddAsync(bill)).ReturnsAsync(bill);
            _mapperMock.Setup(m => m.Map<BillDTO>(bill)).Returns(billDTO);

            // Act
            BillDTO result = await _billService.AddBillAsync(billCreateDTO);

            // Assert
            _mapperMock.Verify(m => m.Map<BillModel>(billCreateDTO), Times.Once);
            _userRepositoryMock.Verify(u => u.GetByID(int.Parse(billCreateDTO.OwnerID)), Times.Once);
            _billRepositoryMock.Verify(b => b.AddAsync(bill), Times.Once);
            Assert.Equal(billDTO, result);
        }

        [Fact]
        public async Task AddBillAsync_GivenInvalidUserID_ShouldThrowException()
        {
            // Arrange
            var billCreateDTO = new BillCreateDTO
            {
                OwnerID = "132913901",
                Title = "Dinner",
                Usernames = new Dictionary<string, double> { { "John", 50.0 } },
                TotalAmount = 50.0
            };
            _userRepositoryMock.Setup(x => x.GetByID(int.Parse(billCreateDTO.OwnerID))).Returns((UserModel)null!);

            // Act and assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _billService.AddBillAsync(billCreateDTO);
            });

            Assert.Equal("User with ID 132913901 Not Found.", exception.Message);

            // Verify that the method was called as expected
            _userRepositoryMock.Verify(repo => repo.GetByID(It.IsAny<int>()), Times.Once);
            _billRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<BillModel>()), Times.Never); // Ensure no bill were added
        }

        [Fact]
        public async Task DeleteBillAsync_GivenBillID_ShouldDeleteBill()
        {
            // Arrange
            var billId = 1;

            _billRepositoryMock.Setup(b => b.GetByIDAsync(billId)).ReturnsAsync(bill);
            _billRepositoryMock.Setup(b => b.DeleteAsync(bill)).ReturnsAsync(1);

            // Act
            await _billService.DeleteBillAsync(billId);

            // Assert
            _billRepositoryMock.Verify(b => b.GetByIDAsync(billId), Times.Once);
            _billRepositoryMock.Verify(b => b.DeleteAsync(bill), Times.Once);
        }

        [Fact]
        public async Task DeleteBill_GivenInvalidBillID_ShouldThrowNotFoundException()
        {
            // Arrange
            int billID = 123;

            _billRepositoryMock.Setup(x => x.GetByID(billID)).Returns((BillModel)null!);
            _billRepositoryMock.Setup(b => b.DeleteAsync(bill)).ReturnsAsync(0);
            // Act and assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => { await _billService.DeleteBillAsync(billID); });

            // Assert
            _billRepositoryMock.Verify(x => x.GetByIDAsync(billID), Times.Once);
            _billRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<BillModel>()), Times.Never);
        }

        [Fact]
        public async Task DeleteBill_GivenDBError_ShouldThrowInvalidOperationException()
        {
            // Arrange
            int billID = 123;

            _billRepositoryMock.Setup(x => x.GetByIDAsync(billID)).ReturnsAsync(bill);
            _billRepositoryMock.Setup(b => b.DeleteAsync(bill)).ReturnsAsync(0);

            // Act and assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => { await _billService.DeleteBillAsync(billID); });

            // Assert
            Assert.Equal("No Users Were Deleted.", exception.Message);
            _billRepositoryMock.Verify(x => x.GetByIDAsync(billID), Times.Once);
            _billRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<BillModel>()), Times.Once);
        }
    }
}