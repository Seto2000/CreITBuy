﻿using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CreITBuy.TokenProviders
{
    public class EmailConfirmationTokenProvider<TUser>
                               : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public EmailConfirmationTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<EmailConfirmationTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
                                           : base(dataProtectionProvider, options, logger)
        {

        }
    }
}