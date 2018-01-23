namespace SBIReportUtility.Entities
{
    public class BaseModel
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public int DeletedBy { get; set; }
        public string DeletedOn { get; set; }
    }
}
