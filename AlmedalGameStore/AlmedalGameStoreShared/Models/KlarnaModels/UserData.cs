namespace AlmedalGameStoreShared.Models.KlarnaModels;

public class UserData
{
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string date_of_birth { get; set; }
    public string phone { get; set; }
    public Address address { get; set; }
}