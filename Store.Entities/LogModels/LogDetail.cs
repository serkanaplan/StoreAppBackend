﻿using System.Text.Json;

namespace Store.Entities.LogModels;

public class LogDetail
{
    public Object? ModelModel { get; set; }
    public Object? Controller { get; set; }
    public Object? Action { get; set; }
    public Object? Id { get; set; }
    public Object? CreateAt { get; set; }

    public LogDetail() => CreateAt = DateTime.UtcNow;
    public override string ToString() => JsonSerializer.Serialize(this);
}
