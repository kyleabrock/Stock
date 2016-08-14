﻿using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Stock.Core.Domain;

namespace Stock.Core.Tests
{
    [TestFixture]
    class ExportSchemaTest
    {
        [Test]
        public void Can_generate_schema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Unit).Assembly);

            new SchemaExport(cfg).Execute(true, false, false);
        }

        [Test]
        public void Can_update_schema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Unit).Assembly);
            
            new SchemaUpdate(cfg).Execute(true, false);
        }
    }
}
