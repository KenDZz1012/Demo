using System;
using System.Collections.Generic;

namespace Channel.Domain.Entities;

public partial class ServerInviteLink
{
    public Guid Id { get; set; }

    public Guid Serverid { get; set; }

    public string Code { get; set; } = null!;

    public DateTime? Expiredat { get; set; }

    public DateTime? Createdat { get; set; }

    public Guid? Createdby { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual Server Server { get; set; } = null!;
}
