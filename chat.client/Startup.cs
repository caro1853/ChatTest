using chat.core.Interfaces;
using chat.core.Services;
using chat.Data;
using chat.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chat.client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<AppDbContext>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<ISessionRepository, SessionRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IMessageService, MessageService>();
        }
    }
}