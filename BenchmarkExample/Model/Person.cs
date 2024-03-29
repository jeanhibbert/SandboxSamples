﻿namespace ObjectCopyExample.Model;

public class Person
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset PlaceOfBirth { get; set; }
    public List<Address> Addresses { get; set; } = new List<Address>();
}
