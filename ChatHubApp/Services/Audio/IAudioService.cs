using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Audio
{
    public interface IAudioService
    {
        Task StartRecordingAsync();
        Task<Stream> StopRecordingAsync();
        bool IsRecording { get; }
    }
}
