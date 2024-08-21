namespace windows_audio_api.Models
{
    public class VolumeRequest
    {
        public required int ProcessId { get; set; }
        public required float Volume { get; set; }
    }
}
