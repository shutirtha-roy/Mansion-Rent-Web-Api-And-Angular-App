﻿using MansionRentBackend.Application.DbContexts;
using MansionRentBackend.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MansionRentBackend.Application.UnitOfWorks
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IMansionRepository Mansions { get; private set; }
        public IUserRepository Users { get; private set; }

        public ApplicationUnitOfWork(IApplicationDbContext dbContext, 
            IMansionRepository mansionRepository,
            IUserRepository userRepository) : base((DbContext)dbContext)
        {
            Mansions = mansionRepository;
            Users = userRepository;
        }
    }
}