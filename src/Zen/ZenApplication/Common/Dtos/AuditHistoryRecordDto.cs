﻿namespace Zen.Application.Common.Dtos;

/// <summary>
/// Represents an audit history record.
/// </summary>
public class AuditHistoryRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public string Snapshot { get; set; } = string.Empty;
    public string ChangedBy { get; set; } = string.Empty;
}