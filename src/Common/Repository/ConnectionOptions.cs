﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repository
{
    public class ConnectionOptions
    {
        public string ConnectionString { get; set; }

        public bool EnableDbContextMigration { get; set; }
    }
}
