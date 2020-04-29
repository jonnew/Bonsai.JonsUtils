## Jon's Utilities for Bonsai

I'm dumping random [Bonsai](https://bonsai-rx.org/) convenience nodes in this repo that might be useful for others. This namespace contains

1. A MatrixMap node for reordering the rows or columns of a matrix of data. This is useful for, e.g. applying a channel map to ephys data.
2. A OpenCVMatSocket node that can OpenCV Mats over a UDP socket to some other program. They are sent as raw data so some a-priori coordination about matrix size and element type is required.

### Nuget Packaging
In VS package manager console
```
Install-Package NuGet.CommandLine
```
then,
```
nuget pack Bonsai.JonsUtils.csproj -properties Configuration=Release
```

### License
MIT