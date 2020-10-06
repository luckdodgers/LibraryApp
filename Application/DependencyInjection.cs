﻿using AutoMapper;
using LibraryApp.Application.Common.Behaviours;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Infrastructure.Persistance;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LibraryApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IApplicationDbContext), typeof(AppDbContext));
            //services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionBehaviour<,,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));

            return services;
        }
    }
}