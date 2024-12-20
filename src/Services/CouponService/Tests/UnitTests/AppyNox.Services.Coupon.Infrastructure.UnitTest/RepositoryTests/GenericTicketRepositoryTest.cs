﻿using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.Exceptions;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Domain.Localization;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds;
using Microsoft.Extensions.Localization;
using Moq;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.RepositoryTests;

public class GenericTicketRepositoryTest : IClassFixture<RepositoryFixture>
{
    #region [ Fields ]

    private readonly ICacheService _cacheService;

    private readonly NoxInfrastructureLoggerStub<UnitOfWorkBase> _unitOfWorkBaseLogger;

    private readonly NoxInfrastructureLoggerStub<GenericRepositoryBase<Ticket>> _genericRepositoryLogger;

    #endregion

    #region [ Public Constructors ]

    public GenericTicketRepositoryTest(RepositoryFixture fixture)
    {
        _unitOfWorkBaseLogger = new NoxInfrastructureLoggerStub<UnitOfWorkBase>();
        _genericRepositoryLogger = new NoxInfrastructureLoggerStub<GenericRepositoryBase<Ticket>>();

        _cacheService = fixture.RedisCacheService.Object;
        var localizer = new Mock<IStringLocalizer>();
        localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

        var localizerFactory = new Mock<IStringLocalizerFactory>();
        localizerFactory.Setup(lf => lf.Create(typeof(CouponDomainResourceService))).Returns(localizer.Object);

        CouponDomainResourceService.Initialize(localizerFactory.Object);
    }

    #endregion

    #region [ CRUD Methods ]

    #region [ Read ]

    [Fact]
    public async Task GetAllAsync_ShouldReturnEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        await context.SeedOneTicket(unitOfWork);
        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = CommonDetailLevels.Simple,
            PageNumber = 1,
            PageSize = 1,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnTwo()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        await context.SeedMultipleTickets(unitOfWork, 2, 1);
        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 1,
            PageSize = 2,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Equal(2, result.ItemsCount);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnCorrectEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var tickets = await context.SeedMultipleTickets(unitOfWork, 2, 1);
        Assert.NotNull(tickets);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 2,
            PageSize = 1,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Single(result.Items);

        Assert.Equal(tickets.Last().Id, result.Items.First().Id);
        Assert.Equal(tickets.Last().Title, result.Items.First().Title);
        Assert.Equal(tickets.Last().Content, result.Items.First().Content);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFifty()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        await context.SeedMultipleTickets(unitOfWork, 50, 5);
        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 1,
            PageSize = 50,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Equal(50, result.ItemsCount);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var tickets = await context.SeedMultipleTickets(unitOfWork, 50, 5);
        Assert.NotNull(tickets);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 5,
            PageSize = 5,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        var expectedTickets = tickets.Skip(20).Take(5).ToList();
        var resultList = result.Items.ToList();

        Assert.NotNull(result);
        Assert.Equal(expectedTickets.Count, result.ItemsCount);

        for (int i = 0; i < expectedTickets.Count; i++)
        {
            Assert.Equal(expectedTickets[i].Id, resultList[i].Id);
            Assert.Equal(expectedTickets[i].Title, resultList[i].Title);
            Assert.Equal(expectedTickets[i].Content, resultList[i].Content);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var existingTicket = await context.SeedOneTicket(unitOfWork);
        Assert.NotNull(existingTicket);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);
        var result = await repository.GetByIdAsync(existingTicket.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldValuesBeCorrect()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var existingTicket = await context.SeedOneTicket(unitOfWork);
        Assert.NotNull(existingTicket);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);

        var result = await repository.GetByIdAsync(existingTicket.Id);

        Assert.NotNull(result);

        Assert.Equal(existingTicket.Id, result.Id);
        Assert.Equal(existingTicket.Title, result.Title);
        Assert.Equal(existingTicket.Content, result.Content);
        Assert.NotNull(existingTicket.Tags);
        Assert.NotNull(result.Tags);
        Assert.Equal(existingTicket.Tags.Count, result.Tags.Count());
    }

    #endregion

    #region [ Create ]

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);

        TicketTag tagToAdd = new()
        {
            Description = "description"
        };
        Ticket ticketToAdd = new()
        {
            Title = "test title",
            Content = "test content",
            ReportDate = DateTime.Now,
            Tags = [tagToAdd]
        };

        await repository.AddAsync(ticketToAdd);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(ticketToAdd.Id);

        Assert.NotNull(result);

        Assert.Equal(ticketToAdd.Id, result.Id);
        Assert.Equal(ticketToAdd.Title, result.Title);
        Assert.Equal(ticketToAdd.Content, result.Content);
        Assert.NotNull(ticketToAdd.Tags);
        Assert.NotNull(result.Tags);
        Assert.Equal(ticketToAdd.Tags.First().Id, result.Tags.First().Id);
        Assert.Equal(ticketToAdd.Tags.First().Description, result.Tags.First().Description);
    }

    #endregion

    #region [ Update ]

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTicket()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var existingTicket = await context.SeedOneTicket(unitOfWork);
        Assert.NotNull(existingTicket);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);

        // Hold ticket old data
        Guid id = existingTicket.Id;
        string title = $"{existingTicket.Title} updated";
        string content = $"{existingTicket.Content} updated";
        DateTime reportDate = existingTicket.ReportDate;

        // Create the update dto
        TicketSimpleUpdateDto ticketSimpleUpdateDto = new() { Id = id, Content = content, Title = title};

        repository.Update(existingTicket, ticketSimpleUpdateDto);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(id, result.Id);
        Assert.Equal(title, result.Title);
        Assert.Equal(content, result.Content);
        Assert.Equal(reportDate, result.ReportDate); // make sure the value which is not in update dto is not changed.
    }

    #endregion

    #region [ Delete ]

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var existingTicket = await context.SeedOneTicket(unitOfWork);
        Assert.NotNull(existingTicket);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);

        await repository.RemoveByIdAsync(existingTicket.Id);
        await unitOfWork.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<NoxEntityNotFoundException<Ticket>>(async () =>
        {
            var result = await repository.GetByIdAsync(existingTicket.Id);
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeleteEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var existingTicket = await context.SeedOneTicket(unitOfWork);
        Assert.NotNull(existingTicket);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);

        await repository.RemoveByIdAsync(existingTicket.Id);
        await unitOfWork.SaveChangesAsync();

        Ticket result = await repository.GetByIdAsync(existingTicket.Id, true);

        Assert.NotNull(result);
        Assert.True(result.IsDeleted);
        Assert.NotNull(result.DeletedBy);
        Assert.NotNull(result.DeletedDate);
    }

    [Fact]
    public async Task DeleteAsync_ShouldHardDeleteEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _unitOfWorkBaseLogger);
        var existingTicket = await context.SeedOneTicket(unitOfWork);
        Assert.NotNull(existingTicket);

        var repository = new GenericRepository<Ticket>(context, _genericRepositoryLogger);

        await repository.RemoveByIdAsync(existingTicket.Id, true);
        await unitOfWork.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<NoxEntityNotFoundException<Ticket>>(async () =>
        {
            var result = await repository.GetByIdAsync(existingTicket.Id, true);
        });

        Assert.NotNull(exception);
    }

    #endregion

    #endregion
}