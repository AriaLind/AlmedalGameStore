﻿namespace AlmedalGameStoreSQLApi.Endpoints.Users.UpdateById;

public class Request
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string NewPassword { get; set; }
    public string CurrentPassword { get; set; }
    public string Email { get; set; }
}