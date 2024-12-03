using PexitaMVC.Infrastructure.Services;
using PexitaMVC.Infrastructure.Data;
using Moq;
using Microsoft.EntityFrameworkCore;
using PexitaMVC.Core.Entites;
using AutoMapper;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Application.DTOs;

namespace PexitaMVC.Tests
{
    public class BillServiceTests
    {
        private readonly Mock<IBillRepository> _billRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly BillService _billService;

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
        }

        [Fact]
        public async Task AddBillAsync_ShouldAddBillAndReturnBillDTO()
        {
            // Arrange
            var billCreateDTO = new BillCreateDTO
            {
                OwnerID = 1,
                Title = "Dinner",
                Usernames = new Dictionary<string, double> { { "John", 50.0 } }
            };

            UserModel owner = new() { Id = "1", UserName = "Owner", Name = "Eli" };
            BillModel billModel = new() { Id = 1, Title = billCreateDTO.Title, User = owner, UserID = owner.Id };
            UserModel payer = new() { Id = "2", Name = "John", UserName = "John" };
            List<PaymentModel> paymentModels = [new() { User = payer, Amount = 50.0, Bill = billModel, UserId = payer.Id }];


            var billDTO = new BillDTO()
            {
                Payments = [new SubPaymentDTO() { Amount = paymentModels[0].Amount, IsPaid = true, BillID = billModel.Id }],
                Title = billCreateDTO.Title,
                Owner = new SubUserDTO() { ID = owner.Id, Name = owner.Name, Username = owner.UserName },
                TotalAmount = billModel.TotalAmount,
            };

            _mapperMock.Setup(m => m.Map<BillModel>(billCreateDTO)).Returns(billModel);
            _userRepositoryMock.Setup(u => u.GetByID(billCreateDTO.OwnerID)).Returns(owner);
            _userRepositoryMock.Setup(u => u.GetUsersByUsernamesAsync(It.IsAny<ICollection<string>>())).ReturnsAsync([payer]);
            _billRepositoryMock.Setup(b => b.AddAsync(billModel)).ReturnsAsync(billModel);
            _mapperMock.Setup(m => m.Map<BillDTO>(billModel)).Returns(billDTO);

            // Act
            BillDTO result = await _billService.AddBillAsync(billCreateDTO);

            // Assert
            _mapperMock.Verify(m => m.Map<BillModel>(billCreateDTO), Times.Once);
            _userRepositoryMock.Verify(u => u.GetByID(billCreateDTO.OwnerID), Times.Once);
            _billRepositoryMock.Verify(b => b.AddAsync(billModel), Times.Once);
            Assert.Equal(billDTO, result);
        }

        [Fact]
        public async Task DeleteBillAsync_ShouldDeleteBill()
        {
            // Arrange
            var billId = 1;
            var bill = new BillModel { Id = billId, Title = "Dinner", User = new UserModel { Name = "Alireza" }, UserID = "1" };

            _billRepositoryMock.Setup(b => b.GetByIDAsync(billId)).ReturnsAsync(bill);
            _billRepositoryMock.Setup(b => b.DeleteAsync(bill)).Returns(Task.CompletedTask);

            // Act
            await _billService.DeleteBillAsync(billId);

            // Assert
            _billRepositoryMock.Verify(b => b.GetByIDAsync(billId), Times.Once);
            _billRepositoryMock.Verify(b => b.DeleteAsync(bill), Times.Once);
        }

    }

}