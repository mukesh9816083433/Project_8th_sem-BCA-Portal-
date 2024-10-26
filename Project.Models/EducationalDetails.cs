public class EducationalDetails
{
    public int Id { get; set; }
    public string Level { get; set; }
    public string College { get; set; }
    public string Programme { get; set; }
    public string AcademicYear { get; set; }
    public string RegistrationNumber { get; set; }

    public String MarksheetPic10 { get; set; }
    public String MaeksheetPic12 { get; set; }
    public int UserID { get; set; }

    public bool IsApproved { get; set; }
    public bool IsDeanApproved { get; set; } = false;
}
