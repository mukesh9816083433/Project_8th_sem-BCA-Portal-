public class ContactDetails
{
    public int Id { get; set; }
    public string District { get; set; }
    public string PermanentAddress { get; set; }
    public string ContactAddress { get; set; }
    public string ContactPhoneNo { get; set; }
    public int UserID { get; set; }

    public bool IsApproved { get; set; }
    public bool IsDeanApproved { get; set; } = false;
}
