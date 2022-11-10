using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062242)]
    public class AddedUnitTable : Migration
    {
        public override void Up()
        {
            Create.Table("Units")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Modified").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now)
                .WithColumn("SerialNumber").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsConnected").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsAcquisitioning").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("ConnectionId").AsString().Nullable()
                .WithColumn("PairedIds").AsString().NotNullable().WithDefaultValue("[]")
                .WithColumn("Name").AsString().NotNullable().WithDefaultValue("");
        }

        public override void Down()
        {
            Delete.Table("Units");
        }
    }
}