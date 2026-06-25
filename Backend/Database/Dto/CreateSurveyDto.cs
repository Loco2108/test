using System.Text.Json.Serialization;

namespace Backend.Dto;

public class CreateSurveyDto
{
    public string Title { get; set; } = string.Empty;
    [JsonConverter(typeof(NullableGuidJsonConverter))]
    public Guid? FolderId { get; set; }
}
