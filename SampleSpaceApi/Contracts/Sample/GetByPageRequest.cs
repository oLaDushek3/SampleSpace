namespace SampleSpaceApi.Contracts.Sample;

public record GetByPageRequest(
    int Limit,
    int NumberOfPage);