using NAudio.Wave;
using SampleSpaceBll.Abstractions.Sample;

namespace SampleSpaceInfrastructure;

public class SampleTrimmer : ISampleTrimmer
{
    public (Stream? trimmedSample, string error) TrimMp3File(Stream inStream, TimeSpan cutFromStart, TimeSpan cutFromEnd)
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
}