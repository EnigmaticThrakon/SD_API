using FluentMigrator.Builders.Create.Table;

namespace AgerDevice.DataAccess.Migrations
{
    /// <summary>
    /// Extension methods for common column properties. 
    /// IMPORTANT: The methods below should NEVER be modified, only new versions should be created.
    /// </summary>
    internal static class MigrationExtensions
    {
        public static ICreateTableWithColumnSyntax WithAgerDeviceModel(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("Id").AsString(36).PrimaryKey()
                .WithColumn("Timestamp").AsDateTime()
                .WithColumn("Modified").AsDateTime().Nullable();
        }
    }
}