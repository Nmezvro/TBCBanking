using System;
using System.Data;

namespace TBCBanking.Domain.Models.DbExtensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ProcedureParameterAttribute : Attribute
    {
        public ProcedureParameterAttribute(string name, ParameterType pType, DbType dbType)
        {
            Name = name;
            PType = pType;
            DbType = dbType;
        }

        public ProcedureParameterAttribute(string name, ParameterType pType)
        {
            Name = name;
            PType = pType;
        }

        public ProcedureParameterAttribute(string name)
        {
            Name = name;
            PType = ParameterType.Input;
        }

        public ProcedureParameterAttribute(ParameterType pType)
        {
            Name = null;
            PType = pType;
        }

        public ProcedureParameterAttribute()
        {
            Name = null;
            PType = ParameterType.Input;
        }

        public string Name { get; }
        public ParameterType PType { get; }
        public DbType? DbType { get; }
    }
}
