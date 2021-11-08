namespace Refactoring.LegacyService
{
    public class Position
    {
        public const string PositionType_SecuritySpecialist = "SecuritySpecialist";
        public const string PositionType_FeatureDeveloper = "FeatureDeveloper";

        public int Id { get; set; }

        public string Name { get; set; }

        public PositionStatus Status { get; set; }
    }

    public enum PositionStatus
    {
        none = 0
    }
}
