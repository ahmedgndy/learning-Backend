
using System;
using System.Data;
using Dapper;

public class GuidHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value) => parameter.Value = value.ToString();

    public override Guid Parse(object value) => Guid.Parse(value.ToString());
}