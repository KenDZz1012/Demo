using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Common.Constants;

namespace Account.Domain.Entities;

[Table("User_Relationship")]
public partial class UserRelationship
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    public Guid RequesterId { get; set; }

    public Guid AddresseeId { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AddresseeId")]
    [InverseProperty("UserRelationshipAddressees")]
    public virtual User Addressee { get; set; } = null!;

    [ForeignKey("RequesterId")]
    [InverseProperty("UserRelationshipRequesters")]
    public virtual User Requester { get; set; } = null!;
}
