﻿
namespace KKBookstore.Data.Entities;

public class Option : BaseEntity, ISoftDelete, ITrackable
{
    public int OptionID { get; set; }
    public int ProductID { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedWhen { get; set; }
    public int LastEditedBy { get; set; }
    public User LastEditedByUser { get; set; }
    public DateTimeOffset LastEditedWhen { get; set; }
}
