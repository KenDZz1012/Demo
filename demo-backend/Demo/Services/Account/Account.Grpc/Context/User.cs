using System;
using System.Collections.Generic;

namespace Account.Grpc.Context;

public partial class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }

}
