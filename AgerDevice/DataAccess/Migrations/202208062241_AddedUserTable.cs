using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062241)]
    public class AddedUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Modified").AsDateTime().NotNullable()
                .WithColumn("SerialNumber").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("LastConnected").AsDateTime().NotNullable()
                .WithColumn("ConnectionId").AsString().Nullable()
                .WithColumn("UserName").AsString().NotNullable().WithDefaultValue("");
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}