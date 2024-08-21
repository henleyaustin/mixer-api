using Microsoft.AspNetCore.Mvc;
using windows_audio_api.Models;

namespace windows_audio_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly AudioManager _audioManager;
        private readonly ILogger<AudioController> _logger;

        public AudioController(AudioManager audioManager, ILogger<AudioController> logger)
        {
            _audioManager = audioManager;
            _logger = logger;
        }

        /// <summary>
        /// Sets the volume for a specific application.
        /// </summary>
        /// <param name="request">The volume request containing the app name and volume level.</param>
        [HttpPost("set_volume")]
        public IActionResult SetVolume([FromBody] VolumeRequest request)
        {
            if (request.Volume < 0 || request.Volume > 100)
            {
                _logger.LogWarning($"Invalid volume level: {request.Volume}. Must be between 0 and 100.");
                return BadRequest("Volume must be between 0 and 100.");
            }

            try
            {
                _audioManager.SetApplicationVolume(request.ProcessId, request.Volume);
                _logger.LogInformation($"Volume set to {request.Volume} for {request.ProcessId}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error setting volume for {request.ProcessId}");
                return StatusCode(500, "An error occurred while setting the volume.");
            }
        }

        /// <summary>
        /// Gets the current volume for a specific application.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        [HttpGet("get_volume/{appName}")]
        public IActionResult GetVolume(string appName)
        {
            try
            {
                var volume = _audioManager.GetApplicationVolume(appName);
                if (volume == -1)
                {
                    _logger.LogWarning($"Application {appName} not found.");
                    return NotFound($"Application {appName} not found.");
                }
                _logger.LogInformation($"Volume for {appName} is {volume}");
                return Ok(volume);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting volume for {appName}");
                return StatusCode(500, "An error occurred while getting the volume.");
            }
        }

        /// <summary>
        /// Retrieves a list of processes with audio sessions.
        /// </summary>
        [HttpGet("audio-processes")]
        public IActionResult GetProcessesWithAudio()
        {
            try
            {
                var processList = _audioManager.GetProcesses();
                _logger.LogInformation("Retrieved process list with audio sessions.");
                return Ok(processList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving process list with audio sessions.");
                return StatusCode(500, "An error occurred while retrieving the process list.");
            }
        }
    }
}
