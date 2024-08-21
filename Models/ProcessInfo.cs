namespace windows_audio_api.Models
{
    public class ProcessInfo
    {
        public required string Name { get; set; }
        public required int Id { get; set; }
        public required float CurrentVolume { get; set; }
    }

}
