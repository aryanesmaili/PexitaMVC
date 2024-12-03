using AutoMapper;
using Moq;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Services;

namespace PexitaMVC.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;
        private readonly List<BillModel> unpaidBills;
        private readonly List<BillDTO> unpaidBillsDTO;
        private readonly List<BillDTO> paidBillsDTO;
        private readonly BillDTO paidBillDTO;
        private readonly List<BillModel> AllPaidBills;
        private readonly List<BillModel> MixedBills;
        private readonly List<PaymentModel> OwnerPayments = [new() { UserId = "1", Amount = 20, IsPaid = true }, new() { Amount = 5, UserId = "1", IsPaid = false }];
        private readonly UserModel Owner = new()
        {
            Name = "John",
            Email = "John@Gmail.com",
            UserName = "JohnDoe",
            PhoneNumber = "1234567890",
        };
        private readonly BillModel paidBill;
        private readonly BillModel unpaidBill;
        private readonly BillDTO unpaidBillDTO;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new(_userRepositoryMock.Object, _mapperMock.Object);

            paidBill = new()
            {
                Id = 13,
                OwnerID = "1",
                Owner = Owner,
                Title = "Lunch",
                TotalAmount = 50,
                BillPayments = [OwnerPayments[0], new() { UserId = "2", IsPaid = true, Amount = 30, },]
            };

            paidBillDTO = new()
            {
                ID = paidBill.Id,
                Title = paidBill.Title,
                TotalAmount = paidBill.TotalAmount,
                Owner = new() { Name = Owner.Name, Email = Owner.Email, PhoneNumber = Owner.PhoneNumber, Username = Owner.UserName, },
                Payments = [new() { Amount = paidBill.BillPayments.ToList()[1].Amount, IsPaid = false, UserID = paidBill.BillPayments.ToList()[1].UserId }],
            };

            unpaidBill = new()
            {
                Id = 12,
                OwnerID = "1",
                Owner = Owner,
                Title = "Dinner",
                TotalAmount = 20,
                BillPayments = [OwnerPayments[1], new() { Amount = 15, UserId = "2" },]
            };

            unpaidBillDTO = new()
            {
                ID = unpaidBill.Id,
                Title = unpaidBill.Title,
                TotalAmount = unpaidBill.TotalAmount,
                Owner = new() { Name = Owner.Name, Email = Owner.Email, PhoneNumber = Owner.PhoneNumber, Username = Owner.UserName, },
                Payments = [new() { Amount = unpaidBill.BillPayments.ToList()[1].Amount, IsPaid = false, UserID = unpaidBill.BillPayments.ToList()[1].UserId }],
            };

            Owner.UserPayments = OwnerPayments;
            Owner.Bills = [paidBill, unpaidBill];

            AllPaidBills = [paidBill];
            unpaidBills = [unpaidBill];
            MixedBills = [paidBill, unpaidBill];
            unpaidBillsDTO = [unpaidBillDTO];
            paidBillsDTO = [paidBillDTO];
        }

        [Fact]
        public void GetUnpaidBillsForUser_GivenUserID_ShouldReturnBills()
        {
            // Arrange
            string UserID = "1";

            _userRepositoryMock.Setup(x => x.GetUnpaidBillsForUser(UserID)).Returns(unpaidBills);
            _mapperMock.Setup(x => x.Map<BillDTO>(It.IsAny<BillModel>()))
                       .Returns(unpaidBillDTO);

            // Act
            var result = _userService.GetUnpaidBillsForUser(UserID);

            //Assert
            Assert.Equal(unpaidBillsDTO, result.ToList());
        }

        [Fact]
        public void GetUnpaidBillsForUser_GivenInvalidUserID_ShoudlReturnEmpty()
        {
            //Arrange
            string UserID = "3";

            _userRepositoryMock.Setup(x => x.GetUnpaidBillsForUser(UserID)).Returns([]);
            _mapperMock.Setup(x => x.Map<BillDTO>(It.IsAny<BillModel>()))
                       .Returns(unpaidBillDTO);
            // Act
            var result = _userService.GetUnpaidBillsForUser(UserID);

            // Assert
            Assert.Equal([], result);
        }

        [Fact]
        public void GetUnpaidBillsForUser_GivenValidIDWithNoDebts_ShoudlReturnEmpty()
        {
            // Arrange
            string UserID = "1";

            _userRepositoryMock.Setup(x => x.GetUnpaidBillsForUser(UserID)).Returns([]);
            _mapperMock.Setup(x => x.Map<BillDTO>(It.IsAny<BillModel>()))
                       .Returns(paidBillDTO);

            // Act
            var result = _userService.GetUnpaidBillsForUser(UserID);

            // Assert
            Assert.Equal([], result.ToList());
        }

        [Fact]
        public void IsUserInDebt_GivenInDebtUser_ReturnsTrue()
        {
            // Arrange
            string UserID = "1";
            _userRepositoryMock.Setup(x => x.GetWithRelations(int.Parse(UserID), u => u.UserPayments)).Returns(Owner);

            // Act
            bool result = _userService.IsUserInDebt(UserID);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsUserInDebt_GivenUserWithoutDebt_ReturnsFalse()
        {
            // Arrange
            string UserID = "1";
            PaymentModel p = Owner.UserPayments.First(X => !X.IsPaid);
            Owner.UserPayments.Remove(p);

            _userRepositoryMock.Setup(x => x.GetWithRelations(int.Parse(UserID), u => u.UserPayments)).Returns(Owner);

            // Act
            bool result = _userService.IsUserInDebt(UserID);

            // Assert
            Assert.False(result);
        }
    }
}
