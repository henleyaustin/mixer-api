using NAudio.CoreAudioApi;
using System.Diagnostics;
using windows_audio_api.Models;

namespace windows_audio_api
{
    public class AudioManager
    {
        private readonly MMDeviceEnumerator _deviceEnumerator;
        private readonly MMDevice _device;
        private readonly ILogger<AudioManager> _logger;

        public AudioManager(ILogger<AudioManager> logger)
        {
            _logger = logger;

            try
            {
                _deviceEnumerator = new MMDeviceEnumerator();
                _device = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                if (_device == null)
                {
                    throw new InvalidOperationException("No default audio endpoint found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing AudioManager.");
                throw;
            }
        }

        public void SetApplicationVolume(int processId, float volume)
        {
            try
            {
                var session = GetAudioSessionById(processId);
                if (session != null)
                {
                    if (session.SimpleAudioVolume != null)
                    {
                        session.SimpleAudioVolume.Volume = volume;
                        _logger.LogInformation($"Set volume for {session.DisplayName} to {volume * 100.0f}%.");
                    }
                    else
                    {
                        _logger.LogWarning($"Audio session for process {processId} does not have a volume control.");
                    }
                }
                else
                {
                    _logger.LogWarning($"Process {processId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error setting volume for process {processId}.");
            }
        }

        public float GetApplicationVolume(string processName)
        {
            try
            {
                var session = GetAudioSessionByName(processName);
                if (session != null)
                {
                    if (session.SimpleAudioVolume != null)
                    {
                        return session.SimpleAudioVolume.Volume;
                    }
                    else
                    {
                        _logger.LogWarning($"Audio session for process {processName} does not have a volume control.");
                        return -1; // No volume control found
                    }
                }

                _logger.LogWarning($"Process {processName} not found.");
                return -1; // Process not found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting volume for process {processName}.");
                return -1; // Error occurred
            }
        }

        public List<ProcessInfo> GetProcesses()
        {
            var processList = new List<ProcessInfo>();
            var addedProcessIds = new HashSet<int>();

            try
            {
                var sessions = _device.AudioSessionManager.Sessions;

                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    int processId = (int)session.GetProcessID;

                    if (processId != 0 && !addedProcessIds.Contains(processId))
                    {
                        try
                        {
                            var process = Process.GetProcessById(processId);
                            processList.Add(new ProcessInfo
                            {
                                Name = process.ProcessName,
                                Id = process.Id,
                                CurrentVolume = session.SimpleAudioVolume?.Volume * 100.0f ?? 0.0f
                            });
                            addedProcessIds.Add(processId);
                        }
                        catch (ArgumentException ex)
                        {
                            _logger.LogWarning(ex, $"Process with ID {processId} exited before it could be accessed.");
                        }
                    }
                }

                _logger.LogInformation("Retrieved process list with audio sessions.");
                return processList.OrderBy(p => p.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving processes.");
                return new List<ProcessInfo>(); // Return an empty list if an error occurs
            }
        }

        private AudioSessionControl? GetAudioSessionByName(string processName)
        {
            try
            {
                var sessions = _device.AudioSessionManager.Sessions;

                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    if (session.GetProcessID != 0 && Process.GetProcessById((int)session.GetProcessID).ProcessName == processName)
                    {
                        return session;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving audio session for process {processName}.");
                return null;
            }
        }

        private AudioSessionControl? GetAudioSessionById(int id)
        {
            try
            {
                var sessions = _device.AudioSessionManager.Sessions;

                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    if (session.GetProcessID != 0 && Process.GetProcessById((int)session.GetProcessID).Id == id)
                    {
                        return session;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving audio session for process ID {id}.");
                return null;
            }
        }
    }
}
