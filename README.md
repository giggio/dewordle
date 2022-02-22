# Dewordle

[![NuGet version (dewordle)](https://img.shields.io/nuget/v/dewordle?color=blue)](https://www.nuget.org/packages/dewordle/)
[![Build app](https://github.com/giggio/dewordle/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/giggio/dewordle/actions/workflows/build.yml)
[![codecov](https://codecov.io/gh/giggio/dewordle/branch/main/graph/badge.svg?token=O8UDJRBFR1)](https://codecov.io/gh/giggio/dewordle)

A command line tool to help you play Wordle-like games. Written in C#, cross platform.

It is small, has only around 15MB. If you have the .NET Runtime or Sdk it is less than 200KB.

It does not require .NET ou .NET Framework to run.

## Running

Download the version to your OS (see bellow) and run it like this:

```bash
dewordle [<path to words file>]
```

The file is a line separated list of words with 5 letters. See bellow how to generate yours.


### Running the framework dependent .dll

The framework dependent dewordle.dll can be run where you already have the .NET Runtime installed. You'll need the
.NET 6 runtime, and run it like this:

```bash
dotnet dewordle.dll [<path to words file>]
```

You will need all the files that are in the .tgz, decompress it to a directory and run it from there.

## Installing

### Standalone binaries

Download an artifact from the latest
[release](https://github.com/giggio/dewordle/releases/latest)
and add it to your path.

There are artifacts for Windows and Linux (x86, ARM and Musl). The Linux ones are
are dynamic binaries, they can't run on distroless containers (`FROM scratch`).

* `dewordle-linux-arm` - Linux ARM
* `dewordle-linux-musl-x64` - Linux x64 Musl (Alpine etc)
* `dewordle-linux-x64` - Linux ARM
* `dewordle.exe` - Windows x64
* `dewordle.tgz` - Cross platform, [framework dependent](https://docs.microsoft.com/en-us/dotnet/core/deploying/#publish-framework-dependent)

The `.pdb` files are symbol files and are only needed for debugging, not running the application.

### As a dotnet cli tool

This tool can be installed as a dotnet global tool, if you have the .NET Sdk installed.
It is hosted [on nuget.org as `dewordle`](https://www.nuget.org/packages/dewordle).
Install with:

```bash
dotnet tool install --global dewordle
```

## Generating the word list

Clone the project with submodules

```bash
git clone test --recursive https://github.com/giggio/dewordle.git
```

And run `./words/makeWords.sh <language>`. Only `pt-BR`  works right now.
Check out the `makeWords.sh` file to see how you can generate for other languages using the available submodule.
If you update it to support other languages, send a PR!

## Contributing

Questions, comments, bug reports, and pull requests are all welcome.  Submit them at
[the project on GitHub](https://github.com/giggio/dewordle).

Bug reports that include steps-to-reproduce (including code) are the
best. Even better, make them in the form of pull requests.

## Author

[Giovanni Bassi](https://github.com/giggio).

## License

Licensed under the MIT License.
