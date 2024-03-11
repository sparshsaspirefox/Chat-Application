using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Audio
{
    public class AudioService : IAudioService
    {
        private IAudioManager _audioManager;
        private IAudioRecorder _audioRecorder;
        public bool IsRecording => _audioRecorder.IsRecording;
        public AudioService(IAudioManager audioManager)
        {
            _audioManager = audioManager;
            _audioRecorder = audioManager.CreateRecorder();
        }
            
        public async Task StartRecordingAsync()
        {
            await _audioRecorder.StartAsync();
        }

        public async Task<Stream> StopRecordingAsync()
        {
            var recordedAudio = await _audioRecorder.StopAsync();
            return recordedAudio.GetAudioStream();
        }
    }

}
