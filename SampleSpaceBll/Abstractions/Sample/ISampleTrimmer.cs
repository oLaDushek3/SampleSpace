namespace SampleSpaceBll.Abstractions.Sample;

public interface ISampleTrimmer
{
    public (Stream? trimmedSample, string error) TrimMp3File(Stream inStream, TimeSpan cutFromStart, TimeSpan cutFromEnd);
    public (Stream? trimmedSample, string error) TestTrimMp3File(Stream inStream, TimeSpan cutStart, TimeSpan cutEnd);
}