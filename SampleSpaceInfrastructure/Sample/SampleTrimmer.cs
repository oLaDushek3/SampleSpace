using FFMpegCore;
using FFMpegCore.Pipes;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using SampleSpaceBll.Abstractions.Sample;

namespace SampleSpaceInfrastructure.Sample;

public class SampleTrimmer(IOptions<FfMpegOptions> options) : ISampleTrimmer
{
    private readonly FfMpegOptions _options = options.Value;
    
    public (Stream? trimmedSample, string error) TrimMp3File(Stream inStream, TimeSpan cutFromStart,
        TimeSpan cutFromEnd)
    {
        if (cutFromStart > cutFromEnd)
            return (null, "Cut start cannot be greater than cut end");

        var outStream = new MemoryStream();

        using var reader = new Mp3FileReader(inStream);
        while (reader.ReadNextFrame() is { } frame)
        {
            if (reader.CurrentTime >= cutFromStart)
            {
                if (reader.CurrentTime <= cutFromEnd)
                    outStream.Write(frame.RawData, 0, frame.RawData.Length);
                else break;
            }
        }

        return (outStream, string.Empty);
    }

    public (Stream? trimmedSample, string error) TestTrimMp3File(Stream inStream, TimeSpan cutStart, TimeSpan cutEnd)
    {
        GlobalFFOptions.Configure(new FFOptions { BinaryFolder = _options.FfMpegExeFolderPath });

        var outStream = new MemoryStream();

        //A temporary solution to avoid the error "Pipe is broken"(System.IO.IOException: The channel was closed)
        try
        {
            FFMpegArguments
                .FromPipeInput(new StreamPipeSource(inStream))
                .OutputToPipe(new StreamPipeSink(outStream),
                    options => options
                        .Seek(cutStart)
                        .EndSeek(cutEnd)
                        .ForceFormat("mp3"))
                .ProcessSynchronously();
        }
        catch
        {
            // ignored
        }
        
        return (outStream, string.Empty);
    }
}