﻿using Microsoft.EntityFrameworkCore;

namespace Lib;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }


}