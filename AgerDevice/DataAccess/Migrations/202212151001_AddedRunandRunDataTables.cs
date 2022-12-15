using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202212151001)]
    public class AddedRunandRunDataTables : Migration
    {
        public override void Up()
        {
            Create.Table("Runs")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Modified").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now)
                .WithColumn("AssociatedUnit").AsGuid().NotNullable()
                .WithColumn("StartTime").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now)
                .WithColumn("EndTime").AsDateTime().Nullable()
                .WithColumn("Duration").AsInt32().Nullable()
                .WithColumn("NumEntries").AsInt64().Nullable();

            Create.Table("RunDataTemplate")
                .WithColumn("Timestamp").AsDateTime().PrimaryKey().NotNullable()
                .WithColumn("Temperature").AsDecimal().Nullable()
                .WithColumn("Weight").AsDecimal().Nullable()
                .WithColumn("AirFlow").AsDecimal().Nullable()
                .WithColumn("Humidity").AsDecimal().Nullable()
                .WithColumn("DoorClosed").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Table("Runs");
            Delete.Table("RunDataTemplate");
        }
    }
}