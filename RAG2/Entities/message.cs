using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RAG2.Entities;

[Table("message")]
public partial class message
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [StringLength(1024)]
    public string? message_content { get; set; }

    [StringLength(255)]
    public string? user_id { get; set; }

    [Column(TypeName = "int(11)")]
    public int? comefrom_id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? create_at { get; set; }
}
